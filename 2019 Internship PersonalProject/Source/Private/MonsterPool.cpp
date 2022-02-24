// Fill out your copyright notice in the Description page of Project Settings.

#include "MonsterPool.h"


// Sets default values
AMonsterPool::AMonsterPool()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void AMonsterPool::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void AMonsterPool::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

