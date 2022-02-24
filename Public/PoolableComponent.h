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

	// Ǯ ������Ʈ �ʱ�ȭ
	UFUNCTION()
		void InitObject(class AObjectPool* ObjectPool);

	// Ǯ ������Ʈ�� �Ҹ�
	UFUNCTION()
		void Despawn();

	// Ǯ ������Ʈ�� ��ȯ
	UFUNCTION()
		void Spawn();

private:
	class AObjectPool* ObjectPool;
};
