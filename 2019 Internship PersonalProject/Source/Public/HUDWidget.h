// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Blueprint/UserWidget.h"
#include "HUDWidget.generated.h"

/**
 * 
 */
UCLASS()
class BLUEHOLE_PROJECT_API UHUDWidget : public UUserWidget
{
	GENERATED_BODY()
	
public:
	// 캐릭터 상태 바인딩 함수
	void BindCharacterStat(class UMyCharacterStatComponent* CharacterStat);

	// 플레이어 상태 바인딩 함수
	void BindPlayerState(class AMyPlayerState* PlayerState);

	// 스킬 상태 바인딩 함수
	void BindSkillState(TArray<class USkill*> SkillList);

	// 스킬 이미지를 적용하는 함수
	void SetSkillImage(int SkillSlotIndex, UTexture2D* Image);

	virtual void NativeConstruct() override;

	// 캐릭터 HP를 업데이트하는 델리게이트 함수
	void UpdateCharacterHP();

	// 캐릭터 Mana를 업데이트하는 델리게이트 함수
	void UpdateCharacterMana();

	// 플레이어 상태를 업데이트하는 델리게이트 함수
	void UpdatePlayerState();

	// 스킬 상태를 업데이트하는 델리게이트 함수
	void UpdateSkillState(USkill* Skill, int SlotIndex);

	// 스킬 쿨타임이 0인 경우 업데이트 델리게이트 함수
	void UpdateSkillCooltimeIsZero(int SlotIndex);

	// 옵션 창 켜기
	UFUNCTION()
		void SetActiveOptionWindow();

	// 스킬 리스트 창 켜기
	UFUNCTION()
		void SetActiveSkillList();

	// 스킬 연계 창 켜기
	UFUNCTION()
		void SetActiveSkillLinkage();

	// 스킬 드래그 중 함수
	UFUNCTION()
		void DraggingSkill();

	// 스킬 삽입 함수
	UFUNCTION()
		void PushSkill(int TargetIndex);

	// 스킬 슬롯 교체 함수
	UFUNCTION()
		void ChangeSkill(int TargetIndex, int DraggingIndex);

	// 스킬 리스트에서 스킬 이름 값으로 있는지 여부를 판정하는 함수
	UFUNCTION()
		int FindSkillByName(FString SkillName);

	// 드래그 되고 있는 스킬 인덱스
	UPROPERTY()
		int DraggingSkillIndex;

	// 드래그 중인지 여부
	UPROPERTY()
		bool IsDragging;

	// 스킬 슬롯 이미지 리스트
	TArray<class UButton*> SkillSlotList;

	class USkillLinkageWidget* SkillLinkageWidget;

	class USkillListWidget* SkillListWidget;

private:
	TWeakObjectPtr<class UMyCharacterStatComponent> CurrentCharacterStat;
	TWeakObjectPtr<class AMyPlayerState> CurrentPlayerState;

	UPROPERTY()
		class UInputBufferManager* InputBufferManager;

	UPROPERTY()
		class UProgressBar* HPBar;

	UPROPERTY()
		class UProgressBar* ManaBar;

	UPROPERTY()
		class UProgressBar* EXPBar;

	UPROPERTY()
		class UTextBlock* PlayerName;

	UPROPERTY()
		class UTextBlock* PlayerLevel;

	UPROPERTY()
		class UTextBlock* CurrentScore;

	UPROPERTY()
		class UTextBlock* HighScore;

	UPROPERTY()
		class UButton* OptionButtonUI;

	UPROPERTY()
		class UButton* SkillListButtonUI;

	UPROPERTY()
		class UButton* SkillLinkageButtonUI;

	// HP 텍스트
	UPROPERTY()
		class UTextBlock* HPText;

	// Mana 텍스트
	UPROPERTY()
		class UTextBlock* ManaText;

	// 스킬 쿨타임 시간 표시 텍스트 리스트
	UPROPERTY()
		TArray<class UTextBlock*> SkillSlotTextList;

	// 스킬 쿨타임 게이지 표시 리스트
	UPROPERTY()
		TArray<class UProgressBar*> SkillSlotGaugeList;

	// 플레이어 컨트롤러
	UPROPERTY()
		class AMyPlayerController* PlayerController;

	// 스킬 컨트롤러
	UPROPERTY(EditAnywhere)
		class USkillController* SkillController;

	// Player InputComponent
	UPROPERTY()
		class UInputComponent* PlayerInputComponent;

	// 옵션 위젯 활성화 여부
	UPROPERTY()
		bool IsActiveOptionWidget;

	// 스킬 리스트 위젯 활성화 여부
	UPROPERTY()
		bool IsActiveSkillListWidget;

	// 스킬 연계 위젯 활성화 여부
	UPROPERTY()
		bool IsActiveSkillLinkageWidget;
};