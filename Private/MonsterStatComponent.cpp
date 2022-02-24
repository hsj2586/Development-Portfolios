// Fill out your copyright notice in the Description page of Project Settings.

#include "MonsterStatComponent.h"
#include "MyGameInstance.h"

UMonsterStatComponent::UMonsterStatComponent()
{
	PrimaryComponentTick.bCanEverTick = false;
	bWantsInitializeComponent = true;
}

FMonsterData* UMonsterStatComponent::GetData()
{
	return CurrentStatData;
}

void UMonsterStatComponent::SetNewData(FName MonsterName)
{
	auto GameInstance = Cast<UMyGameInstance>(UGameplayStatics::GetGameInstance(GetWorld()));
	CurrentStatData = GameInstance->GetMonsterData(MonsterName);

	if (CurrentStatData != nullptr)
	{
		SetHP(CurrentStatData->HealthPoint);
	}
}

void UMonsterStatComponent::SetDamage(float NewDamage)
{
	SetHP(FMath::Clamp<float>(CurrentHP - NewDamage, 0.0f, CurrentStatData->HealthPoint));
}

float UMonsterStatComponent::GetHP()
{
	return CurrentHP;
}

void UMonsterStatComponent::SetHP(float NewHP)
{
	CurrentHP = NewHP;
	OnHPChanged.Broadcast();
	if (CurrentHP < KINDA_SMALL_NUMBER)
	{
		CurrentHP = 0.0f;
		OnHPIsZero.Broadcast();
	}
}

int UMonsterStatComponent::GetDropExp() const
{
	return CurrentStatData->dropExp;
}

float UMonsterStatComponent::GetHPRatio() const
{
	return (CurrentStatData->HealthPoint < KINDA_SMALL_NUMBER) ?
		0.0f : (CurrentHP / CurrentStatData->HealthPoint);
}