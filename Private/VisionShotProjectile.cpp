
#include "VisionShotProjectile.h"
#include "VisionShot.h"

AVisionShotProjectile::AVisionShotProjectile()
{
	InitialLifeSpan = 2.0f;
	CollisionComponent = CreateDefaultSubobject<USphereComponent>(TEXT("SphereComponent"));
	CollisionComponent->BodyInstance.SetCollisionProfileName(TEXT("VisionProjectile"));
	CollisionComponent->OnComponentHit.AddDynamic(this, &AVisionShotProjectile::OnHit);
	CollisionComponent->OnComponentBeginOverlap.AddDynamic(this, &AVisionShotProjectile::OnOverlapBegin);
	RootComponent = CollisionComponent;

	ProjectileMovementComponent = CreateDefaultSubobject<UProjectileMovementComponent>(TEXT("ProjectileMovementComponent"));
	ProjectileMovementComponent->SetUpdatedComponent(CollisionComponent);
	ProjectileMovementComponent->InitialSpeed = 650.0f;
	ProjectileMovementComponent->MaxSpeed = 1200.0f;
	ProjectileMovementComponent->bRotationFollowsVelocity = true;
	ProjectileMovementComponent->bShouldBounce = true;
	ProjectileMovementComponent->Bounciness = 0.3f;
	ProjectileMovementComponent->ProjectileGravityScale = 0;
}

void AVisionShotProjectile::Init(UVisionShot* VisionShot_, float Damage, float LifeSpan, float Range)
{
	VisionShot = VisionShot_;
	this->Damage = Damage;
	this->ProjectileLifeSpan = LifeSpan;
	this->Range = Range;
}

void AVisionShotProjectile::FireInDirection(const FVector & ShootDirection)
{
	ProjectileMovementComponent->Velocity = ShootDirection * ProjectileMovementComponent->InitialSpeed;
}

void AVisionShotProjectile::OnHit(UPrimitiveComponent * HitComponent, AActor * OtherActor, UPrimitiveComponent * OtherComponent, FVector NormalImpulse, const FHitResult & Hit)
{
	if(OtherActor != this)
	{
		Destroy();
	}
}

void AVisionShotProjectile::OnOverlapBegin(class UPrimitiveComponent* OverlappedComp, class AActor* OtherActor, class UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
	if (OtherActor != this && Cast<AMonster>(OtherActor))
	{
		FDamageEvent DamageEvent;
		OtherActor->TakeDamage(Damage, DamageEvent, nullptr, this);
	}
}

void AVisionShotProjectile::EndPlay(EEndPlayReason::Type Reason)
{
	VisionShot->OnProjectileDestroyed();
}