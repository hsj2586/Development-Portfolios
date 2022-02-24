// Fill out your copyright notice in the Description page of Project Settings.

#include "HUDWidget.h"
#include "Components/ProgressBar.h"
#include "Components/TextBlock.h"
#include "MyCharacterStatComponent.h"
#include "Skill.h"
#include "MyPlayerState.h"
#include "Image.h"
#include "Button.h"
#include "MyPlayerController.h"
#include "OptionWindow.h"
#include "SkillListWidget.h"
#include "SkillLinkageWidget.h"
#include "SkillController.h"
#include "Rolling.h"
#include "InputBufferManager.h"
#include "Runtime/Engine/Public/EngineUtils.h"

void UHUDWidget::BindCharacterStat(UMyCharacterStatComponent* CharacterStat)
{
	CurrentCharacterStat = CharacterStat;
	CharacterStat->OnHPChanged.AddUObject(this, &UHUDWidget::UpdateCharacterHP);
	CharacterStat->OnManaChanged.AddUObject(this, &UHUDWidget::UpdateCharacterMana);
}

void UHUDWidget::BindPlayerState(AMyPlayerState* PlayerState)
{
	CurrentPlayerState = PlayerState;
	PlayerState->OnPlayerStateChanged.AddUObject(this, &UHUDWidget::UpdatePlayerState);
}

void UHUDWidget::BindSkillState(TArray<class USkill*> SkillList)
{
	for (int i = 0; i < SkillList.Num(); i++)
	{
		if (SkillList[i] != nullptr)
		{
			SkillList[i]->OnCooltimeChanged.Clear();
			SkillList[i]->OnCooltimeIsZero.Clear();
			SkillList[i]->OnCooltimeChanged.AddUObject(this, &UHUDWidget::UpdateSkillState, SkillList[i], i);
			SkillList[i]->OnCooltimeIsZero.AddUObject(this, &UHUDWidget::UpdateSkillCooltimeIsZero, i);
		}
	}
}

void UHUDWidget::SetSkillImage(int SkillSlotIndex, UTexture2D* Image)
{
	SkillSlotList[SkillSlotIndex]->WidgetStyle.Normal.SetResourceObject(Image);
	SkillSlotList[SkillSlotIndex]->WidgetStyle.Hovered.SetResourceObject(Image);
	SkillSlotList[SkillSlotIndex]->WidgetStyle.Pressed.SetResourceObject(Image);
}

void UHUDWidget::NativeConstruct()
{
	Super::NativeConstruct();
	HPBar = Cast<UProgressBar>(GetWidgetFromName(TEXT("pbHP")));
	ManaBar = Cast<UProgressBar>(GetWidgetFromName(TEXT("pbMana")));
	EXPBar = Cast<UProgressBar>(GetWidgetFromName(TEXT("pbExp")));
	PlayerName = Cast<UTextBlock>(GetWidgetFromName(TEXT("txtPlayerName")));
	PlayerLevel = Cast<UTextBlock>(GetWidgetFromName(TEXT("txtLevel")));
	HPText = Cast<UTextBlock>(GetWidgetFromName(TEXT("txtHP")));
	ManaText = Cast<UTextBlock>(GetWidgetFromName(TEXT("txtMana")));
	OptionButtonUI = Cast<UButton>(GetWidgetFromName(TEXT("OptionButton")));
	SkillListButtonUI = Cast<UButton>(GetWidgetFromName(TEXT("SkillListButton")));
	SkillLinkageButtonUI = Cast<UButton>(GetWidgetFromName(TEXT("SkillLinkageButton")));

	IsDragging = false;
	IsActiveOptionWidget = false;
	TActorIterator<AMyPlayerController> ControllerIter(GetWorld());
	PlayerController = *ControllerIter;

	TActorIterator<AMyCharacter> CharacterIter(GetWorld());
	SkillController = ((*CharacterIter)->FindComponentByClass<USkillController>());
	PlayerInputComponent = (*CharacterIter)->FindComponentByClass<UInputComponent>();

	for (int i = 1; i <= 10; i++)
	{
		FString SkillSlotName = "SkillSlot" + FString::FromInt(i);
		SkillSlotList.Add(
			Cast<UButton>(GetWidgetFromName(FName(*SkillSlotName)))
		);

		FString SkillSlotTextName = "SkillSlotText" + FString::FromInt(i);
		SkillSlotTextList.Add(
			Cast<UTextBlock>(GetWidgetFromName(FName(*SkillSlotTextName)))
		);

		FString SkillSlotGaugeName = "SkillSlotGauge" + FString::FromInt(i);
		SkillSlotGaugeList.Add(
			Cast<UProgressBar>(GetWidgetFromName(FName(*SkillSlotGaugeName)))
		);

		SkillSlotList[i - 1]->OnPressed.AddDynamic(this, &UHUDWidget::DraggingSkill);
	}

	OptionButtonUI->OnClicked.AddDynamic(this, &UHUDWidget::SetActiveOptionWindow);
	SkillListButtonUI->OnClicked.AddDynamic(this, &UHUDWidget::SetActiveSkillList);
	SkillLinkageButtonUI->OnClicked.AddDynamic(this, &UHUDWidget::SetActiveSkillLinkage);
}

