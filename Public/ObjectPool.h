// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/Actor.h"
#include "ObjectPool.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API AObjectPool : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AObjectPool();

	void AddToPool(class UPoolableComponent* PoolObject);
 
	AActor* RemoveFromPool();

private:
	// ������ ������Ʈ�� ����Ʈ
	TArray<class UPoolableComponent*> SpawnedObjectList;

	// Ǯ���� ������ ������Ʈ�� ����Ʈ
	TArray<class UPoolableComponent*> PoolableObjectList;
};
