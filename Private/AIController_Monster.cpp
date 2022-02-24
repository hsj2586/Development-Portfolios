// Fill out your copyright notice in the Description page of Project Settings.

#include "AIController_Monster.h"
#include "BehaviorTree/BehaviorTree.h"
#include "BehaviorTree/BlackboardData.h"
#include "BehaviorTree/BlackboardComponent.h"

const FName AAIController_Monster::HomePosKey(TEXT("HomePos"));
const FName AAIController_Monster::PatrolPosKey(TEXT("PatrolPos"));
const FName AAIController_Monster::TargetKey(TEXT("Target"));

AAIController_Monster::AAIController_Monster()
{
	static ConstructorHelpers::FObjectFinder<UBlackboardData> BBObject(
		TEXT("/Game/AI/BB_Monster.BB_Monster"));
	if (BBObject.Succeeded())
	{
		BBAsset = BBObject.Object;
	}

	static ConstructorHelpers::FObjectFinder<UBehaviorTree> BTObject(
		TEXT("/Game/AI/BT_Monster.BT_Monster"));
	if (BTObject.Succeeded())
	{
		BTAsset = BTObject.Object;
	}
}

void AAIController_Monster::Possess(APawn * InPawn)
{
	Super::Possess(InPawn);
}

void AAIController_Monster::UnPossess()
{
	Super::UnPossess();
}


void AAIController_Monster::RunAI()
{
	if (UseBlackboard(BBAsset, Blackboard))
	{
		Blackboard->SetValueAsVector(HomePosKey, GetPawn()->GetActorLocation());
		if (!RunBehaviorTree(BTAsset))
		{
			UE_LOG(LogTemp, Warning, TEXT("AIController couldn't run behavior tree!"));
		}
	}
}

void AAIController_Monster::StopAI()
{
	auto BehaviorTreeComponent = Cast<UBehaviorTreeComponent>(BrainComponent);
	if (BehaviorTreeComponent != nullptr)
	{
		BehaviorTreeComponent->StopTree(EBTStopMode::Safe);
	}
}