void UHUDWidget::UpdateCharacterHP()
{
	// HP ������ ���α׷��� �ٿ� ����
	HPBar->SetPercent(CurrentCharacterStat->GetHPRatio());

	// HP �ؽ�Ʈ�� ����
	FString TempText = FString::FromInt(CurrentCharacterStat->GetHP()) + "/" +
		FString::FromInt(CurrentCharacterStat->GetData()->Health);
	HPText->SetText(FText::FromString(TempText));
}

void UHUDWidget::UpdateCharacterMana()
{
	// Mana ������ ���α׷��� �ٿ� ����
	ManaBar->SetPercent(CurrentCharacterStat->GetManaRatio());

	// Mana �ؽ�Ʈ�� ����
	FString TempText = FString::FromInt(CurrentCharacterStat->GetMana()) + "/" +
		FString::FromInt(CurrentCharacterStat->GetData()->Mana);
	ManaText->SetText(FText::FromString(TempText));
}

void UHUDWidget::UpdatePlayerState()
{
	EXPBar->SetPercent(CurrentPlayerState->GetExpRatio());
	PlayerName->SetText(FText::FromString(CurrentPlayerState->GetPlayerName()));
	PlayerLevel->SetText(FText::FromString(FString::FromInt(CurrentPlayerState->GetCharacterLevel())));
}

void UHUDWidget::UpdateSkillState(USkill* Skill, int SlotIndex)
{
	// float�� ��Ÿ�� ������ int������ ��ȯ�� �ؽ�Ʈ�� �ݿ�
	SkillSlotTextList[SlotIndex]->SetText(
		FText::FromString(FString::FromInt(Skill->GetRemainCooltime() + 1)));

	// ��ų ��Ÿ���� ������ ��ų ���α׷��� �ٿ� �ݿ�
	SkillSlotGaugeList[SlotIndex]->SetPercent(Skill->GetCooltimeRatio());
}

void UHUDWidget::UpdateSkillCooltimeIsZero(int SlotIndex)
{
	SkillSlotTextList[SlotIndex]->SetText(FText::FromString(""));
}

void UHUDWidget::SetActiveOptionWindow()
{
	if (IsActiveOptionWidget)
	{
		PlayerController->DestroyOptionWidget();
		IsActiveOptionWidget = false;
	}
	else
	{
		PlayerController->CreateOptionWidget();
		IsActiveOptionWidget = true;
	}
}

void UHUDWidget::SetActiveSkillList()
{
	if (IsActiveSkillListWidget)
	{
		PlayerController->DestroySkillListWidget();
		IsActiveSkillListWidget = false;
	}
	else
	{
		PlayerController->CreateSkillListWidget();
		IsActiveSkillListWidget = true;
	}
}

void UHUDWidget::SetActiveSkillLinkage()
{
	if (IsActiveSkillLinkageWidget)
	{
		PlayerController->DestroySkillLinkageWidget();
		IsActiveSkillLinkageWidget = false;
	}
	else
	{
		PlayerController->CreateSkillLinkageWidget();
		IsActiveSkillLinkageWidget = true;
	}
}

