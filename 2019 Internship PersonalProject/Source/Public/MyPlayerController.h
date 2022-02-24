// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/PlayerController.h"
#include "MyPlayerController.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API AMyPlayerController : public APlayerController
{
	GENERATED_BODY()

protected:
	virtual void BeginPlay() override;

	virtual void Tick(float DeltaTime) override;

	// HUD ���� Ŭ����
	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite, Category = UI)
		TSubclassOf<class UHUDWidget> HUDWidgetClass;

	// �ɼ� ���� Ŭ����
	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite, Category = UI)
		TSubclassOf<class UOptionWindow> OptionWidgetClass;

	// ��ų ���� Ŭ����
	UPROPERTY(VisibleAnywhere, Category = UI)
		TSubclassOf<class USkillWidget> SkillWidgetClass;

	// ��ų ����Ʈ ���� Ŭ����
	UPROPERTY(VisibleAnywhere, Category = UI)
		TSubclassOf<class USkillListWidget> SkillListWidgetClass;

	// ��ų ���� ���� Ŭ����
	UPROPERTY(VisibleAnywhere, Category = UI)
		TSubclassOf<class USkillLinkageWidget> SkillLinkageWidgetClass;

	// ��ų ���� UI ���� Ŭ����
	UPROPERTY(VisibleAnywhere, Category = UI)
		TSubclassOf<class USkillLinkageUIWidget> SkillLinkageUIWidgetClass;

	// �޺� ���� Ŭ����
	UPROPERTY(VisibleAnywhere, Category = UI)
		TSubclassOf<class UComboWidget> ComboWidgetClass;

	// �Է� ���� �Ŵ���
	UPROPERTY(VisibleAnywhere, Category = Stat)
		class UInputBufferManager* InputBufferManager;

public:
	AMyPlayerController();

	virtual void PostInitializeComponents() override;

	virtual void Possess(APawn* aPawn) override;

	virtual bool InputKey(FKey Key, EInputEvent EventType, float AmountDepressed, bool bGamepad) override;

	class UHUDWidget* GetHUDWidget();

	void SetHUDWidget();

	void NPCKill(class AMonster* KilledNPC) const;

	// OptionWidget ����
	void CreateOptionWidget();

	// OptionWidget �ı�
	void DestroyOptionWidget();

	// SkillWidget ����
	void CreateSkillWidget(UObject* SkillImage);

	// SkillWidget �ı�
	void DestroySkillWidget();

	// SkillListWidget ����
	void CreateSkillListWidget();

	// SkillListWidget �ı�
	void DestroySkillListWidget();

	// SkillLinkageWidget ����
	void CreateSkillLinkageWidget();

	// SkillLinkageWidget �ı�
	void DestroySkillLinkageWidget();

	// SkillLinkageUIWidget ����
	void CreateSkillLinkageUIWidget(UTexture2D* LinkageImage, FString LinkageInputText, FString SkillNameText);

	// SkillLinkageUIWidget �ı�
	void DestroySkillLinkageUIWidget();

	// ComboWidget Ȱ��ȭ
	void CreateComboWidget();

	// ��ų �޺� Ÿ�̹� ���� ��ȯ �Լ�
	bool IsComboTiming();

	// Ŀ�ǵ� �Է� ���� Ȯ�� �Լ�
	bool GetCheckCommand(SkillType CommandName);

	float ElapsTime;

private:
	// ���̽� UI â �ν��Ͻ�
	UPROPERTY()
		class UHUDWidget* HUDWidget;

	// �ɼ� â �ν��Ͻ�
	UPROPERTY()
		class UOptionWindow* OptionWindow;

	// ��ų ����Ʈ â �ν��Ͻ�
	UPROPERTY()
		class USkillListWidget* SkillListWindow;

	// ��ų ���� â �ν��Ͻ�
	UPROPERTY()
		class USkillLinkageWidget* SkillLinkageWindow;

	// ��ų ���� �ν��Ͻ�
	UPROPERTY(VisibleAnywhere, Category = UI)
		class USkillWidget* SkillWidget;

	// ��ų ���� UI ���� �ν��Ͻ�
	UPROPERTY(VisibleAnywhere, Category = UI)
		class USkillLinkageUIWidget* SkillLinkageUIWindow;

	// �޺� ���� �ν��Ͻ�
	UPROPERTY(VisibleAnywhere, Category = UI)
		class UComboWidget* ComboWindow;

	UPROPERTY()
		class AMyPlayerState* MyPlayerState;
};
