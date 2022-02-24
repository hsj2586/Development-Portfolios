// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Animation/AnimInstance.h"
#include "MyAnimInstance.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API UMyAnimInstance : public UAnimInstance
{
	GENERATED_BODY()

public:
	UMyAnimInstance();
	void SetAnimMontage(UAnimMontage* AnimMontage);
	virtual void NativeUpdateAnimation(float DeltaSeconds) override;

	void SetAttackAnim();
	void SetRollingAnim();
	void SetShotLaunchAnim();
	void SetVisionShotAnim();
	void SetDeadAnim();

	bool GetIsAttacking();
	bool GetIsRolling();
	bool GetIsInAir();
	bool GetUsingShotLaunch();
	bool GetUsingVisionShot();

	void PlayAttack();
	void PlaySkill();

	// Attack �ִϸ��̼��� �� ����
	UFUNCTION()
		void AnimNotify_AttackEnd();

	// Rolling �ִϸ��̼��� �� ����
	UFUNCTION()
		void AnimNotify_RollingEnd();

	// ShotLaunch �ִϸ��̼��� �� ����
	UFUNCTION()
		void AnimNotify_ShotLaunchEnd();

	// VisionShot �ִϸ��̼��� �� ����
	UFUNCTION()
		void AnimNotify_VisionShotEnd();

	// �ִϸ��̼� �ൿ ���� �Լ�
	bool CancelAnimation(SkillType Type);

private:
	// ���� �̵� �ӵ�
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		float CurrentPawnSpeed;

	// ���������� ����
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		bool IsAttacking;

	// ���߿� ���ִ��� ����
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		bool IsInAir;

	// ������ ��ų ��������� ����
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		bool IsRolling;

	// �׾����� ����
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		bool IsDead;

	// ShotLaunch�� ��������� ����
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		bool IsUsingShotLaunch;
	
	// VisionShot�� ��������� ����
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		bool IsUsingVisionShot;

	UPROPERTY(VisibleDefaultsOnly, BlueprintReadOnly, Category = Attack, Meta = (AllowPrivateAccess = true))
		UAnimMontage* AttackMontage;

	UPROPERTY(VisibleDefaultsOnly, BlueprintReadOnly, Category = Attack, Meta = (AllowPrivateAccess = true))
		UAnimMontage* SkillMontage;
};
