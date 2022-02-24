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
	// SkillListWidget 드래그 중이었다면,
	if (IsDragging)
	{
		IsDragging = false;
		PlayerController->DestroySkillWidget();
		return;
	}

	// HUDWidget 드래그 중이었다면,
	if (HUDWidget->IsDragging)
	{
		HUDWidget->IsDragging = false;
		PlayerController->DestroySkillWidget();
		return;
	}

	// SkillLinkageWidget 드래그 중이었다면,
	if (SkillLinkageWidget->IsDragging)
	{
		SkillLinkageWidget->IsDragging = false;
		PlayerController->DestroySkillWidget();
		return;
	}

	// 드래그 중이 아니었다면,
	for (int i = 0; i < SkillSlotList.Num(); i++)
	{
		if (SkillSlotList[i]->IsPressed())
		{
			DraggingSkillIndex = i;
			IsDragging = true;

			// 스킬 이미지 인스턴스 생성
			PlayerController->CreateSkillWidget(SkillSlotList[DraggingSkillIndex]->WidgetStyle.Normal.GetResourceObject());
			return;
		}
	}
}