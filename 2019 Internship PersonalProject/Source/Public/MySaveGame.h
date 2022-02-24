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

	// �÷��̾� �̸�
	UPROPERTY()
		FString PlayerName;

	// �÷��̾� ����
	UPROPERTY()
		FString PlayerClass;

	// �÷��̾� ����
	UPROPERTY()
		int32 Level;

	// �÷��̾� ����ġ
	UPROPERTY()
		int32 Exp;
};
