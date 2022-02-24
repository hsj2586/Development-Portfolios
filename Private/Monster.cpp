// Fill out your copyright notice in the Description page of Project Settings.

#include "Monster.h"
#include "AIController_Monster.h"
#include "MyPlayerController.h"
#include "MyCharacter.h"
#include "BehaviorTree/BlackboardComponent.h"
#include "MonsterStatComponent.h"
#include "Runtime/AIModule/Classes/BrainComponent.h"
#include "Components/WidgetComponent.h"
#include "CharacterWidget.h"
#include "CharacterSetting.h"
#include "MyGameInstance.h"
#include "Weapon.h"
#include "PoolableComponent.h"
#include "Runtime/Engine/Public/EngineUtils.h"

AMonster::AMonster()
{
	PrimaryActorTick.bCanEverTick = true;
	AIControllerClass = AAIController_Monster::StaticClass();
	AutoPossessAI = EAutoPossessAI::PlacedInWorldOrSpawned;
	CharacterStat = CreateDefaultSubobject<UMonsterStatComponent>(TEXT("CHARACTERSTAT"));
	PoolableComponent = CreateDefaultSubobject<UPoolableComponent>(TEXT("POOLABLECOMPONENT"));
	HPBarWidget = CreateDefaultSubobject<UWidgetComponent>(TEXT("HPBARWIDGET"));
	HPBarWidget->SetupAttachment(GetMesh());
	HPBarWidget->SetRelativeLocation(FVector(0.0f, 0.0f, 270.0f));
	HPBarWidget->SetEnableGravity(false);
	HPBarWidget->bGenerateOverlapEvents = false;
	HPBarWidget->SetCanEverAffectNavigation(false);
	GetMesh()->SetRelativeLocationAndRotation(FVector(0, 0, -100), FQuat(FRotator(0, -90, 0)));
	GetMesh()->SetCollisionProfileName(TEXT("NoCollision"));
	GetMesh()->SetEnableGravity(false);
	GetMesh()->bPerBoneMotionBlur = false;
	GetCapsuleComponent()->SetCollisionProfileName(TEXT("Monster"));
	GetCapsuleComponent()->SetCapsuleHalfHeight(100.0f);
	GetCapsuleComponent()->SetCapsuleRadius(60.0f);
	GetCapsuleComponent()->SetEnableGravity(false);

	GetCharacterMovement()->bOrientRotationToMovement = true;
	GetCharacterMovement()->RotationRate = FRotator(0.0f, 720.0f, 0.0f);
	GetCharacterMovement()->MaxWalkSpeed = 400.0f;

	HPBarWidget->SetWidgetSpace(EWidgetSpace::Screen);

	DeadTimer = 5.0f;

	// ���� ����
	static ConstructorHelpers::FClassFinder<UUserWidget> UI_HUD(TEXT("/Game/UI/UI_HPBar.UI_HPBar_C"));
	if (UI_HUD.Succeeded())
	{
		HPBarWidget->SetWidgetClass(UI_HUD.Class);
		HPBarWidget->SetDrawSize(FVector2D(160.0f, 60.0f));
	}

#pragma region PREINIT STATE SETTING
	SetActorHiddenInGame(true);
	HPBarWidget->SetHiddenInGame(true);
	bCanBeDamaged = false;
#pragma endregion
}

void AMonster::Tick(float DeltaSeconds)
{
	// �÷��̾�� �Ÿ���� �� ������ ����
	if (Player && HPBarWidget)
	{
		float Distance = Player->GetDistanceTo(this);
		if (Distance <= 2000.0f)
		{
			HPBarWidget->SetHiddenInGame(false);
		}
		else
		{
			HPBarWidget->SetHiddenInGame(true);
		}

		if (Distance <= 8000.0f)
		{
			SetActorHiddenInGame(false);
			GetMesh()->MeshComponentUpdateFlag = EMeshComponentUpdateFlag::AlwaysTickPose;
		}
		else
		{
			SetActorHiddenInGame(true);
			GetMesh()->MeshComponentUpdateFlag = EMeshComponentUpdateFlag::OnlyTickPoseWhenRendered;
		}
	}
}

