// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/Pawn.h"
#include "ExplosionParticle.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API AExplosionParticle : public APawn
{
	GENERATED_BODY()

public:
	AExplosionParticle();

protected:
	virtual void BeginPlay() override;

private:
	float Range;

public:
	virtual void Tick(float DeltaTime) override;

	void Execute(float Damage, float InitialLifeSpan,
		float DetectRadius);
};
