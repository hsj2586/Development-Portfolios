// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/GameModeBase.h"
#include "Bluehole_projectGameModeBase.generated.h"

/**
 * 
 */
UCLASS()
class BLUEHOLE_PROJECT_API ABluehole_projectGameModeBase : public AGameModeBase
{
	GENERATED_BODY()

	ABluehole_projectGameModeBase();
public:
	virtual void PostLogin(APlayerController* NewPlayer) override;
};
