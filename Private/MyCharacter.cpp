
#include "MyCharacter.h"
#include "MyAnimInstance.h"
#include "FireProjectile.h"
#include "MyCharacterStatComponent.h"
#include "AIController_Monster.h"
#include "MyPlayerController.h"
#include "Components/WidgetComponent.h"
#include "CharacterWidget.h"
#include "CharacterSetting.h"
#include "Weapon.h"
#include "MyPlayerState.h"
#include "HUDWidget.h"
#include "SkillController.h"
#include "MyCameraShake.h"
#include "Bluehole_projectGameModeBase.h"

AMyCharacter::AMyCharacter()
{
	PrimaryActorTick.bCanEverTick = false;
	IsMouseModeActive = false;
	SpringArm = CreateDefaultSubobject<USpringArmComponent>(TEXT("SPRINGARM"));
	Camera = CreateDefaultSubobject<UCameraComponent>(TEXT("CAMERA"));
	CharacterStat = CreateDefaultSubobject<UMyCharacterStatComponent>(TEXT("CHARACTERSTAT"));
	SkillController = CreateDefaultSubobject<USkillController>(TEXT("SKILLCONTROLLER"));

	SpringArm->SetupAttachment(GetCapsuleComponent());
	Camera->SetupAttachment(SpringArm);

	SpringArm->TargetArmLength = 450.0f;
	SpringArm->SetRelativeRotation(FRotator::ZeroRotator);
	SpringArm->bUsePawnControlRotation = true;
	SpringArm->bInheritPitch = true;
	SpringArm->bInheritRoll = true;
	SpringArm->bInheritYaw = true;
	SpringArm->bDoCollisionTest = true;
	bUseControllerRotationYaw = false;
	GetMesh()->SetRelativeLocationAndRotation(FVector(0.0f, 0.0f, -88.0f), FRotator(0.0f, -90.0f, 0.0f));

	GetMesh()->SetCollisionProfileName(TEXT("NoCollision"));
	GetCapsuleComponent()->SetCollisionProfileName(TEXT("Player"));
	GetCharacterMovement()->bOrientRotationToMovement = true;
	GetCharacterMovement()->RotationRate = FRotator(0.0f, 720.0f, 0.0f);
	DeadTimer = 5.0f;

#pragma region PREINIT STATE SETTING
	SetActorHiddenInGame(true);
	bCanBeDamaged = false;
#pragma endregion
}

void AMyCharacter::BeginPlay()
{
	Super::BeginPlay();

	GameMode = Cast<ABluehole_projectGameModeBase>(GetWorld()->GetAuthGameMode());
	Controller_ = Cast<AMyPlayerController>(GetController());

	// 위젯 세팅
	Controller_->SetHUDWidget();

	SetUserState(ECharacterState::LOADING);

	// 에셋 경로 설정
	auto DefaultSetting = GetDefault<UCharacterSetting>();
	FCharacterData* StatData = CharacterStat->GetData();
	MeshAssetPath = DefaultSetting->CharacterMeshAssets[StatData->MeshIndex];
	WeaponAssetPath = DefaultSetting->CharacterWeaponAssets[StatData->WeaponIndex];
	AnimAssetPath = DefaultSetting->CharacterAnimAssets[StatData->AnimIndex];
	MontageAssetPath = DefaultSetting->CharacterMontageAssets[StatData->MontageIndex];
	
	if (StatData->AttackType == (int)AttackType::Ranged)
	{
		ProjectileAssetPath = DefaultSetting->CharacterProjectileAssets[StatData->ProjectileIndex];
	}

	GetCharacterMovement()->JumpZVelocity = StatData->JumpVelocity;
	GetCharacterMovement()->MaxWalkSpeed = StatData->MoveSpeed;

	// 메시 세팅
	USkeletalMesh* NewMesh = Cast<USkeletalMesh>(StaticLoadObject(USkeletalMesh::StaticClass(), NULL, *MeshAssetPath.ToString()));
	if (NewMesh)
	{
		GetMesh()->SetSkeletalMesh(NewMesh);
	}

	// 무기 메시 세팅
	UStaticMesh* NewWeaponMesh = Cast<UStaticMesh>(StaticLoadObject(UStaticMesh::StaticClass(), NULL, *WeaponAssetPath.ToString()));
	FName WeaponSocket(TEXT("hand_rSocket"));
	EquippedWeapon = GetWorld()->SpawnActor<AWeapon>(FVector::ZeroVector, FRotator::ZeroRotator);
	EquippedWeapon->InitWeaponMesh(NewWeaponMesh);
	if (EquippedWeapon != nullptr)
	{
		EquippedWeapon->AttachToComponent(GetMesh(), FAttachmentTransformRules::SnapToTargetIncludingScale,
			WeaponSocket);
	}

	GetMesh()->SetAnimationMode(EAnimationMode::AnimationBlueprint);

	// 애님 인스턴스 세팅
	UClass* NewAnim = StaticLoadClass(UMyAnimInstance::StaticClass(), NULL, *AnimAssetPath.ToString());
	if (NewAnim)
	{
		GetMesh()->SetAnimInstanceClass(NewAnim);
	}
	MyAnim = Cast<UMyAnimInstance>(GetMesh()->GetAnimInstance());

	// 애님 몽타주 세팅
	UAnimMontage* NewAnimMontage = Cast<UAnimMontage>(StaticLoadObject(UAnimMontage::StaticClass(), NULL, *MontageAssetPath.ToString()));
	if (NewAnimMontage)
	{
		MyAnim->SetAnimMontage(NewAnimMontage);
	}

	// 발사체 세팅
	UClass* NewProjectile = StaticLoadClass(AFireProjectile::StaticClass(), NULL, *ProjectileAssetPath.ToString());
	if (NewProjectile)
	{
		ProjectileClass = NewProjectile;
	}

	// 카메라 섀이크 세팅
	MyShake = UMyCameraShake::StaticClass();

	// 스킬 컨트롤러 초기화
	SkillController->Init();

	SetUserState(ECharacterState::READY);
}

