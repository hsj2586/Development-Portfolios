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
	// ��ų �巡�� �� �Լ�
	UFUNCTION()
		void DraggingSkill();

	// �巡�� �ǰ� �ִ� ��ų �ε���
	UPROPERTY()
		int DraggingSkillIndex;

	// �巡�� ������ ����
	UPROPERTY()
		bool IsDragging;

	class UHUDWidget* HUDWidget;

	class USkillLinkageWidget* SkillLinkageWidget;

	// �巡�� ���� ��ų�� �����͸� ��ȯ�ϴ� �Լ�
	struct FSkillData* GetDraggingSkillData();

	// �巡�� ���� ��ų�� �̹����� ��ȯ�ϴ� �Լ�
	UObject * GetDraggingSkillImage();

protected:
	virtual void NativeConstruct() override;
	
private:
	class AMyPlayerController* PlayerController;

	// ��ų ���� �̹��� ����Ʈ
	TArray<class UButton*> SkillSlotList;
};
