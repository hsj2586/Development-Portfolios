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

	// ���� �ʱ�ȭ �Լ�
	virtual void Init(UInputComponent * PlayerInput, int InputIndex, struct FSkillData* LoadedSkillData) override;

	// ���� ƽ ������Ʈ
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	// ���� ��ų�ߵ� �Լ�
	virtual void DoSkill() override;

	// ��ư ������ ���� ���� ��� �Լ�
	void VisionExplosion();

	// �߻�ü�� �ı��Ǿ��� ��(����, �ڵ� ��� �ش�), ȣ��Ǵ� �Լ�
	void OnProjectileDestroyed();

	// �߻�ü Ŭ����
	UPROPERTY(EditDefaultsOnly, Category = Projectile)
		TSubclassOf<class AVisionShotProjectile> ProjectileClass;

	// ���� ��ƼŬ Ŭ����
	UPROPERTY(EditDefaultsOnly, Category = Projectile)
		TSubclassOf<class AExplosionParticle> ExplosionClass;

	// �ѱ� ��ġ
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Gameplay)
		FVector MuzzleOffset;

	// ��ų ��ư�� �ѹ� �� ������ �� ��������Ʈ
	FOnPressDelegate OnPress;

	// �߻�ü ���� �� �ӽ� ���庯��
	AVisionShotProjectile* VisionShotProjectile;
};
