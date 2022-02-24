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
	// ĳ���� ���� ���ε� �Լ�
	void BindCharacterStat(class UMyCharacterStatComponent* CharacterStat);

	// �÷��̾� ���� ���ε� �Լ�
	void BindPlayerState(class AMyPlayerState* PlayerState);

	// ��ų ���� ���ε� �Լ�
	void BindSkillState(TArray<class USkill*> SkillList);

	// ��ų �̹����� �����ϴ� �Լ�
	void SetSkillImage(int SkillSlotIndex, UTexture2D* Image);

	virtual void NativeConstruct() override;

	// ĳ���� HP�� ������Ʈ�ϴ� ��������Ʈ �Լ�
	void UpdateCharacterHP();

	// ĳ���� Mana�� ������Ʈ�ϴ� ��������Ʈ �Լ�
	void UpdateCharacterMana();

	// �÷��̾� ���¸� ������Ʈ�ϴ� ��������Ʈ �Լ�
	void UpdatePlayerState();

	// ��ų ���¸� ������Ʈ�ϴ� ��������Ʈ �Լ�
	void UpdateSkillState(USkill* Skill, int SlotIndex);

	// ��ų ��Ÿ���� 0�� ��� ������Ʈ ��������Ʈ �Լ�
	void UpdateSkillCooltimeIsZero(int SlotIndex);

	// �ɼ� â �ѱ�
	UFUNCTION()
		void SetActiveOptionWindow();

	// ��ų ����Ʈ â �ѱ�
	UFUNCTION()
		void SetActiveSkillList();

	// ��ų ���� â �ѱ�
	UFUNCTION()
		void SetActiveSkillLinkage();

	// ��ų �巡�� �� �Լ�
	UFUNCTION()
		void DraggingSkill();

	// ��ų ���� �Լ�
	UFUNCTION()
		void PushSkill(int TargetIndex);

	// ��ų ���� ��ü �Լ�
	UFUNCTION()
		void ChangeSkill(int TargetIndex, int DraggingIndex);

	// ��ų ����Ʈ���� ��ų �̸� ������ �ִ��� ���θ� �����ϴ� �Լ�
	UFUNCTION()
		int FindSkillByName(FString SkillName);

	// �巡�� �ǰ� �ִ� ��ų �ε���
	UPROPERTY()
		int DraggingSkillIndex;

	// �巡�� ������ ����
	UPROPERTY()
		bool IsDragging;

	// ��ų ���� �̹��� ����Ʈ
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

	// HP �ؽ�Ʈ
	UPROPERTY()
		class UTextBlock* HPText;

	// Mana �ؽ�Ʈ
	UPROPERTY()
		class UTextBlock* ManaText;

	// ��ų ��Ÿ�� �ð� ǥ�� �ؽ�Ʈ ����Ʈ
	UPROPERTY()
		TArray<class UTextBlock*> SkillSlotTextList;

	// ��ų ��Ÿ�� ������ ǥ�� ����Ʈ
	UPROPERTY()
		TArray<class UProgressBar*> SkillSlotGaugeList;

	// �÷��̾� ��Ʈ�ѷ�
	UPROPERTY()
		class AMyPlayerController* PlayerController;

	// ��ų ��Ʈ�ѷ�
	UPROPERTY(EditAnywhere)
		class USkillController* SkillController;

	// Player InputComponent
	UPROPERTY()
		class UInputComponent* PlayerInputComponent;

	// �ɼ� ���� Ȱ��ȭ ����
	UPROPERTY()
		bool IsActiveOptionWidget;

	// ��ų ����Ʈ ���� Ȱ��ȭ ����
	UPROPERTY()
		bool IsActiveSkillListWidget;

	// ��ų ���� ���� Ȱ��ȭ ����
	UPROPERTY()
		bool IsActiveSkillLinkageWidget;
};