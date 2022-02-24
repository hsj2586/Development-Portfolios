// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Bluehole_project.h"
#include "Components/ActorComponent.h"
#include "Runtime/SlateCore/Public/Input/Events.h"
#include "InputBufferManager.generated.h"


UCLASS( ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class BLUEHOLE_PROJECT_API UInputBufferManager : public UActorComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UInputBufferManager();

public:
	// �Է� ������ �Է�
	void InsertInput(FKey Input);

	// �Է� ������ ����
	void PopInput();

	// �Է� Ŀ�ǵ��� ���� ����
	bool CheckCommand(SkillType CommandName);

	// �Է� Ŀ�ǵ带 �������� �Լ�
	TArray<FKey> GetCommand(SkillType CommandName);
private:
	// Ű �Է� ����
	TArray<FKey> KeyInputBuffer;

	// �Է� ���� ����
	TArray<float> InputIntervalBuffer;
};