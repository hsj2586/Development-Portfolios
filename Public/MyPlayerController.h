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

	// HUD 위젯 클래스
	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite, Category = UI)
		TSubclassOf<class UHUDWidget> HUDWidgetClass;

	// 옵션 위젯 클래스
	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite, Category = UI)
		TSubclassOf<class UOptionWindow> OptionWidgetClass;

	// 스킬 위젯 클래스
	UPROPERTY(VisibleAnywhere, Category = UI)
		TSubclassOf<class USkillWidget> SkillWidgetClass;

	// 스킬 리스트 위젯 클래스
	UPROPERTY(VisibleAnywhere, Category = UI)
		TSubclassOf<class USkillListWidget> SkillListWidgetClass;

	// 스킬 연계 위젯 클래스
	UPROPERTY(VisibleAnywhere, Category = UI)
		TSubclassOf<class USkillLinkageWidget> SkillLinkageWidgetClass;

	// 스킬 연계 UI 위젯 클래스
	UPROPERTY(VisibleAnywhere, Category = UI)
		TSubclassOf<class USkillLinkageUIWidget> SkillLinkageUIWidgetClass;

	// 콤보 위젯 클래스
	UPROPERTY(VisibleAnywhere, Category = UI)
		TSubclassOf<class UComboWidget> ComboWidgetClass;

	// 입력 버퍼 매니저
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

	// OptionWidget 생성
	void CreateOptionWidget();

	// OptionWidget 파괴
	void DestroyOptionWidget();

	// SkillWidget 생성
	void CreateSkillWidget(UObject* SkillImage);

	// SkillWidget 파괴
	void DestroySkillWidget();

	// SkillListWidget 생성
	void CreateSkillListWidget();

	// SkillListWidget 파괴
	void DestroySkillListWidget();

	// SkillLinkageWidget 생성
	void CreateSkillLinkageWidget();

	// SkillLinkageWidget 파괴
	void DestroySkillLinkageWidget();

	// SkillLinkageUIWidget 생성
	void CreateSkillLinkageUIWidget(UTexture2D* LinkageImage, FString LinkageInputText, FString SkillNameText);

	// SkillLinkageUIWidget 파괴
	void DestroySkillLinkageUIWidget();

	// ComboWidget 활성화
	void CreateComboWidget();

	// 스킬 콤보 타이밍 여부 반환 함수
	bool IsComboTiming();

	// 커맨드 입력 조건 확인 함수
	bool GetCheckCommand(SkillType CommandName);

	float ElapsTime;

private:
	// 베이스 UI 창 인스턴스
	UPROPERTY()
		class UHUDWidget* HUDWidget;

	// 옵션 창 인스턴스
	UPROPERTY()
		class UOptionWindow* OptionWindow;

	// 스킬 리스트 창 인스턴스
	UPROPERTY()
		class USkillListWidget* SkillListWindow;

	// 스킬 연계 창 인스턴스
	UPROPERTY()
		class USkillLinkageWidget* SkillLinkageWindow;

	// 스킬 위젯 인스턴스
	UPROPERTY(VisibleAnywhere, Category = UI)
		class USkillWidget* SkillWidget;

	// 스킬 연계 UI 위젯 인스턴스
	UPROPERTY(VisibleAnywhere, Category = UI)
		class USkillLinkageUIWidget* SkillLinkageUIWindow;

	// 콤보 위젯 인스턴스
	UPROPERTY(VisibleAnywhere, Category = UI)
		class UComboWidget* ComboWindow;

	UPROPERTY()
		class AMyPlayerState* MyPlayerState;
};
