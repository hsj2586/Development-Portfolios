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

	// NPC �ʱ�ȭ �Լ�
	void Init(struct FNPCData* NPCData);

	// ����Ʈ�� �ִ� �ൿ �Լ�
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

	// NPC �̸�
	FString NPCName;

	// ����Ʈ �Ŵ���
	class AQuestManager* QuestManager;

	// NPC�� ���� ����Ʈ
	class AQuest* havingQuest;
};
