// Fill out your copyright notice in the Description page of Project Settings.

#include "ExplosionParticle.h"
#include "DrawDebugHelpers.h"
#include "Monster.h"
#include "Components/SphereComponent.h"

AExplosionParticle::AExplosionParticle()
{
	PrimaryActorTick.bCanEverTick = true;
	Range = 150.0f;
}

void AExplosionParticle::BeginPlay()
{
	Super::BeginPlay();
}

void AExplosionParticle::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);
}

void AExplosionParticle::Execute(float Damage, float InitialLifeSpan, float DetectRadius)
{
	if (!GetWorld()) return;

	SetLifeSpan(InitialLifeSpan);

	TArray<FOverlapResult> OverlapResults;
	FCollisionQueryParams CollisionQueryParam(NAME_None, false, this);
	bool bResult = GetWorld()->OverlapMultiByChannel(
		OverlapResults,
		GetActorLocation(),
		FQuat::Identity,
		ECollisionChannel::ECC_GameTraceChannel1,
		FCollisionShape::MakeSphere(DetectRadius),
		CollisionQueryParam
	);

	if (bResult)
	{
		for (auto OverlapResult : OverlapResults)
		{
			AMonster* Monster = Cast<AMonster>(OverlapResult.GetActor());
			if (Monster)
			{
				FDamageEvent DamageEvent;
				Monster->TakeDamage(Damage, DamageEvent, GetController(), this);
			}
		}
	}
}