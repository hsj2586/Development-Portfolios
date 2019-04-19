using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AriesSkill : Skill
{
    private PlayerProperty playerProperty;
    private GameObject SlashObjectPool; // 공격 효과 풀
    private GameObject SlashEffectsPool; // 타격 효과 풀
    private Transform[] SlashEffects;
    private Transform[] SlashSubEffects;
    private Transform transform_;
    private int tempIdx;

    public void Init(PlayerProperty pp, Transform tf)
    {
        playerProperty = pp;
        transform_ = tf;
        playerProperty.Attackable = true;
        playerProperty.IsDead = false;
        tempIdx = 0;

        SkillImage = Resources.Load<Sprite>("SkillImage/" + GetType().Name.Substring(0, GetType().Name.Length - 5));
        GameObject.Find("SkillButton").transform.GetChild(0).GetComponent<Image>().overrideSprite = SkillImage;

        /* 임시 오브젝트 풀링 코드, 추후에는 해당 클래스의 이펙트 풀을 읽어와 동적으로 
         * 생성하는 방향으로 구현해야 할 것.
         */
        SlashObjectPool = GameObject.Find("SlashEffect");
        SlashEffectsPool = GameObject.Find("SlashSubEffectPool");
        SlashEffects = new Transform[SlashObjectPool.transform.childCount];
        SlashSubEffects = new Transform[SlashObjectPool.transform.childCount];
        for (int i = 0; i < SlashObjectPool.transform.childCount; i++)
        {
            SlashEffects[i] = SlashObjectPool.transform.GetChild(i);
            SlashSubEffects[i] = SlashEffectsPool.transform.GetChild(i);
        }
    }
    // 양자리 클래스의 스킬을 구현

    public override void DoSkill()
    {
        if (CheckSkillAvailability())
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform_.position, 3.0f);
            playerProperty.StartCoroutine(SlashEffect());
            if (cols.Length > 0)
            {
                for (int i = 0; i < cols.Length; ++i)
                {
                    if (cols[i].tag == "Enemy")
                    {
                        cols[i].SendMessage("BeAttacked", playerProperty, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            StartCoroutine(SkillActivation());
        }
    }

    private IEnumerator SlashEffect() // 공격 파티클 이펙트 생성
    {
        tempIdx = Random.Range(0, SlashEffects.Length);
        yield return new WaitForSeconds(0.05f);
        GameObject temp = SlashEffects[tempIdx].GetComponent<ObjectPool>().ObjectPoolPop(playerProperty.Direction);
        yield return new WaitForSeconds(0.2f);
        SlashEffects[tempIdx].GetComponent<ObjectPool>().ObjectPoolPush(temp);
    }
}
