using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StageGrade
{
    Common, // 일반
    Rare,   // 레어
    Boss  // 보스
}

[Serializable]
public class StageData
{
    // 직렬화에 사용되는 스테이지 데이터 클래스.
    public int stage_idx;
    public StageGrade stage_grade;
    public string stage_texture;
    public string stage_reward;
    public string stage_enemylist;
    public int stage_exp;

    public StageData(int stage_idx, StageGrade stage_grade, string stage_texture, string stage_reward, string stage_enemylist, int stage_exp)
    {
        this.stage_idx = stage_idx;
        this.stage_grade = stage_grade;
        this.stage_texture = stage_texture;
        this.stage_reward = stage_reward;
        this.stage_enemylist = stage_enemylist;
        this.stage_exp = stage_exp;
    }
}
