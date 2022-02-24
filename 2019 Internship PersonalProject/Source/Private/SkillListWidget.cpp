// Fill out your copyright notice in the Description page of Project Settings.

#include "SkillListWidget.h"
#include "MyGameInstance.h"
#include "CharacterSetting.h"
#include "MyPlayerController.h"
#include "Skill.h"
#include "Button.h"
#include "TextBlock.h"
#include "HUDWidget.h"
#include "SkillLinkageWidget.h"
#include "Runtime/Engine/Public/EngineUtils.h"

struct FSkillData* USkillListWidget::GetDraggingSkillData()
{
	return Cast<UMyGameInstance>(GetWorld()->
		GetGameInstance())->GetSkillData(DraggingSkillIndex + 1);
}

UObject * USkillListWidget::GetDraggingSkillImage()
{
	return SkillSlotList[DraggingSkillIndex]->WidgetStyle.Normal.GetResourceObject();
}

void USkillListWidget::NativeConstruct()
{
	IsDragging = false;
	DraggingSkillIndex = 0;

	auto GameInstance = Cast<UMyGameInstance>(GetWorld()->GetGameInstance());
	auto DefaultSetting = GetDefault<UCharacterSetting>();
	TActorIterator<AMyPlayerController> ControllerIter(GetWorld());
	PlayerController = *ControllerIter;

	for (int i = 1; i <= 3; i++)
	{
		auto TempSkillData = GameInstance->GetSkillData(i);
		FString SkillSlotName = "SkillImage" + FString::FromInt(i);
		FString SkillTextName = "SkillText" + FString::FromInt(i);
		UButton* SkillButton = Cast<UButton>(GetWidgetFromName(FName(*SkillSlotName)));
		SkillSlotList.Add(SkillButton);
		UTextBlock* SkillText = Cast<UTextBlock>(GetWidgetFromName(FName(*SkillTextName)));
		UTexture2D* NewTexture = Cast<UTexture2D>(StaticLoadObject(UTexture2D::StaticClass(), NULL, *DefaultSetting->SkillImageAssets[i - 1].ToString()));
		SkillButton->WidgetStyle.Normal.SetResourceObject(NewTexture);
		SkillButton->WidgetStyle.Hovered.SetResourceObject(NewTexture);
		SkillButton->WidgetStyle.Pressed.SetResourceObject(NewTexture);
		SkillText->SetText(FText::FromString(TempSkillData->SkillName));

		SkillSlotList[i - 1]->OnPressed.AddDynamic(this, &USkillListWidget::DraggingSkill);
	}
}

void USkillListWidget::DraggingSkill()
{
	// SkillListWidget �巡�� ���̾��ٸ�,
	if (IsDragging)
	{
		IsDragging = false;
		PlayerController->DestroySkillWidget();
		return;
	}

	// HUDWidget �巡�� ���̾��ٸ�,
	if (HUDWidget->IsDragging)
	{
		HUDWidget->IsDragging = false;
		PlayerController->DestroySkillWidget();
		return;
	}

	// SkillLinkageWidget �巡�� ���̾��ٸ�,
	if (SkillLinkageWidget->IsDragging)
	{
		SkillLinkageWidget->IsDragging = false;
		PlayerController->DestroySkillWidget();
		return;
	}

	// �巡�� ���� �ƴϾ��ٸ�,
	for (int i = 0; i < SkillSlotList.Num(); i++)
	{
		if (SkillSlotList[i]->IsPressed())
		{
			DraggingSkillIndex = i;
			IsDragging = true;

			// ��ų �̹��� �ν��Ͻ� ����
			PlayerController->CreateSkillWidget(SkillSlotList[DraggingSkillIndex]->WidgetStyle.Normal.GetResourceObject());
			return;
		}
	}
}