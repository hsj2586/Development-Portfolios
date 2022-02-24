// Fill out your copyright notice in the Description page of Project Settings.

#include "Quest_KillBoss.h"
#include "Engine/DataTable.h"
#include "MyGameInstance.h"
#include "Monster.h"

void AQuest_KillBoss::Init(UDataTable* DataTable, int RowIndex)
{
	Super::Init(DataTable, RowIndex);
	RowData = DataTable->FindRow<FQuestData_KillBoss>(*FString::FromInt(RowIndex), TEXT(""));
	QuestName = RowData->QuestName;
	QuestDescription = RowData->QuestDesc;
}

void AQuest_KillBoss::StartQuest()
{
	// 보스 몬스터 데이터 로드 후, 스폰 및 초기화
	FMonsterData* MonsterData = Cast<UMyGameInstance>(GetGameInstance())->GetMonsterData(*RowData->MonsterName);
	BossMonster = GetWorld()->SpawnActor<AMonster>();
	//BossMonster->Init(MonsterData);

	// 추가로 몬스터의 위치를 지정해줘야 함.
	// KillBoss 퀘스트 데이터 시트에 위치 정보를 추가해야 할듯.
	// 임시 코드 사용.
	BossMonster->SetActorLocation(FVector(645.0f, 3210.0f, 132.0f));

	IsStarted = true;
}

bool AQuest_KillBoss::ConfirmQuestCompletion()
{
	if (IsStarted)
	{
		if (!BossMonster)
		{
			UE_LOG(LogTemp, Warning, TEXT("KillBoss Quest Completed"));
			return true;
		}
	}
	return false;
}

void AQuest_KillBoss::GiveRewardToPlayer()
{
}
