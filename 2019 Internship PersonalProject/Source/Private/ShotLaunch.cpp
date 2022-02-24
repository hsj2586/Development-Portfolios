#include "ShotLaunch.h"
#include "MyPlayerController.h"
#include "ShotLaunchProjectile.h"
#include "ShotLaunchMuzzle.h"
#include "Monster.h"

// �ڻ��� 30���� �ٻ簪
#define COS_30DEGREE 0.866025

UShotLaunch::UShotLaunch()
{
	static ConstructorHelpers::FClassFinder<AShotLaunchProjectile> Projectile
	(TEXT("/Game/Blueprints/BP_ShotLaunchProjectile.BP_ShotLaunchProjectile_C"));

	if (Projectile.Succeeded())
	{
		ProjectileClass = Projectile.Class;
	}

	static ConstructorHelpers::FClassFinder<AShotLaunchMuzzle> Muzzle
	(TEXT("Blueprint'/Game/Blueprints/BP_ShotLaunchMuzzle.BP_ShotLaunchMuzzle_C'"));

	if (Muzzle.Succeeded())
	{
		MuzzleClass = Muzzle.Class;
	}
}

void UShotLaunch::Init(UInputComponent *PlayerInput, int InputIndex, struct FSkillData* LoadedSkillData)
{
	Super::Init(PlayerInput, InputIndex, LoadedSkillData);

	// ��ų Input ����
	FString InputName = "Skill" + FString::FromInt(SkillSlotIndex);
	InputComponent->BindAction(FName(*InputName), EInputEvent::IE_Pressed, this, &UShotLaunch::DoSkill);
}

void UShotLaunch::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction * ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);
	
	// �˹� ����
	if (Anim && Anim->GetUsingShotLaunch())
	{
		Player->AddMovementInput(Player->GetActorForwardVector() * -1);
	}
}

void UShotLaunch::DoSkill()
{
	CheckLinkageSkill();
	Anim = Cast<UMyAnimInstance>(Player->GetMesh()->GetAnimInstance());

	if (RemainCooltime > 0) return;
	if (Player->CharacterStat->CheckManaIsZero(Mana)) return;
	if (!Anim->CancelAnimation(SkillType::ShotLaunch)) return;

	// ��Ÿ�� �ʱ�ȭ �� ���� �Һ�
	RemainCooltime = CoolTime;
	Player->CharacterStat->ConsumeMana(Mana);

	// �ִϸ��̼� �۵�
	Anim->SetShotLaunchAnim();
	Anim->PlaySkill();

	// ��Ʈ�ѷ� �������� ȸ���ϴ� ����
	FVector CameraLocation;
	FRotator CameraRotation;
	Player->GetActorEyesViewPoint(CameraLocation, CameraRotation);
	Player->SetActorRotation(FRotator(Player->GetActorRotation().Pitch,
		CameraRotation.Yaw, 0));

	// ī�޶� ����ũ �߻�
	AMyPlayerController* Controller = Cast<AMyPlayerController>(Player->GetController());
	if (Player->MyShake != NULL)
	{
		Controller->PlayerCameraManager->PlayCameraShake(Player->MyShake, 1.0f);
	}

	Player->GetCharacterMovement()->bOrientRotationToMovement = false;
	Player->GetCharacterMovement()->MaxWalkSpeed = 3000.0f;

	// �߻� ��ġ����
	FVector MuzzleLocation = CameraLocation + FTransform(CameraRotation).TransformVector(MuzzleOffset);
	FRotator MuzzleRotation = CameraRotation;
	MuzzleRotation.Pitch += 10.0f;

	UWorld* World = GetWorld();
	if (World)
	{
		// Muzzle ��ƼŬ ����
		World->SpawnActor<AShotLaunchMuzzle>(MuzzleClass, MuzzleLocation, MuzzleRotation);

		DamagingToEnemiesInRange();

		// Forward ���� �������� 5�������� �߻�ü ����
		FVector LaunchDirection = FVector(Player->GetActorForwardVector().X, Player->GetActorForwardVector().Y, 0);
		for (int i = 0; i < 5; i++)
		{
			AShotLaunchProjectile* SpawningProjectile = World->SpawnActor<AShotLaunchProjectile>(ProjectileClass, MuzzleLocation, MuzzleRotation);
			if (SpawningProjectile)
			{
				FVector Temp = GetFireDirection(LaunchDirection, i * 15 - 30);
				SpawningProjectile->FireInDirection(Temp);
			}
		}
	}
}

FVector UShotLaunch::GetFireDirection(FVector ForwardVector, float Degree)
{
	// ���� ���� �������� ��ȯ
	float Radian = FMath::DegreesToRadians(Degree);

	// ȸ�� ��ȯ
	float TargetX = ForwardVector.X * FMath::Cos(Radian) - ForwardVector.Y * FMath::Sin(Radian);
	float TargetY = ForwardVector.X * FMath::Sin(Radian) + ForwardVector.Y * FMath::Cos(Radian);
	FVector TargetVector = FVector(TargetX, TargetY, 0);

	return TargetVector;
}

void UShotLaunch::DamagingToEnemiesInRange()
{
	// ��ü �ݸ����� ���ؼ� �浹ü�� ����
	TArray<FOverlapResult> OverlapResults;
	FCollisionQueryParams CollisionQueryParam(NAME_None, false, Player);
	bool bResult = GetWorld()->OverlapMultiByChannel(
		OverlapResults,
		Player->GetActorLocation(),
		FQuat::Identity,
		ECollisionChannel::ECC_GameTraceChannel1,
		FCollisionShape::MakeSphere(800.0f),
		CollisionQueryParam
	);

	if (bResult)
	{
		for (auto OverlapResult : OverlapResults)
		{
			AMonster* Monster = Cast<AMonster>(OverlapResult.GetActor());
			if (Monster)
			{
				// ���� ���� ���� �þ߰� �ȿ� �������� �Ǵ�
				FVector ForwardVector = Player->GetActorForwardVector().GetSafeNormal();
				FVector DirectionToMonster = (Monster->GetActorLocation() - Player->GetActorLocation()).GetSafeNormal();
				float DotResult = FVector::DotProduct(ForwardVector, DirectionToMonster);

				if (DotResult >= COS_30DEGREE)
				{
					// ������ ó�� ����
					FDamageEvent DamageEvent;
					Monster->TakeDamage(Value, DamageEvent, Cast<AMyCharacter>(GetOwner())->GetController(), GetOwner());
				}
			}
		}
	}
}
