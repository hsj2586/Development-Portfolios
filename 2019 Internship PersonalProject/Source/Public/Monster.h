// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "GameFramework/Character.h"
#include "MonsterAnimInstance.h"
#include "FireProjectile.h"
#include "Monster.generated.h"

DECLARE_MULTICAST_DELEGATE(FOnAttackEndDelegate);

UCLASS()
class BLUEHOLE_PROJECT_API AMonster : public ACharacter
{
	GENERATED_BODY()

public:
	AMonster();

	void Tick(float DeltaSeconds) override;

	// 몬스터 초기화 함수
	void Init(struct FMonsterData* StatData, USkeletalMesh* NewMesh,
		UStaticMesh* NewWeaponMesh, UClass* NewAnim, UAnimMontage* NewAnimMontage);

	void Attack();

	FOnAttackEndDelegate OnAttackEnd;

	virtual float TakeDamage(float DamageAmount, struct FDamageEvent const& DamageEvent,
		class AController* EventInstigator, AActor* DamageCauser) override;

	// DropExp를 반환하는 함수
	int32 GetExp() const;

	// 캐릭터 스텟 컴포넌트
	UPROPERTY(VisibleAnywhere)
		class UPoolableComponent* PoolableComponent;

	// 캐릭터 스텟 컴포넌트
	UPROPERTY(VisibleAnywhere, Category = Stat)
		class UMonsterStatComponent* CharacterStat;

	// HP바 위젯
	UPROPERTY(VisibleAnywhere, Category = UI)
		class UWidgetComponent* HPBarWidget;

	// 투사체 클래스
	UPROPERTY(EditDefaultsOnly, Category = Projectile)
		TSubclassOf<class AFireProjectile> ProjectileClass;

private:
	UFUNCTION()
		void OnAttackMontageEnded(UAnimMontage* Montage, bool bInterrupted);

	UPROPERTY()
		class UMonsterAnimInstance* MyAnim;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = State, Meta = (AllowPrivateAccess = true))
		float DeadTimer;

	UPROPERTY()
		class AWeapon* EquippedWeapon = nullptr;

	FTimerHandle DeadTimerHandle = { };

	bool IsAttacking = false;

	FSoftObjectPath MeshAssetPath;

	FSoftObjectPath WeaponAssetPath;

	FSoftClassPath AnimAssetPath;

	FSoftObjectPath MontageAssetPath;

	FSoftClassPath ProjectileAssetPath;

	class AMyCharacter* Player;
};
