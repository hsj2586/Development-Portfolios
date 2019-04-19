using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    // 전투 종료 후, 보상을 결정하고 지급하는 스크립트.

    Stage stage;
    int probabilty;
    float value;
    Account account;
    List<EquipmentData> temp_obj;
    List<EquipmentData> reward_param; // battle_manager에서 보상 UI창에 필요한 정보를 전달하는 목적의 리스트 변수

    public List<EquipmentData> TryGetReward(Stage stage)
    {
        this.stage = stage;
        reward_param = new List<EquipmentData>();
        for (int i = 0; i < stage.Access_reward.Count; i++)
        {
            GetProbability(i);
        }
        return reward_param;
    }

    public void GetProbability(int idx)
    {
        probabilty = Random.Range(0, 100);
        switch (stage.Access_grade)
        {
            case StageGrade.Common: // 골드 100%, 레어 40%
                value = 100 - idx * 40;
                break;
            case StageGrade.Rare: // 골드 100%, 레어 40%, 유니크 13.33%
                value = 100 / (idx + 1) - 10 * idx;
                break;
            case StageGrade.Boss: // 골드 100%, 레어 50%, 유니크 25%, 전설 12.5%
                value = 100 / Mathf.Pow(2, idx);
                break;
        }
        print("Random 값 : " + probabilty + ", 상한 값 : " + value);
        if (probabilty <= value) // 확률에 당첨된 경우 보상 획득
        {
            print("당첨!");
            GetReward(idx);
        }
    }

    void GetReward(int idx)
    {
        switch (idx)
        {
            case 0: // 골드 획득
                int temp;
                account = FileManager.AccountDataLoad("SaveFile/AccountData.txt");
                temp = ((int)stage.Access_grade + 1) * 500; // 스테이지 등급에 따라 골드 획득량 결정
                FileManager.DataGenerate<AccountData>("SaveFile/AccountData.txt",
                    new AccountData(account.Access_name, account.Access_level, account.Access_gold + temp, account.Access_numofcharacter, account.Access_currentstage));
                FileManager.DataGenerate<Account>("SaveFile/AccountData.txt", account);
                break;
            case 1: // 레어 아이템 획득
                temp_obj = FileManager.ListDataLoad<EquipmentData>("SaveFile/EquipmentData/RareEquipmentList.txt");
                reward_param.Add(temp_obj[Random.Range(0, temp_obj.Count)]);
                temp_obj = FileManager.ListDataLoad<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt");
                temp_obj.Add(reward_param[reward_param.Count-1]);
                FileManager.ListDataGenerate<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt", temp_obj);
                break;
            case 2: // 유니크 아이템 획득
                temp_obj = FileManager.ListDataLoad<EquipmentData>("SaveFile/EquipmentData/UniqueEquipmentList.txt");
                reward_param.Add(temp_obj[Random.Range(0, temp_obj.Count)]);
                temp_obj = FileManager.ListDataLoad<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt");
                temp_obj.Add(reward_param[reward_param.Count - 1]);
                FileManager.ListDataGenerate<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt", temp_obj);
                break;
            case 3: // 전설 아이템 획득
                temp_obj = FileManager.ListDataLoad<EquipmentData>("SaveFile/EquipmentData/LegendEquipmentList.txt");
                reward_param.Add(temp_obj[Random.Range(0, temp_obj.Count)]);
                temp_obj = FileManager.ListDataLoad<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt");
                temp_obj.Add(reward_param[reward_param.Count - 1]);
                FileManager.ListDataGenerate<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt", temp_obj);
                break;
        }
    }
}
