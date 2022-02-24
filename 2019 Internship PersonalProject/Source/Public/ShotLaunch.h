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

	// 가상 초기화 함수
	virtual void Init(UInputComponent * PlayerInput, int InputIndex, struct FSkillData* LoadedSkillData) override;

	// 가상 틱 컴포넌트
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	// 가상 스킬발동 함수
	virtual void DoSkill() override;

	// 산탄 발사 방향 계산 함수
	FVector GetFireDirection(FVector ForwardVector, float Degree);

	// 부채꼴 범위 내의 적에게 데미지를 적용하는 함수
	void DamagingToEnemiesInRange();

	// 투사체 클래스
	UPROPERTY(EditDefaultsOnly, Category = Projectile)
		TSubclassOf<class AShotLaunchProjectile> ProjectileClass;

	// 폭발 이펙트 클래스
	UPROPERTY(EditDefaultsOnly, Category = Projectile)
		TSubclassOf<class AShotLaunchMuzzle> MuzzleClass;
	
	// 총구 위치
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Gameplay)
		FVector MuzzleOffset;
};
