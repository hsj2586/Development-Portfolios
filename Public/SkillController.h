// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "SkillController.generated.h"


UCLASS(ClassGroup = (Custom), meta = (BlueprintSpawnableComponent))
class BLUEHOLE_PROJECT_API USkillController : public UActorComponent
{
	GENERATED_BODY()

public:
	USkillController();

public:
	// ��ų ��Ʈ�ѷ� �ʱ�ȭ
	void Init();

	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	// ��ų Ŭ���� ���丮
	class USkill* GetSkillClass(int SkillNameIndex, FName SkillName_);

	// ��ų ����Ʈ
	UPROPERTY(EditAnywhere, Category = Skill)
		TArray<class USkill*> SkillList;

	// ��ų ���� �ε����� �����ϴ� �Լ�
	void UpdateSkillList();

private:
	// AnimInstance
	class UMyAnimInstance* Anim;

	// �÷��̾� ��Ʈ�ѷ�
	class AMyPlayerController* Controller;

	// SkillLinkageWidget
	class USkillLinkageWidget* SkillLinkageWidget;
};
