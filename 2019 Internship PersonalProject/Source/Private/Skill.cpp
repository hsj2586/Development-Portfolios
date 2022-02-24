
#include "Skill.h"
#include "MyCharacter.h"
#include "MyPlayerController.h"
#include "SkillController.h"
#include "MyGameInstance.h"
#include "HUDWidget.h"
#include "Button.h"

USkill::USkill()
{
	PrimaryComponentTick.bCanEverTick = true;
}

void USkill::DoSkill()
{
}

FString USkill::GetSkillName()
{
	return SkillName;
}

void USkill::SetRemainCooltime(float NewCooltime)
{
	RemainCooltime = NewCooltime;
}

float USkill::GetRemainCooltime()
{
	return RemainCooltime;
}

float USkill::GetCooltimeRatio()
{
	return RemainCooltime / CoolTime;
}

void USkill::Init(UInputComponent * PlayerInput, int InputIndex, struct FSkillData* LoadedSkillData)
{// ��ų ��� ��ü ����
	Player = Cast<AMyCharacter>(PlayerInput->GetOwner());
	Anim = Cast<UMyAnimInstance>(Player->GetMesh()->GetAnimInstance());

	InputComponent = PlayerInput;
	SkillSlotIndex = InputIndex;
	PrimaryComponentTick.bCanEverTick = true;
	LinkageSkillIndex = 0;
	IsLinkageSkillEnded = false;

	// ��ų Stat ����
	if (LoadedSkillData)
	{
		RegisterComponent();
		SkillName = LoadedSkillData->SkillName;
		CoolTime = LoadedSkillData->CoolTime;
		Mana = LoadedSkillData->Mana;
		Value = LoadedSkillData->Value;
		RemainCooltime = 0;
	}
}

void USkill::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction * ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);
	if (RemainCooltime >= 0.0f)
	{
		RemainCooltime -= DeltaTime;
		OnCooltimeChanged.Broadcast();

		if (RemainCooltime <= 0.0f)
		{
			OnCooltimeIsZero.Broadcast();
			RemainCooltime = 0;
		}
	}
}

void USkill::CheckLinkageSkill()
{
	// ���� ��ų�� �����Ѵٸ�,
	if (LinkageSkillIndex != 0)
	{
		AMyPlayerController* PlayerController = Cast<AMyPlayerController>(Player->GetController());
		auto GameInstance = Cast<UMyGameInstance>(GetWorld()->GetGameInstance());
		FString LinkageSkillName = GameInstance->GetSkillData(LinkageSkillIndex)->SkillName;

		for (int i = 0; i < Player->SkillController->SkillList.Num(); i++)
		{
			USkill* TempSkill = Player->SkillController->SkillList[i];
			// ���� ��ų�� ��ų����Ʈ�� ������ ���, 
			if (TempSkill != nullptr && TempSkill->GetSkillName() == LinkageSkillName)
			{
				// ��Ÿ���� ���� Ȯ��
				if (TempSkill->RemainCooltime > 0)
					return;
				// ���� ���� Ȯ��
				if (Player->CharacterStat->CheckManaIsZero(TempSkill->Mana))
					return;

				// TempSkill�� ���� Ű�� ���ε��ϰ�, ��ų ���� UI������ Ȱ��ȭ
				UObject* SkillImage = PlayerController->GetHUDWidget()->SkillSlotList[i]->WidgetStyle.Normal.GetResourceObject();
				FString SkillNameText = TempSkill->GetSkillName();
				PlayerController->CreateSkillLinkageUIWidget(Cast<UTexture2D>(SkillImage), TEXT("R"), SkillNameText);
				InputComponent->BindAction(TEXT("LinkageSkill"), EInputEvent::IE_Pressed, this, &USkill::DoLinkageSkill);

				// 2�� �� ���� �Լ�(UI �� ���ε� �ڵ� �Ҹ�) ȣ��
				FTimerHandle TimerHandle;
				GetWorld()->GetTimerManager().SetTimer(TimerHandle, this, &USkill::LinkageSkillEnd, 2.0f, false);
			}
		}
	}
}

void USkill::DoLinkageSkill()
{
	if (!Anim->CancelAnimation(SkillType::ShotLaunch)) return;

	AMyPlayerController* PlayerController = Cast<AMyPlayerController>(Player->GetController());
	auto GameInstance = Cast<UMyGameInstance>(GetWorld()->GetGameInstance());
	int SkillType_ = GameInstance->GetSkillIndex(GameInstance->GetSkillData(LinkageSkillIndex)->SkillName) + 1;

	// Ŀ�ǵ� �Է� ���� Ȯ��
	if (!PlayerController->GetCheckCommand((SkillType)(SkillType_))) return;

	FString LinkageSkillName = GameInstance->GetSkillData(LinkageSkillIndex)->SkillName;

	// �޺� Ÿ�̹� Ȯ��
	if (PlayerController->IsComboTiming())
	{
		UE_LOG(LogTemp, Warning, TEXT("Combo Hit!"));
		PlayerController->CreateComboWidget();
	}

	// ���� ���� ��ų ���ε� ����, ��ų ���� UI������ ��Ȱ��ȭ
	PlayerController->DestroySkillLinkageUIWidget();
	for (int i = 0; i < InputComponent->GetNumActionBindings(); i++)
	{
		if (InputComponent->GetActionBinding(i).ActionName == TEXT("LinkageSkill"))
		{
			InputComponent->RemoveActionBinding(i);
			break;
		}
	}

	for (int i = 0; i < Player->SkillController->SkillList.Num(); i++)
	{
		USkill* TempSkill = Player->SkillController->SkillList[i];
		if (TempSkill != nullptr && TempSkill->GetSkillName() == LinkageSkillName)
		{
			TempSkill->DoSkill();
			IsLinkageSkillEnded = true;
			return;
		}
	}
}

void USkill::LinkageSkillEnd()
{
	if (!IsLinkageSkillEnded)
	{
		AMyPlayerController* PlayerController = Cast<AMyPlayerController>(Player->GetController());
		auto GameInstance = Cast<UMyGameInstance>(GetWorld()->GetGameInstance());

		// ���� ���� ��ų ���ε� ����, ��ų ���� UI������ ��Ȱ��ȭ
		PlayerController->DestroySkillLinkageUIWidget();
		for (int i = 0; i < InputComponent->GetNumActionBindings(); i++)
		{
			if (InputComponent->GetActionBinding(i).ActionName == TEXT("LinkageSkill"))
			{
				InputComponent->RemoveActionBinding(i);
				break;
			}
		}
	}
	else
	{
		IsLinkageSkillEnded = false;
	}
}
