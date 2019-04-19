using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public struct SkillData
{
    // 직렬화에 사용되는 스킬 데이터 구조체.
    public string skill_name;
    public string skill_manual;
    public float skill_cooltime;
    public int skill_image;
}
