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
	// 입력 버퍼의 입력
	void InsertInput(FKey Input);

	// 입력 버퍼의 삭제
	void PopInput();

	// 입력 커맨드의 판정 여부
	bool CheckCommand(SkillType CommandName);

	// 입력 커맨드를 가져오는 함수
	TArray<FKey> GetCommand(SkillType CommandName);
private:
	// 키 입력 버퍼
	TArray<FKey> KeyInputBuffer;

	// 입력 간격 버퍼
	TArray<float> InputIntervalBuffer;
};