void AMyCharacter::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);
	PlayerInputComponent->BindAction(TEXT("Jump"), EInputEvent::IE_Pressed, this, &AMyCharacter::Jump);
	PlayerInputComponent->BindAction(TEXT("Attack"), EInputEvent::IE_Pressed, this, &AMyCharacter::Attack);
	PlayerInputComponent->BindAction(TEXT("Option"), EInputEvent::IE_Pressed, this, &AMyCharacter::MouseModeSwitch);

	PlayerInputComponent->BindAxis(TEXT("Up"), this, &AMyCharacter::UpDown);
	PlayerInputComponent->BindAxis(TEXT("Down"), this, &AMyCharacter::UpDown);
	PlayerInputComponent->BindAxis(TEXT("Left"), this, &AMyCharacter::LeftRight);
	PlayerInputComponent->BindAxis(TEXT("Right"), this, &AMyCharacter::LeftRight);
	PlayerInputComponent->BindAxis(TEXT("LookUp"), this, &AMyCharacter::LookUp);
	PlayerInputComponent->BindAxis(TEXT("Turn"), this, &AMyCharacter::Turn);
}

void AMyCharacter::PostInitializeComponents()
{
	Super::PostInitializeComponents();
}

void AMyCharacter::Tick(float DeltaTime)
{

}

void AMyCharacter::SetUserState(ECharacterState NewState)
{
	UserState = NewState;
	GameMode->SetUserState(NewState);

	switch (UserState)
	{
	case ECharacterState::LOADING:
	{
		// PREINIT 스테이트 이후 로딩 처리 등록
		DisableInput(Controller_);

		Controller_->GetHUDWidget()->BindCharacterStat(CharacterStat);

		auto PlayerState_ = Cast<AMyPlayerState>(PlayerState);
		CharacterStat->SetNewData(PlayerState_->GetCharacterLevel());
		SetActorHiddenInGame(true);
		bCanBeDamaged = false;
		break;
	}
	case ECharacterState::READY:
	{
		// 게임 세팅 로드가 완료된 이후 처리 등록
		SetActorHiddenInGame(false);
		bCanBeDamaged = true;

		CharacterStat->OnHPIsZero.AddLambda([this]()->void {
			SetUserState(ECharacterState::DEAD);
		});

		EnableInput(Controller_);
		break;
	}
	case ECharacterState::DEAD:
	{
		// 죽었을 시 후처리 등록
		UE_LOG(LogTemp, Warning, TEXT("CHARACTER DEAD STATE START."));
		SetActorEnableCollision(false);
		GetMesh()->SetHiddenInGame(false);
		MyAnim->SetDeadAnim();
		bCanBeDamaged = false;
		DisableInput(Controller_);

		GetWorld()->GetTimerManager().SetTimer(DeadTimerHandle, FTimerDelegate::CreateLambda([this]() -> void {
			//Cast<APlayerController>(GetController())->RestartLevel();
			UE_LOG(LogTemp, Warning, TEXT("GAME OVER."));
			EquippedWeapon->Destroy();
			Destroy();
		}), DeadTimer, false);

		break;
	}
	}
}

