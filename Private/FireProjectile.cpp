// Fill out your copyright notice in the Description page of Project Settings.

#include "FireProjectile.h"
#include "ExplosionParticle.h"
#include "MyCharacter.h"

AFireProjectile::AFireProjectile()
{
	m_OnHit = false;
	InitialLifeSpan = 0.8f;
	TimerCount = 5;
	CollisionComponent = CreateDefaultSubobject<USphereComponent>(TEXT("SphereComponent"));
	CollisionComponent->BodyInstance.SetCollisionProfileName(TEXT("GeneralProjectile"));
	CollisionComponent->OnComponentHit.AddDynamic(this, &AFireProjectile::OnHit);
	CollisionComponent->InitSphereRadius(15.0f);
	RootComponent = CollisionComponent;

	ProjectileMovementComponent = CreateDefaultSubobject<UProjectileMovementComponent>(TEXT("ProjectileMovementComponent"));
	ProjectileMovementComponent->SetUpdatedComponent(CollisionComponent);
	ProjectileMovementComponent->InitialSpeed = 1500.0f;
	ProjectileMovementComponent->MaxSpeed = 2000.0f;
	ProjectileMovementComponent->bRotationFollowsVelocity = true;
	ProjectileMovementComponent->bShouldBounce = true;
	ProjectileMovementComponent->Bounciness = 0.3f;
	ProjectileMovementComponent->ProjectileGravityScale = 0;

	static ConstructorHelpers::FClassFinder<AExplosionParticle> Explosion_particle(
		TEXT("Blueprint'/Game/Blueprints/BP_ExplosionParticle.BP_ExplosionParticle_C'"));
	if (Explosion_particle.Succeeded())
	{
		ExplosionClass = Explosion_particle.Class;
	}

	static ConstructorHelpers::FClassFinder<ASmokeParticle> Smoke_particle(
		TEXT("Blueprint'/Game/Blueprints/BP_SmokeParticle.BP_SmokeParticle_C'"));
	if (Smoke_particle.Succeeded())
	{
		SmokeClass = Smoke_particle.Class;
	}

	static ConstructorHelpers::FClassFinder<AMuzzleFlashParticle> MuzzleFlash_particle(
		TEXT("Blueprint'/Game/Blueprints/BP_MuzzleFlashParticle.BP_MuzzleFlashParticle_C'"));
	if (MuzzleFlash_particle.Succeeded())
	{
		MuzzleFlashClass = MuzzleFlash_particle.Class;
	}
}

void AFireProjectile::BeginPlay()
{
	Super::BeginPlay();

	AMuzzleFlashParticle* MuzzleFlash = GetWorld()->SpawnActor<AMuzzleFlashParticle>(
		MuzzleFlashClass,
		GetActorTransform().GetLocation(),
		GetActorTransform().Rotator());

	GetWorldTimerManager().SetTimer(TimerHandle, this, &AFireProjectile::SpawnSmokeParticle, 0.2f, true, 0.0f);
}

void AFireProjectile::EndPlay(const EEndPlayReason::Type EndPlayReason)
{
	if (!m_OnHit)
	{
		AExplosionParticle* Explosion = GetWorld()->SpawnActor<AExplosionParticle>(ExplosionClass, GetActorTransform().GetLocation(),
			GetActorTransform().Rotator());
	}
}

void AFireProjectile::Init(float Damage, float LifeSpan, float Range)
{
	this->Damage = Damage;
	this->ProjectileLifeSpan = LifeSpan;
	this->Range = Range;
}

void AFireProjectile::FireInDirection(const FVector & ShootDirection)
{
	ProjectileMovementComponent->Velocity = ShootDirection * ProjectileMovementComponent->InitialSpeed;
}

void AFireProjectile::SpawnSmokeParticle()
{
	// Smoke 생성 함수, 반복적으로 호출
	if (--TimerCount < 1)
	{
		GetWorldTimerManager().ClearTimer(TimerHandle);
		TimerCount = 5;
	}

	ASmokeParticle* Smoke = GetWorld()->SpawnActor<ASmokeParticle>(
		SmokeClass,
		GetActorTransform().GetLocation(),
		GetActorTransform().Rotator());
}

void AFireProjectile::OnHit(UPrimitiveComponent * HitComponent, AActor * OtherActor, UPrimitiveComponent * OtherComponent, FVector NormalImpulse, const FHitResult & Hit)
{
	if (OtherActor != this && !Cast<AMyCharacter>(OtherActor))
	{
		m_OnHit = true;
		AExplosionParticle* Explosion = GetWorld()->SpawnActor<AExplosionParticle>(
			ExplosionClass,
			GetActorTransform().GetLocation(),
			GetActorTransform().Rotator());

		Explosion->Execute(Damage, ProjectileLifeSpan, Range);

		Destroy();
	}
}
