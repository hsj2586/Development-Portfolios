// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Skill.h"
#include "VisionShot.generated.h"

DECLARE_MULTICAST_DELEGATE(FOnPressDelegate);

UCLASS()
class BLUEHOLE_PROJECT_API UVisionShot : public USkill
{
	GENERATED_BODY()

public:
	UVisionShot();

	// 가상 초기화 함수
	virtual void Init(UInputComponent * PlayerInput, int InputIndex, struct FSkillData* LoadedSkillData) override;

	// 가상 틱 컴포넌트
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	// 가상 스킬발동 함수
	virtual void DoSkill() override;

	// 버튼 누를시 비전 폭발 기능 함수
	void VisionExplosion();

	// 발사체가 파괴되었을 때(수동, 자동 모두 해당), 호출되는 함수
	void OnProjectileDestroyed();

	// 발사체 클래스
	UPROPERTY(EditDefaultsOnly, Category = Projectile)
		TSubclassOf<class AVisionShotProjectile> ProjectileClass;

	// 폭발 파티클 클래스
	UPROPERTY(EditDefaultsOnly, Category = Projectile)
		TSubclassOf<class AExplosionParticle> ExplosionClass;

	// 총구 위치
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Gameplay)
		FVector MuzzleOffset;

	// 스킬 버튼을 한번 더 눌렀을 시 델리게이트
	FOnPressDelegate OnPress;

	// 발사체 생성 후 임시 저장변수
	AVisionShotProjectile* VisionShotProjectile;
};
