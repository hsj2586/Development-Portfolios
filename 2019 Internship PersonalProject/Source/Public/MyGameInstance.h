// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Engine/DataTable.h"
#include "Engine/GameInstance.h"
#include "MyGameInstance.generated.h"

// 캐릭터 데이터를 csv 형식으로 저장하기 위한 구조체
USTRUCT(BlueprintType)
struct FCharacterData : public FTableRowBase
{
	GENERATED_BODY()

public:
	FCharacterData() : Level(1), AttackType(0), NextExp(30), Health(100), Mana(50),
		AttackPower(30), JumpVelocity(400.0f), MoveSpeed(600.0f), MeshIndex(0),
		WeaponIndex(0), AnimIndex(0), MontageIndex(0), ProjectileIndex(0) {}

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int Level;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int AttackType;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int NextExp;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float Health;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float Mana;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float AttackPower;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float JumpVelocity;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float MoveSpeed;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int MeshIndex;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int WeaponIndex;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int AnimIndex;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int MontageIndex;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int ProjectileIndex;
};

// 스테이지(NPC 리스트) 데이터를 csv 형식으로 저장하기 위한 구조체
USTRUCT(BlueprintType)
struct FStageNPCData : public FTableRowBase
{
	GENERATED_BODY()

public:
	FStageNPCData() : Index(1), NPCName("Default"), XPos(0.0f), YPos(0.0f), ZPos(0.0f), RotateValue(0.0f) {}

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int Index;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		FString NPCName;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float XPos;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float YPos;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float ZPos;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float RotateValue;
};

// NPC 데이터를 csv 형식으로 저장하기 위한 구조체
USTRUCT(BlueprintType)
struct FNPCData : public FTableRowBase
{
	GENERATED_BODY()

public:
	FNPCData() : NPCName("Default"), MeshIndex(0), AnimIndex(0), QuestType(0), QuestIndex(0) {}

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		FString NPCName;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int MeshIndex;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int AnimIndex;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int QuestType;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int QuestIndex;
};

// 스테이지(몬스터 리스트) 데이터를 csv 형식으로 저장하기 위한 구조체
USTRUCT(BlueprintType)
struct FStageMonsterData : public FTableRowBase
{
	GENERATED_BODY()

public:
	FStageMonsterData() : Index(1), MName("Default"), XPos(0.0f), YPos(0.0f), ZPos(0.0f), RotateValue(0.0f) {}

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int Index;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		FString MName;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float XPos;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float YPos;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float ZPos;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float RotateValue;
};

// 몬스터 데이터를 csv 형식으로 저장하기 위한 구조체
USTRUCT(BlueprintType)
struct FMonsterData : public FTableRowBase
{
	GENERATED_BODY()

public:
	FMonsterData() : MName("Default"), MonsterType(0), AttackType(0), MeshIndex(0),
		WeaponIndex(0), AnimIndex(0), MontageIndex(0), ProjectileIndex(0),
		MeshScale(1), HealthPoint(100), AttackPower(0), AttackRange(0), MoveSpeed(0), dropExp(30),
		DetectRadius(0) {}

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		FString MName;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int MonsterType;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int AttackType;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int MeshIndex;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int WeaponIndex;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int AnimIndex;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int MontageIndex;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int ProjectileIndex;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float MeshScale;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float HealthPoint;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float AttackPower;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float AttackRange;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float MoveSpeed;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float dropExp;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float DetectRadius;
};

// 스킬 데이터를 csv 형식으로 저장하기 위한 구조체
USTRUCT(BlueprintType)
struct FSkillData : public FTableRowBase
{
	GENERATED_BODY()

public:
	FSkillData() : SkillName("Default"), Value(0), CoolTime(0), Mana(0), SkillImageIndex(0) {}

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		FString SkillName;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int Value;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float CoolTime;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float Mana;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		int SkillImageIndex;
};

// 'KillBoss' 퀘스트를 csv 형식으로 저장하기 위한 구조체
USTRUCT(BlueprintType)
struct FQuestData_KillBoss : public FTableRowBase
{
	GENERATED_BODY()

public:
	FQuestData_KillBoss() : QuestName("Default"), QuestDesc("Default"), MonsterName("Default"), RewardType(0), RewardValue(0) {}

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		FString QuestName;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		FString QuestDesc;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		FString MonsterName;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float RewardType;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float RewardValue;
};

// 'Interaction' 퀘스트를 csv 형식으로 저장하기 위한 구조체
USTRUCT(BlueprintType)
struct FQuestData_Interaction : public FTableRowBase
{
	GENERATED_BODY()

public:
	FQuestData_Interaction() : QuestName("Default"), QuestDesc("Default"), InteractionName("Default"), RewardType(0), RewardValue(0) {}

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		FString QuestName;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		FString QuestDesc;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		FString InteractionName;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float RewardType;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float RewardValue;
};

// 보상 정보를 csv 형식으로 저장하기 위한 구조체
USTRUCT(BlueprintType)
struct FQuestRewardData : public FTableRowBase
{
	GENERATED_BODY()

public:
	FQuestRewardData() : RewardName("Default"), RewardValue(0) {}

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		FString RewardName;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Data")
		float RewardValue;
};

UCLASS()
class BLUEHOLE_PROJECT_API UMyGameInstance : public UGameInstance
{
	GENERATED_BODY()
public:
	UMyGameInstance();

	virtual void Init() override;

	FCharacterData* GetCharacterData(int Level);

	UDataTable* GetStageNPCDataTable();

	FNPCData* GetNPCData(FName NPCName);

	UDataTable* GetStageMonsterDataTable();

	FMonsterData* GetMonsterData(FName MName);

	FSkillData* GetSkillData(int SkillNameIndex);

	int GetSkillIndex(FString SkillName);

	UDataTable* GetQuestDataTable_KillBoss();

	UDataTable* GetQuestDataTable_Interaction();

	FQuestRewardData* GetQuestRewardData(int Type);

	class AQuestManager* GetQuestManager();

private:
	UPROPERTY()
		class UDataTable* CharacterTable;

	UPROPERTY()
		class UDataTable* StageNPCTable;

	UPROPERTY()
		class UDataTable* NPCTable;

	UPROPERTY()
		class UDataTable* StageMonsterTable;

	UPROPERTY()
		class UDataTable* MonsterTable;

	UPROPERTY()
		class UDataTable* SkillTable;

	UPROPERTY()
		class UDataTable* QuestTable_KillBoss;

	UPROPERTY()
		class UDataTable* QuestTable_Interaction;

	UPROPERTY()
		class UDataTable* QuestRewardTable;

	UPROPERTY()
		class AQuestManager* QuestManager;
};