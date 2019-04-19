using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedUp : StatusEffect
{
    // 공격 속도가 증가하는 상태 변화(버프)효과 기능을 담당하는 스크립트.

    float atkspeed;

    public IEnumerator GetStatusEffect(GameObject target, float lasting_time, GameObject particle, float atkspeed)
    {
        Access_target = target;
        Access_lastingtime = lasting_time;
        Access_particle = particle;
        Access_battlemanager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        this.atkspeed = atkspeed;
        return this.Activate();
    }

    protected override IEnumerator Activate()
    {
        float elapstime = 0;
        Access_target.GetComponent<AllyCharacter>().Access_atkspeed *= atkspeed;
        while(elapstime <= Access_lastingtime)
        {
            yield return StartCoroutine(GetTurn());
            yield return new WaitForFixedUpdate();
            elapstime += Time.deltaTime;
        }
        Access_target.GetComponent<AllyCharacter>().Access_atkspeed /= atkspeed;
        StatusEffectListPop(Access_searchIndex); // 자신이 귀속하고 있는 캐릭터의 버프리스트에서 제거
        Destroy(gameObject);
    }
}
