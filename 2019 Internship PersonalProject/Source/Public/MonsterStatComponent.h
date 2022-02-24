// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Components/ActorComponent.h"
#include "MonsterStatComponent.generated.h"

DECLARE_MULTICAST_DELEGATE(FOnHPIsZeroDelegate);
DECLARE_MULTICAST_DELEGATE(FOnHPChangedDelegate);

UCLASS( ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class BLUEHOLE_PROJECT_API UMonsterStatComponent : public UActorComponent
{
	GENERATED_BODY()

public:
	// Sets default values for this component's properties
	UMonsterStatComponent();

public:
	struct FMonsterData* GetData();
	void SetNewData(FName MonsterName);
	void SetDamage(float NewDamage);
	void SetHP(float NewHP);
	float GetHP();
	float GetHPRatio() const;
	int GetDropExp() const;

	FOnHPIsZeroDelegate OnHPIsZero;
	FOnHPChangedDelegate OnHPChanged;

private:
	struct FMonsterData* CurrentStatData = nullptr;

	UPROPERTY(Transient, VisibleInstanceOnly, Category = Stat, Meta = (AllowPrivateAccess = true))
		float CurrentHP;
};
