// Fill out your copyright notice in the Description page of Project Settings.

#include "Weapon.h"


// Sets default values
AWeapon::AWeapon()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = false;

	Weapon = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("WEAPON"));
	RootComponent = Weapon;
}

void AWeapon::InitWeaponMesh(UStaticMesh* NewMesh)
{
	if (NewMesh)
	{
		Weapon->SetStaticMesh(NewMesh);
	}
}

// Called when the game starts or when spawned
void AWeapon::BeginPlay()
{
	Super::BeginPlay();
}