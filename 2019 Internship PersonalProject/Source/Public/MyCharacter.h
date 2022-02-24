// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/Character.h"
#include "MyGameInstance.h"
#include "MyCharacter.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API AMyCharacter : public ACharacter
{
	GENERATED_BODY()

protected:
	virtual void BeginPlay() override;

public:

	AMyCharacter();

	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

	virtual void PostInitializeComponents() override;

	virtual void Tick(float DeltaTime) override;

	void UpDown(float NewAxisValue);

	void LeftRight(float NewAxisValue);

	void LookUp(float NewAxisValue);

	void Turn(float NewAxisValue);

	// ���� ������Ʈ Setter
	void SetUserState(ECharacterState NewState);

	// ���� ������Ʈ Getter
	ECharacterState GetUserState() const;

	void Attack();

	float TakeDamage(float DamageAmount, FDamageEvent const & DamageEvent, AController * EventInstigator, AActor * DamageCauser);

	// �ѱ� ��ġ
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Gameplay)
		FVector MuzzleOffset;

	// �÷��̾� ��Ʈ�ѷ�
	UPROPERTY(VisibleAnywhere)
		class AMyPlayerController* Controller_;

	// ���� �ν��Ͻ�
	UPROPERTY(VisibleAnywhere)
		class ABluehole_projectGameModeBase* GameMode;

	// ������ ��(ī�޶� ��ġ��)
	UPROPERTY(VisibleAnywhere, Category = Camera)
		USpringArmComponent* SpringArm;

	// ī�޶�
	UPROPERTY(VisibleAnywhere, Category = Camera)
		UCameraComponent* Camera;

	// ����ü Ŭ����
	UPROPERTY(EditDefaultsOnly, Category = Projectile)
		TSubclassOf<class AFireProjectile> ProjectileClass;

	// ĳ���� ���� ������Ʈ
	UPROPERTY(VisibleAnywhere, Category = Stat)
		class UMyCharacterStatComponent* CharacterStat;

	// ��ų ��Ʈ�ѷ� ������Ʈ
	UPROPERTY(VisibleAnywhere, Category = Stat)
		class USkillController* SkillController;

	// ī�޶� ����ũ
	UPROPERTY(EditAnywhere, Category = Stat)
		TSubclassOf<UCameraShake> MyShake;

private:

	// ���콺 ��� Ȱ��ȭ/��Ȱ��ȭ
	void MouseModeSwitch();

	// ���콺 ��� Ȱ��ȭ ����
	UPROPERTY()
		bool IsMouseModeActive;

	// �ִ� �ν��Ͻ�
	UPROPERTY()
		class UMyAnimInstance* MyAnim;

	// ���� ������Ʈ
	UPROPERTY(Transient, VisibleInstanceOnly, BlueprintReadOnly, Category = State, Meta = (AllowPrivateAccess = true))
		ECharacterState UserState;

	// ���� ���� Ÿ�̸�
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = State, Meta = (AllowPrivateAccess = true))
		float DeadTimer;

	// ���� ����
	UPROPERTY()
		class AWeapon* EquippedWeapon = nullptr;

	FTimerHandle DeadTimerHandle = { };

	FSoftObjectPath MeshAssetPath;

	FSoftObjectPath WeaponAssetPath;

	FSoftClassPath AnimAssetPath;

	FSoftObjectPath MontageAssetPath;

	FSoftClassPath ProjectileAssetPath;

	// �Է� ���ۿ� ����
	class UViewportWorldInteraction* Interaction;

	int32 AssetIndex = 0;
};