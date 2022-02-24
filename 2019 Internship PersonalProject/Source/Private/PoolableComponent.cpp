// Fill out your copyright notice in the Description page of Project Settings.

#include "PoolableComponent.h"
#include "ObjectPool.h"

UPoolableComponent::UPoolableComponent()
{
	PrimaryComponentTick.bCanEverTick = false;
}

void UPoolableComponent::InitObject(AObjectPool * ObjectPool_)
{
	this->ObjectPool = ObjectPool_;
	GetOwner()->SetActorHiddenInGame(true);
	GetOwner()->SetActorEnableCollision(false);
	GetOwner()->bCanBeDamaged = false;
	GetOwner()->SetActorTickEnabled(false);
}

void UPoolableComponent::Despawn()
{
	GetOwner()->SetActorHiddenInGame(true);
	GetOwner()->SetActorEnableCollision(false);
	GetOwner()->bCanBeDamaged = false;
	GetOwner()->SetActorTickEnabled(false);

	ObjectPool->AddToPool(this);
}

void UPoolableComponent::Spawn()
{
	GetOwner()->SetActorHiddenInGame(false);
	GetOwner()->SetActorEnableCollision(true);
	GetOwner()->bCanBeDamaged = true;
	GetOwner()->SetActorTickEnabled(true);
}
