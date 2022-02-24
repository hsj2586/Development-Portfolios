// Fill out your copyright notice in the Description page of Project Settings.

#include "VisionShot.h"
#include "VisionShotProjectile.h"
#include "ExplosionParticle.h"
#include "MyPlayerController.h"
#include "HUDWidget.h"
#include "Button.h"

UVisionShot::UVisionShot()
{
	static ConstructorHelpers::FClassFinder<AVisionShotProjectile> Projectile
	(TEXT("Blueprint'/Game/Blueprints/BP_VisionShotProjectile.BP_VisionShotProjectile_C'"));

	if (Projectile.Succeeded())
	{
		ProjectileClass = Projectile.Class;
	}

	static ConstructorHelpers::FClassFinder<AExplosionParticle> Explosion_particle(
		TEXT("Blueprint'/Game/Blueprints/BP_VisionExplosionParticle.BP_VisionExplosionParticle_C'"));
	if (Explosion_particle.Succeeded())
	{
		ExplosionClass = Explosion_particle.Class;
	}
}

void UVisionShot::Init(UInputComponent *PlayerInput, int InputIndex, struct FSkillData* LoadedSkillData)
{
	Super::Init(PlayerInput, InputIndex, LoadedSkillData);

	// ��ų Input ����
	FString InputName = "Skill" + FString::FromInt(SkillSlotIndex);
	InputComponent->BindAction(FName(*InputName), EInputEvent::IE_Pressed, this, &UVisionShot::DoSkill);
}

void UVisionShot::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction * ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);
}

void UVisionShot::DoSkill()
{
	// ��ų ��ư ������ ��, ��������Ʈ ��ε�ĳ��Ʈ (ȣ�� �ı�)
	OnPress.Broadcast();

	if (RemainCooltime > 0) return;
	if (Player->CharacterStat->CheckManaIsZero(Mana)) return;
	if (!Anim->CancelAnimation(SkillType::VisionShot)) return;

	// ��Ÿ�� �ʱ�ȭ �� ���� �Һ�
	RemainCooltime = CoolTime;
	Player->CharacterStat->ConsumeMana(Mana);

	if (Anim)
	{
		// ��Ʈ�ѷ� �������� ȸ���ϴ� ����
		FVector CameraLocation;
		FRotator CameraRotation;
		Player->GetActorEyesViewPoint(CameraLocation, CameraRotation);
		Player->SetActorRotation(FRotator(Player->GetActorRotation().Pitch, CameraRotation.Yaw, 0));
		Player->bUseControllerRotationYaw = true;

		Anim->SetVisionShotAnim();
		Anim->PlaySkill();

		FVector MuzzleLocation = CameraLocation + FTransform(CameraRotation).TransformVector(MuzzleOffset);
		FRotator MuzzleRotation = CameraRotation;

		MuzzleRotation.Pitch += 10.0f;

		UWorld* World = GetWorld();
		if (World)
		{
			// �߻�ü ����
			FVector LaunchDirection = MuzzleRotation.Vector();
			VisionShotProjectile = World->SpawnActor<AVisionShotProjectile>(ProjectileClass, MuzzleLocation, MuzzleRotation);
			if (VisionShotProjectile)
			{
				// ���� �Լ� ��������Ʈ ����
				OnPress.AddUObject(this, &UVisionShot::VisionExplosion);

				// ���� ��ų UI Ȱ��ȭ �� �Է� ���ε�
				AMyPlayerController* PlayerController = Cast<AMyPlayerController>(Player->GetController());
				UObject* SkillImage = PlayerController->GetHUDWidget()->SkillSlotList[SkillSlotIndex - 1]->WidgetStyle.Normal.GetResourceObject();
				FString SkillNameText = GetSkillName();
				PlayerController->CreateSkillLinkageUIWidget(Cast<UTexture2D>(SkillImage), TEXT("R"), SkillNameText);
				InputComponent->BindAction(TEXT("LinkageSkill"), EInputEvent::IE_Pressed, this, &UVisionShot::VisionExplosion);


				VisionShotProjectile->Init(this, 50, 2.0f, 0);
				VisionShotProjectile->FireInDirection(LaunchDirection);
			}
		}
	}
}

void UVisionShot::VisionExplosion()
{
	AExplosionParticle* Explosion = GetWorld()->SpawnActor<AExplosionParticle>(
		ExplosionClass,
		VisionShotProjectile->GetActorTransform().GetLocation(),
		VisionShotProjectile->GetActorTransform().Rotator());

	Explosion->Execute(120.0f, 1.5f, 400.0f);

	VisionShotProjectile->Destroy();
	VisionShotProjectile = nullptr;

	// ���� ��ų üũ
	CheckLinkageSkill();
}

void UVisionShot::OnProjectileDestroyed()
{
	// ActionInput ���ε� ���� �� �缳��, �߻�ü �ı�
	FString InputName = "Skill" + FString::FromInt(SkillSlotIndex);
	for (int i = 0; i < InputComponent->GetNumActionBindings(); i++)
	{
		if (InputName == InputComponent->GetActionBinding(i).ActionName.ToString())
		{
			InputComponent->RemoveActionBinding(i);
			break;
		}
	}

	// ���� ���� ��ų ���ε� ����, ��ų ���� UI������ ��Ȱ��ȭ
	AMyPlayerController* PlayerController = Cast<AMyPlayerController>(Player->GetController());
	PlayerController->DestroySkillLinkageUIWidget();
	for (int i = 0; i < InputComponent->GetNumActionBindings(); i++)
	{
		if (InputComponent->GetActionBinding(i).ActionName == TEXT("LinkageSkill"))
		{
			InputComponent->RemoveActionBinding(i);
			break;
		}
	}

	OnPress.Clear();
	InputComponent->BindAction(FName(*InputName), EInputEvent::IE_Pressed, this, &UVisionShot::DoSkill);
}