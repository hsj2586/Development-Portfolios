// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/Actor.h"
#include "Weapon.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API AWeapon : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AWeapon();

	// 무기 메시 세팅 함수
	void InitWeaponMesh(UStaticMesh* NewMesh);

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:
	UPROPERTY(VisibleAnywhere, Category = Weapon)
		UStaticMeshComponent* Weapon;
	
};
