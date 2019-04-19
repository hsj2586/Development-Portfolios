using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Constellation : MonoBehaviour
{
    #region 별자리 기본 속성
    int level;

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }
    #endregion
    // 별자리 클래스의 기반이 되는 클래스
    protected PlayerProperty playerProperty;
    protected PlayerBehaviour playerBehaviour;
    private GameObject SlashObjectPool; // 공격 효과 풀
    private GameObject SlashEffectsPool; // 타격 효과 풀
    private Transform[] SlashEffects;
    private Transform[] SlashSubEffects;
    protected Skill skill;
    private int tempIdx;

    void Awake()
    {
        playerProperty = GetComponent<PlayerProperty>();
        playerBehaviour = GetComponent<PlayerBehaviour>();

        playerProperty.Attackable = true;
        playerProperty.IsDead = false;
        tempIdx = 0;

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
        Init();
    }

    public void Attack() // 공격 행동
    {
        if (playerProperty.Attackable && !playerProperty.IsDead) // 공격 가능한 상태(공격속도 조건)이면서, 죽지 않았을 경우
        {
            StartCoroutine(WaitingNextAttack());
            DoAttack(); // 추상 메소드를 호출
        }
    }

    IEnumerator WaitingNextAttack() // 공격 속도 동기화를 위한 코루틴
    {
        playerProperty.Attackable = false;
        yield return new WaitForSeconds(playerProperty.AttackSpeed);
        playerProperty.Attackable = true;
    }

    public void Skill()
    {
        skill.DoSkill();
    }

    protected IEnumerator SlashEffect() // 공격 파티클 이펙트 생성
    {
        tempIdx = Random.Range(0, SlashEffects.Length);
        yield return new WaitForSeconds(0.05f);
        GameObject temp = SlashEffects[tempIdx].GetComponent<ObjectPool>().ObjectPoolPop(playerProperty.Direction);
        yield return new WaitForSeconds(0.2f);
        SlashEffects[tempIdx].GetComponent<ObjectPool>().ObjectPoolPush(temp);
    }

    public IEnumerator SlashSubEffect() // 타격시 공격 파티클 이펙트 생성
    {
        GameObject temp2 = SlashSubEffects[tempIdx].GetComponent<ObjectPool>().ObjectPoolPop(playerProperty.Direction, transform.position);
        yield return new WaitForSeconds(0.5f);
        SlashSubEffects[tempIdx].GetComponent<ObjectPool>().ObjectPoolPush(temp2);
    }

    protected abstract void Init(); // 별자리 클래스의 초기화

    protected abstract void DoAttack(); // 별자리 클래스의 공격 행동
}
