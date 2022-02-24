// Fill out your copyright notice in the Description page of Project Settings.

#include "MyAnimInstance.h"
#include "MyCharacter.h"
#include "GameFramework/Character.h"
#include "GameFramework/PawnMovementComponent.h"
#include "MyCharacterStatComponent.h"

UMyAnimInstance::UMyAnimInstance()
{
	CurrentPawnSpeed = 0.0f;
	IsInAir = false;
	IsDead = false;
	IsRolling = false;
	IsUsingShotLaunch = false;
	IsAttacking = false;
	IsUsingVisionShot = false;
};

void UMyAnimInstance::SetAnimMontage(UAnimMontage* AnimMontage)
{
	// 어택 애님 몽타주 세팅
	if (AnimMontage)
	{
		AttackMontage = AnimMontage;
	}

	// 스킬 애님 몽타주 세팅
	UAnimMontage* NewAnimMontage = Cast<UAnimMontage>(StaticLoadObject(UAnimMontage::StaticClass(), NULL,
		TEXT("/Game/Animation/MyCharacterSkillAnimMontage.MyCharacterSkillAnimMontage")));
	if (NewAnimMontage)
	{
		SkillMontage = NewAnimMontage;
	}
}

void UMyAnimInstance::NativeUpdateAnimation(float DeltaSeconds)
{
	Super::NativeUpdateAnimation(DeltaSeconds);
	auto Pawn = TryGetPawnOwner();
	if (!::IsValid(Pawn)) return;

	if (!IsDead)
	{
		CurrentPawnSpeed = Pawn->GetVelocity().Size();
		auto Character = Cast<ACharacter>(Pawn);
		if (Character)
		{
			IsInAir = Character->GetMovementComponent()->IsFalling();
		}
	}
}

void UMyAnimInstance::SetAttackAnim()
{
	IsAttacking = true;
}

void UMyAnimInstance::SetDeadAnim()
{
	IsDead = true;
}

void UMyAnimInstance::SetRollingAnim()
{
	IsRolling = true;
}

void UMyAnimInstance::SetShotLaunchAnim()
{
	IsUsingShotLaunch = true;
}

void UMyAnimInstance::SetVisionShotAnim()
{
	IsUsingVisionShot = true;
}

bool UMyAnimInstance::GetIsAttacking()
{
	return IsAttacking;
}

bool UMyAnimInstance::GetIsRolling()
{
	return IsRolling;
}

bool UMyAnimInstance::GetIsInAir()
{
	return IsInAir;
}

bool UMyAnimInstance::GetUsingShotLaunch()
{
	return IsUsingShotLaunch;
}

bool UMyAnimInstance::GetUsingVisionShot()
{
	return IsUsingVisionShot;
}

void UMyAnimInstance::PlayAttack()
{
	Montage_Play(AttackMontage);
}

void UMyAnimInstance::PlaySkill()
{
	Montage_Play(SkillMontage);
}

void UMyAnimInstance::AnimNotify_AttackEnd()
{
	IsAttacking = false;
	AMyCharacter* Owner = Cast<AMyCharacter>(GetOwningActor());
	Owner->bUseControllerRotationYaw = false;
}

void UMyAnimInstance::AnimNotify_RollingEnd()
{
	IsRolling = false;
	AMyCharacter* Owner = Cast<AMyCharacter>(GetOwningActor());
	Owner->bCanBeDamaged = true;
	Owner->GetCharacterMovement()->MaxWalkSpeed = Owner->CharacterStat->GetData()->MoveSpeed;
}

void UMyAnimInstance::AnimNotify_ShotLaunchEnd()
{
	IsUsingShotLaunch = false;
	AMyCharacter* Owner = Cast<AMyCharacter>(GetOwningActor());
	Owner->GetCharacterMovement()->MaxWalkSpeed = Owner->CharacterStat->GetData()->MoveSpeed;
	Owner->GetCharacterMovement()->bOrientRotationToMovement = true;
	Owner->EnableInput(Cast<APlayerController>(Owner->GetController()));
}

void UMyAnimInstance::AnimNotify_VisionShotEnd()
{
	IsUsingVisionShot = false;
	AMyCharacter* Owner = Cast<AMyCharacter>(GetOwningActor());
	Owner->bUseControllerRotationYaw = false;
	StopAllMontages(0.1f);
}

bool UMyAnimInstance::CancelAnimation(SkillType Type)
{
	switch (Type)
	{
	case SkillType::Attack:
	{
		// Attack의 경우 애니메이션 캔슬
		if (GetIsAttacking()) return false;
		if (GetIsRolling()) return false;
		if (GetIsInAir()) return false;
		if (GetUsingShotLaunch()) return false;
	}
	break;
	default:
	{
		// Attack외의 경우 애니메이션 캔슬;
		if (GetIsRolling()) return false;
		if (GetIsInAir()) return false;
		if (GetUsingShotLaunch()) return false;
	}
	break;
	}

	AnimNotify_AttackEnd();
	AnimNotify_RollingEnd();
	AnimNotify_ShotLaunchEnd();
	StopAllMontages(0.05f);
	return true;
}
