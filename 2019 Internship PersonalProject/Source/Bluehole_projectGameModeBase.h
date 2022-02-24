
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
	// ����(�÷��̾�) ���� �Լ�
	virtual void PostLogin(APlayerController* NewPlayer) override;

	// ���� ������Ʈ Setter
	void SetUserState(ECharacterState NewState);

	// ���� ������Ʈ Getter
	ECharacterState GetUserState() const;

private:
	// ���� ������Ʈ
	UPROPERTY(Transient, VisibleInstanceOnly, BlueprintReadOnly, Category = State, Meta = (AllowPrivateAccess = true))
		ECharacterState UserState;

	// ���� ������Ʈ Ǯ
	UPROPERTY()
		class AObjectPool* MonsterObjectPool;

	// NPC Ŭ����
	UPROPERTY(EditDefaultsOnly)
		TSubclassOf<class ANonePlayerCharacter> NPCClass;

	// ���� Ŭ����
	UPROPERTY(EditDefaultsOnly)
		TSubclassOf<class AMonster> MonsterClass;
};