// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "EngineMinimal.h"

UENUM(BlueprintType)
enum class ECharacterState : uint8
{
	PREINIT,
	LOADING,
	READY,
	DEAD
};

UENUM(BlueprintType)
enum class CharacterType : uint8
{
	Gunner
};

UENUM(BlueprintType)
enum class MonsterType : uint8
{
	General,
	Boss
};

UENUM(BlueprintType)
enum class AttackType : uint8
{
	Meele,
	Ranged
};

UENUM(BlueprintType)
enum class SkillType : uint8
{
	Attack,
	Rolling,
	ShotLaunch,
	VisionShot
};

UENUM(BlueprintType)
enum class QuestType : uint8
{
	KillBoss,
	Interaction
};

UENUM(BlueprintType)
enum class InputSetting : uint8
{
	UP,
	DOWN,
	LEFT,
	RIGHT,
	ATTACK,
	JUMP
};