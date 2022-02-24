// Fill out your copyright notice in the Description page of Project Settings.

#include "ObjectPool.h"
#include "PoolableComponent.h"

// Sets default values
AObjectPool::AObjectPool()
{
	PrimaryActorTick.bCanEverTick = true;

}

void AObjectPool::AddToPool(UPoolableComponent * PoolObject)
{
	PoolableObjectList.Add(PoolObject);

	if(!SpawnedObjectList.Find(PoolObject))
		SpawnedObjectList.Remove(PoolObject);
}

AActor* AObjectPool::RemoveFromPool()
{
	if (PoolableObjectList.Num() != 0)
	{
		UPoolableComponent* temp = PoolableObjectList[0];
		temp->Spawn();
		SpawnedObjectList.Add(temp);
		PoolableObjectList.Remove(temp);
		return temp->GetOwner();
	}
	else
		return nullptr;
}
