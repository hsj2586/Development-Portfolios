// Fill out your copyright notice in the Description page of Project Settings.

#include "SkillLinkageUIWidget.h"
#include "Image.h"
#include "TextBlock.h"
#include "ProgressBar.h"

void USkillLinkageUIWidget::NativeConstruct()
{
	LinkageImage = Cast<UImage>(GetWidgetFromName(FName(TEXT("LinkageImage"))));
	LinkageInputText = Cast<UTextBlock>(GetWidgetFromName(FName(TEXT("LinkageText"))));
	EdgeImage = Cast<UImage>(GetWidgetFromName(FName(TEXT("EdgeImage"))));
	SkillNameText = Cast<UTextBlock>(GetWidgetFromName(FName(TEXT("SkillNameText"))));
	LinkageGauge = Cast<UProgressBar>(GetWidgetFromName(FName(TEXT("LinkageGauge"))));

	ElapsTime = 0;
}

void USkillLinkageUIWidget::Init(UTexture2D* LinkageImage_, FString LinkageInputText_, FString SkillNameText_)
{
	LinkageImage->Brush.SetResourceObject(LinkageImage_);
	LinkageInputText->SetText(FText::FromString(LinkageInputText_));
	SkillNameText->SetText(FText::FromString(SkillNameText_));
	LinkageGauge->SetPercent(0);
	ElapsTime = 0;
}

bool USkillLinkageUIWidget::CheckComboTiming()
{
	return (ElapsTime >= 0.666f && ElapsTime <= 1.333f) ? true : false;
}

void USkillLinkageUIWidget::NativeTick(const FGeometry& MyGeometry, float InDeltaTime)
{
	ElapsTime += InDeltaTime;
	float Value = FMath::Abs(FMath::Sin(ElapsTime * 3.0f));
	EdgeImage->SetColorAndOpacity(FLinearColor(0.0f, 0.980108f, 1.0f, Value));
	LinkageGauge->SetPercent(ElapsTime / 2.0f);
}