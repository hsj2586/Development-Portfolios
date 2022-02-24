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

	// ����Ʈ�� �߰��ϴ� �Լ�
	void AddQuest(class AQuest* Quest);
	
	// ����Ʈ�� ����Ϸ� ó���ϴ� �Լ�
	void CompleteQuest(int QuestIndex);

private:
	UPROPERTY(VisibleInstanceOnly, Category = Quest)
	TArray<class AQuest*> QuestList;
};
