// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Skill.h"
#include "ShotLaunch.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API UShotLaunch : public USkill
{
	GENERATED_BODY()
	
public:
	UShotLaunch();

	// ���� �ʱ�ȭ �Լ�
	virtual void Init(UInputComponent * PlayerInput, int InputIndex, struct FSkillData* LoadedSkillData) override;

	// ���� ƽ ������Ʈ
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	// ���� ��ų�ߵ� �Լ�
	virtual void DoSkill() override;

	// ��ź �߻� ���� ��� �Լ�
	FVector GetFireDirection(FVector ForwardVector, float Degree);

	// ��ä�� ���� ���� ������ �������� �����ϴ� �Լ�
	void DamagingToEnemiesInRange();

	// ����ü Ŭ����
	UPROPERTY(EditDefaultsOnly, Category = Projectile)
		TSubclassOf<class AShotLaunchProjectile> ProjectileClass;

	// ���� ����Ʈ Ŭ����
	UPROPERTY(EditDefaultsOnly, Category = Projectile)
		TSubclassOf<class AShotLaunchMuzzle> MuzzleClass;
	
	// �ѱ� ��ġ
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Gameplay)
		FVector MuzzleOffset;
};
