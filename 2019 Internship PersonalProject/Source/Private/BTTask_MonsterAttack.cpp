// Fill out your copyright notice in the Description page of Project Settings.

#include "BTTask_MonsterAttack.h"
#include "AIController_Monster.h"
#include "Monster.h"
#include "MyCharacter.h"

UBTTask_MonsterAttack::UBTTask_MonsterAttack()
{
	bNotifyTick = true;
	IsAttacking = false;
}

EBTNodeResult::Type UBTTask_MonsterAttack::ExecuteTask(UBehaviorTreeComponent & OwnerComp, uint8 * NodeMemory)
{
	EBTNodeResult::Type Result = Super::ExecuteTask(OwnerComp, NodeMemory);

	auto Owner = Cast<AMonster>(OwnerComp.GetAIOwner()->GetPawn());
	if (Owner == nullptr)
		return EBTNodeResult::Failed;

	Owner->Attack();

	IsAttacking = true;
	Owner->OnAttackEnd.AddLambda([this]()->void {IsAttacking = false; });

	return EBTNodeResult::InProgress;
}

void UBTTask_MonsterAttack::TickTask(UBehaviorTreeComponent & OwnerComp, uint8 * NodeMemory, float DeltaSeconds)
{
	Super::TickTask(OwnerComp, NodeMemory, DeltaSeconds);
	if (!IsAttacking)
	{
		FinishLatentTask(OwnerComp, EBTNodeResult::Succeeded);
	}
}
