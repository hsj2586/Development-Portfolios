// Fill out your copyright notice in the Description page of Project Settings.

#include "MyPlayerState.h"
#include "MyGameInstance.h"
#include "MySaveGame.h"

AMyPlayerState::AMyPlayerState()
{
	CharacterLevel = 1;
	Exp = 0;
	PlayerClassName = "Gunner";
	SaveSoltName = TEXT("Player1");
}

void AMyPlayerState::SetPlayerClass(FString ClassName)
{
	PlayerClassName = ClassName;
}

int32 AMyPlayerState::GetCharacterLevel() const
{
	return CharacterLevel;
}

float AMyPlayerState::GetExpRatio() const
{
	if (CurrentStatData->NextExp <= KINDA_SMALL_NUMBER)
		return 0.0f;

	float Result = (float)Exp / (float)CurrentStatData->NextExp;
	return Result;
}

bool AMyPlayerState::AddExp(int32 IncomeExp)
{
	if (CurrentStatData->NextExp == -1)
		return false;

	bool DidLevelUp = false;
	Exp = Exp + IncomeExp;
	if (Exp >= CurrentStatData->NextExp)
	{
		Exp -= CurrentStatData->NextExp;
		SetCharacterLevel(CharacterLevel + 1);
		DidLevelUp = true;
	}

	OnPlayerStateChanged.Broadcast();
	SavePlayerData();
	return DidLevelUp;
}

void AMyPlayerState::InitPlayerData()
{
	// 로컬 플레이어 데이터 로드
	auto MySaveGame = Cast<UMySaveGame>(UGameplayStatics::LoadGameFromSlot(
		SaveSoltName, 0));
	if (MySaveGame == nullptr)
	{
		MySaveGame = GetMutableDefault<UMySaveGame>();
	}

	SetPlayerName(MySaveGame->PlayerName);
	SetPlayerClass(MySaveGame->PlayerClass);
	SetCharacterLevel(MySaveGame->Level);
	Exp = MySaveGame->Exp;
	SavePlayerData();
}

void AMyPlayerState::SavePlayerData()
{
	UMySaveGame* NewPlayerData = NewObject<UMySaveGame>();
	NewPlayerData->PlayerName = GetPlayerName();
	NewPlayerData->PlayerClass = PlayerClassName;
	NewPlayerData->Level = GetCharacterLevel();
	NewPlayerData->Exp = Exp;

	if (!UGameplayStatics::SaveGameToSlot(NewPlayerData, SaveSoltName, 0))
	{
		UE_LOG(LogTemp, Warning, TEXT("SAVE ERROR!"));
	}
}

void AMyPlayerState::SetCharacterLevel(int32 NewCharacterLevel)
{
	auto GameInstance = Cast<UMyGameInstance>(GetGameInstance());
	CurrentStatData = GameInstance->GetCharacterData(NewCharacterLevel);
	CharacterLevel = NewCharacterLevel;
}
