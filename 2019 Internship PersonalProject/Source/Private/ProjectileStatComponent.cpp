// Fill out your copyright notice in the Description page of Project Settings.

#include "ProjectileStatComponent.h"


// Sets default values for this component's properties
UProjectileStatComponent::UProjectileStatComponent()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = true;
}


// Called when the game starts
void UProjectileStatComponent::BeginPlay()
{
	Super::BeginPlay();
}

void UProjectileStatComponent::InitializeComponent()
{
	Super::InitializeComponent();
}

void UProjectileStatComponent::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);
}

void UProjectileStatComponent::SetDamage(float NewDamage)
{
	Attack = NewDamage;
}

void UProjectileStatComponent::SetLifeSpan(float NewLifeSpan)
{
	LifeSpan = NewLifeSpan;
}

float UProjectileStatComponent::GetLifeSpan()
{
	return LifeSpan;
}

void UProjectileStatComponent::SetRange(float NewRange)
{
	Range = NewRange;
}

float UProjectileStatComponent::GetRange()
{
	return Range;
}

float UProjectileStatComponent::GetDamage()
{
	return Attack;
}

