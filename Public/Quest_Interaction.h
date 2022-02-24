// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Quest.h"
#include "Quest_Interaction.generated.h"

/**
 * 
 */
UCLASS()
class BLUEHOLE_PROJECT_API AQuest_Interaction : public AQuest
{
	GENERATED_BODY()
	
public:
	// 퀘스트를 초기화하는 함수
	virtual void Init(class UDataTable* DataTable, int RowIndex) override;

	// 퀘스트를 시작하는 함수
	virtual void StartQuest() override;

	// 퀘스트가 완료되었는지 확인하는 함수
	virtual bool ConfirmQuestCompletion() override;

	// 퀘스트의 보상을 주는 함수
	virtual void GiveRewardToPlayer() override;
};
