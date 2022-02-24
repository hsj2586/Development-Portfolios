// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Components/ActorComponent.h"
#include "MyCharacterStatComponent.generated.h"

DECLARE_MULTICAST_DELEGATE(FOnHPIsZeroDelegate);
DECLARE_MULTICAST_DELEGATE(FOnHPChangedDelegate);
DECLARE_MULTICAST_DELEGATE(FOnManaChangedDelegate);

UCLASS( ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class BLUEHOLE_PROJECT_API UMyCharacterStatComponent : public UActorComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UMyCharacterStatComponent();

protected:
	// Called when the game starts
	virtual void BeginPlay() override;
	virtual void InitializeComponent() override;

public:	
	// Called every frame
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

public:
	struct FCharacterData* GetData();

	// 레벨에 따른 새 데이터를 적용하는 함수
	void SetNewData(int NewLevel);

	// 데미지를 받았을 때 계산해 HP를 적용하는 함수
	void SetDamage(float NewDamage);
	
	// 마나를 소비했을 때 계산해 마나를 적용하는 함수
	void ConsumeMana(float NewMana);

	// CurrentHP 값을 반환하는 함수
	float GetHP();

	// CurrentMana 값을 반환하는 함수
	float GetMana();

	// 새 HP값을 적용하는 함수
	void SetHP(float NewHP);

	// 새 Mana값을 적용하는 함수
	void SetMana(float NewMana);

	// Mana값이 0인지 반환하는 함수
	bool CheckManaIsZero(float RequiredMana);

	// HP 비율을 반환하는 함수
	float GetHPRatio() const;

	// Mana 비율을 반환하는 함수
	float GetManaRatio() const;

	FOnHPIsZeroDelegate OnHPIsZero;
	FOnHPChangedDelegate OnHPChanged;
	FOnManaChangedDelegate OnManaChanged;

private:
	struct FCharacterData* CurrentStatData = nullptr;

	UPROPERTY(EditInstanceOnly, Category = Stat, Meta = (AllowPrivateAccess = true))
		int32 Level;

	UPROPERTY(Transient, VisibleInstanceOnly, Category = Stat, Meta = (AllowPrivateAccess = true))
		float CurrentHP;

	UPROPERTY(Transient, VisibleInstanceOnly, Category = Stat, Meta = (AllowPrivateAccess = true))
		float CurrentMana;
};
