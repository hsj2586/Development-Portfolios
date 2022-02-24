// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Skill.h"
#include "Rolling.generated.h"

// ������ ����
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
	// ���� �ʱ�ȭ �Լ�
	virtual void Init(UInputComponent * PlayerInput, int InputIndex, struct FSkillData* LoadedSkillData) override;

	// ���� ƽ ������Ʈ
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	// ���� ��ų�ߵ� �Լ�
	virtual void DoSkill() override;

	// ���� �Ǻ� �Լ�
	void GetDirection();

private:
	Direction RollingDirection;
};