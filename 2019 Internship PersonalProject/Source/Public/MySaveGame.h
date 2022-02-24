// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/SaveGame.h"
#include "MySaveGame.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API UMySaveGame : public USaveGame
{
	GENERATED_BODY()

public:
	UMySaveGame();

	// 플레이어 이름
	UPROPERTY()
		FString PlayerName;

	// 플레이어 직업
	UPROPERTY()
		FString PlayerClass;

	// 플레이어 레벨
	UPROPERTY()
		int32 Level;

	// 플레이어 경험치
	UPROPERTY()
		int32 Exp;
};
