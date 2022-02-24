// Fill out your copyright notice in the Description page of Project Settings.

#include "CharacterWidget.h"
#include "MonsterStatComponent.h"
#include "Components/ProgressBar.h"

void UCharacterWidget::BindCharacterStat(UMonsterStatComponent* NewCharacterStat)
{
	CurrentCharacterStat = NewCharacterStat;
	NewCharacterStat->OnHPChanged.AddUObject(this, &UCharacterWidget::UpdateHPWidget);
}

void UCharacterWidget::NativeConstruct()
{
	Super::NativeConstruct();
	HPProgressBar = Cast<UProgressBar>(GetWidgetFromName("PB_HPBar"));
	UpdateHPWidget();
}

void UCharacterWidget::UpdateHPWidget()
{
	if (CurrentCharacterStat.IsValid())
	{
		if (HPProgressBar != nullptr)
		{
			HPProgressBar->SetPercent(CurrentCharacterStat->GetHPRatio());
		}
	}
}