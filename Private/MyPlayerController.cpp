// Fill out your copyright notice in the Description page of Project Settings.

#include "MyPlayerController.h"
#include "HUDWidget.h"
#include "MyPlayerState.h"
#include "MyCharacter.h"
#include "Monster.h"
#include "OptionWindow.h"
#include "SkillWidget.h"
#include "SkillListWidget.h"
#include "SkillLinkageWidget.h"
#include "SkillLinkageUIWidget.h"
#include "ComboWidget.h"
#include "InputBufferManager.h"

void AMyPlayerController::Tick(float DeltaTime)
{
	ElapsTime += DeltaTime;
}

AMyPlayerController::AMyPlayerController()
{
	InputBufferManager = CreateDefaultSubobject<UInputBufferManager>(TEXT("INPUTBUFFERMANAGER"));

	static ConstructorHelpers::FClassFinder<UHUDWidget> UI_HUD_C(
		TEXT("/Game/UI/UI_HUD.UI_HUD_C")
	);
	if (UI_HUD_C.Succeeded())
	{
		HUDWidgetClass = UI_HUD_C.Class;
	}

	static ConstructorHelpers::FClassFinder<UOptionWindow> OptionWidget(
		TEXT("/Game/UI/OptionWidget.OptionWidget_C")
	);
	if (OptionWidget.Succeeded())
	{
		OptionWidgetClass = OptionWidget.Class;
	}

	static ConstructorHelpers::FClassFinder<USkillWidget> SkillWidget_(
		TEXT("/Game/UI/SkillWidget.SkillWidget_C")
	);
	if (SkillWidget_.Succeeded())
	{
		SkillWidgetClass = SkillWidget_.Class;
	}

	static ConstructorHelpers::FClassFinder<USkillListWidget> SkillListWidget(
		TEXT("/Game/UI/SkillListWidget.SkillListWidget_C")
	);
	if (SkillListWidget.Succeeded())
	{
		SkillListWidgetClass = SkillListWidget.Class;
	}

	static ConstructorHelpers::FClassFinder<USkillLinkageWidget> SkillLinkageWidget(
		TEXT("/Game/UI/SkillLinkageWidget.SkillLinkageWidget_C")
	);
	if (SkillLinkageWidget.Succeeded())
	{
		SkillLinkageWidgetClass = SkillLinkageWidget.Class;
	}

	static ConstructorHelpers::FClassFinder<USkillLinkageUIWidget> SkillLinkageUIWidget(
		TEXT("/Game/UI/SkillLinkageUIWidget.SkillLinkageUIWidget_C")
	);
	if (SkillLinkageUIWidget.Succeeded())
	{
		SkillLinkageUIWidgetClass = SkillLinkageUIWidget.Class;
	}

	static ConstructorHelpers::FClassFinder<UComboWidget> ComboWidget(
		TEXT("/Game/UI/ComboWidget.ComboWidget_C")
	);
	if (ComboWidget.Succeeded())
	{
		ComboWidgetClass = ComboWidget.Class;
	}
}

void AMyPlayerController::BeginPlay()
{
	ElapsTime = 0;

	HUDWidget = CreateWidget<UHUDWidget>(this, HUDWidgetClass);
	HUDWidget->AddToViewport();

	OptionWindow = CreateWidget<UOptionWindow>(this, OptionWidgetClass);
	OptionWindow->AddToViewport();
	OptionWindow->SetVisibility(ESlateVisibility::Hidden);

	SkillListWindow = CreateWidget<USkillListWidget>(this, SkillListWidgetClass);
	SkillListWindow->AddToViewport();
	SkillListWindow->SetVisibility(ESlateVisibility::Hidden);

	SkillLinkageWindow = CreateWidget<USkillLinkageWidget>(this, SkillLinkageWidgetClass);
	SkillLinkageWindow->AddToViewport();
	SkillLinkageWindow->SetVisibility(ESlateVisibility::Hidden);

	SkillLinkageUIWindow = CreateWidget<USkillLinkageUIWidget>(this, SkillLinkageUIWidgetClass);
	SkillLinkageUIWindow->AddToViewport();
	SkillLinkageUIWindow->SetVisibility(ESlateVisibility::Hidden);

	SkillWidget = CreateWidget<USkillWidget>(this, SkillWidgetClass);
	SkillWidget->AddToViewport();
	SkillWidget->SetVisibility(ESlateVisibility::Hidden);

	ComboWindow = CreateWidget<UComboWidget>(this, ComboWidgetClass);
	ComboWindow->AddToViewport();
	ComboWindow->SetVisibility(ESlateVisibility::Hidden);

	// 위젯간 통신을 위한 초기화
	HUDWidget->SkillListWidget = SkillListWindow;
	HUDWidget->SkillLinkageWidget = SkillLinkageWindow;
	SkillListWindow->HUDWidget = HUDWidget;
	SkillListWindow->SkillLinkageWidget = SkillLinkageWindow;
	SkillLinkageWindow->SkillListWidget = SkillListWindow;
	SkillLinkageWindow->HUDWidget = HUDWidget;
}

