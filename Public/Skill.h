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

	// 플레이어 클래스 (스킬의 사용 주체를 알기 위한 변수)
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		AMyCharacter* Player;

	// 애님 인스턴스
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		UMyAnimInstance* Anim;

	// 플레이어 인풋 컴포넌트 (스킬 입력 바인딩을 위한 변수)
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		UInputComponent* InputComponent;

	// 스킬 이름
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		FString SkillName;

	// 값 (스킬에 따라 다양한 용도로 사용할 예정.)
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		float Value;

	// 쿨타임
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		float CoolTime;

	// 소모 마나
	UPROPERTY(Category = Skill, BlueprintReadOnly)
		float Mana;

	// 남은 쿨타임
	float RemainCooltime = 0;

	// 스킬 슬롯 인덱스
	int SkillSlotIndex;

public:
	// 가상 초기화 함수
	virtual void Init(UInputComponent* PlayerInput, int InputIndex, struct FSkillData* LoadedSkillData = nullptr);

	// 가상 틱 컴포넌트
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	// 가상 스킬 발동 함수
	virtual void DoSkill();

	// 연계 스킬 사용 가능 여부 확인 및 세팅하는 함수
	void CheckLinkageSkill();

	// 연계 스킬 발동 함수
	void DoLinkageSkill();

	// 연계 스킬 자동 지연 종료 함수
	void LinkageSkillEnd();

	// 스킬 이름 Getter
	FString GetSkillName();

	// 남은 쿨타임 Setter
	void SetRemainCooltime(float NewCooltime);

	// 남은 쿨타임 Getter
	float GetRemainCooltime();

	// 쿨타임 Ratio Getter
	float GetCooltimeRatio();

	// 쿨타임 카운팅 알림 델리게이트 변수
	FOnCooltimeChangedDelegate OnCooltimeChanged;

	// 쿨타임이 0인 경우 델리게이트 변수
	FOnCooltimeIsZeroDelegate OnCooltimeIsZero;

	// 다음 연계 스킬 이름 변수
	int LinkageSkillIndex;

private:
	// 연계 스킬을 사용했는지 여부
	bool IsLinkageSkillEnded;
};