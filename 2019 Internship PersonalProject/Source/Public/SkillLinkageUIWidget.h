// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Blueprint/UserWidget.h"
#include "SkillLinkageUIWidget.generated.h"

/**
 *
 */
UCLASS()
class BLUEHOLE_PROJECT_API USkillLinkageUIWidget : public UUserWidget
{
	GENERATED_BODY()

protected:
	virtual void NativeTick(const FGeometry& MyGeometry, float InDeltaTime) override;

public:
	virtual void NativeConstruct() override;

	void Init(UTexture2D* LinkageImage, FString LinkageInputText, FString SkillNameText);

	bool CheckComboTiming();

private:
	class UImage* LinkageImage;

	class UTextBlock* LinkageInputText;

	// 시각적인 효과를 위한 이미지 변수
	class UImage* EdgeImage;

	// 스킬 이름 표시 변수
	class UTextBlock* SkillNameText;

	// 스킬 게이지 변수
	class UProgressBar* LinkageGauge;

	float ElapsTime;
};