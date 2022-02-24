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

	// 유저 스테이트 Setter
	void SetUserState(ECharacterState NewState);

	// 유저 스테이트 Getter
	ECharacterState GetUserState() const;

	void Attack();

	float TakeDamage(float DamageAmount, FDamageEvent const & DamageEvent, AController * EventInstigator, AActor * DamageCauser);

	// 총구 위치
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = Gameplay)
		FVector MuzzleOffset;

	// 플레이어 컨트롤러
	UPROPERTY(VisibleAnywhere)
		class AMyPlayerController* Controller_;

	// 게임 인스턴스
	UPROPERTY(VisibleAnywhere)
		class ABluehole_projectGameModeBase* GameMode;

	// 스프링 암(카메라 거치대)
	UPROPERTY(VisibleAnywhere, Category = Camera)
		USpringArmComponent* SpringArm;

	// 카메라
	UPROPERTY(VisibleAnywhere, Category = Camera)
		UCameraComponent* Camera;

	// 투사체 클래스
	UPROPERTY(EditDefaultsOnly, Category = Projectile)
		TSubclassOf<class AFireProjectile> ProjectileClass;

	// 캐릭터 스텟 컴포넌트
	UPROPERTY(VisibleAnywhere, Category = Stat)
		class UMyCharacterStatComponent* CharacterStat;

	// 스킬 컨트롤러 컴포넌트
	UPROPERTY(VisibleAnywhere, Category = Stat)
		class USkillController* SkillController;

	// 카메라 섀이크
	UPROPERTY(EditAnywhere, Category = Stat)
		TSubclassOf<UCameraShake> MyShake;

private:

	// 마우스 모드 활성화/비활성화
	void MouseModeSwitch();

	// 마우스 모드 활성화 여부
	UPROPERTY()
		bool IsMouseModeActive;

	// 애님 인스턴스
	UPROPERTY()
		class UMyAnimInstance* MyAnim;

	// 유저 스테이트
	UPROPERTY(Transient, VisibleInstanceOnly, BlueprintReadOnly, Category = State, Meta = (AllowPrivateAccess = true))
		ECharacterState UserState;

	// 죽음 지연 타이머
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = State, Meta = (AllowPrivateAccess = true))
		float DeadTimer;

	// 장착 무기
	UPROPERTY()
		class AWeapon* EquippedWeapon = nullptr;

	FTimerHandle DeadTimerHandle = { };

	FSoftObjectPath MeshAssetPath;

	FSoftObjectPath WeaponAssetPath;

	FSoftClassPath AnimAssetPath;

	FSoftObjectPath MontageAssetPath;

	FSoftClassPath ProjectileAssetPath;

	// 입력 버퍼용 변수
	class UViewportWorldInteraction* Interaction;

	int32 AssetIndex = 0;
};