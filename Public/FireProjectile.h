// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/Actor.h"
#include "SmokeParticle.h"
#include "MuzzleFlashParticle.h"
#include "ProjectileStatComponent.h"
#include "GameFramework/ProjectileMovementComponent.h"
#include "FireProjectile.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API AFireProjectile : public AActor
{
	GENERATED_BODY()

private:
	bool m_OnHit;

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;
	virtual void EndPlay(const EEndPlayReason::Type EndPlayReason) override;

public:
	// Called every frame
	AFireProjectile();

	void Init(float Damage, float LifeSpan, float Range);

	UPROPERTY(VisibleDefaultsOnly, Category = Projectile)
		USphereComponent* CollisionComponent;

	UPROPERTY(VisibleAnywhere, Category = Movement)
		UProjectileMovementComponent* ProjectileMovementComponent;

	// 폭발 파티클 클래스
	UPROPERTY(EditDefaultsOnly)
		TSubclassOf<class AExplosionParticle> ExplosionClass;

	// 총구 화염 파티클 클래스
	UPROPERTY(EditDefaultsOnly)
		TSubclassOf<class AMuzzleFlashParticle> MuzzleFlashClass;

	// Smoke 파티클 클래스
	UPROPERTY(EditDefaultsOnly)
		TSubclassOf<class ASmokeParticle> SmokeClass;

	// 반복 호출 카운트
	int TimerCount;

	// Smoke 파티클 반복 생성 메소드
	void SpawnSmokeParticle();

	// 타이머 핸들
	FTimerHandle TimerHandle;

	void FireInDirection(const FVector& ShootDirection);

	UFUNCTION()
		void OnHit(UPrimitiveComponent* HitComponent, AActor* OtherActor, UPrimitiveComponent* OtherComponent, FVector NormalImpulse, const FHitResult& Hit);

private:
	float Damage;
	float ProjectileLifeSpan;
	float Range;
};
