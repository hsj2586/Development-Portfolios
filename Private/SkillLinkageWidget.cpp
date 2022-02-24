// Fill out your copyright notice in the Description page of Project Settings.

#include "SkillLinkageWidget.h"
#include "MyGameInstance.h"
#include "CharacterSetting.h"
#include "MyPlayerController.h"
#include "Skill.h"
#include "Image.h"
#include "Button.h"
#include "TextBlock.h"
#include "HUDWidget.h"
#include "SkillListWidget.h"
#include "SkillController.h"
#include "Runtime/Engine/Public/EngineUtils.h"

FSkillData * USkillLinkageWidget::GetDraggingSkillData()
{
	return Cast<UMyGameInstance>(GetWorld()->
		GetGameInstance())->GetSkillData(DraggingSkillIndex + 1);
}

UObject * USkillLinkageWidget::GetDraggingSkillImage()
{
	return Cast<UTexture2D>(SkillSlotList[DraggingSkillIndex]->WidgetStyle.Normal.GetResourceObject());
}

void USkillLinkageWidget::SetSkillImage(int SkillSlotIndex, UTexture2D* Image)
{
	SkillSlotList[SkillSlotIndex]->WidgetStyle.Normal.SetResourceObject(Image);
	SkillSlotList[SkillSlotIndex]->WidgetStyle.Hovered.SetResourceObject(Image);
	SkillSlotList[SkillSlotIndex]->WidgetStyle.Pressed.SetResourceObject(Image);
}

void USkillLinkageWidget::ChangeSkillSelf(int TargetIndex)
{
	SetSkillImage(TargetIndex, Cast<UTexture2D>(GetDraggingSkillImage()));
	SetSkillImage(DraggingSkillIndex, nullptr);
	
	SkillList[TargetIndex] = SkillList[DraggingSkillIndex];
	SkillList[DraggingSkillIndex] = nullptr;

	// 연계 스킬 인덱스 갱신
	SkillController->UpdateSkillList();
}

void USkillLinkageWidget::NativeConstruct()
{
	IsDragging = false;
	DraggingSkillIndex = 0;

	auto GameInstance = Cast<UMyGameInstance>(GetWorld()->GetGameInstance());
	auto DefaultSetting = GetDefault<UCharacterSetting>();
	TActorIterator<AMyPlayerController> ControllerIter(GetWorld());
	PlayerController = *ControllerIter;
	TActorIterator<AMyCharacter> CharacterIter(GetWorld());
	SkillController = ((*CharacterIter)->FindComponentByClass<USkillController>());

	// 더미 스킬 리스트 삽입
	for (int j = 0; j < 3; j++)
	{
		SkillList.Add(nullptr);
	}

	for (int i = 1; i <= 3; i++)
	{
		auto TempSkillData = GameInstance->GetSkillData(i);
		FString SkillImageName = "OriginalSkill" + FString::FromInt(i);
		FString SkillSlotName = "LinkageSkill" + FString::FromInt(i);
		UImage* SkillImage = Cast<UImage>(GetWidgetFromName(FName(*SkillImageName)));
		UButton* SkillButton = Cast<UButton>(GetWidgetFromName(FName(*SkillSlotName)));
		SkillSlotList.Add(SkillButton);
		UTexture2D* NewTexture = Cast<UTexture2D>(StaticLoadObject(UTexture2D::StaticClass(), NULL, *DefaultSetting->SkillImageAssets[i - 1].ToString()));
		SkillImage->Brush.SetResourceObject(NewTexture);

		SkillSlotList[i - 1]->OnPressed.AddDynamic(this, &USkillLinkageWidget::DraggingSkill);
	}
}

void USkillLinkageWidget::DraggingSkill()
{
	// SkillLinkageWidget 드래그 중이었다면,
	if (IsDragging)
	{
		for (int i = 0; i < SkillSlotList.Num(); i++)
		{
			if (SkillSlotList[i]->IsPressed())
			{
				// 같은 스킬 슬롯을 클릭했을 경우,
				if (i == DraggingSkillIndex)
				{
					IsDragging = false;
					PlayerController->DestroySkillWidget();
					return;
				}

				// 같은 스킬을 클릭했을 경우,
				if (SkillList[DraggingSkillIndex]->GetSkillName() ==
					Cast<UMyGameInstance>(GetWorld()->
						GetGameInstance())->GetSkillData(i + 1)->SkillName)
				{
					IsDragging = false;
					PlayerController->DestroySkillWidget();
					return;
				}

				ChangeSkillSelf(i);
				IsDragging = false;
				PlayerController->DestroySkillWidget();
				return;
			}
		}
	}

	// HUDWidget 드래그 중이었다면,
	if (HUDWidget->IsDragging)
	{
		for (int i = 0; i < SkillSlotList.Num(); i++)
		{
			if (SkillSlotList[i]->IsPressed())
			{
				// 같은 스킬일 경우,
				if (Cast<UMyGameInstance>(GetWorld()->
					GetGameInstance())->GetSkillData(i + 1)->SkillName ==
					SkillController->SkillList[HUDWidget->DraggingSkillIndex]->GetSkillName())
				{
					HUDWidget->IsDragging = false;
					PlayerController->DestroySkillWidget();
					return;
				}

				SkillList[i] = SkillController->SkillList[HUDWidget->DraggingSkillIndex];
				SetSkillImage(i, 
				 Cast<UTexture2D>(HUDWidget->SkillSlotList[HUDWidget->DraggingSkillIndex]->WidgetStyle.Normal.GetResourceObject()));
				
				// 연계 스킬 인덱스 갱신
				SkillController->UpdateSkillList();

				HUDWidget->IsDragging = false;
				PlayerController->DestroySkillWidget();
				return;
			}
		}
	}

	// SkillListWidget 드래그 중이었다면,
	if (SkillListWidget->IsDragging)
	{
		SkillListWidget->IsDragging = false;
		PlayerController->DestroySkillWidget();
		return;
	}

	// 드래그 중이 아니었다면,
	for (int i = 0; i < SkillSlotList.Num(); i++)
	{
		if (SkillSlotList[i]->IsPressed())
		{
			if (SkillSlotList[i]->WidgetStyle.Normal.GetResourceObject() == nullptr)
			{
				// 빈 슬롯을 눌렀을 경우 드래그 캔슬
				return;
			}
			DraggingSkillIndex = i;
			//UE_LOG(LogTemp, Warning, TEXT("index : %d"), DraggingSkillIndex);
			IsDragging = true;

			// 스킬 이미지 인스턴스 생성
			PlayerController->CreateSkillWidget(SkillSlotList[DraggingSkillIndex]->WidgetStyle.Normal.GetResourceObject());
			return;
		}
	}
}