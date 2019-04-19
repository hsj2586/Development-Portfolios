using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyCharacter : Character, CharacterBehaviour
{
    // 아군 캐릭터에게 적용되는 스크립트.

    GameObject[] allies;
    GameObject[] enemies;
    IEnumerator main_routine;
    BattleManager battle_manager;
    Skill skill;

    #region UI 변수
    Slider hpslider;
    Slider hpslider2;
    Slider attackslider;
    GameObject headupPos; // UI를 머리위에 위치시키기 위한 변수
    GameObject canvas; // UI 변수
    BattleUIManager battle_uimanager;
    GameObject isdead_UI;
    BarAnimation baranim; // 슬라이더 움직임 제어를 위한 변수
    #endregion

    public void Init()
    {
        skill = (Skill)GetComponent(Access_skillname);
        allies = GameObject.FindGameObjectsWithTag("Ally");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        isdead_UI = GameObject.FindGameObjectWithTag("ScreenUI").transform.Find(Access_prefabname).Find("IsDead").gameObject;
        battle_manager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        battle_uimanager = battle_manager.GetComponent<BattleUIManager>();
        Access_animator = GetComponent<Animator>();
        AttackTypeSetting();

        #region 슬라이더 UI 초기화
        canvas = battle_uimanager.Sliderpool_Pop(this.gameObject);
        hpslider = canvas.transform.Find("HpBar").Find("HpSlider").GetComponent<Slider>();
        hpslider2 = canvas.transform.Find("HpBar").Find("HpSlider2").GetComponent<Slider>();
        attackslider = canvas.transform.Find("AtkSlider").GetComponent<Slider>();
        headupPos = transform.GetChild(0).gameObject;
        canvas.transform.position = headupPos.transform.position;
        baranim = GetComponent<BarAnimation>();
        baranim.Init(hpslider, hpslider2, attackslider);
        #endregion

        #region 스크린 UI 초기화
        ((Skill)GetComponent(Access_skillname)).Init();
        Transform screenUI = GameObject.FindGameObjectWithTag("ScreenUI").transform.Find(canvas.name);
        screenUI.gameObject.SetActive(true);
        screenUI.Find("FaceImage").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprite/" + Access_faceimage.ToString());
        screenUI.Find("SkillImage").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprite/skillImage" + Access_skillimage.ToString());
        #endregion

        #region 장비 능력치 적용
        for (int i = 0; i < Access_equipments.Count; i++)
        {
            if (Access_equipment(i) != null)
            {
                Access_atkpower += Access_equipment(i).atkpower;
                Access_atkspeed += Access_equipment(i).atkspeed;
                Access_defpower += Access_equipment(i).defpower;
            }
        }
        #endregion

        canvas.transform.GetChild(0).GetComponent<Text>().text = "LV." + Access_level.ToString() + " " + Access_charactername;
        Access_State = CharacterState.Idle;
        main_routine = Idle();
        StartCoroutine(main_routine);
        StartCoroutine(update());
    }

    public IEnumerator Attack()
    {
        while (skill.Access_skillstate == SkillState.Active)
        {
            yield return null;
        }
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length != 0)
        {
            baranim.AccessAttacksliderValue = 0;

            yield return StartCoroutine(GetComponent<AttackBehaviour>().AttackAnim(enemies[0], Access_atkpower));
            Access_State = CharacterState.Idle;
            main_routine = Idle();
            StartCoroutine(main_routine);
        }
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
        EnemyCharacter temp = opponent.GetComponent<EnemyCharacter>();
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
            isdead_UI.SetActive(true);
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

    public void LevelUp()
    {
        switch (Access_Class)
        {
            case Class.Knight:
                Access_atkpower += 5;
                Access_atkspeed += 0.01f;
                Access_defpower += 2;
                Access_maxhealthpoint += 50;
                break;
            case Class.Archer:
                Access_atkpower += 2;
                Access_atkspeed += 0.02f;
                Access_defpower += 1;
                Access_maxhealthpoint += 30;
                break;
            case Class.Wizard:
                Access_atkpower += 6;
                Access_atkspeed += 0.01f;
                Access_defpower += 1;
                Access_maxhealthpoint += 25;
                break;
        }
    }
}
