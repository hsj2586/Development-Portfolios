#include "ShotLaunch.h"
#include "MyPlayerController.h"
#include "ShotLaunchProjectile.h"
#include "ShotLaunchMuzzle.h"
#include "Monster.h"

// 코사인 30도의 근사값
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

	// 스킬 Input 세팅
	FString InputName = "Skill" + FString::FromInt(SkillSlotIndex);
	InputComponent->BindAction(FName(*InputName), EInputEvent::IE_Pressed, this, &UShotLaunch::DoSkill);
}

void UShotLaunch::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction * ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);
	
	// 넉백 로직
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

	// 쿨타임 초기화 및 마나 소비
	RemainCooltime = CoolTime;
	Player->CharacterStat->ConsumeMana(Mana);

	// 애니메이션 작동
	Anim->SetShotLaunchAnim();
	Anim->PlaySkill();

	// 컨트롤러 방향으로 회전하는 로직
	FVector CameraLocation;
	FRotator CameraRotation;
	Player->GetActorEyesViewPoint(CameraLocation, CameraRotation);
	Player->SetActorRotation(FRotator(Player->GetActorRotation().Pitch,
		CameraRotation.Yaw, 0));

	// 카메라 섀이크 발생
	AMyPlayerController* Controller = Cast<AMyPlayerController>(Player->GetController());
	if (Player->MyShake != NULL)
	{
		Controller->PlayerCameraManager->PlayCameraShake(Player->MyShake, 1.0f);
	}

	Player->GetCharacterMovement()->bOrientRotationToMovement = false;
	Player->GetCharacterMovement()->MaxWalkSpeed = 3000.0f;

	// 발사 위치조정
	FVector MuzzleLocation = CameraLocation + FTransform(CameraRotation).TransformVector(MuzzleOffset);
	FRotator MuzzleRotation = CameraRotation;
	MuzzleRotation.Pitch += 10.0f;

	UWorld* World = GetWorld();
	if (World)
	{
		// Muzzle 파티클 생성
		World->SpawnActor<AShotLaunchMuzzle>(MuzzleClass, MuzzleLocation, MuzzleRotation);

		DamagingToEnemiesInRange();

		// Forward 방향 기준으로 5방향으로 발사체 생성
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
	// 각도 값을 라디안으로 변환
	float Radian = FMath::DegreesToRadians(Degree);

	// 회전 변환
	float TargetX = ForwardVector.X * FMath::Cos(Radian) - ForwardVector.Y * FMath::Sin(Radian);
	float TargetY = ForwardVector.X * FMath::Sin(Radian) + ForwardVector.Y * FMath::Cos(Radian);
	FVector TargetVector = FVector(TargetX, TargetY, 0);

	return TargetVector;
}

void UShotLaunch::DamagingToEnemiesInRange()
{
	// 구체 콜리전을 통해서 충돌체를 포착
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
				// 내적 값을 구해 시야각 안에 들어오는지 판단
				FVector ForwardVector = Player->GetActorForwardVector().GetSafeNormal();
				FVector DirectionToMonster = (Monster->GetActorLocation() - Player->GetActorLocation()).GetSafeNormal();
				float DotResult = FVector::DotProduct(ForwardVector, DirectionToMonster);

				if (DotResult >= COS_30DEGREE)
				{
					// 데미지 처리 로직
					FDamageEvent DamageEvent;
					Monster->TakeDamage(Value, DamageEvent, Cast<AMyCharacter>(GetOwner())->GetController(), GetOwner());
				}
			}
		}
	}
}
