// Fill out your copyright notice in the Description page of Project Settings.

#include "MyGameInstance.h"
#include "QuestManager.h"

// 게임 데이터를 세팅해주는 인스턴스

UMyGameInstance::UMyGameInstance()
{
	FString CharacterDataPath = TEXT("/Game/GameData/CharacterData.CharacterData");
	static ConstructorHelpers::FObjectFinder<UDataTable> DT_Character(*CharacterDataPath);
	if (DT_Character.Succeeded())
	{
		CharacterTable = DT_Character.Object;
	}

	FString StageNPCDataPath = TEXT("/Game/GameData/StageNPCData.StageNPCData");
	static ConstructorHelpers::FObjectFinder<UDataTable> DT_StageNPC(*StageNPCDataPath);
	if (DT_StageNPC.Succeeded())
	{
		StageNPCTable = DT_StageNPC.Object;
	}

	FString NPCDataPath = TEXT("/Game/GameData/NPCData.NPCData");
	static ConstructorHelpers::FObjectFinder<UDataTable> DT_NPC(*NPCDataPath);
	if (DT_NPC.Succeeded())
	{
		NPCTable = DT_NPC.Object;
	}

	FString StageMonsterDataPath = TEXT("/Game/GameData/StageMonsterData.StageMonsterData");
	static ConstructorHelpers::FObjectFinder<UDataTable> DT_StageMonster(*StageMonsterDataPath);
	if (DT_StageMonster.Succeeded())
	{
		StageMonsterTable = DT_StageMonster.Object;
	}

	FString MonsterDataPath = TEXT("/Game/GameData/MonsterData.MonsterData");
	static ConstructorHelpers::FObjectFinder<UDataTable> DT_Monster(*MonsterDataPath);
	if (DT_Monster.Succeeded())
	{
		MonsterTable = DT_Monster.Object;
	}

	FString SkillDataPath = TEXT("/Game/GameData/SkillData.SkillData");
	static ConstructorHelpers::FObjectFinder<UDataTable> DT_Skill(*SkillDataPath);
	if (DT_Skill.Succeeded())
	{
		SkillTable = DT_Skill.Object;
	}

	FString QuestDataPath_KB = TEXT("/Game/GameData/QuestData_KillBoss.QuestData_KillBoss");
	static ConstructorHelpers::FObjectFinder<UDataTable> DT_QDP_KB(*QuestDataPath_KB);
	if (DT_QDP_KB.Succeeded())
	{
		QuestTable_KillBoss = DT_QDP_KB.Object;
	}

	FString QuestDataPath_IT = TEXT("/Game/GameData/QuestData_Interaction.QuestData_Interaction");
	static ConstructorHelpers::FObjectFinder<UDataTable> DT_QDP_IT(*QuestDataPath_IT);
	if (DT_QDP_IT.Succeeded())
	{
		QuestTable_Interaction = DT_QDP_IT.Object;
	}

	FString RewardDataPath = TEXT("/Game/GameData/QuestRewardData.QuestRewardData");
	static ConstructorHelpers::FObjectFinder<UDataTable> DT_Reward(*RewardDataPath);
	if (DT_Reward.Succeeded())
	{
		QuestRewardTable = DT_Reward.Object;
	}
}

void UMyGameInstance::Init()
{
	Super::Init();

	// 퀘스트 매니저 스폰
	QuestManager = GetWorld()->SpawnActor<AQuestManager>();
}

FCharacterData * UMyGameInstance::GetCharacterData(int Level)
{
	return CharacterTable->FindRow<FCharacterData>(*FString::FromInt(Level), TEXT(""));
}

UDataTable * UMyGameInstance::GetStageNPCDataTable()
{
	return StageNPCTable;
}

UDataTable * UMyGameInstance::GetStageMonsterDataTable()
{
	return StageMonsterTable;
}

FMonsterData * UMyGameInstance::GetMonsterData(FName MName)
{
	return MonsterTable->FindRow<FMonsterData>(MName, TEXT(""));
}

FNPCData * UMyGameInstance::GetNPCData(FName NPCName)
{
	return NPCTable->FindRow<FNPCData>(NPCName, TEXT(""));
}

FSkillData * UMyGameInstance::GetSkillData(int SkillNameIndex)
{
	return SkillTable->FindRow<FSkillData>(*FString::FromInt(SkillNameIndex), TEXT(""));
}

int UMyGameInstance::GetSkillIndex(FString SkillName)
{
	TArray<FSkillData*> SkillDatas;
	SkillTable->GetAllRows(TEXT(""), SkillDatas);
	for (int i = 0; i < SkillDatas.Num(); i++)
	{
		if (SkillDatas[i]->SkillName == SkillName)
			return i;
	}
	return -1;
}

UDataTable * UMyGameInstance::GetQuestDataTable_KillBoss()
{
	return QuestTable_KillBoss;
}

UDataTable * UMyGameInstance::GetQuestDataTable_Interaction()
{
	return QuestTable_Interaction;
}

FQuestRewardData * UMyGameInstance::GetQuestRewardData(int Type)
{
	return QuestRewardTable->FindRow<FQuestRewardData>(*FString::FromInt(Type), TEXT(""));
}

AQuestManager * UMyGameInstance::GetQuestManager()
{
	return QuestManager;
}
