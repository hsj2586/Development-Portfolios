using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillState { Ready, Set, Active };

public interface SkillBehaviour // 스킬 인터페이스, 행동을 기재함
{
    void Init();
    IEnumerator Idle();
    void isActive();
    IEnumerator IconAnimation();
}

public class Skill : MonoBehaviour, SkillBehaviour
{
    // '스킬'에 대한 정보를 가지는 클래스.
    [SerializeField]
    private string skill_name;
    [SerializeField]
    private string skill_manual;
    [SerializeField]
    private float skill_cooltime;
    [SerializeField]
    private int skill_image;
    [SerializeField]
    private SkillState state;
    GameObject skillUI;
    GameObject skillUI_child;
    GameObject skillUI_text;
    Image skillUI_anim;
    IEnumerator main_routine;
    IEnumerator sub_routine;
    BattleManager battle_manager;
    bool clickable;

    #region 스킬 속성 접근
    public string Access_skillname
    {
        get { return skill_name; }
        set { skill_name = value; }
    }

    public float Access_skillcooltime
    {
        get { return skill_cooltime; }
        set { skill_cooltime = value; }
    }

    public int Access_skillimage
    {
        get { return skill_image; }
        set { skill_image = value; }
    }

    public SkillState Access_skillstate
    {
        get { return state; }
        set { state = value; }
    }

    public string Access_skill_manual
    {
        get { return this.skill_manual; }
        set { skill_manual = value; }
    }

    public IEnumerator Access_mainroutine
    {
        get { return this.main_routine; }
        set { this.main_routine = value; }
    }
    #endregion

    public void Init() // 스킬 세팅 초기화
    {
        battle_manager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        skillUI = GameObject.FindGameObjectWithTag("ScreenUI").transform.Find(GetComponent<AllyCharacter>().Access_prefabname).Find("SkillImage").gameObject;
        skillUI.GetComponent<Button>().onClick.AddListener(() => isActive());
        skillUI_child = skillUI.transform.Find("Image").gameObject;
        skillUI_text = skillUI.transform.Find("Text").gameObject;
        skillUI_anim = GameObject.FindGameObjectWithTag("ScreenUI").transform.Find(GetComponent<AllyCharacter>().Access_prefabname).Find("Background4").
            GetComponent<Image>();
        clickable = true;
        sub_routine = Idle();
        StartCoroutine(sub_routine);
    }

    public IEnumerator Idle()
    {
        while (battle_manager.Access_turn) { yield return new WaitForFixedUpdate(); }
        yield return null;
        if (state == SkillState.Ready)
        {
            if (skillUI_child.GetComponent<Image>().fillAmount <= 0)
            {
                state = SkillState.Set;
                skillUI.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                skillUI_text.SetActive(false);
            }
            else
            {
                skillUI_child.GetComponent<Image>().fillAmount -= 0.0167f / skill_cooltime; // 0.0167 -> 1/60
                skillUI_text.GetComponent<Text>().text = Mathf.Ceil(skill_cooltime * skillUI_child.GetComponent<Image>().fillAmount).ToString();
            }
            sub_routine = Idle();
            StartCoroutine(sub_routine);
        }
        else if (state == SkillState.Set)
        {
            sub_routine = IconAnimation();
            StartCoroutine(sub_routine);
        }
    }

    public void isActive() // 스킬 버튼 클릭시에 발동하는 코루틴
    {
        if (state == SkillState.Set && clickable == true && battle_manager.Access_battlecond == true)
        {
            clickable = false;
            battle_manager.GetTurn(Activate(), GetComponent<AllyCharacter>());
        }
    }

    IEnumerator Activate() // 스킬 조건 만족시 발동하는 코루틴
    {
        state = SkillState.Active;
        skillUI.GetComponent<Image>().color = new Color(1, 1, 1, 0.5882f); // 0.5882 -> 150/255
        skillUI_child.GetComponent<Image>().fillAmount = 1;
        StopCoroutine(sub_routine);
        skillUI_anim.color = new Color(1, 0.5882f, 0, 0.7843f); // 0.7843 -> 200/255
        main_routine = ActiveSkill();
        yield return StartCoroutine(main_routine);
    }

    public IEnumerator IconAnimation() // 스킬이 활성화 될 경우 (쿨타임이 만족할 경우) 표시되는 UI 애니메이션 효과 코루틴 
    {
        float elapstime = 0;
        while (state == SkillState.Set)
        {
            yield return null;
            elapstime += 0.1f;
            float value = Mathf.Abs(Mathf.Sin(elapstime));
            skillUI_anim.color = new Color(value, value * 0.5882f, 0, value * 0.7843f);
        }
    }

    public void Stop_routine() // 스킬을 보유한 Summoner가 죽었을 경우 모든 코루틴을 stop 시키는 메소드
    {
        StopCoroutine(main_routine);
        StopCoroutine(sub_routine);
        skillUI_anim.color = new Color(0, 0, 0, 0.7843f);
        skillUI_text.SetActive(false);
        skillUI_child.GetComponent<Image>().fillAmount = 0;
    }

    protected void PlaySound(int idx) // 음향 재생
    {
        SoundManager.Instance.PlaySound(GetComponent<Character>().Access_audioclip(idx));
    }

    protected void SetAnimation(int idx) // 애니메이션 세팅
    {
        GetComponent<Character>().Access_animator.SetInteger("State", idx);
    }

    protected IEnumerator InitializeSkill() // 스킬 상태 초기화
    {
        yield return null;
        clickable = true;
        skillUI_text.gameObject.SetActive(true);
        state = SkillState.Ready;
        sub_routine = Idle();
        StartCoroutine(sub_routine);
        skillUI_anim.color = new Color(0, 0, 0, 0.7843f);
    }

    protected void AttackTrigger(GameObject opponent, float damage)
    {
        GetComponent<AllyCharacter>().AttackTrigger(opponent, damage);
    }

    protected void StatusEffectListPush(IEnumerator status_effect, StatusEffect component)
    {
        int idx = GetComponent<StatusEffectManager>().StatusEffectListPush(status_effect);
        component.Access_searchIndex = idx;
    }

    protected virtual IEnumerator ActiveSkill() { yield return null; } // 세부 스킬 구현을 기재.
}