void UHUDWidget::DraggingSkill()
{
	// HUDWidget �巡�� ���̾��ٸ�,
	if (IsDragging)
	{
		for (int i = 0; i < SkillSlotList.Num(); i++)
		{
			if (SkillSlotList[i]->IsPressed())
			{
				ChangeSkill(i, DraggingSkillIndex);
				IsDragging = false;
				PlayerController->DestroySkillWidget();
				return;
			}
		}
	}

	// SkillListWidget �巡�� ���̾��ٸ�,
	if (SkillListWidget->IsDragging)
	{
		for (int i = 0; i < SkillSlotList.Num(); i++)
		{
			if (SkillSlotList[i]->IsPressed())
			{
				// ���� ��ų����Ʈ�� ��ų�� �����Ѵٸ�, ���� ��ų�� �ڸ� ��ü
				FString DraggingSkillName = SkillListWidget->GetDraggingSkillData()->SkillName;
				int FindingResult = FindSkillByName(DraggingSkillName);
				if (FindingResult != -1)
					ChangeSkill(i, FindingResult);
				else // ���� ��ų����Ʈ�� ��ų�� ���� ���� �ʴ´ٸ�, ��ų ���� ����
					PushSkill(i);

				SkillListWidget->IsDragging = false;
				PlayerController->DestroySkillWidget();
				return;
			}
		}
	}

	// SkillLinkageWidget �巡�� ���̾��ٸ�,
	if (SkillLinkageWidget->IsDragging)
	{
		for (int i = 0; i < SkillSlotList.Num(); i++)
		{
			if (SkillSlotList[i]->IsPressed())
			{
				// ���� ��ų����Ʈ�� ��ų�� �����Ѵٸ�, ���� ��ų�� �ڸ� ��ü
				FString DraggingSkillName = SkillLinkageWidget->
					SkillList[SkillLinkageWidget->DraggingSkillIndex]->GetSkillName();
				int FindingResult = FindSkillByName(DraggingSkillName);
				if (FindingResult != -1)
					ChangeSkill(i, FindingResult);
				else // ���� ��ų����Ʈ�� ��ų�� ���� ���� �ʴ´ٸ�, ��ų ���� ����
					PushSkill(i);

				SkillLinkageWidget->IsDragging = false;
				PlayerController->DestroySkillWidget();
				return;
			}
		}
	}

	// �巡�� ���� �ƴϾ��ٸ�,
	for (int i = 0; i < SkillSlotList.Num(); i++)
	{
		if (SkillSlotList[i]->IsPressed())
		{
			if (SkillSlotList[i]->WidgetStyle.Normal.GetResourceObject() == nullptr)
			{
				// �� ������ ������ ��� �巡�� ĵ��
				return;
			}
			DraggingSkillIndex = i;
			IsDragging = true;

			// ��ų �̹��� �ν��Ͻ� ����
			PlayerController->CreateSkillWidget(SkillSlotList[DraggingSkillIndex]->WidgetStyle.Normal.GetResourceObject());
			return;
		}
	}
}

void UHUDWidget::PushSkill(int TargetIndex)
{
	struct FSkillData* DraggingSkillData = SkillListWidget->GetDraggingSkillData();
	SetSkillImage(TargetIndex, Cast<UTexture2D>(SkillListWidget->GetDraggingSkillImage()));
	USkill* Skill = Cast<USkill>(SkillController->GetSkillClass(SkillListWidget->DraggingSkillIndex + 1, FName(*DraggingSkillData->SkillName)));
	SkillController->SkillList[TargetIndex] = Skill;
	SkillController->SkillList[TargetIndex]->Init(PlayerInputComponent, TargetIndex + 1, DraggingSkillData);
	SkillController->UpdateSkillList();
	BindSkillState(SkillController->SkillList);
}

void UHUDWidget::ChangeSkill(int TargetIndex, int DraggingIndex)
{
	// ��ų �̹��� Swap
	UTexture2D* FirstImage = Cast<UTexture2D>(SkillSlotList[TargetIndex]->WidgetStyle.Normal.GetResourceObject());
	UTexture2D* SecondImage = Cast<UTexture2D>(SkillSlotList[DraggingIndex]->WidgetStyle.Normal.GetResourceObject());
	SetSkillImage(DraggingIndex, FirstImage);
	SetSkillImage(TargetIndex, SecondImage);

	// ��ų ����Ʈ �� Input �ε��� Swap
	SkillController->SkillList.Swap(DraggingIndex, TargetIndex);
	FString InputName1 = "Skill" + FString::FromInt(DraggingIndex + 1);
	FString InputName2 = "Skill" + FString::FromInt(TargetIndex + 1);

	// ���� ��ų ���ε� ����
	if (PlayerInputComponent)
	{
		for (int i = 0; i < PlayerInputComponent->GetNumActionBindings(); i++)
		{
			if (PlayerInputComponent->GetActionBinding(i).ActionName.ToString() == InputName1)
			{
				PlayerInputComponent->RemoveActionBinding(i);
				break;
			}
		}
		for (int i = 0; i < PlayerInputComponent->GetNumActionBindings(); i++)
		{
			if (PlayerInputComponent->GetActionBinding(i).ActionName.ToString() == InputName2)
			{
				PlayerInputComponent->RemoveActionBinding(i);
				break;
			}
		}
	}

	// ��ų Input ���ε� �缳��
	if(SkillController->SkillList[DraggingIndex])
		SkillController->SkillList[DraggingIndex]->Init(PlayerInputComponent, DraggingIndex + 1);
	if (SkillController->SkillList[TargetIndex])
		SkillController->SkillList[TargetIndex]->Init(PlayerInputComponent, TargetIndex + 1);

	// ���� ��ų �ε��� ���� �� ��ų UI ��������Ʈ ����
	SkillController->UpdateSkillList();
	BindSkillState(SkillController->SkillList);
}

int UHUDWidget::FindSkillByName(FString SkillName)
{
	for (int i = 0; i < SkillController->SkillList.Num(); i++)
	{
		if (SkillController->SkillList[i] != nullptr && SkillController->SkillList[i]->GetSkillName() == SkillName)
			return i;
	}
	return -1;
}
