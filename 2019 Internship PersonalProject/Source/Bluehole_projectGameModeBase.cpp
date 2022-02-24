// Fill out your copyright notice in the Description page of Project Settings.

#include "Bluehole_projectGameModeBase.h"
#include "MyCharacter.h"
#include "Monster.h"
#include "MyPlayerController.h"
#include "MyPlayerState.h"
#include "MyGameInstance.h"
#include "NonePlayerCharacter.h"
#include "Runtime/Engine/Public/EngineUtils.h"
#include "ObjectPool.h"
#include "PoolableComponent.h"
#include <time.h>
#include "CharacterSetting.h"

ABluehole_projectGameModeBase::ABluehole_projectGameModeBase()
{
	DefaultPawnClass = AMyCharacter::StaticClass();
	PlayerControllerClass = AMyPlayerController::StaticClass();
	PlayerStateClass = AMyPlayerState::StaticClass();
	MonsterClass = AMonster::StaticClass();
	NPCClass = ANonePlayerCharacter::StaticClass();
}

void ABluehole_projectGameModeBase::PostLogin(APlayerController * NewPlayer)
{
	Super::PostLogin(NewPlayer);
	auto PlayerState = Cast<AMyPlayerState>(NewPlayer->PlayerState);
	PlayerState->InitPlayerData();

	// �÷��̾� ��Ʈ�ѷ� �α��� ����, �������� ���� ����
	auto GameInstance = Cast<UMyGameInstance>(GetGameInstance());
	auto StageNPCDataTable = GameInstance->GetStageNPCDataTable();
	auto StageMonsterDataTable = GameInstance->GetStageMonsterDataTable();

	// �������� ������ �ε�
	TArray<FStageNPCData*> NPCDataList;
	StageNPCDataTable->GetAllRows<FStageNPCData>(TEXT(""), NPCDataList);

	TArray<FStageMonsterData*> MonsterDataList;
	StageMonsterDataTable->GetAllRows<FStageMonsterData>(TEXT(""), MonsterDataList);

	// ���� ������Ʈ Ǯ ���� �� ���� �Ҵ�
	MonsterObjectPool = GetWorld()->SpawnActor<AObjectPool>();
	if (MonsterObjectPool)
	{
		for (int i = 0; i < 20; i++)
		{
			for (int j = 0; j < 25; j++)
			{
				FVector Pos = FVector(-17000 + i * 900, -15000 + j * 966, 1000);
				AActor* tempMonster = GetWorld()->SpawnActor<AMonster>(MonsterClass, Pos, FRotator(0, 0, 0));
				if (tempMonster)
				{
					if (tempMonster->FindComponentByClass<UPoolableComponent>())
					{
						tempMonster->FindComponentByClass<UPoolableComponent>()->InitObject(MonsterObjectPool);
						MonsterObjectPool->AddToPool(tempMonster->FindComponentByClass<UPoolableComponent>());
					}
				}
			}
		}
	}

	// ���� ���� ����
	UE_LOG(LogTemp, Warning, TEXT("Spawning Start"));
	clock_t start, end;
	start = clock();

	// ���� ������ �ε�
	auto DefaultSetting = GetDefault<UCharacterSetting>();
	FMonsterData* MonsterData = GameInstance->GetMonsterData(*MonsterDataList[0]->MName);
	FSoftObjectPath MeshAssetPath = DefaultSetting->MonsterMeshAssets[MonsterData->MeshIndex];
	FSoftObjectPath WeaponAssetPath = DefaultSetting->MonsterWeaponAssets[MonsterData->WeaponIndex];
	FSoftObjectPath AnimAssetPath = DefaultSetting->MonsterAnimAssets[MonsterData->AnimIndex];
	FSoftObjectPath MontageAssetPath = DefaultSetting->MonsterMontageAssets[MonsterData->MontageIndex];	
	
	// ���� ���ҽ� �ε�
	USkeletalMesh* NewMesh = Cast<USkeletalMesh>(StaticLoadObject(USkeletalMesh::StaticClass(), NULL, *MeshAssetPath.ToString()));
	UStaticMesh* NewWeaponMesh = Cast<UStaticMesh>(StaticLoadObject(UStaticMesh::StaticClass(), NULL, *WeaponAssetPath.ToString()));
	UClass* NewAnim = StaticLoadClass(UMonsterAnimInstance::StaticClass(), NULL, *AnimAssetPath.ToString());
	UAnimMontage* NewAnimMontage = Cast<UAnimMontage>(StaticLoadObject(UAnimMontage::StaticClass(), NULL, *MontageAssetPath.ToString()));
	

	for (int i = 0; i < 20; i++)
	{
		for (int j = 0; j < 25; j++)
		{
			FVector Pos = FVector(-17000 + i * 900, -15000 + j * 966, 1000);
			AMonster* Monster = Cast<AMonster>(MonsterObjectPool->RemoveFromPool());
			Monster->SetActorLocation(Pos);
			Monster->Init(MonsterData, NewMesh, NewWeaponMesh, NewAnim, NewAnimMontage);
		}
	}
	end = clock();
	float result = (float)(end - start);
	UE_LOG(LogTemp, Warning, TEXT("Spawning End Time : %f ms"), result);

#pragma region TestCode
	//auto DefaultSetting = GetDefault<UCharacterSetting>();
	//FMonsterData* MonsterData = GameInstance->GetMonsterData(*MonsterDataList[1]->MName);
	//FSoftObjectPath MeshAssetPath = DefaultSetting->MonsterMeshAssets[MonsterData->MeshIndex];
	//FSoftObjectPath WeaponAssetPath = DefaultSetting->MonsterWeaponAssets[MonsterData->WeaponIndex];
	//FSoftObjectPath AnimAssetPath = DefaultSetting->MonsterAnimAssets[MonsterData->AnimIndex];
	//FSoftObjectPath MontageAssetPath = DefaultSetting->MonsterMontageAssets[MonsterData->MontageIndex];

	//// ���� ���ҽ� �ε�
	//USkeletalMesh* NewMesh = Cast<USkeletalMesh>(StaticLoadObject(USkeletalMesh::StaticClass(), NULL, *MeshAssetPath.ToString()));
	//UStaticMesh* NewWeaponMesh = Cast<UStaticMesh>(StaticLoadObject(UStaticMesh::StaticClass(), NULL, *WeaponAssetPath.ToString()));
	//UClass* NewAnim = StaticLoadClass(UMonsterAnimInstance::StaticClass(), NULL, *AnimAssetPath.ToString());
	//UAnimMontage* NewAnimMontage = Cast<UAnimMontage>(StaticLoadObject(UAnimMontage::StaticClass(), NULL, *MontageAssetPath.ToString()));

	//FRotator Rot = FRotator(0, 0, 0);

	//FVector Pos = FVector(-57, 1811, 120);
	//AMonster* Monster = Cast<AMonster>(MonsterObjectPool->RemoveFromPool());
	//Monster->SetActorLocation(Pos);
	//Monster->Init(MonsterData, NewMesh, NewWeaponMesh, NewAnim, NewAnimMontage);
#pragma endregion
}

void ABluehole_projectGameModeBase::SetUserState(ECharacterState NewState)
{
	UserState = NewState;
}

ECharacterState ABluehole_projectGameModeBase::GetUserState() const
{
	return UserState;
}