// Fill out your copyright notice in the Description page of Project Settings.

#include "InputBufferManager.h"
#include "MyPlayerController.h"

UInputBufferManager::UInputBufferManager()
{
	PrimaryComponentTick.bCanEverTick = true;
}

void UInputBufferManager::InsertInput(FKey Input)
{
	KeyInputBuffer.Add(Input);
	InputIntervalBuffer.Add(Cast<AMyPlayerController>(GetOwner())->ElapsTime);

	FTimerHandle TimerHandle;
	GetWorld()->GetTimerManager().SetTimer(TimerHandle, this, &UInputBufferManager::PopInput, 1, false);

	//UE_LOG(LogTemp, Warning, TEXT("%s input is inserted, input interval is %f"),
		//*KeyInputBuffer[KeyInputBuffer.Num() - 1].GetFName().ToString(), InputIntervalBuffer[InputIntervalBuffer.Num() - 1]);
}

void UInputBufferManager::PopInput()
{
	if (KeyInputBuffer.Num() != 0)
	{
		//UE_LOG(LogTemp, Warning, TEXT("%s input is deleted"),
			//*KeyInputBuffer[0].GetFName().ToString());

		KeyInputBuffer.RemoveAt(0);
		InputIntervalBuffer.Remove(0);
	}
}

bool UInputBufferManager::CheckCommand(SkillType CommandName)
{
	//UE_LOG(LogTemp, Warning, TEXT("Check Command"));
	TArray<FKey> Command = GetCommand(CommandName);

	// 입력 버퍼와 커맨드 길이 비교
	if (KeyInputBuffer.Num() > Command.Num())
	{
		for (int i = 0; i < Command.Num(); i++)
		{
			// 커맨드 비교 및 시간 차 조건 확인 
			if (KeyInputBuffer[KeyInputBuffer.Num() - 2 - i].GetFName() != Command[Command.Num() - i - 1].GetFName() ||
				InputIntervalBuffer[InputIntervalBuffer.Num() - i - 1] > 0.2f)
			{
				return false;
			}
		}
		//UE_LOG(LogTemp, Warning, TEXT("COMMAND ON"));
		return true;
	}
	return false;
}

TArray<FKey> UInputBufferManager::GetCommand(SkillType CommandName)
{
	TArray<FKey> Command;

	switch (CommandName)
	{
	case SkillType::Rolling:
	{
		Command.Add(FKey("A"));
		Command.Add(FKey("S"));
		Command.Add(FKey("D"));
		break;
	}

	case SkillType::ShotLaunch:
	{
		Command.Add(FKey("W"));
		Command.Add(FKey("S"));
		break;
	}

	case SkillType::VisionShot:
	{
		Command.Add(FKey("A"));
		Command.Add(FKey("D"));
		break;
	}
	}
	return Command;
}