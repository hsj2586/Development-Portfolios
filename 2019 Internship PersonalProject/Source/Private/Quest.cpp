// Fill out your copyright notice in the Description page of Project Settings.

#include "Quest.h"
#include "Engine/DataTable.h"

AQuest::AQuest()
{
	PrimaryActorTick.bCanEverTick = true;
	IsStarted = false;
}

void AQuest::BeginPlay()
{
	Super::BeginPlay();
	
}

void AQuest::GetQuestName()
{
}

void AQuest::Init(UDataTable * DataTable, int RowIndex)
{
}

void AQuest::StartQuest()
{
}

bool AQuest::ConfirmQuestCompletion()
{
	return false;
}

void AQuest::GiveRewardToPlayer()
{
}
