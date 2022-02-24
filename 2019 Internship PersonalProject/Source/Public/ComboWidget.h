// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Blueprint/UserWidget.h"
#include "ComboWidget.generated.h"

/**
 *
 */
UCLASS()
class BLUEHOLE_PROJECT_API UComboWidget : public UUserWidget
{
	GENERATED_BODY()

public:
	// 콤보를 초기화 하는 함수
	void Init();

	// 콤보를 카운트 하는 함수
	void CountCombo();

protected:
	virtual void NativeTick(const FGeometry& MyGeometry, float InDeltaTime) override;
	virtual void NativeConstruct() override;

private:
	class UTextBlock* ComboCountText;
	float ElapsTime;
	int ComboCounting;
	bool IsVisible;
};
