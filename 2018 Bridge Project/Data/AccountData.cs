using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AccountData
{
    public int numOfLux; // 럭스의 개수
    int totalExperienceOfSunGirl; // 태양소녀의 경험치
    public List<ConstellationDictionary> listOfConstellation; // 보유 별자리 리스트

    public AccountData(int numOfLux, int totalExperienceOfSunGirl, List<ConstellationDictionary> listOfConstellation)
    {
        this.numOfLux = numOfLux;
        this.totalExperienceOfSunGirl = totalExperienceOfSunGirl;
        this.listOfConstellation = listOfConstellation;
    }
}
