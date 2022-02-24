// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Skill.h"
#include "Rolling.generated.h"

// 구르는 방향
UENUM(BlueprintType)
enum class Direction : uint8
{
	Forward,
	ForwardRight,
	Right,
	BackRight,
	Back,
	BackLeft,
	Left,
	ForwardLeft,
	Neutral,
};

UCLASS()
class BLUEHOLE_PROJECT_API URolling : public USkill
{
	GENERATED_BODY()
	
public:
	// 가상 초기화 함수
	virtual void Init(UInputComponent * PlayerInput, int InputIndex, struct FSkillData* LoadedSkillData) override;

	// 가상 틱 컴포넌트
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	// 가상 스킬발동 함수
	virtual void DoSkill() override;

	// 방향 판별 함수
	void GetDirection();

private:
	Direction RollingDirection;
};