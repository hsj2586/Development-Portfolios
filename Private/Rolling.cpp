// Fill out your copyright notice in the Description page of Project Settings.

#include "Rolling.h"
#include "ShotLaunch.h"

void URolling::Init(UInputComponent *PlayerInput, int InputIndex, struct FSkillData* LoadedSkillData)
{
	Super::Init(PlayerInput, InputIndex, LoadedSkillData);

	// 스킬 Input 세팅
	FString InputName = "Skill" + FString::FromInt(SkillSlotIndex);
	InputComponent->BindAction(FName(*InputName), EInputEvent::IE_Pressed, this, &URolling::DoSkill);
}

void URolling::DoSkill()
{
	if (RemainCooltime > 0) return;
	if (Player->CharacterStat->CheckManaIsZero(Mana)) return;
	if (!Anim->CancelAnimation(SkillType::Rolling)) return;

	// 쿨타임 초기화 및 마나 소비
	RemainCooltime = CoolTime;
	Player->CharacterStat->ConsumeMana(Mana);
	GetDirection();

	if (Anim)
	{
		Anim->SetRollingAnim();

		// 일시적으로 무적
		Player->bCanBeDamaged = false;
		Player->GetCharacterMovement()->MaxWalkSpeed = 900.0f;

		// 플레이어에게 ShotLaunch 스킬이 있다면 쿨타임 초기화
		UShotLaunch* ShotLaunch = GetOwner()->FindComponentByClass<UShotLaunch>();
		if (ShotLaunch)
		{
			ShotLaunch->SetRemainCooltime(0.0f);
		}
	}

	CheckLinkageSkill();
}

void URolling::GetDirection()
{
	float UpDown = GetOwner()->GetInputAxisValue("UpDown");
	float LeftRight = GetOwner()->GetInputAxisValue("LeftRight");

	if (UpDown > 0)
	{
		if (LeftRight > 0)
			RollingDirection = Direction::ForwardRight;
		else if (LeftRight == 0)
			RollingDirection = Direction::Forward;
		else
			RollingDirection = Direction::ForwardLeft;
	}
	else if (UpDown == 0)
	{
		if (LeftRight > 0)
			RollingDirection = Direction::Right;
		else if (LeftRight == 0)
			RollingDirection = Direction::Neutral;
		else
			RollingDirection = Direction::Left;
	}
	else if (UpDown < 0)
	{
		if (LeftRight > 0)
			RollingDirection = Direction::BackRight;
		else if (LeftRight == 0)
			RollingDirection = Direction::Back;
		else
			RollingDirection = Direction::BackLeft;
	}

	// 마지막에 입력된 회전 방향으로 Rolling
	if (RollingDirection != Direction::Neutral)
	{
		FVector CameraLocation;
		FRotator CameraRotation;
		Player->GetActorEyesViewPoint(CameraLocation, CameraRotation);
		Player->SetActorRotation(FRotator(Player->GetActorRotation().Pitch,
			CameraRotation.Yaw, Player->GetActorRotation().Roll));
		Player->AddActorLocalRotation(FRotator(0, (int)RollingDirection * 45, 0));
	}
}

void URolling::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	if (Anim && Anim->GetIsRolling())
	{
		Player->AddMovementInput(Player->GetActorForwardVector(), 2);
	}
}