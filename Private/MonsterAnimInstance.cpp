// Fill out your copyright notice in the Description page of Project Settings.

#include "MonsterAnimInstance.h"
#include "BehaviorTree/BlackboardComponent.h"
#include "MyCharacter.h"
#include "AIController_Monster.h"

UMonsterAnimInstance::UMonsterAnimInstance()
{
	CurrentPawnSpeed = 0.0f;
}

void UMonsterAnimInstance::NativeUpdateAnimation(float DeltaSeconds)
{
	Super::NativeUpdateAnimation(DeltaSeconds);
	auto Pawn = TryGetPawnOwner();
	if (!::IsValid(Pawn)) return;

	if (!IsDead)
	{
		CurrentPawnSpeed = Pawn->GetVelocity().Size();
	}
}

void UMonsterAnimInstance::SetAnimMontage(UAnimMontage* AnimMontage)
{
	if (AnimMontage)
	{
		AttackMontage = AnimMontage;
	}
}

void UMonsterAnimInstance::PlayAttack()
{
	Montage_Play(AttackMontage, 1.3f);
}

void UMonsterAnimInstance::SetDeadAnim()
{
	IsDead = true;
}

void UMonsterAnimInstance::AnimNotify_GiveDamage()
{
	AAIController_Monster* Controller = Cast<AAIController_Monster>(GetOwningActor()->GetInstigatorController());
	auto Target = Cast<AMyCharacter>(Controller->GetBlackboardComponent()->GetValueAsObject(Controller->TargetKey));
	if (Target == nullptr)
		return;

	FDamageEvent DamageEvent;
	Target->TakeDamage(50.0f, DamageEvent, Controller, GetOwningActor());
}