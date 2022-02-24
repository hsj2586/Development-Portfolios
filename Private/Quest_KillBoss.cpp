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
	// ���� ���� ������ �ε� ��, ���� �� �ʱ�ȭ
	FMonsterData* MonsterData = Cast<UMyGameInstance>(GetGameInstance())->GetMonsterData(*RowData->MonsterName);
	BossMonster = GetWorld()->SpawnActor<AMonster>();
	//BossMonster->Init(MonsterData);

	// �߰��� ������ ��ġ�� ��������� ��.
	// KillBoss ����Ʈ ������ ��Ʈ�� ��ġ ������ �߰��ؾ� �ҵ�.
	// �ӽ� �ڵ� ���.
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
