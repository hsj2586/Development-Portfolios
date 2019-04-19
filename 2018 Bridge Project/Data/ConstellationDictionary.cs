using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConstellationDictionary
{
    public string constellationName;
    public int totalSkillExp;
    public string contellationContent;
    public bool isLocked;

    public ConstellationDictionary(string constellationName, int totalSkillExp, string contellationContent, bool isLocked)
    {
        this.constellationName = constellationName;
        this.totalSkillExp = totalSkillExp;
        this.contellationContent = contellationContent;
        this.isLocked = isLocked;
    }

    public int GetLevelOfSkill() // 스킬 레벨
    {
        int level = 1;
        float temp = totalSkillExp;
        for (int n = 1; ; n++) // 레벨이 10까지, 각각 100, 300, 700, 1300, 2100 ... 의 경험치로 모델을 가정.
                               // 100 * ( n^2 - n + 1 ) 의 계차수열 형태
        {
            temp = temp - 100 * (Mathf.Pow(n, 2) - n + 1);
            if (temp >= 0)
                level++;
            else
                return level;
        }
    }

    public float GetRatioExpOfLevel() // 현재 스킬 레벨의 경험치 비율
    {
        float temp = totalSkillExp;

        for (int n = 1; ; n++) // 레벨이 10까지, 각각 100, 300, 700, 1300, 2100 ... 의 경험치로 모델을 가정.
                               // 100 * ( n^2 - n + 1 ) 의 계차수열 형태
        {
            temp = temp - 100 * (Mathf.Pow(n, 2) - n + 1);
            if (temp < 0)
            {
                temp = temp + 100 * (Mathf.Pow(n, 2) - n + 1);
                return (temp / (100 * (Mathf.Pow(n, 2) - n + 1)));
            }
        }
    }
}