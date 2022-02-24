// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Components/ActorComponent.h"
#include "PoolableComponent.generated.h"

UCLASS( ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class BLUEHOLE_PROJECT_API UPoolableComponent : public UActorComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UPoolableComponent();

	// 풀 오브젝트 초기화
	UFUNCTION()
		void InitObject(class AObjectPool* ObjectPool);

	// 풀 오브젝트를 소멸
	UFUNCTION()
		void Despawn();

	// 풀 오브젝트를 소환
	UFUNCTION()
		void Spawn();

private:
	class AObjectPool* ObjectPool;
};
