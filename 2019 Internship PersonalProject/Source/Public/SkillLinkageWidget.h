// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Blueprint/UserWidget.h"
#include "SkillLinkageWidget.generated.h"

/**
 * 
 */
UCLASS()
class BLUEHOLE_PROJECT_API USkillLinkageWidget : public UUserWidget
{
	GENERATED_BODY()

public:
	// 스킬 드래그 중 함수
	UFUNCTION()
		void DraggingSkill();

	// 드래그 되고 있는 스킬 인덱스
	UPROPERTY()
		int DraggingSkillIndex;

	// 드래그 중인지 여부
	UPROPERTY()
		bool IsDragging;

	// 스킬 리스트
	UPROPERTY(EditAnywhere, Category = Skill)
		TArray<class USkill*> SkillList;

	class UHUDWidget* HUDWidget;

	class USkillListWidget* SkillListWidget;

	// 스킬 이미지를 입력하는 함수
	void SetSkillImage(int SkillSlotIndex, UTexture2D* Image);

	// 드래깅 중인 스킬의 데이터를 반환하는 함수
	struct FSkillData* GetDraggingSkillData();

	// 드래깅 중인 스킬의 이미지를 반환하는 함수
	UObject * GetDraggingSkillImage();

	// 스킬 연계 창 내에서 스킬 변화가 일어나는 경우
	void ChangeSkillSelf(int TargetIndex);

protected:
	virtual void NativeConstruct() override;
	
private:
	class AMyPlayerController* PlayerController;

	// 스킬 슬롯 이미지 리스트
	TArray<class UButton*> SkillSlotList;

	// 스킬 컨트롤러
	UPROPERTY(EditAnywhere)
		class USkillController* SkillController;
};
