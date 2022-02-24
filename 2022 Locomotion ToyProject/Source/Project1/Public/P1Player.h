// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "P1Player.generated.h"

#define MAX_WALK_SPEED 600.0f
#define RIGHT_WALK_SPEED_MULTIFLIER 0.4f
#define BACK_WALK_SPEED_MULTIFLIER 0.3f

UCLASS(config=Game)
class PROJECT1_API AP1Player : public ACharacter
{
	GENERATED_BODY()
public:
	AP1Player(const FObjectInitializer& ObjectInitializer);

	void GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const override;

	virtual void BeginPlay() override;
	virtual void Tick(float DeltaTime) override;
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;
	virtual void Jump() override;

	virtual void AddControllerYawInput(float Val) override;

	UFUNCTION(BlueprintCallable)
	void AnimNotify_EndAttack();

	UFUNCTION(BlueprintCallable)
	void AnimNotify_OnLanding();

public:
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Replicated)
		bool bIsAttacking;

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Replicated)
		bool bIsBlocking;

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Replicated)
		float fowardValue;

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Replicated)
		float rightValue;

private:
	void MoveForward(float value);
	UFUNCTION(Server, Reliable) void MoveForwardByServer(const float value, const FVector vec, const FRotator rot);
	virtual void MoveForwardByServer_Implementation(const float value, const FVector vec, const FRotator rot);

	void MoveRight(float value);
	UFUNCTION(Server, Reliable) void MoveRightByServer(const float value, const FVector vec, const FRotator rot);
	virtual void MoveRightByServer_Implementation(const float value, const FVector vec, const FRotator rot);

	UFUNCTION(Server, Reliable) void DoAttack();
	virtual void DoAttack_Implementation();

	UFUNCTION(Server, Reliable) void DoBlock(float bActive);
	virtual void DoBlock_Implementation(float bActive);
	
	void AddYawRotation(float value);
	UFUNCTION(Server, Reliable) void AddYawRotationByServer(float value);
	virtual void AddYawRotationByServer_Implementation(float value);

	UFUNCTION(Server, Reliable) void SetIgnoreMovementInput(bool bOn);
	virtual void SetIgnoreMovementInput_Implementation(bool bOn);
	
	UFUNCTION(Server, Reliable) void SetIgnoreLookInput(bool bOn);
	virtual void SetIgnoreLookInput_Implementation(bool bOn);

	UFUNCTION(Server, Reliable) void BeginOverlapEvent(class UPrimitiveComponent* HitComp, AActor* OtherActor, class UPrimitiveComponent* OtherComp,
		int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult);
	void BeginOverlapEvent_Implementation(class UPrimitiveComponent* HitComp, AActor* OtherActor, class UPrimitiveComponent* OtherComp,
		int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult);

	void AddPitchRotation(float value);

	bool CheckEnableAttack();
	bool CheckEnableJump();
	bool CheckEnableBlock(float bActive);

private:
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = Camera, meta = (AllowPrivateAccess = "true"))
		class USpringArmComponent* SpringArm;

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = Camera, meta = (AllowPrivateAccess = "true"))
		class UCameraComponent* MainCamera;

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = Camera, meta = (AllowPrivateAccess = "true"))
		class UBoxComponent* WeaponCollisionBox;
};
