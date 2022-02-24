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
	// �޺��� �ʱ�ȭ �ϴ� �Լ�
	void Init();

	// �޺��� ī��Ʈ �ϴ� �Լ�
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
