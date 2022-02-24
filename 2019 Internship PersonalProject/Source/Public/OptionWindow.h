// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Blueprint/UserWidget.h"
#include "OptionWindow.generated.h"

/**
 * 
 */
UCLASS()
class BLUEHOLE_PROJECT_API UOptionWindow : public UUserWidget
{
	GENERATED_BODY()

public:
	virtual void NativeConstruct() override;

	// �Է��� �޴� �Լ�
	UFUNCTION()
		void ReceiveInput();

	// Ű���� �Է� �Լ�
	UFUNCTION()
		FEventReply OnKeyDown(FGeometry MyGeometry, FKeyEvent InKeyEvent);

	// ���콺 �Է� �Լ�
	UFUNCTION()
		FEventReply OnMouseButtonDown(FGeometry MyGeometry, const FPointerEvent& MouseEvent);
	
private:
	// InputSetting ��ư ����Ʈ
	UPROPERTY()
		TArray<class UButton*> InputSettingButtonList;

	// InputSetting ��ư �ؽ�Ʈ ����Ʈ
	UPROPERTY()
		TArray<class UTextBlock*> InputSettingButtonTextList;

	// Input ��� �������� ����
	bool IsWaitingInput;

	// ��ư ���� �ε���
	int ButtonIndex;

	// �Էµ� Ű ���� ����
	UPROPERTY()
		FKey Input;

	UPlayerInput* PlayerInput;
};
