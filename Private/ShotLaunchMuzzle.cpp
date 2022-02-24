// Fill out your copyright notice in the Description page of Project Settings.

#include "ShotLaunchMuzzle.h"

AShotLaunchMuzzle::AShotLaunchMuzzle()
{
	PrimaryActorTick.bCanEverTick = true;
	InitialLifeSpan = 1.5f;
}

void AShotLaunchMuzzle::BeginPlay()
{
	Super::BeginPlay();
}
