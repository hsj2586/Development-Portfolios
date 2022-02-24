// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/Character.h"
#include "NonePlayerCharacter.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API ANonePlayerCharacter : public ACharacter
{
	GENERATED_BODY()

public:
	ANonePlayerCharacter();

protected:
	virtual void BeginPlay() override;

	virtual void PostInitializeComponents() override;

public:

	// NPC 초기화 함수
	void Init(struct FNPCData* NPCData);

	// 퀘스트를 주는 행동 함수
	void GiveAQuest();

	UFUNCTION()
		void OnOverlapBegin(class UPrimitiveComponent* OverlappedComp, class AActor* OtherActor, class UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult);

	UFUNCTION()
		void OnOverlapEnd(class UPrimitiveComponent* OverlappedComp, class AActor* OtherActor, class UPrimitiveComponent* OtherComp, int32 OtherBodyIndex);

private:
	UPROPERTY(VisibleAnywhere)
		USphereComponent* Trigger;

	FSoftObjectPath MeshAssetPath;

	FSoftClassPath AnimAssetPath;

	// NPC 이름
	FString NPCName;

	// 퀘스트 매니저
	class AQuestManager* QuestManager;

	// NPC가 가진 퀘스트
	class AQuest* havingQuest;
};
