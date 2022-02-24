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

	// ����Ʈ �̸�
	FString QuestName;

	// ����Ʈ ���
	FString QuestDescription;

	// ����Ʈ�� ���� ����
	bool IsStarted;

public:
	// ����Ʈ�� �̸��� �������� �Լ�
	void GetQuestName();

	// ����Ʈ�� �ʱ�ȭ�ϴ� �Լ�
	virtual void Init(class UDataTable* DataTable, int RowIndex);

	// ����Ʈ�� �����ϴ� �Լ�
	virtual void StartQuest();

	// ����Ʈ�� �Ϸ�Ǿ����� Ȯ���ϴ� �Լ�
	virtual bool ConfirmQuestCompletion();

	// ����Ʈ�� ������ �ִ� �Լ�
	virtual void GiveRewardToPlayer();
};
