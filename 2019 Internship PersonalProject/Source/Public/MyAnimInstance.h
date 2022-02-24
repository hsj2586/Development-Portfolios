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

	// Attack 애니메이션의 끝 설정
	UFUNCTION()
		void AnimNotify_AttackEnd();

	// Rolling 애니메이션의 끝 설정
	UFUNCTION()
		void AnimNotify_RollingEnd();

	// ShotLaunch 애니메이션의 끝 설정
	UFUNCTION()
		void AnimNotify_ShotLaunchEnd();

	// VisionShot 애니메이션의 끝 설정
	UFUNCTION()
		void AnimNotify_VisionShotEnd();

	// 애니메이션 행동 결정 함수
	bool CancelAnimation(SkillType Type);

private:
	// 폰의 이동 속도
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		float CurrentPawnSpeed;

	// 공격중인지 여부
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		bool IsAttacking;

	// 공중에 떠있는지 여부
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		bool IsInAir;

	// 구르기 스킬 사용중인지 여부
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		bool IsRolling;

	// 죽었는지 여부
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		bool IsDead;

	// ShotLaunch를 사용중인지 여부
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		bool IsUsingShotLaunch;
	
	// VisionShot을 사용중인지 여부
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Pawn, Meta = (AllowPrivateAccess = true))
		bool IsUsingVisionShot;

	UPROPERTY(VisibleDefaultsOnly, BlueprintReadOnly, Category = Attack, Meta = (AllowPrivateAccess = true))
		UAnimMontage* AttackMontage;

	UPROPERTY(VisibleDefaultsOnly, BlueprintReadOnly, Category = Attack, Meta = (AllowPrivateAccess = true))
		UAnimMontage* SkillMontage;
};
