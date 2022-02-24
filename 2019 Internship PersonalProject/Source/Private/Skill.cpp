
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
{// 스킬 사용 주체 설정
	Player = Cast<AMyCharacter>(PlayerInput->GetOwner());
	Anim = Cast<UMyAnimInstance>(Player->GetMesh()->GetAnimInstance());

	InputComponent = PlayerInput;
	SkillSlotIndex = InputIndex;
	PrimaryComponentTick.bCanEverTick = true;
	LinkageSkillIndex = 0;
	IsLinkageSkillEnded = false;

	// 스킬 Stat 세팅
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
	// 연계 스킬이 존재한다면,
	if (LinkageSkillIndex != 0)
	{
		AMyPlayerController* PlayerController = Cast<AMyPlayerController>(Player->GetController());
		auto GameInstance = Cast<UMyGameInstance>(GetWorld()->GetGameInstance());
		FString LinkageSkillName = GameInstance->GetSkillData(LinkageSkillIndex)->SkillName;

		for (int i = 0; i < Player->SkillController->SkillList.Num(); i++)
		{
			USkill* TempSkill = Player->SkillController->SkillList[i];
			// 연계 스킬이 스킬리스트에 존재할 경우, 
			if (TempSkill != nullptr && TempSkill->GetSkillName() == LinkageSkillName)
			{
				// 쿨타임이 조건 확인
				if (TempSkill->RemainCooltime > 0)
					return;
				// 마나 조건 확인
				if (Player->CharacterStat->CheckManaIsZero(TempSkill->Mana))
					return;

				// TempSkill을 연계 키에 바인딩하고, 스킬 연계 UI위젯을 활성화
				UObject* SkillImage = PlayerController->GetHUDWidget()->SkillSlotList[i]->WidgetStyle.Normal.GetResourceObject();
				FString SkillNameText = TempSkill->GetSkillName();
				PlayerController->CreateSkillLinkageUIWidget(Cast<UTexture2D>(SkillImage), TEXT("R"), SkillNameText);
				InputComponent->BindAction(TEXT("LinkageSkill"), EInputEvent::IE_Pressed, this, &USkill::DoLinkageSkill);

				// 2초 후 지연 함수(UI 및 바인딩 자동 소멸) 호출
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

	// 커맨드 입력 여부 확인
	if (!PlayerController->GetCheckCommand((SkillType)(SkillType_))) return;

	FString LinkageSkillName = GameInstance->GetSkillData(LinkageSkillIndex)->SkillName;

	// 콤보 타이밍 확인
	if (PlayerController->IsComboTiming())
	{
		UE_LOG(LogTemp, Warning, TEXT("Combo Hit!"));
		PlayerController->CreateComboWidget();
	}

	// 기존 연계 스킬 바인딩 끊고, 스킬 연계 UI위젯을 비활성화
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

		// 기존 연계 스킬 바인딩 끊고, 스킬 연계 UI위젯을 비활성화
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