ECharacterState AMyCharacter::GetUserState() const
{
	return UserState;
}

void AMyCharacter::UpDown(float NewAxisValue)
{
	if (MyAnim->GetIsInAir()) return;
	if (MyAnim->GetIsRolling()) return;
	if (MyAnim->GetUsingShotLaunch()) return;

	AddMovementInput(FRotationMatrix(GetControlRotation()).GetUnitAxis(EAxis::X), NewAxisValue);
}

void AMyCharacter::LeftRight(float NewAxisValue)
{
	if (MyAnim->GetIsInAir()) return;
	if (MyAnim->GetIsRolling()) return;
	if (MyAnim->GetUsingShotLaunch()) return;

	AddMovementInput(FRotationMatrix(GetControlRotation()).GetUnitAxis(EAxis::Y), NewAxisValue);
}

void AMyCharacter::LookUp(float NewAxisValue)
{
	AddControllerPitchInput(NewAxisValue);
}

void AMyCharacter::Turn(float NewAxisValue)
{
	AddControllerYawInput(NewAxisValue);
}

void AMyCharacter::Attack()
{
	if (!MyAnim->CancelAnimation(SkillType::Attack)) return;

	MyAnim->PlayAttack();
	MyAnim->SetAttackAnim();

	if (ProjectileClass)
	{
		FVector CameraLocation;
		FRotator CameraRotation;
		GetActorEyesViewPoint(CameraLocation, CameraRotation);
		SetActorRotation(FRotator(GetActorRotation().Pitch, CameraRotation.Yaw, GetActorRotation().Roll));
		bUseControllerRotationYaw = true;

		// MuzzleOffset 을 카메라 스페이스에서 월드 스페이스로 변환
		FVector MuzzleLocation = CameraLocation + FTransform(CameraRotation).TransformVector(MuzzleOffset);
		FRotator MuzzleRotation = CameraRotation;

		// 조준을 위치 조정
		MuzzleRotation.Pitch += 10.0f;
		UWorld* World = GetWorld();
		if (World)
		{
			FActorSpawnParameters SpawnParams;
			SpawnParams.Owner = this;
			SpawnParams.Instigator = Instigator;
			AFireProjectile* Projectile = World->SpawnActor<AFireProjectile>(ProjectileClass, MuzzleLocation, MuzzleRotation, SpawnParams);
			if (Projectile)
			{
				// 투사체 정보 초기화
				Projectile->Init(50.0f, 1.5f, 150.0f);
				FVector LaunchDirection = MuzzleRotation.Vector();
				Projectile->FireInDirection(LaunchDirection);
			}
		}
	}
}

void AMyCharacter::MouseModeSwitch()
{
	if (IsMouseModeActive)
	{
		// 마우스 모드 비활성화
		Controller_->bShowMouseCursor = false;
		Controller_->SetIgnoreLookInput(false);
		Controller_->SetIgnoreMoveInput(false);
		IsMouseModeActive = false;

		/*for (int i = 0; i < InputComponent->GetNumActionBindings(); i++)
		{
			if (InputComponent->GetActionBinding(i).ActionName != TEXT("Option"))
			{
				InputComponent->GetActionBinding(i).bConsumeInput = false;
			}
		}*/
	}
	else
	{
		// 마우스 모드 활성화
		Controller_->bShowMouseCursor = true;
		Controller_->SetIgnoreLookInput(true);
		Controller_->SetIgnoreMoveInput(true);
		IsMouseModeActive = true;

		/*for (int i = 0; i < InputComponent->GetNumActionBindings(); i++)
		{
			InputComponent->GetActionBinding(i).bConsumeInput = true;
		}*/
	}
}

float AMyCharacter::TakeDamage(float DamageAmount, FDamageEvent const & DamageEvent, AController * EventInstigator, AActor * DamageCauser)
{
	float FinalDamage = Super::TakeDamage(DamageAmount, DamageEvent, EventInstigator, DamageCauser);
	UE_LOG(LogTemp, Warning, TEXT("Player takes damage : %f"), FinalDamage);
	CharacterStat->SetDamage(FinalDamage);
	return FinalDamage;
}