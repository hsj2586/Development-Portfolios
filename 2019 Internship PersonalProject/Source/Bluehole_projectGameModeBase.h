
#pragma once

#include "Bluehole_project.h"
#include "GameFramework/GameModeBase.h"
#include "Bluehole_projectGameModeBase.generated.h"

UCLASS()
class BLUEHOLE_PROJECT_API ABluehole_projectGameModeBase : public AGameModeBase
{
	GENERATED_BODY()

		ABluehole_projectGameModeBase();

public:
	// 유저(플레이어) 접속 함수
	virtual void PostLogin(APlayerController* NewPlayer) override;

	// 유저 스테이트 Setter
	void SetUserState(ECharacterState NewState);

	// 유저 스테이트 Getter
	ECharacterState GetUserState() const;

private:
	// 유저 스테이트
	UPROPERTY(Transient, VisibleInstanceOnly, BlueprintReadOnly, Category = State, Meta = (AllowPrivateAccess = true))
		ECharacterState UserState;

	// 몬스터 오브젝트 풀
	UPROPERTY()
		class AObjectPool* MonsterObjectPool;

	// NPC 클래스
	UPROPERTY(EditDefaultsOnly)
		TSubclassOf<class ANonePlayerCharacter> NPCClass;

	// 몬스터 클래스
	UPROPERTY(EditDefaultsOnly)
		TSubclassOf<class AMonster> MonsterClass;
};