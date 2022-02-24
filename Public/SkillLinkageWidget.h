// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Blueprint/UserWidget.h"
#include "SkillLinkageWidget.generated.h"

/**
 * 
 */
UCLASS()
class BLUEHOLE_PROJECT_API USkillLinkageWidget : public UUserWidget
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

	// ��ų ����Ʈ
	UPROPERTY(EditAnywhere, Category = Skill)
		TArray<class USkill*> SkillList;

	class UHUDWidget* HUDWidget;

	class USkillListWidget* SkillListWidget;

	// ��ų �̹����� �Է��ϴ� �Լ�
	void SetSkillImage(int SkillSlotIndex, UTexture2D* Image);

	// �巡�� ���� ��ų�� �����͸� ��ȯ�ϴ� �Լ�
	struct FSkillData* GetDraggingSkillData();

	// �巡�� ���� ��ų�� �̹����� ��ȯ�ϴ� �Լ�
	UObject * GetDraggingSkillImage();

	// ��ų ���� â ������ ��ų ��ȭ�� �Ͼ�� ���
	void ChangeSkillSelf(int TargetIndex);

protected:
	virtual void NativeConstruct() override;
	
private:
	class AMyPlayerController* PlayerController;

	// ��ų ���� �̹��� ����Ʈ
	TArray<class UButton*> SkillSlotList;

	// ��ų ��Ʈ�ѷ�
	UPROPERTY(EditAnywhere)
		class USkillController* SkillController;
};
