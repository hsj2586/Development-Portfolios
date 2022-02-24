// Fill out your copyright notice in the Description page of Project Settings.
#include "P1Player.h"
#include "P1PlayerController.h"
#include "P1CharacterMovementComponent.h"
#include "P1SpringArmComponent.h"
#include "Components/CapsuleComponent.h"
#include "Components/BoxComponent.h"
#include "Camera/CameraComponent.h"
#include "GameFramework/Controller.h"
#include "GameFramework/SpringArmComponent.h"

#include <Runtime/Engine/Public/Net/UnrealNetwork.h>

// 스크린 디버그 출력 예시
//FString str = FString::Printf(TEXT("Name : %s"), *GetNetOwner()->GetName());
//GEngine->AddOnScreenDebugMessage(-1, 3.0f, FColor::Red, str);

// Sets default values
AP1Player::AP1Player(const FObjectInitializer& ObjectInitializer)
	: Super(ObjectInitializer.SetDefaultSubobjectClass<UP1CharacterMovementComponent>(ACharacter::CharacterMovementComponentName))
{
 	// Set this character to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	GetCapsuleComponent()->InitCapsuleSize(42.f, 96.0f);
	GetCapsuleComponent()->SetGenerateOverlapEvents(true);
	GetCapsuleComponent()->OnComponentBeginOverlap.AddDynamic(this, &AP1Player::BeginOverlapEvent);

	GetCharacterMovement()->RotationRate = FRotator(0.0f, 360.0f, 0.0f);
	GetCharacterMovement()->JumpZVelocity = 550.f;
	GetCharacterMovement()->AirControl = 0.1f;
	GetCharacterMovement()->MaxWalkSpeed = MAX_WALK_SPEED;
	GetCharacterMovement()->MaxAcceleration = 1024.f;
	GetCharacterMovement()->BrakingDecelerationWalking = 1024.f;

	SpringArm = CreateDefaultSubobject<UP1SpringArmComponent>(TEXT("SpringArm"));
	SpringArm->AttachTo(GetCapsuleComponent());
	SpringArm->TargetArmLength = 300.0f;
	SpringArm->SetWorldRotation(FRotator(-60.f, 0.f, 0.f));
	SpringArm->bEnableCameraLag = true;
	SpringArm->CameraLagSpeed = 8;
	SpringArm->CameraLagMaxDistance = 1.2f;
	SpringArm->bEnableCameraRotationLag = true;
	SpringArm->CameraRotationLagSpeed = 10;
	SpringArm->CameraLagMaxTimeStep = 0.5f;
	SpringArm->bUsePawnControlRotation = false;

	MainCamera = CreateDefaultSubobject<UCameraComponent>(TEXT("Camera"));
	MainCamera->AttachTo(SpringArm, USpringArmComponent::SocketName);

	static ConstructorHelpers::FObjectFinder<USkeletalMesh> mesh(TEXT("SkeletalMesh'/Game/Resources/Sword_and_Shield_Pack/P1Player_MeleeArmed_SkeletalMesh.P1Player_MeleeArmed_SkeletalMesh'"));
	GetMesh()->SetSkeletalMesh(mesh.Object);
	
	WeaponCollisionBox = CreateDefaultSubobject<UBoxComponent>(TEXT("WeaponCollisionBox"));
	FName socketName = GetMesh()->GetSocketBoneName(FName("Sword_joint"));
	WeaponCollisionBox->AttachToComponent(GetMesh(), FAttachmentTransformRules::SnapToTargetIncludingScale, socketName);

	// Setting Replication
	bReplicates = true;
	GetCharacterMovement()->SetIsReplicated(true);
}

void AP1Player::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
	Super::GetLifetimeReplicatedProps(OutLifetimeProps);

	DOREPLIFETIME(AP1Player, bIsAttacking);
	DOREPLIFETIME(AP1Player, bIsBlocking);
	DOREPLIFETIME(AP1Player, fowardValue);
	DOREPLIFETIME(AP1Player, rightValue);
}

void AP1Player::Tick(float DeltaSeconds)
{
	Super::Tick(DeltaSeconds);
}

void AP1Player::BeginPlay()
{
	Super::BeginPlay();
}

void AP1Player::MoveForward(float value)
{
	if ((Controller == nullptr)) return;
	if (Controller->IsMoveInputIgnored()) return;
	if (bIsAttacking || bIsBlocking) return;

	fowardValue = value >= 0.0f ? value : BACK_WALK_SPEED_MULTIFLIER * value;
	if (fowardValue != 0.0f)
	{
		const FRotator Rotation = Controller->GetControlRotation();
		const FRotator YawRotation(0, Rotation.Yaw, 0);
		const FVector Direction = FRotationMatrix(YawRotation).GetUnitAxis(EAxis::X);

		if (!HasAuthority())
		{
			// Only Excute by Client.
			AddMovementInput(Direction, fowardValue);
			SetActorRotation(YawRotation);
		}
		MoveForwardByServer(fowardValue, Direction, YawRotation);
	}
	else
		MoveForwardByServer(fowardValue, FVector::ZeroVector, FRotator::ZeroRotator);
}

void AP1Player::MoveForwardByServer_Implementation(const float value, const FVector vec, const FRotator rot)
{
	if (value != 0.0f)
	{
		AddMovementInput(vec, value);
		SetActorRotation(rot);
	}
	fowardValue = value;
}

