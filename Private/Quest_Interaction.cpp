// Fill out your copyright notice in the Description page of Project Settings.

#include "Quest_Interaction.h"

void AQuest_Interaction::Init(UDataTable* DataTable, int RowIndex)
{
	Super::Init(DataTable, RowIndex);
}

void AQuest_Interaction::StartQuest()
{
}

bool AQuest_Interaction::ConfirmQuestCompletion()
{
	return false;
}

void AQuest_Interaction::GiveRewardToPlayer()
{
}
