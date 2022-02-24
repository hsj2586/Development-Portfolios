// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Blueprint/UserWidget.h"
#include "SkillWidget.generated.h"

/**
 * 
 */
UCLASS()
class BLUEHOLE_PROJECT_API USkillWidget : public UUserWidget
{
	GENERATED_BODY()

public:
	// 스킬 이미지 세팅
	void SetSkillImage(UObject* Image);

protected:
	virtual void NativeConstruct() override;
	virtual void NativeTick(const FGeometry& MyGeometry, float InDeltaTime) override;

private:
	class AMyPlayerController* PlayerController;

	UPROPERTY()
		class UImage* SkillImage;
};
