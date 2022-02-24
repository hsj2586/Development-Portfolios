// Fill out your copyright notice in the Description page of Project Settings.

#include "MyCharacterStatComponent.h"
#include "MyGameInstance.h"

// Sets default values for this component's properties
UMyCharacterStatComponent::UMyCharacterStatComponent()
{
	PrimaryComponentTick.bCanEverTick = false;
	bWantsInitializeComponent = true;
	Level = 1;
}


// Called when the game starts
void UMyCharacterStatComponent::BeginPlay()
{
	Super::BeginPlay();
}

void UMyCharacterStatComponent::InitializeComponent()
{
	Super::InitializeComponent();
	SetNewData(Level);
}


// Called every frame
void UMyCharacterStatComponent::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	// ...
}

void UMyCharacterStatComponent::SetNewData(int NewLevel)
{
	auto GameInstance = Cast<UMyGameInstance>(UGameplayStatics::GetGameInstance(GetWorld()));
	if (GameInstance)
	{
		CurrentStatData = GameInstance->GetCharacterData(NewLevel);
	}

	if (CurrentStatData != nullptr)
	{
		Level = NewLevel;
		SetHP(CurrentStatData->Health);
		SetMana(CurrentStatData->Mana);
	}
}

void UMyCharacterStatComponent::SetDamage(float NewDamage)
{
	SetHP(FMath::Clamp<float>(CurrentHP - NewDamage, 0.0f, CurrentStatData->Health));
}

void UMyCharacterStatComponent::ConsumeMana(float NewMana)
{
	SetMana(FMath::Clamp<float>(CurrentMana - NewMana, 0.0f, CurrentStatData->Mana));
}

float UMyCharacterStatComponent::GetHP()
{
	return CurrentHP;
}

float UMyCharacterStatComponent::GetMana()
{
	return CurrentMana;
}

struct FCharacterData* UMyCharacterStatComponent::GetData()
{
	return CurrentStatData;
}

void UMyCharacterStatComponent::SetHP(float NewHP)
{
	CurrentHP = NewHP;
	OnHPChanged.Broadcast();
	if (CurrentHP < KINDA_SMALL_NUMBER)
	{
		CurrentHP = 0.0f;
		OnHPIsZero.Broadcast();
	}
}

void UMyCharacterStatComponent::SetMana(float NewMana)
{
	CurrentMana = NewMana;
	OnManaChanged.Broadcast();
	if (CurrentMana < KINDA_SMALL_NUMBER)
	{
		CurrentMana = 0.0f;
	}
}

bool UMyCharacterStatComponent::CheckManaIsZero(float RequiredMana)
{
	return (CurrentMana - RequiredMana) >= 0.0f ? false : true;
}

float UMyCharacterStatComponent::GetHPRatio() const
{
	return (CurrentHP < KINDA_SMALL_NUMBER) ? 
		0.0f : (CurrentHP / CurrentStatData->Health);
}

float UMyCharacterStatComponent::GetManaRatio() const
{
	return (CurrentMana < KINDA_SMALL_NUMBER) ?
		0.0f : (CurrentMana / CurrentStatData->Mana);
}
