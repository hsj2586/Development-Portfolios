// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/Pawn.h"
#include "GameFramework/FloatingPawnMovement.h"
#include "MyPawn.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API AMyPawn : public APawn
{
	GENERATED_BODY()

public:
	// Sets default values for this pawn's properties
	AMyPawn();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	// Called to bind functionality to input
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;
	virtual void PostInitializeComponents() override;
	virtual void PossessedBy(AController* NewController) override;

	UPROPERTY(VisibleAnywhere, Category = Collision)
		UCapsuleComponent* Capsule;

	UPROPERTY(VisibleAnywhere, Category = Collision)
		USkeletalMeshComponent* Mesh;

	UPROPERTY(VisibleAnywhere, Category = Collision)
		UFloatingPawnMovement* Movement;

	UPROPERTY(VisibleAnywhere, Category = Collision)
		USpringArmComponent* SpringArm;

	UPROPERTY(VisibleAnywhere, Category = Collision)
		UCameraComponent* Camera;

	void UpDown(float NewAxisValue);
	void LeftRight(float NewAxisValue);
};
