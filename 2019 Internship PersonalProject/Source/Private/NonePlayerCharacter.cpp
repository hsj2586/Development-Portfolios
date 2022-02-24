// Fill out your copyright notice in the Description page of Project Settings.

#include "NonePlayerCharacter.h"
#include "MyCharacter.h"
#include "QuestManager.h"
#include "MyGameInstance.h"
#include "CharacterSetting.h"
#include "Runtime/Engine/Classes/Animation/AnimInstance.h"
#include "Quest.h"
#include "Quest_KillBoss.h"
#include "Quest_Interaction.h"

ANonePlayerCharacter::ANonePlayerCharacter()
{
	PrimaryActorTick.bCanEverTick = false;

	GetMesh()->SetRelativeLocation(FVector(0, 0, -90));
	GetCapsuleComponent()->SetCollisionProfileName(TEXT("Pawn"));
	RootComponent = GetCapsuleComponent();
	GetMesh()->SetupAttachment(RootComponent);

	Trigger = CreateDefaultSubobject<USphereComponent>(TEXT("TRIGGER"));
	Trigger->SetupAttachment(RootComponent);
	Trigger->SetCollisionProfileName(TEXT("NPC"));
	Trigger->SetSphereRadius(180.0f);
	Trigger->SetRelativeLocation(FVector(0, 0, 0));
}

void ANonePlayerCharacter::BeginPlay()
{
	Super::BeginPlay();
}

void ANonePlayerCharacter::PostInitializeComponents()
{
	Super::PostInitializeComponents();
	Trigger->OnComponentBeginOverlap.AddDynamic(this, &ANonePlayerCharacter::OnOverlapBegin);
	Trigger->OnComponentEndOverlap.AddDynamic(this, &ANonePlayerCharacter::OnOverlapEnd);
}

void ANonePlayerCharacter::Init(struct FNPCData * NPCData)
{
	// 에셋 경로 설정
	auto DefaultSetting = GetDefault<UCharacterSetting>();
	NPCName = NPCData->NPCName;
	MeshAssetPath = DefaultSetting->NPCMeshAssets[NPCData->MeshIndex];
	AnimAssetPath = DefaultSetting->NPCAnimAssets[NPCData->AnimIndex];

	// 메시 세팅
	USkeletalMesh* NewMesh = Cast<USkeletalMesh>(StaticLoadObject(USkeletalMesh::StaticClass(), NULL, *MeshAssetPath.ToString()));
	if (NewMesh)
	{
		GetMesh()->SetSkeletalMesh(NewMesh);
	}

	// 애님 인스턴스 세팅
	UClass* NewAnim = StaticLoadClass(UAnimInstance::StaticClass(), NULL, *AnimAssetPath.ToString());
	if (NewAnim)
	{
		UE_LOG(LogTemp, Warning, TEXT("ANIM SET"));
		GetMesh()->SetAnimInstanceClass(NewAnim);
	}

	QuestManager = Cast<UMyGameInstance>(GetGameInstance())->GetQuestManager();

	// 퀘스트 세팅
	switch ((QuestType)NPCData->QuestType)
	{
	case QuestType::KillBoss:
	{
		UDataTable* DataTable = Cast<UMyGameInstance>(GetGameInstance())->GetQuestDataTable_KillBoss();
		AQuest_KillBoss* Quest = GetWorld()->SpawnActor<AQuest_KillBoss>();
		Quest->Init(DataTable, NPCData->QuestIndex);
		havingQuest = Quest;
	}
	break;

	case QuestType::Interaction:
	{
		UDataTable* DataTable = Cast<UMyGameInstance>(GetGameInstance())->GetQuestDataTable_Interaction();
		AQuest_Interaction* Quest = GetWorld()->SpawnActor<AQuest_Interaction>();
		Quest->Init(DataTable, NPCData->QuestIndex);
		havingQuest = Quest;
	}
	break;
	}
}

void ANonePlayerCharacter::GiveAQuest()
{
	if (havingQuest != nullptr && QuestManager)
	{
		UE_LOG(LogTemp, Warning, TEXT("take a Quest"));
		QuestManager->AddQuest(havingQuest);
		havingQuest->StartQuest();
		havingQuest = nullptr;
	}
	else
	{
		UE_LOG(LogTemp, Warning, TEXT("Not have Quest."));
	}
}

void ANonePlayerCharacter::OnOverlapBegin(UPrimitiveComponent * OverlappedComp, AActor * OtherActor, UPrimitiveComponent * OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult & SweepResult)
{
	AMyCharacter* Player = Cast<AMyCharacter>(OtherActor);
	if (Player)
	{
		Player->InputComponent->BindAction(TEXT("Interaction"), EInputEvent::IE_Pressed, this, &ANonePlayerCharacter::GiveAQuest);
	}
}

void ANonePlayerCharacter::OnOverlapEnd(UPrimitiveComponent * OverlappedComp, AActor * OtherActor, UPrimitiveComponent * OtherComp, int32 OtherBodyIndex)
{
	AMyCharacter* Player = Cast<AMyCharacter>(OtherActor);
	if (Player)
	{
		UInputComponent* PlayerInputComponent = Player->InputComponent;
		for (int i = 0; i < PlayerInputComponent->GetNumActionBindings(); i++)
		{
			if (TEXT("Interaction") == PlayerInputComponent->GetActionBinding(i).ActionName.ToString())
			{
				PlayerInputComponent->RemoveActionBinding(i);
				break;
			}
		}
	}
}