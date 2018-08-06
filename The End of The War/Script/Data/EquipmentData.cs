using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EquipmentGrade
{
    common, // 일반
    rare,   // 레어
    unique, // 유니크
    legend  // 전설
}

public enum EquipmentMainType
{
    Head, // 투구류 : Helm, Hat
    Weapon, // 무기류 : Sword, Bow, Staff
    Armour, // 갑옷류 : Plate, Cloth
    Accessorie // 장신구류 : Neckless
}
public enum EquipmentSubType
{
    Helm,  // 판금투구
    Hat,   // 천모자

    Sword, // 검
    Dagger, // 단검
    Axe, // 도끼
    Bow,   // 활
    Staff, // 지팡이

    Plate, // 판금갑옷
    Cloth, // 천갑옷

    Neckless, // 목걸이
    Ring // 반지
}

[Serializable]
public class EquipmentData
{
    // 직렬화에 사용되는 장비 데이터 클래스.
    public string equipmentname;
    public EquipmentMainType maintype;
    public EquipmentSubType subtype;
    public EquipmentGrade grade;
    public float atkpower;
    public float defpower;
    public float atkspeed;
    public int cost;
    public bool isMounted;
    public string whoMounted;

    public EquipmentData(string equipmentname, EquipmentMainType maintype, EquipmentSubType subtype, EquipmentGrade grade, float atkpower, float defpower, float atkspeed, int cost, bool isMounted, string whoMounted)
    {
        this.equipmentname = equipmentname;
        this.maintype = maintype;
        this.subtype = subtype;
        this.grade = grade;
        this.atkpower = atkpower;
        this.defpower = defpower;
        this.atkspeed = atkspeed;
        this.cost = cost;
        this.isMounted = isMounted;
        this.whoMounted = whoMounted;
    }
}
