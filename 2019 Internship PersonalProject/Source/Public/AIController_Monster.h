// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "AIController.h"
#include "AIController_Monster.generated.h"

DECLARE_MULTICAST_DELEGATE(FOnAttackEndDelegate);

UCLASS()
class BLUEHOLE_PROJECT_API AAIController_Monster : public AAIController
{
	GENERATED_BODY()

public:
	AAIController_Monster();
	virtual void Possess(APawn* InPawn) override;
	virtual void UnPossess() override;

	static const FName HomePosKey;
	static const FName PatrolPosKey;
	static const FName TargetKey;

	void RunAI();
	void StopAI();

private:
	bool IsAttacking = false;

	UPROPERTY()
		class UBehaviorTree* BTAsset;

	UPROPERTY()
		class UBlackboardData* BBAsset;
};
