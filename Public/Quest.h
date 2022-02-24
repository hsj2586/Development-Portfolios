// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/Actor.h"
#include "Quest.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API AQuest : public AActor
{
	GENERATED_BODY()

public:
	AQuest();

protected:
	virtual void BeginPlay() override;

	// 퀘스트 이름
	FString QuestName;

	// 퀘스트 요약
	FString QuestDescription;

	// 퀘스트의 시작 여부
	bool IsStarted;

public:
	// 퀘스트의 이름을 가져오는 함수
	void GetQuestName();

	// 퀘스트를 초기화하는 함수
	virtual void Init(class UDataTable* DataTable, int RowIndex);

	// 퀘스트를 시작하는 함수
	virtual void StartQuest();

	// 퀘스트가 완료되었는지 확인하는 함수
	virtual bool ConfirmQuestCompletion();

	// 퀘스트의 보상을 주는 함수
	virtual void GiveRewardToPlayer();
};
