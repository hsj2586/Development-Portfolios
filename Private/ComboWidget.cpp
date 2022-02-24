// Fill out your copyright notice in the Description page of Project Settings.

#include "ComboWidget.h"
#include "TextBlock.h"

void UComboWidget::NativeConstruct()
{
	ComboCountText = Cast<UTextBlock>(GetWidgetFromName(FName(TEXT("ComboCountText"))));
	ElapsTime = 0;
	ComboCounting = 0;
	IsVisible = false;
}

void UComboWidget::Init()
{
	SetVisibility(ESlateVisibility::HitTestInvisible);
	IsVisible = true;
	CountCombo();
}

void UComboWidget::CountCombo()
{
	ElapsTime = 0;
	ComboCounting += 1;
	ComboCountText->SetText(FText::FromString(FString::FromInt(ComboCounting)));
}

void UComboWidget::NativeTick(const FGeometry& MyGeometry, float InDeltaTime)
{
	if (IsVisible)
	{
		if (ElapsTime > 2.0f)
		{
			SetVisibility(ESlateVisibility::Hidden);
			IsVisible = false;
			ComboCounting = 0;
		}
		ElapsTime += InDeltaTime;
	}
}