// Fill out your copyright notice in the Description page of Project Settings.

#include "Bluehole_projectGameModeBase.h"
#include "MyCharacter.h"
#include "MyPlayerController.h"

ABluehole_projectGameModeBase::ABluehole_projectGameModeBase()
{
	DefaultPawnClass = AMyCharacter::StaticClass();
	PlayerControllerClass = AMyPlayerController::StaticClass();
}

void ABluehole_projectGameModeBase::PostLogin(APlayerController * NewPlayer)
{
	Super::PostLogin(NewPlayer);
}