void AMyPlayerController::PostInitializeComponents()
{
	Super::PostInitializeComponents();
}

void AMyPlayerController::Possess(APawn* aPawn)
{
	Super::Possess(aPawn);
}

bool AMyPlayerController::InputKey(FKey Key, EInputEvent EventType, float AmountDepressed, bool bGamepad)
{
	if (EventType == EInputEvent::IE_Pressed)
	{
		InputBufferManager->InsertInput(Key);
		ElapsTime = 0;
	}
	Super::InputKey(Key, EventType, AmountDepressed, bGamepad);
	return true;
}

UHUDWidget * AMyPlayerController::GetHUDWidget()
{
	return HUDWidget;
}

void AMyPlayerController::SetHUDWidget()
{
	FInputModeGameOnly InputMode;
	SetInputMode(InputMode);
	InputYawScale = 2.5f;
	InputPitchScale = -1.5f;

	MyPlayerState = Cast<AMyPlayerState>(PlayerState);
	HUDWidget->BindPlayerState(MyPlayerState);
	MyPlayerState->OnPlayerStateChanged.Broadcast();
}

void AMyPlayerController::NPCKill(AMonster* KilledNPC) const
{
	MyPlayerState->AddExp(KilledNPC->GetExp());
	UE_LOG(LogTemp, Warning, TEXT("Player gets %d EXP from %s.")
		, KilledNPC->GetExp(), *KilledNPC->GetActorLabel());
}

void AMyPlayerController::CreateOptionWidget()
{
	OptionWindow->SetVisibility(ESlateVisibility::SelfHitTestInvisible);
}

void AMyPlayerController::DestroyOptionWidget()
{
	OptionWindow->SetVisibility(ESlateVisibility::Hidden);
}

void AMyPlayerController::CreateSkillWidget(UObject* SkillImage)
{
	SkillWidget->SetSkillImage(SkillImage);
	SkillWidget->SetVisibility(ESlateVisibility::HitTestInvisible);
}

void AMyPlayerController::DestroySkillWidget()
{
	SkillWidget->SetVisibility(ESlateVisibility::Hidden);
}

void AMyPlayerController::CreateSkillListWidget()
{
	SkillListWindow->SetVisibility(ESlateVisibility::SelfHitTestInvisible);
}

void AMyPlayerController::DestroySkillListWidget()
{
	SkillListWindow->SetVisibility(ESlateVisibility::Hidden);
}

void AMyPlayerController::CreateSkillLinkageWidget()
{
	SkillLinkageWindow->SetVisibility(ESlateVisibility::SelfHitTestInvisible);
}

void AMyPlayerController::DestroySkillLinkageWidget()
{
	SkillLinkageWindow->SetVisibility(ESlateVisibility::Hidden);
}

void AMyPlayerController::CreateSkillLinkageUIWidget(UTexture2D* LinkageImage, FString LinkageInputText, FString SkillNameText)
{
	SkillLinkageUIWindow->Init(LinkageImage, LinkageInputText, SkillNameText);
	SkillLinkageUIWindow->SetVisibility(ESlateVisibility::SelfHitTestInvisible);
}

void AMyPlayerController::DestroySkillLinkageUIWidget()
{
	SkillLinkageUIWindow->SetVisibility(ESlateVisibility::Hidden);
}

void AMyPlayerController::CreateComboWidget()
{
	ComboWindow->Init();
}

bool AMyPlayerController::IsComboTiming()
{
	return SkillLinkageUIWindow->CheckComboTiming();
}

bool AMyPlayerController::GetCheckCommand(SkillType CommandName)
{
	return InputBufferManager->CheckCommand(CommandName);
}