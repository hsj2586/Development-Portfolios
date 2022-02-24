// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Quest.h"
#include "Quest_KillBoss.generated.h"

/**
 * 
 */
UCLASS()
class BLUEHOLE_PROJECT_API AQuest_KillBoss : public AQuest
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

private:
	// óġ�ؾ� �� ����
	class AMonster* BossMonster;

	// óġ�ؾ� �� ���� ������
	struct FQuestData_KillBoss* RowData;
};
