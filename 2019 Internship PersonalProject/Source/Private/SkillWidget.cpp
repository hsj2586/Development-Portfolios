// Fill out your copyright notice in the Description page of Project Settings.

#include "SkillWidget.h"
#include "MyPlayerController.h"
#include "Components/Image.h"
#include "Runtime/Engine/Public/EngineUtils.h"
#include "Runtime/UMG/Public/Blueprint/WidgetLayoutLibrary.h"

void USkillWidget::SetSkillImage(UObject* Image)
{
	SkillImage->Brush.SetResourceObject(Image);
}

void USkillWidget::NativeConstruct()
{
	SkillImage = Cast<UImage>(GetWidgetFromName(TEXT("SkillTexture")));
	TActorIterator<AMyPlayerController> It(GetWorld());
	PlayerController = *It;
}

void USkillWidget::NativeTick(const FGeometry & MyGeometry, float InDeltaTime)
{
	float MouseX, MouseY;
	PlayerController->GetMousePosition(MouseX, MouseY);
	FVector2D MousePosition = UWidgetLayoutLibrary::GetMousePositionOnViewport(this);
	SetRenderTranslation(MousePosition);
}