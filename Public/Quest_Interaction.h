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
	// ����Ʈ�� �ʱ�ȭ�ϴ� �Լ�
	virtual void Init(class UDataTable* DataTable, int RowIndex) override;

	// ����Ʈ�� �����ϴ� �Լ�
	virtual void StartQuest() override;

	// ����Ʈ�� �Ϸ�Ǿ����� Ȯ���ϴ� �Լ�
	virtual bool ConfirmQuestCompletion() override;

	// ����Ʈ�� ������ �ִ� �Լ�
	virtual void GiveRewardToPlayer() override;
};
