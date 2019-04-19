using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    private int skillLevel; // 스킬 레벨
    private float cooltime; // 쿨타임
    [SerializeField]
    private Sprite skillImage;

    bool skillAvailability; // 스킬 사용 가능 여부
    float remainCooltime; // 쿨타임 계산용 변수
    SkillUIUpdate skillUIUpdate;

    public float Cooltime
    {
        get
        {
            return cooltime;
        }

        set
        {
            cooltime = value;
        }
    }

    protected int SkillLevel
    {
        get
        {
            return skillLevel;
        }

        set
        {
            skillLevel = value;
        }
    }

    protected Sprite SkillImage
    {
        get
        {
            return skillImage;
        }

        set
        {
            skillImage = value;
        }
    }

    public void SkillInit(int skillLevel, float cooltime)
    {
        this.skillLevel = skillLevel;
        this.cooltime = cooltime;
        skillAvailability = true;
        skillUIUpdate = GameObject.Find("SkillButton").GetComponent<SkillUIUpdate>();
    }

    public bool CheckSkillAvailability() // 스킬 사용 가능 여부를 묻는 메소드
    {
        return skillAvailability ? true : false;
    }

    public IEnumerator SkillActivation() // 스킬 발동 직후 처리 메소드
    {
        skillAvailability = false;
        skillUIUpdate.UIOn();
        remainCooltime = cooltime;
        while (remainCooltime >= 0)
        {
            yield return null;
            remainCooltime -= Time.deltaTime;
            skillUIUpdate.UIUpdate(cooltime, remainCooltime);
        }
        skillUIUpdate.UIOff();
        skillAvailability = true;
    }

    public abstract void DoSkill();
}
