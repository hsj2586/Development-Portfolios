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

	// ���� ��ų �ε��� ����
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

	// ���� ��ų ����Ʈ ����
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
	// SkillLinkageWidget �巡�� ���̾��ٸ�,
	if (IsDragging)
	{
		for (int i = 0; i < SkillSlotList.Num(); i++)
		{
			if (SkillSlotList[i]->IsPressed())
			{
				// ���� ��ų ������ Ŭ������ ���,
				if (i == DraggingSkillIndex)
				{
					IsDragging = false;
					PlayerController->DestroySkillWidget();
					return;
				}

				// ���� ��ų�� Ŭ������ ���,
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

	// HUDWidget �巡�� ���̾��ٸ�,
	if (HUDWidget->IsDragging)
	{
		for (int i = 0; i < SkillSlotList.Num(); i++)
		{
			if (SkillSlotList[i]->IsPressed())
			{
				// ���� ��ų�� ���,
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
				
				// ���� ��ų �ε��� ����
				SkillController->UpdateSkillList();

				HUDWidget->IsDragging = false;
				PlayerController->DestroySkillWidget();
				return;
			}
		}
	}

	// SkillListWidget �巡�� ���̾��ٸ�,
	if (SkillListWidget->IsDragging)
	{
		SkillListWidget->IsDragging = false;
		PlayerController->DestroySkillWidget();
		return;
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
			//UE_LOG(LogTemp, Warning, TEXT("index : %d"), DraggingSkillIndex);
			IsDragging = true;

			// ��ų �̹��� �ν��Ͻ� ����
			PlayerController->CreateSkillWidget(SkillSlotList[DraggingSkillIndex]->WidgetStyle.Normal.GetResourceObject());
			return;
		}
	}
}