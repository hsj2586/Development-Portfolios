using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct CharacterData
{
    // 직렬화에 사용되는 캐릭터 데이터 구조체.
    // 캐릭터 전투 정보
    public string character_name;
    public string tag;
    public Class character_class;
    public int faceimage;
    public int skillimage;
    public AttackType atktype;
    public float atkpower;
    public float atkspeed;
    public float defpower;
    public float healthpoint;
    public string skillname;

    // 캐릭터 비전투 정보
    public int level;
    public int exp;

    // 캐릭터 데이터 로드 정보
    public string prefabname;
    public string audioclip;

    //장비
    public string eqipment;

    public CharacterData(string character_name, string tag, int faceimage, int skillimage, AttackType atktype , float atkpower, float atkspeed, float defpower, float healthpoint, string skillname, 
        int level, int exp, string prefabname, string audioclip, string eqipment, Class character_class)
    {
        this.character_name = character_name;
        this.tag = tag;
        this.faceimage = faceimage;
        this.skillimage = skillimage;
        this.atktype = atktype;
        this.atkpower = atkpower;
        this.atkspeed = atkspeed;
        this.defpower = defpower;
        this.healthpoint = healthpoint;
        this.skillname = skillname;
        this.level = level;
        this.exp = exp;
        this.prefabname = prefabname;
        this.audioclip = audioclip;
        this.eqipment = eqipment;
        this.character_class = character_class;
    }
}
