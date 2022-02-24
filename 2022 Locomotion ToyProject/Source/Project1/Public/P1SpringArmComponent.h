// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/SpringArmComponent.h"
#include "P1SpringArmComponent.generated.h"

/**
 * 
 */
UCLASS()
class PROJECT1_API UP1SpringArmComponent : public USpringArmComponent
{
	GENERATED_BODY()
	
	/** Updates the desired arm location, calling BlendLocations to do the actual blending if a trace is done */
	virtual void UpdateDesiredArmLocation(bool bDoTrace, bool bDoLocationLag, bool bDoRotationLag, float DeltaTime) override;
};