void AMonster::Init(struct FMonsterData* StatData, USkeletalMesh* NewMesh, 
	UStaticMesh* NewWeaponMesh, UClass* NewAnim, UAnimMontage* NewAnimMontage)
{
	// �÷��̾� ����
	TActorIterator<AMyCharacter> iter(GetWorld());
	Player = *iter;

	// ���� �̸����� ���� ���� ������ ����
	CharacterStat->SetNewData(*StatData->MName);
	SetActorScale3D(FVector(StatData->MeshScale, StatData->MeshScale, StatData->MeshScale));
	GetCharacterMovement()->MaxWalkSpeed = StatData->MoveSpeed;

	// �޽� ����
	GetMesh()->SetSkeletalMesh(NewMesh);

	// ���� �޽� ����
	FName WeaponSocket(TEXT("b_MF_Weapon_R_Socket"));
	EquippedWeapon = GetWorld()->SpawnActor<AWeapon>(FVector::ZeroVector, FRotator::ZeroRotator);
	EquippedWeapon->InitWeaponMesh(NewWeaponMesh);
	if (EquippedWeapon != nullptr)
	{
		EquippedWeapon->AttachToComponent(GetMesh(), FAttachmentTransformRules::SnapToTargetIncludingScale,
			WeaponSocket);
	}
	EquippedWeapon->SetActorEnableCollision(false);
	EquippedWeapon->Weapon->bGenerateOverlapEvents = false;
	EquippedWeapon->Weapon->SetEnableGravity(false);

	// �ִ� �ν��Ͻ� ����
	GetMesh()->SetAnimInstanceClass(NewAnim);
	MyAnim = Cast<UMonsterAnimInstance>(GetMesh()->GetAnimInstance());

	// �ִ� ��Ÿ�� ����
	MyAnim->SetAnimMontage(NewAnimMontage);
	MyAnim->OnMontageEnded.AddDynamic(this, &AMonster::OnAttackMontageEnded);

	// ���Ÿ� ���� ������ ���, �߻�ü�� ����
	if (StatData->AttackType == (int)AttackType::Ranged)
	{
		ProjectileAssetPath = GetDefault<UCharacterSetting>()->MonsterProjectileAssets[StatData->ProjectileIndex];
		UClass* NewProjectile = StaticLoadClass(AFireProjectile::StaticClass(), NULL, *ProjectileAssetPath.ToString());
		if (NewProjectile)
		{
			ProjectileClass = NewProjectile;
		}
	}

	// ���� UI ������Ʈ ����
	auto CharacterWidget = Cast<UCharacterWidget>(HPBarWidget->GetUserWidgetObject());
	if (CharacterWidget != nullptr)
	{
		CharacterWidget->BindCharacterStat(CharacterStat);
	}

	// ���� ��ó�� ����
	CharacterStat->OnHPIsZero.AddLambda([this]()->void {
		MyAnim->SetDeadAnim();
		SetActorEnableCollision(false);
		AAIController_Monster* AIController = Cast<AAIController_Monster>(GetController());
		AIController->GetBrainComponent()->StopLogic(FString("Dead"));

		GetWorld()->GetTimerManager().SetTimer(DeadTimerHandle, FTimerDelegate::CreateLambda([this]() -> void {
			AAIController_Monster* AIController = Cast<AAIController_Monster>(GetController());
			UE_LOG(LogTemp, Warning, TEXT("MONSTER DEAD."));
			EquippedWeapon->Destroy();
			HPBarWidget->SetHiddenInGame(true);
			AIController->StopAI();
			PoolableComponent->Despawn();
		}), DeadTimer, false);
	});

	Cast<AAIController_Monster>(GetController())->RunAI();
}

void AMonster::Attack()
{
	MyAnim->PlayAttack();
	IsAttacking = true;
}

float AMonster::TakeDamage(float DamageAmount, FDamageEvent const & DamageEvent, AController * EventInstigator, AActor * DamageCauser)
{
	float FinalDamage = Super::TakeDamage(DamageAmount, DamageEvent, EventInstigator, DamageCauser);
	UE_LOG(LogTemp, Warning, TEXT("Player takes damage : %f"), FinalDamage);

	CharacterStat->SetDamage(FinalDamage);
	if (CharacterStat->GetHP() < KINDA_SMALL_NUMBER)
	{
		// ������(�÷��̾� ��Ʈ�ѷ�) Ž��
		AMyPlayerController* Instigator_ = nullptr;
		TActorIterator<AMyPlayerController> Iter(GetWorld());
		Instigator_ = *Iter;
		if (Instigator_)
		{
			Instigator_->NPCKill(this);
		}
	}
	return FinalDamage;
}

void AMonster::OnAttackMontageEnded(UAnimMontage * Montage, bool bInterrupted)
{
	OnAttackEnd.Broadcast();
	IsAttacking = false;
}

int32 AMonster::GetExp() const
{
	return CharacterStat->GetDropExp();
}