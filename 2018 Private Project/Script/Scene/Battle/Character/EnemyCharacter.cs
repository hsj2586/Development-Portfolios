using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCharacter : Character, CharacterBehaviour
{
    // 적군 캐릭터에게 적용되는 스크립트.

    GameObject[] allies;
    GameObject[] enemies;
    IEnumerator main_routine;
    BattleManager battle_manager;

    #region UI 변수
    Slider hpslider;
    Slider hpslider2;
    Slider attackslider;
    GameObject headupPos; // UI를 머리위에 위치시키기 위한 변수
    GameObject canvas; // UI 변수
    BattleUIManager battle_uimanager;
    BarAnimation baranim; // 슬라이더 움직임 제어를 위한 변수
    #endregion

    public void Init()
    {
        allies = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = GameObject.FindGameObjectsWithTag("Ally");
        battle_manager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        battle_uimanager = battle_manager.GetComponent<BattleUIManager>();
        Access_animator = GetComponent<Animator>();
        AttackTypeSetting();

        #region 슬라이더 UI 초기화
        canvas = battle_uimanager.Sliderpool_Pop(this.gameObject);
        canvas.transform.GetChild(0).GetComponent<Text>().text = Access_charactername;
        hpslider = canvas.transform.GetChild(2).GetChild(2).GetComponent<Slider>();
        hpslider2 = canvas.transform.GetChild(2).GetChild(1).GetComponent<Slider>();
        attackslider = canvas.transform.GetChild(1).GetComponent<Slider>();
        headupPos = transform.GetChild(0).gameObject;
        canvas.transform.position = headupPos.transform.position;
        baranim = GetComponent<BarAnimation>();
        baranim.Init(hpslider, hpslider2, attackslider);
        #endregion

        Access_State = CharacterState.Idle;
        main_routine = Idle();
        StartCoroutine(main_routine);
        StartCoroutine(update());
    }

    public IEnumerator Idle()
    {
        switch (Access_State)
        {
            case CharacterState.Stun:
                main_routine = Idle();
                StartCoroutine(main_routine);
                break;
            case CharacterState.Attack:
                main_routine = Attack();
                battle_manager.GetTurn(main_routine, GetComponent<Character>());
                break;
            case CharacterState.Idle:
                while (battle_manager.Access_turn) { yield return new WaitForFixedUpdate(); } // 공격 기회인지 아닌지 체크, 현재 다른 캐릭터가 공격중일 경우 무한루프
                yield return null;
                baranim.AccessAttacksliderValue += 1 / 60f * Access_atkspeed;
                if (baranim.AccessAttacksliderValue == 1)
                {
                    Access_State = CharacterState.Attack;
                }
                main_routine = Idle();
                StartCoroutine(main_routine);
                break;
            case CharacterState.Dead:
                break;
        }
    }

    public IEnumerator Attack()
    {
        enemies = GameObject.FindGameObjectsWithTag("Ally");
        if (enemies.Length != 0)
        {
            baranim.AccessAttacksliderValue = 0;

            yield return StartCoroutine(GetComponent<AttackBehaviour>().AttackAnim(enemies[0], Access_atkpower));
            Access_State = CharacterState.Idle;
            main_routine = Idle();
            StartCoroutine(main_routine);
        }
    }

    protected IEnumerator update()
    {
        while (true)
        {
            yield return null;
            canvas.transform.position = headupPos.transform.position;
        }
    }

    public void AttackTrigger(GameObject opponent, float damage) // 공격 발생 함수
    {
        AllyCharacter temp = opponent.GetComponent<AllyCharacter>();
        temp.AttackedByTrigger(damage);
    }

    public void AttackedByTrigger(float damage) // 공격에 따른 결과 적용 함수
    {
        float calcDamage = damage * (100 / (100 + Access_defpower));
        Access_healthpoint = Mathf.Clamp((Access_healthpoint - calcDamage), 0, Access_maxhealthpoint);
        float hpratio = Access_healthpoint / Access_maxhealthpoint;
        StartCoroutine(baranim.HpSliderAnimation(hpratio));

        if (Access_healthpoint == 0)
        {
            StopCoroutine(main_routine);
            gameObject.tag = "Untagged";
            canvas.SetActive(false);
            baranim.AccessAttacksliderValue = 0;
            SoundManager.Instance.PlaySound(Access_audioclip(1));
            Access_animator.SetInteger("State", 1);
            Access_State = CharacterState.Dead;
            battle_manager.Check();
        }
        else
            battle_uimanager.Labelpool_Pop(transform, calcDamage);
    }
}
