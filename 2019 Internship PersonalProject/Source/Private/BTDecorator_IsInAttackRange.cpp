// Fill out your copyright notice in the Description page of Project Settings.

#include "BTDecorator_IsInAttackRange.h"
#include "AIController_Monster.h"
#include "MyCharacter.h"
#include "Monster.h"
#include "MonsterStatComponent.h"
#include "BehaviorTree/BlackboardComponent.h"

UBTDecorator_IsInAttackRange::UBTDecorator_IsInAttackRange()
{
	NodeName = TEXT("CanAttack");
}

bool UBTDecorator_IsInAttackRange::CalculateRawConditionValue(UBehaviorTreeComponent & OwnerComp, uint8 * NodeMemory) const
{
	bool bResult = Super::CalculateRawConditionValue(OwnerComp, NodeMemory);

	auto ControllingPawn = Cast<AMonster>(OwnerComp.GetAIOwner()->GetPawn());
	if (ControllingPawn == nullptr)
		return false;

	auto Target = Cast<AMyCharacter>(OwnerComp.GetBlackboardComponent()->GetValueAsObject(
		AAIController_Monster::TargetKey));
	if (Target == nullptr)
		return false;

	auto AttackRange = ControllingPawn->CharacterStat->GetData()->AttackRange;
	bResult = (Target->GetDistanceTo(ControllingPawn) <= AttackRange);
	return bResult;
}
