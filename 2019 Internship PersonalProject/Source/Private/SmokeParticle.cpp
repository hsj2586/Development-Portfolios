// Fill out your copyright notice in the Description page of Project Settings.

#include "SmokeParticle.h"


// Sets default values
ASmokeParticle::ASmokeParticle()
{
 	// Set this pawn to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
	InitialLifeSpan = 1.5f;
}

// Called when the game starts or when spawned
void ASmokeParticle::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void ASmokeParticle::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

// Called to bind functionality to input
void ASmokeParticle::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);

}

