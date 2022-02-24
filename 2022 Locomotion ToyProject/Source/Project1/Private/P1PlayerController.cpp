// Fill out your copyright notice in the Description page of Project Settings.


#include "P1PlayerController.h"
#include <Runtime/Engine/Public/Net/UnrealNetwork.h>

AP1PlayerController::AP1PlayerController(const FObjectInitializer& ObjectInitializer)
	: Super(ObjectInitializer)
{
	ResetIgnoreInputFlags();
}

void AP1PlayerController::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
	Super::GetLifetimeReplicatedProps(OutLifetimeProps);

	DOREPLIFETIME(AP1PlayerController, IgnoreMoveInput_Replicated);
	DOREPLIFETIME(AP1PlayerController, IgnoreLookInput_Replicated);
}

void AP1PlayerController::SetIgnoreMoveInput(bool bNewMoveInput)
{
	IgnoreMoveInput_Replicated = FMath::Max(IgnoreMoveInput_Replicated + (bNewMoveInput ? +1 : -1), 0);
}

void AP1PlayerController::ResetIgnoreMoveInput()
{
	IgnoreMoveInput_Replicated = 0;
}

bool AP1PlayerController::IsMoveInputIgnored() const
{
	return (IgnoreMoveInput_Replicated > 0);
}

void AP1PlayerController::SetIgnoreLookInput(bool bNewLookInput)
{
	IgnoreLookInput_Replicated = FMath::Max(IgnoreLookInput_Replicated + (bNewLookInput ? +1 : -1), 0);
}

void AP1PlayerController::ResetIgnoreLookInput()
{
	IgnoreLookInput_Replicated = 0;
}

bool AP1PlayerController::IsLookInputIgnored() const
{
	return (IgnoreLookInput_Replicated > 0);
}

void AP1PlayerController::ResetIgnoreInputFlags()
{
	ResetIgnoreMoveInput();
	ResetIgnoreLookInput();
}

void AP1PlayerController::AddYawInput(float Val)
{
	RotationInput.Yaw += !IsLookInputIgnored() ? Val * InputYawScale : 0.f;
}
