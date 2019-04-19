using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sagittarius : Constellation
{
    public GameObject arrowObjectPool;

    // 사수자리 클래스의 구체화된 스크립트.
    protected override void Init()
    {
        // 사수자리 클래스의 초기화 내용이 들어감.
        arrowObjectPool = GameObject.Find("ArrowObjectPool");
        gameObject.AddComponent<SagittariusSkill>();
        skill = GetComponent<SagittariusSkill>();
        ((SagittariusSkill)(skill)).Init(playerBehaviour, arrowObjectPool);
        skill.SkillInit(LocalAccount.Instance.GetSkillLevel(GetType().Name), 3);
        playerProperty.JumpSensitivity = playerProperty.JumpSensitivity * 1.3f;

    }

    protected override void DoAttack() // 공격 행동의 구체 메소드
    {
        // 사수자리 클래스의 공격 행동 내용이 들어감.
        bool AttackTargetting = false; // 하나 이상의 적이 맞았는지 여부
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, playerProperty.AttackDistance, 1 << 8);
        StartCoroutine(SlashEffect());
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                float temp = cols[i].transform.position.x - transform.position.x;
                if ((playerProperty.Direction && (temp < 0)) || (!playerProperty.Direction && (temp > 0)))
                {
                    if (cols[i].GetComponent<EnemyProperty>() && !cols[i].GetComponent<EnemyProperty>().IsDead)
                    {
                        AttackTargetting = true;
                        cols[i].SendMessage("BeAttacked", GetComponent<PlayerProperty>(), SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }

        if (AttackTargetting)
        {
            Camera.main.transform.DOShakeRotation(0.5f, 0.45f, 30, 90);
            StartCoroutine(SlashSubEffect());
        }
    }
}
