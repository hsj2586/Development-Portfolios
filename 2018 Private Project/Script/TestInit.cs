using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInit : MonoBehaviour
{
    // 데이터 초기화를 하드코딩하는 스크립트.
    void Start()
    {
        FileManager.DataGenerate<CharacterData>("SaveFile/UserData/Character1.txt",
            new CharacterData("기사", "Ally", 1, 2, AttackType.Melee, 80, 0.3f, 3, 810, "Crash", 1, 0, "Character1", "Character1AudioList.txt", "Character1EquipList.txt", Class.Knight));
        FileManager.DataGenerate<CharacterData>("SaveFile/UserData/Character2.txt",
           new CharacterData("궁수", "Ally", 2, 1, AttackType.Range, 45, 0.7f, 2, 620, "Rage", 1, 0, "Character2", "Character2AudioList.txt", "Character2EquipList.txt", Class.Archer));
        FileManager.DataGenerate<CharacterData>("SaveFile/UserData/Character3.txt",
           new CharacterData("마법사", "Ally", 3, 3, AttackType.Range, 85, 0.2f, 1, 550, "Storm", 1, 0, "Character3", "Character3AudioList.txt", "Character3EquipList.txt", Class.Wizard));

        FileManager.DataGenerate<CharacterData>("SaveFile/EnemyData/EnemyData1.txt",
           new CharacterData("좀비", "Enemy", 4, 0, AttackType.Melee, 200, 0.3f, 3, 450, null, 1, 0, "Enemy1", "Enemy1AudioList.txt", null, Class.Enemy));
        FileManager.DataGenerate<CharacterData>("SaveFile/EnemyData/EnemyData2.txt",
           new CharacterData("트롤", "Enemy", 5, 0, AttackType.Melee, 300, 0.4f, 5, 650, null, 1, 0, "Enemy2", "Enemy2AudioList.txt", null, Class.Enemy));
        FileManager.DataGenerate<CharacterData>("SaveFile/EnemyData/EnemyData3.txt",
           new CharacterData("자이언트 트롤", "Enemy", 6, 0, AttackType.Melee, 600, 0.7f, 12, 1350, null, 1, 0, "Enemy3", "Enemy2AudioList.txt", null, Class.Enemy));

        List<string> audiolist = new List<string>();
        audiolist.Add("bow_atk");
        audiolist.Add("female_death");
        FileManager.ListDataGenerate<string>("SaveFile/UserData/Character2AudioList.txt", audiolist);

        audiolist = new List<string>();
        audiolist.Add("energy_atk");
        audiolist.Add("male_death");
        audiolist.Add("storm");
        FileManager.ListDataGenerate<string>("SaveFile/UserData/Character3AudioList.txt", audiolist);

        List<EquipmentData> list = new List<EquipmentData>();
        EquipmentData obj1 = new EquipmentData("강철투구", EquipmentMainType.Head, EquipmentSubType.Helm, EquipmentGrade.common, 0, 20, 0, 1000, false, "마법사");
        EquipmentData obj2 = new EquipmentData("우든스태프", EquipmentMainType.Weapon, EquipmentSubType.Staff, EquipmentGrade.common, 20, 0, 0, 100, false, "마법사");
        EquipmentData obj3 = new EquipmentData("천망토", EquipmentMainType.Armour, EquipmentSubType.Cloth, EquipmentGrade.common, 0, 30, 0, 2000, false, "마법사");
        EquipmentData obj4 = new EquipmentData("사파이어목걸이", EquipmentMainType.Accessorie, EquipmentSubType.Neckless, EquipmentGrade.common, 10, 0, 0.1f, 2000, false, "마법사");
        list.Add(obj1); list.Add(obj2); list.Add(obj3); list.Add(obj4);
        FileManager.ListDataGenerate<EquipmentData>("SaveFile/UserData/Character3EquipList.txt", list);

        FileManager.DataGenerate<AccountData>("SaveFile/AccountData.txt", new AccountData("성준이", 1, 9000, 3, 0));

        FileManager.DataGenerate<StageData>("SaveFile/StageData/Stage1.txt", new StageData(1, StageGrade.Common ,"Stage1", "RewardList1", "EnemyList1", 10));
        List<string> temp = new List<string>(); temp.Add("Gold"); temp.Add("Rare");
        FileManager.ListDataGenerate<string>("SaveFile/StageData/RewardList1.txt", temp);
        List<string> temp2 = new List<string>(); temp2.Add("EnemyData1"); temp2.Add("EnemyData1");
        FileManager.ListDataGenerate<string>("SaveFile/StageData/EnemyList1.txt", temp2);

        FileManager.DataGenerate<StageData>("SaveFile/StageData/Stage2.txt", new StageData(2, StageGrade.Rare , "Stage2", "RewardList2", "EnemyList2", 15));
        FileManager.ListDataGenerate<string>("SaveFile/StageData/RewardList2.txt", temp);
        temp2 = new List<string>();
        temp2.Add("EnemyData1"); temp2.Add("EnemyData2"); temp2.Add("EnemyData2");
        FileManager.ListDataGenerate<string>("SaveFile/StageData/EnemyList2.txt", temp2);

        FileManager.DataGenerate<StageData>("SaveFile/StageData/Stage3.txt", new StageData(3, StageGrade.Boss, "Stage3", "RewardList3", "EnemyList3", 40));
        temp.Add("Unique"); temp.Add("Legend");
        FileManager.ListDataGenerate<string>("SaveFile/StageData/RewardList3.txt", temp);
        temp2 = new List<string>(); temp2.Add("EnemyData3");
        FileManager.ListDataGenerate<string>("SaveFile/StageData/EnemyList3.txt", temp2);
    }
}
