// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Camera/CameraShake.h"
#include "MyCameraShake.generated.h"

/**
 * 
 */
UCLASS()
class BLUEHOLE_PROJECT_API UMyCameraShake : public UCameraShake
{
	GENERATED_BODY()
	
public:
	UMyCameraShake();

public:
	UPROPERTY(EditAnywhere)
		TSubclassOf<UCameraShake> MyShake;
};
