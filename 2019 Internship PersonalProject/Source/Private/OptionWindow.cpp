// Fill out your copyright notice in the Description page of Project Settings.

#include "OptionWindow.h"
#include "Components/TextBlock.h"
#include "Button.h"
#include "Runtime/Engine/Public/EngineUtils.h"
#include "MyCharacter.h"
#include "Runtime/Engine/Classes/GameFramework/PlayerInput.h"
#include "Runtime/Engine/Classes/GameFramework/InputSettings.h"


void UOptionWindow::NativeConstruct()
{
	IsWaitingInput = false;
	ButtonIndex = 0;

	for (int i = 0; i < 6; i++)
	{
		FString InputSettingName = "SettingButton" + FString::FromInt(i);
		InputSettingButtonList.Add(
			Cast<UButton>(GetWidgetFromName(FName(*InputSettingName)))
		);
		InputSettingButtonList[i]->OnPressed.AddDynamic(this, &UOptionWindow::ReceiveInput);

		FString InputSettingTextName = "SettingButtonText" + FString::FromInt(i);
		InputSettingButtonTextList.Add(
			Cast<UTextBlock>(GetWidgetFromName(FName(*InputSettingTextName)))
		);
	}

	TActorIterator<AMyCharacter> It(GetWorld());
	PlayerInput = Cast<APlayerController>((*It)->GetController())->PlayerInput;

	// Axis Input ÃÊ±âÈ­
	PlayerInput->AddAxisMapping(FInputAxisKeyMapping("Up", FKey(TEXT("W")), 1.f));
	PlayerInput->AddAxisMapping(FInputAxisKeyMapping("Down", FKey(TEXT("S")), -1.f));
	PlayerInput->AddAxisMapping(FInputAxisKeyMapping("Left", FKey(TEXT("A")), -1.f));
	PlayerInput->AddAxisMapping(FInputAxisKeyMapping("Right", FKey(TEXT("D")), 1.f));
}

void UOptionWindow::ReceiveInput()
{
	for (int i = 0; i < InputSettingButtonList.Num(); i++)
	{
		if (InputSettingButtonList[i]->IsPressed())
		{
			IsWaitingInput = true;
			ButtonIndex = i;
		}
	}
}

FEventReply UOptionWindow::OnKeyDown(FGeometry MyGeometry, FKeyEvent InKeyEvent)
{
	if (IsWaitingInput)
	{
		Input = InKeyEvent.GetKey();
		InputSettingButtonTextList[ButtonIndex]->SetText(FText::FromString(Input.GetFName().ToString()));
		TArray<FInputAxisKeyMapping> AxisKeyMapping;
		TArray<FInputActionKeyMapping> ActionKeyMapping;

		switch ((InputSetting)ButtonIndex)
		{
		case InputSetting::UP:
		{
			AxisKeyMapping = PlayerInput->GetKeysForAxis("Up");
			for (int i = 0; i < AxisKeyMapping.Num(); i++)
			{
				PlayerInput->AxisMappings.Remove(AxisKeyMapping[i]);
			}
			PlayerInput->AddAxisMapping(FInputAxisKeyMapping("Up", Input, 1.f));
		}
		break;
		case InputSetting::DOWN:
		{
			AxisKeyMapping = PlayerInput->GetKeysForAxis("Down");
			for (int i = 0; i < AxisKeyMapping.Num(); i++)
			{
				PlayerInput->AxisMappings.Remove(AxisKeyMapping[i]);
			}
			PlayerInput->AddAxisMapping(FInputAxisKeyMapping("Down", Input, -1.f));
		}
		break;
		case InputSetting::LEFT:
		{
			AxisKeyMapping = PlayerInput->GetKeysForAxis("Left");
			for (int i = 0; i < AxisKeyMapping.Num(); i++)
			{
				PlayerInput->AxisMappings.Remove(AxisKeyMapping[i]);
			}
			PlayerInput->AddAxisMapping(FInputAxisKeyMapping("Left", Input, -1.f));
		}
		break;
		case InputSetting::RIGHT:
		{
			AxisKeyMapping = PlayerInput->GetKeysForAxis("Right");
			for (int i = 0; i < AxisKeyMapping.Num(); i++)
			{
				PlayerInput->AxisMappings.Remove(AxisKeyMapping[i]);
			}
			PlayerInput->AddAxisMapping(FInputAxisKeyMapping("Right", Input, 1.f));
		}
		break;
		case InputSetting::ATTACK:
		{
			ActionKeyMapping = PlayerInput->GetKeysForAction("Attack");
			for (int i = 0; i < ActionKeyMapping.Num(); i++)
			{
				PlayerInput->ActionMappings.Remove(ActionKeyMapping[i]);
			}
			PlayerInput->AddActionMapping(FInputActionKeyMapping("Attack", Input));
		}
		break;
		case InputSetting::JUMP:
		{
			ActionKeyMapping = PlayerInput->GetKeysForAction("Jump");
			for (int i = 0; i < ActionKeyMapping.Num(); i++)
			{
				PlayerInput->ActionMappings.Remove(ActionKeyMapping[i]);
			}
			PlayerInput->AddActionMapping(FInputActionKeyMapping("Jump", Input));
		}
		break;
		}
		IsWaitingInput = false;
	}
	return FEventReply(true);
}

