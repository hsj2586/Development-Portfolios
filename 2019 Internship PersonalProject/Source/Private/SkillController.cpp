// Fill out your copyright notice in the Description page of Project Settings.

#include "SkillController.h"
#include "Rolling.h"
#include "ShotLaunch.h"
#include "VisionShot.h"
#include "MyPlayerController.h"
#include "HUDWidget.h"
#include "Skill.h"
#include "SkillLinkageWidget.h"

USkillController::USkillController()
{
	PrimaryComponentTick.bCanEverTick = true;
}

void USkillController::Init()
{
	Controller = Cast<AMyPlayerController>(GetOwner()->GetInstigatorController());
	SkillLinkageWidget = Controller->GetHUDWidget()->SkillLinkageWidget;

	// 더미 스킬 리스트 삽입
	for (int i = 0; i < 10; i++)
	{
		SkillList.Add(nullptr);
	}

	// 스킬 UI 델리게이트 등록
	Controller->GetHUDWidget()->BindSkillState(SkillList);
}

void USkillController::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);
}

USkill* USkillController::GetSkillClass(int SkillNameIndex, FName SkillName_)
{
	SkillType TempSkillName = (SkillType)SkillNameIndex;
	switch (TempSkillName)
	{
	case SkillType::Rolling:
		return NewObject<URolling>(this, URolling::StaticClass(), SkillName_);

	case SkillType::ShotLaunch:
		return NewObject<UShotLaunch>(this, UShotLaunch::StaticClass(), SkillName_);

	case SkillType::VisionShot:
		return NewObject<UVisionShot>(this, UVisionShot::StaticClass(), SkillName_);

	default:
		return nullptr;
	}
}

void USkillController::UpdateSkillList()
{
	auto GameInstance = Cast<UMyGameInstance>(GetWorld()->GetGameInstance());

	TArray<int> tempIndexArray;

	// 연계 스킬들의 스킬 데이터 상의 인덱스 저장
	for (int k = 0; k < SkillLinkageWidget->SkillList.Num(); k++)
	{
		// 연계 스킬이 없을 경우
		if (SkillLinkageWidget->SkillList[k] == nullptr)
		{
			tempIndexArray.Add(0);
			continue;
		}

		for (int l = 1; l < SkillLinkageWidget->SkillList.Num() + 1; l++)
		{
			if (SkillLinkageWidget->SkillList[k]->GetName() == GameInstance->GetSkillData(l)->SkillName)
			{
				tempIndexArray.Add(l);
			}
		}
	}

	for (int i = 1; i < SkillLinkageWidget->SkillList.Num() + 1; i++)
	{
		FString SkillName = GameInstance->GetSkillData(i)->SkillName;
		for (int j = 0; j < SkillList.Num(); j++)
		{
			if (SkillList[j] != nullptr)
			{
				// 스킬 이름이 해당될 경우,
				if (SkillList[j]->GetSkillName() == SkillName)
				{
					// 연계 스킬에 해당하는 스킬의 인덱스를, '스킬'의 연계 스킬 인덱스에 입력
					SkillList[j]->LinkageSkillIndex = tempIndexArray[i - 1];
				}
			}
		}
	}
}