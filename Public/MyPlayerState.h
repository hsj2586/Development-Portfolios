// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/PlayerState.h"
#include "MyPlayerState.generated.h"

DECLARE_MULTICAST_DELEGATE(FOnPlayerStateChangedDelegate);

/**
 * 
 */
UCLASS()
class BLUEHOLE_PROJECT_API AMyPlayerState : public APlayerState
{
	GENERATED_BODY()
	
public:
	AMyPlayerState();

	FString SaveSoltName;

	void SetPlayerClass(FString ClassName);
	int32 GetCharacterLevel() const;
	float GetExpRatio() const;
	bool AddExp(int32 IncomeExp);

	void InitPlayerData();
	void SavePlayerData();
	
	FOnPlayerStateChangedDelegate OnPlayerStateChanged;

protected:
	UPROPERTY(Transient)
		FString PlayerClassName;

	UPROPERTY(Transient)
		int32 CharacterLevel;
	
	UPROPERTY(Transient)
		int32 Exp;

private:
	void SetCharacterLevel(int32 NewCharacterLevel);
	struct FCharacterData* CurrentStatData;
};