FEventReply UOptionWindow::OnMouseButtonDown(FGeometry MyGeometry, const FPointerEvent & MouseEvent)
{
	if (IsWaitingInput)
	{
		Input = MouseEvent.GetEffectingButton();
		InputSettingButtonTextList[ButtonIndex]->SetText(FText::FromString(Input.GetFName().ToString()));
		TArray<FInputAxisKeyMapping> AxisKeyMapping;
		TArray<FInputActionKeyMapping> ActionKeyMapping;

		switch ((InputSetting)ButtonIndex)
		{
		case InputSetting::UP:
		{
			AxisKeyMapping = PlayerInput->GetKeysForAxis("Up");
			for (int i = 0; i < AxisKeyMapping.Num(); i++)
			{
				PlayerInput->AxisMappings.Remove(AxisKeyMapping[i]);
			}
			PlayerInput->AddAxisMapping(FInputAxisKeyMapping("Up", Input, 1.f));
		}
		break;
		case InputSetting::DOWN:
		{
			AxisKeyMapping = PlayerInput->GetKeysForAxis("Down");
			for (int i = 0; i < AxisKeyMapping.Num(); i++)
			{
				PlayerInput->AxisMappings.Remove(AxisKeyMapping[i]);
			}
			PlayerInput->AddAxisMapping(FInputAxisKeyMapping("Down", Input, -1.f));
		}
		break;
		case InputSetting::LEFT:
		{
			AxisKeyMapping = PlayerInput->GetKeysForAxis("Left");
			for (int i = 0; i < AxisKeyMapping.Num(); i++)
			{
				PlayerInput->AxisMappings.Remove(AxisKeyMapping[i]);
			}
			PlayerInput->AddAxisMapping(FInputAxisKeyMapping("Left", Input, -1.f));
		}
		break;
		case InputSetting::RIGHT:
		{
			AxisKeyMapping = PlayerInput->GetKeysForAxis("Right");
			for (int i = 0; i < AxisKeyMapping.Num(); i++)
			{
				PlayerInput->AxisMappings.Remove(AxisKeyMapping[i]);
			}
			PlayerInput->AddAxisMapping(FInputAxisKeyMapping("Right", Input, 1.f));
		}
		break;
		case InputSetting::ATTACK:
		{
			ActionKeyMapping = PlayerInput->GetKeysForAction("Attack");
			for (int i = 0; i < ActionKeyMapping.Num(); i++)
			{
				PlayerInput->ActionMappings.Remove(ActionKeyMapping[i]);
			}
			PlayerInput->AddActionMapping(FInputActionKeyMapping("Attack", Input));
		}
		break;
		case InputSetting::JUMP:
		{
			ActionKeyMapping = PlayerInput->GetKeysForAction("Jump");
			for (int i = 0; i < ActionKeyMapping.Num(); i++)
			{
				PlayerInput->ActionMappings.Remove(ActionKeyMapping[i]);
			}
			PlayerInput->AddActionMapping(FInputActionKeyMapping("Jump", Input));
		}
		break;
		}
		IsWaitingInput = false;
	}
	return FEventReply(true);
}
