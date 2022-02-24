// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/Actor.h"
#include "QuestManager.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API AQuestManager : public AActor
{
	GENERATED_BODY()
	
public:	
	AQuestManager();

protected:
	virtual void BeginPlay() override;

public:
	virtual void Tick(float DeltaTime) override;

	// 퀘스트를 추가하는 함수
	void AddQuest(class AQuest* Quest);
	
	// 퀘스트를 수행완료 처리하는 함수
	void CompleteQuest(int QuestIndex);

private:
	UPROPERTY(VisibleInstanceOnly, Category = Quest)
	TArray<class AQuest*> QuestList;
};
