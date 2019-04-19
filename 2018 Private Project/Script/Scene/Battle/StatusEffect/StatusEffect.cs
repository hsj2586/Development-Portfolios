using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    // 상태 변화 효과 기능을 하는 추상 클래스.

    int search_idx; // 외부 검색용 인덱스
    GameObject target;
    float lasting_time;
    GameObject particle;
    BattleManager battle_manager;

    protected GameObject Access_target
    {
        get { return this.target; }
        set { this.target = value; }
    }

    protected float Access_lastingtime
    {
        get { return this.lasting_time; }
        set { this.lasting_time = value; }
    }

    protected GameObject Access_particle
    {
        get { return this.particle; }
        set { this.particle = value; }
    }

    protected BattleManager Access_battlemanager
    {
        get { return this.battle_manager; }
        set { this.battle_manager = value; }
    }

    public int Access_searchIndex
    {
        get { return this.search_idx; }
        set { this.search_idx = value; }
    }

    protected IEnumerator GetTurn()
    {
        while (battle_manager.Access_turn) { yield return new WaitForFixedUpdate(); }
    }

    protected void StatusEffectListPop(int key)
    {
        transform.parent.GetComponent<StatusEffectManager>().StatusEffectListPop(key);
    }

    protected abstract IEnumerator Activate();
}
