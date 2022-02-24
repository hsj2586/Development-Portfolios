// Fill out your copyright notice in the Description page of Project Settings.

#include "BTService_Detect.h"
#include "AIController_Monster.h"
#include "MyCharacter.h"
#include "BehaviorTree/BlackboardComponent.h"
#include "MonsterStatComponent.h"
#include "Monster.h"
#include "DrawDebugHelpers.h"

UBTService_Detect::UBTService_Detect()
{
	NodeName = TEXT("Detect");
	Interval = 1.0f;
}

void UBTService_Detect::TickNode(UBehaviorTreeComponent & OwnerComp, uint8 * NodeMemory, float DeltaSeconds)
{
	Super::TickNode(OwnerComp, NodeMemory, DeltaSeconds);

	APawn* ControllingPawn = OwnerComp.GetAIOwner()->GetPawn();
	if (ControllingPawn == nullptr) return;

	UWorld* World = ControllingPawn->GetWorld();
	FVector Center = ControllingPawn->GetActorLocation();

	DetectRadius = Cast<UMyGameInstance>(World->GetGameInstance())->GetMonsterData(
		*Cast<AMonster>(ControllingPawn)->CharacterStat->GetData()->MName)->DetectRadius;

	if (World == nullptr) return;
	TArray<FOverlapResult> OverlapResults;
	FCollisionQueryParams CollisionQueryParam(NAME_None, false, ControllingPawn);
	bool bResult = World->OverlapMultiByChannel(
		OverlapResults,
		Center,
		FQuat::Identity,
		ECollisionChannel::ECC_GameTraceChannel1,
		FCollisionShape::MakeSphere(DetectRadius),
		CollisionQueryParam
	);

	if (bResult)
	{
		for (auto OverlapResult : OverlapResults)
		{
			AMyCharacter* Player = Cast<AMyCharacter>(OverlapResult.GetActor());
			if (Player && Player->GetController()->IsPlayerController())
			{
				OwnerComp.GetBlackboardComponent()->SetValueAsObject(AAIController_Monster::TargetKey, Player);
				return;
			}
		}
		OwnerComp.GetBlackboardComponent()->SetValueAsObject(AAIController_Monster::TargetKey, nullptr);
	}
}