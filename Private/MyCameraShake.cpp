// Fill out your copyright notice in the Description page of Project Settings.

#include "MyCameraShake.h"

UMyCameraShake::UMyCameraShake()
{
	OscillationDuration = 0.3f;
	OscillationBlendInTime = 0.05f;
	OscillationBlendOutTime = 0.05f;

	RotOscillation.Pitch.Amplitude = FMath::RandRange(0.5f, 1.2f);
	RotOscillation.Pitch.Frequency = FMath::RandRange(40.0f, 50.0f);

	RotOscillation.Yaw.Amplitude = FMath::RandRange(0.5f, 1.2f);
	RotOscillation.Yaw.Frequency = FMath::RandRange(40.0f, 50.0f);
}