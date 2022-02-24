#pragma once

#include "Bluehole_project.h"
#include "GameFramework/Actor.h"
#include "ProjectileStatComponent.h"
#include "GameFramework/ProjectileMovementComponent.h"
#include "VisionShotProjectile.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API AVisionShotProjectile : public AActor
{
	GENERATED_BODY()

protected:
	virtual void EndPlay(EEndPlayReason::Type Reason) override;

public:
	AVisionShotProjectile();

	// 자신을 생성한 VisionShot을 초기화
	void Init(class UVisionShot* VisionShot_, float Damage, float LifeSpan, float Range);

	void FireInDirection(const FVector& ShootDirection);

	UPROPERTY(VisibleAnywhere, Category = Projectile)
		UProjectileStatComponent* ProjectileStatComponent;

	UPROPERTY(VisibleDefaultsOnly, Category = Projectile)
		USphereComponent* CollisionComponent;

	UPROPERTY(VisibleAnywhere, Category = Movement)
		UProjectileMovementComponent* ProjectileMovementComponent;

	UPROPERTY(VisibleAnywhere)
		class UVisionShot* VisionShot;

	UFUNCTION()
		void OnHit(UPrimitiveComponent* HitComponent, AActor* OtherActor, UPrimitiveComponent* OtherComponent, FVector NormalImpulse, const FHitResult& Hit);

	UFUNCTION()
		void OnOverlapBegin(class UPrimitiveComponent* OverlappedComp, class AActor* OtherActor, class UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult);

private:
	float Damage;
	float ProjectileLifeSpan;
	float Range;
};