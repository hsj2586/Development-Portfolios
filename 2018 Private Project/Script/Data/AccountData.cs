using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct AccountData
{
    // 직렬화에 사용되는 계정 데이터 구조체.
    public string username;
    public int level;
    public int gold;
    public int numofCharacter;
    public int current_stage;

    public AccountData(string username, int level, int gold, int numofCharacter, int current_stage)
    {
        this.username = username;
        this.level = level;
        this.gold = gold;
        this.numofCharacter = numofCharacter;
        this.current_stage = current_stage;
    }
}
