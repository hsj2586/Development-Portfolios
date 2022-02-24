// Copyright Epic Games, Inc. All Rights Reserved.


#include "Project1GameModeBase.h"
#include "P1Player.h"
#include "P1PlayerController.h"
#include "UObject/ConstructorHelpers.h"

AProject1GameModeBase::AProject1GameModeBase()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnBPClass(TEXT("/Game/Blueprints/P1Player_BP"));
	if (PlayerPawnBPClass.Class != NULL)
	{
		DefaultPawnClass = PlayerPawnBPClass.Class;
	}
	PlayerControllerClass = AP1PlayerController::StaticClass();
}