void AP1Player::MoveRight(float value)
{
	if ((Controller == nullptr)) return;
	if (Controller->IsMoveInputIgnored()) return;
	if (bIsAttacking || bIsBlocking) return;

	rightValue = RIGHT_WALK_SPEED_MULTIFLIER * value;
	if (rightValue != 0.0f)
	{
		const FRotator Rotation = Controller->GetControlRotation();
		const FRotator YawRotation(0, Rotation.Yaw, 0);
		const FVector Direction = FRotationMatrix(YawRotation).GetUnitAxis(EAxis::Y);

		if (!HasAuthority())
		{
			// Only Excute by Client.
			AddMovementInput(Direction, rightValue);
			SetActorRotation(YawRotation);
		}
		MoveRightByServer(rightValue, Direction, YawRotation);
	}
	else
		MoveRightByServer(rightValue, FVector::ZeroVector, FRotator::ZeroRotator);
}

void AP1Player::MoveRightByServer_Implementation(const float value, const FVector vec, const FRotator rot)
{
	if (value != 0.0f)
	{
		AddMovementInput(vec, value);
		SetActorRotation(rot);
	}
	rightValue = value;
}

void AP1Player::SetIgnoreMovementInput_Implementation(bool bOn)
{
	AP1PlayerController* const PlayerController = CastChecked<AP1PlayerController>(GetController());
	if (PlayerController != nullptr)
	{
		if(bOn == true)
		{
			PlayerController->SetIgnoreMoveInput(true);
		}
		else if (bOn == false)
		{
			PlayerController->ResetIgnoreMoveInput();
		}
	}
}

void AP1Player::SetIgnoreLookInput_Implementation(bool bOn)
{
	AP1PlayerController* const PlayerController = CastChecked<AP1PlayerController>(GetController());
	if (PlayerController != nullptr)
	{
		if (bOn == true)
		{
			PlayerController->SetIgnoreLookInput(true);
		}
		else if (bOn == false)
		{
			PlayerController->ResetIgnoreLookInput();
		}
	}
}

void AP1Player::DoAttack_Implementation()
{
	if (CheckEnableAttack() == false) return;

	bIsAttacking = true;
	SetIgnoreMovementInput(true);
}

void AP1Player::DoBlock_Implementation(float bActive)
{
	bIsBlocking = CheckEnableBlock(bActive);
	SetIgnoreMovementInput(bIsBlocking);
}

bool AP1Player::CheckEnableAttack()
{
	if (bIsAttacking == true) return false;

	bool bIsFalling = GetCharacterMovement()->IsFalling();
	if (bIsFalling == true) return false;

	if (bIsBlocking == true) return false;
	
	return true;
}

bool AP1Player::CheckEnableJump()
{
	if (bIsAttacking == true) return false;

	bool bIsFalling = GetCharacterMovement()->IsFalling();
	if (bIsFalling == true) return false;

	if (bIsBlocking == true) return false;

	return true;
}

bool AP1Player::CheckEnableBlock(float bActive)
{
	if (bIsAttacking == true) return false;

	bool bIsFalling = GetCharacterMovement()->IsFalling();
	if (bIsFalling == true) return false;

	if (bActive == 0.0f) return false;
	
	return true;
}

void AP1Player::AnimNotify_EndAttack()
{
	// 기본 공격 끝 이벤트 처리
	bIsAttacking = false;
	SetIgnoreMovementInput(false);
}

void AP1Player::AnimNotify_OnLanding()
{
	// 점프 끝 이벤트 처리
	SetIgnoreMovementInput(false);
}

void AP1Player::Jump()
{
	if (CheckEnableJump() == false) return;

	SetIgnoreMovementInput(true);
	ACharacter::Jump();
}

void AP1Player::AddControllerYawInput(float Val)
{
	if (Val != 0.f && Controller && Controller->IsLocalPlayerController())
	{
		AP1PlayerController* const PC = CastChecked<AP1PlayerController>(Controller);
		PC->AddYawInput(Val);
	}
}

// Called to bind functionality to input
void AP1Player::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);

	check(PlayerInputComponent);
	PlayerInputComponent->BindAction("Jump", IE_Pressed, this, &ACharacter::Jump);
	PlayerInputComponent->BindAction("Attack", IE_Pressed, this, &AP1Player::DoAttack);

	PlayerInputComponent->BindAxis("MoveForward", this, &AP1Player::MoveForward);
	PlayerInputComponent->BindAxis("MoveRight", this, &AP1Player::MoveRight);
	PlayerInputComponent->BindAxis("Turn", this, &AP1Player::AddYawRotation);
	PlayerInputComponent->BindAxis("LookUp", this, &AP1Player::AddPitchRotation);
	PlayerInputComponent->BindAxis("Block", this, &AP1Player::DoBlock);
}

void AP1Player::BeginOverlapEvent_Implementation(UPrimitiveComponent* HitComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
	if (!HasAuthority()) return;

	FString str = FString::Printf(TEXT("HitComp : %s, OtherActor : %s"), *HitComp->GetOwner()->GetName(), *OtherActor->GetName());
	GEngine->AddOnScreenDebugMessage(-1, 3.0f, FColor::Red, str);
}

void AP1Player::AddPitchRotation(float value)
{
	if (value)
	{
		float pitch = SpringArm->GetRelativeRotation().Pitch - value;
		if (pitch < 0.0f && pitch > -65.0f)
		{
			SpringArm->AddLocalRotation(FRotator(-value, 0, 0));
		}
	}
}

void AP1Player::AddYawRotation(float value)
{
	if (value)
	{
		AddControllerYawInput(value);
		AddYawRotationByServer(value);
	}
}

void AP1Player::AddYawRotationByServer_Implementation(float value)
{
	AddControllerYawInput(value);
}