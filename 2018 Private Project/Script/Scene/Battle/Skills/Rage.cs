using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : Skill
{
    // '궁수'의 스킬 Rage

    IEnumerator buff;
    GameObject particle;

    protected override IEnumerator ActiveSkill()
    {
        SetAnimation(4);
        yield return new WaitForSeconds(2);
        SetAnimation(0);
        yield return StartCoroutine(InitializeSkill());

        particle = Instantiate(Resources.Load<GameObject>("Prefabs/AttackSpeedUp"), transform); // 버프 파티클 생성
        particle.transform.position = transform.position + new Vector3(0, 2.5f, 0);
        particle.AddComponent<AttackSpeedUp>();
        buff = particle.GetComponent<AttackSpeedUp>().GetStatusEffect(gameObject, 3, particle, 2);
        StatusEffectListPush(buff, particle.GetComponent<AttackSpeedUp>());
        StartCoroutine(buff);
    }
}
