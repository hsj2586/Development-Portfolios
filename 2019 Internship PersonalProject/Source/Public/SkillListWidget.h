// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Blueprint/UserWidget.h"
#include "SkillListWidget.generated.h"

/**
 * 
 */
UCLASS()
class BLUEHOLE_PROJECT_API USkillListWidget : public UUserWidget
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

	class UHUDWidget* HUDWidget;

	class USkillLinkageWidget* SkillLinkageWidget;

	// 드래깅 중인 스킬의 데이터를 반환하는 함수
	struct FSkillData* GetDraggingSkillData();

	// 드래깅 중인 스킬의 이미지를 반환하는 함수
	UObject * GetDraggingSkillImage();

protected:
	virtual void NativeConstruct() override;
	
private:
	class AMyPlayerController* PlayerController;

	// 스킬 슬롯 이미지 리스트
	TArray<class UButton*> SkillSlotList;
};
