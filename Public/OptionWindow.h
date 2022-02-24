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

	// 입력을 받는 함수
	UFUNCTION()
		void ReceiveInput();

	// 키보드 입력 함수
	UFUNCTION()
		FEventReply OnKeyDown(FGeometry MyGeometry, FKeyEvent InKeyEvent);

	// 마우스 입력 함수
	UFUNCTION()
		FEventReply OnMouseButtonDown(FGeometry MyGeometry, const FPointerEvent& MouseEvent);
	
private:
	// InputSetting 버튼 리스트
	UPROPERTY()
		TArray<class UButton*> InputSettingButtonList;

	// InputSetting 버튼 텍스트 리스트
	UPROPERTY()
		TArray<class UTextBlock*> InputSettingButtonTextList;

	// Input 대기 상태인지 여부
	bool IsWaitingInput;

	// 버튼 저장 인덱스
	int ButtonIndex;

	// 입력된 키 저장 변수
	UPROPERTY()
		FKey Input;

	UPlayerInput* PlayerInput;
};
