// Fill out your copyright notice in the Description page of Project Settings.

#include "QuestManager.h"
#include "Quest.h"

AQuestManager::AQuestManager()
{
	PrimaryActorTick.bCanEverTick = true;
}

void AQuestManager::BeginPlay()
{
	Super::BeginPlay();
	
}

void AQuestManager::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

	for (int i = 0; i < QuestList.Num(); i++)
	{
		if (QuestList[i]->ConfirmQuestCompletion())
		{
			// 퀘스트 수행 완료일 경우
			CompleteQuest(i);
		}
	}
}

void AQuestManager::AddQuest(AQuest * Quest)
{
	UE_LOG(LogTemp, Warning, TEXT("Add Quest"));
	QuestList.Add(Quest);
}

void AQuestManager::CompleteQuest(int QuestIndex)
{
	QuestList.Remove(QuestList[QuestIndex]);
}