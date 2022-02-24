// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "SkillController.generated.h"


UCLASS(ClassGroup = (Custom), meta = (BlueprintSpawnableComponent))
class BLUEHOLE_PROJECT_API USkillController : public UActorComponent
{
	GENERATED_BODY()

public:
	USkillController();

public:
	// 스킬 컨트롤러 초기화
	void Init();

	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	// 스킬 클래스 팩토리
	class USkill* GetSkillClass(int SkillNameIndex, FName SkillName_);

	// 스킬 리스트
	UPROPERTY(EditAnywhere, Category = Skill)
		TArray<class USkill*> SkillList;

	// 스킬 연계 인덱스를 갱신하는 함수
	void UpdateSkillList();

private:
	// AnimInstance
	class UMyAnimInstance* Anim;

	// 플레이어 컨트롤러
	class AMyPlayerController* Controller;

	// SkillLinkageWidget
	class USkillLinkageWidget* SkillLinkageWidget;
};
