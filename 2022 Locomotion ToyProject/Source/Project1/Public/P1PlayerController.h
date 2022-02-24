// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/PlayerController.h"
#include "P1PlayerController.generated.h"

/**
 * 
 */
UCLASS()
class PROJECT1_API AP1PlayerController : public APlayerController
{
	GENERATED_UCLASS_BODY()
public:
	void GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const override;

	virtual void SetIgnoreMoveInput(bool bNewMoveInput) override;
	virtual void ResetIgnoreMoveInput() override;
	virtual bool IsMoveInputIgnored() const override;
	
	virtual void SetIgnoreLookInput(bool bNewLookInput) override;
	virtual void ResetIgnoreLookInput() override;
	virtual bool IsLookInputIgnored() const override;
	virtual void ResetIgnoreInputFlags() override;

	virtual void AddYawInput(float Val);

private:
	/** Ignores movement input. Stacked state storage, Use accessor function IgnoreMoveInput() */
	UPROPERTY(Replicated)
	uint8 IgnoreMoveInput_Replicated;

	/** Ignores look input. Stacked state storage, use accessor function IgnoreLookInput(). */
	UPROPERTY(Replicated)
	uint8 IgnoreLookInput_Replicated;
};
