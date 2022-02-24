// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Components/ActorComponent.h"
#include "MyGameInstance.h"
#include "MyAnimInstance.h"
#include "MyCharacter.h"
#include "MyCharacterStatComponent.h"
#include "Skill.generated.h"

DECLARE_MULTICAST_DELEGATE(FOnCooltimeChangedDelegate);
DECLARE_MULTICAST_DELEGATE(FOnCooltimeIsZeroDelegate);

UCLASS(ClassGroup = (Custom), meta = (BlueprintSpawnableComponent))
class BLUEHOLE_PROJECT_API USkill : public UActorComponent
{
	GENERATED_BODY()

public:
	USkill();

protected:

	// �÷��̾� Ŭ���� (��ų�� ��� ��ü�� �˱� ���� ����)
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		AMyCharacter* Player;

	// �ִ� �ν��Ͻ�
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		UMyAnimInstance* Anim;

	// �÷��̾� ��ǲ ������Ʈ (��ų �Է� ���ε��� ���� ����)
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		UInputComponent* InputComponent;

	// ��ų �̸�
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		FString SkillName;

	// �� (��ų�� ���� �پ��� �뵵�� ����� ����.)
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		float Value;

	// ��Ÿ��
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		float CoolTime;

	// �Ҹ� ����
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		float Mana;

	// ���� ��Ÿ��
	float RemainCooltime = 0;

	// ��ų ���� �ε���
	int SkillSlotIndex;

public:
	// ���� �ʱ�ȭ �Լ�
	virtual void Init(UInputComponent* PlayerInput, int InputIndex, struct FSkillData* LoadedSkillData = nullptr);

	// ���� ƽ ������Ʈ
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	// ���� ��ų �ߵ� �Լ�
	virtual void DoSkill();

	// ���� ��ų ��� ���� ���� Ȯ�� �� �����ϴ� �Լ�
	void CheckLinkageSkill();

	// ���� ��ų �ߵ� �Լ�
	void DoLinkageSkill();

	// ���� ��ų �ڵ� ���� ���� �Լ�
	void LinkageSkillEnd();

	// ��ų �̸� Getter
	FString GetSkillName();

	// ���� ��Ÿ�� Setter
	void SetRemainCooltime(float NewCooltime);

	// ���� ��Ÿ�� Getter
	float GetRemainCooltime();

	// ��Ÿ�� Ratio Getter
	float GetCooltimeRatio();

	// ��Ÿ�� ī���� �˸� ��������Ʈ ����
	FOnCooltimeChangedDelegate OnCooltimeChanged;

	// ��Ÿ���� 0�� ��� ��������Ʈ ����
	FOnCooltimeIsZeroDelegate OnCooltimeIsZero;

	// ���� ���� ��ų �̸� ����
	int LinkageSkillIndex;

private:
	// ���� ��ų�� ����ߴ��� ����
	bool IsLinkageSkillEnded;
};