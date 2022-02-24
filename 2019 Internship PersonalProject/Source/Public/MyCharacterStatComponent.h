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

	// ������ ���� �� �����͸� �����ϴ� �Լ�
	void SetNewData(int NewLevel);

	// �������� �޾��� �� ����� HP�� �����ϴ� �Լ�
	void SetDamage(float NewDamage);
	
	// ������ �Һ����� �� ����� ������ �����ϴ� �Լ�
	void ConsumeMana(float NewMana);

	// CurrentHP ���� ��ȯ�ϴ� �Լ�
	float GetHP();

	// CurrentMana ���� ��ȯ�ϴ� �Լ�
	float GetMana();

	// �� HP���� �����ϴ� �Լ�
	void SetHP(float NewHP);

	// �� Mana���� �����ϴ� �Լ�
	void SetMana(float NewMana);

	// Mana���� 0���� ��ȯ�ϴ� �Լ�
	bool CheckManaIsZero(float RequiredMana);

	// HP ������ ��ȯ�ϴ� �Լ�
	float GetHPRatio() const;

	// Mana ������ ��ȯ�ϴ� �Լ�
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
