using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterState { Idle, Dead, Attack, Stun }
public enum Class { Brute, Knight, Rogue, Archer, Wizard, Warlock, Enemy }

public interface CharacterBehaviour // 캐릭터 인터페이스, 기본 행동 양식을 기재함
{
    void Init();
    void AttackTrigger(GameObject opponent, float damage);
    void AttackedByTrigger(float damage);
    IEnumerator Idle();
    IEnumerator Attack();
}

public class Character : MonoBehaviour // 캐릭터 클래스, 캐릭터에 귀속되는 데이터
{
    // '캐릭터'에 대한 정보를 가지는 클래스.
    #region 캐릭터 속성 변수
    [SerializeField]
    private string prefabname;
    [SerializeField]
    private string character_name;
    [SerializeField]
    private int level;
    [SerializeField]
    private int exp;
    [SerializeField]
    private AttackType atktype;
    [SerializeField]
    private float atkpower;
    [SerializeField]
    private float atkspeed;
    [SerializeField]
    private float defpower;
    [SerializeField]
    private float healthpoint;
    [SerializeField]
    private float maxhealthpoint;
    [SerializeField]
    private CharacterState state;
    [SerializeField]
    private List<IEnumerator> stat_effects;
    [SerializeField]
    private Class character_class;
    [SerializeField]
    private int faceimage;
    [SerializeField]
    private int skillimage;
    [SerializeField]
    private List<EquipmentData> equipments; // equipments[0] = "무기", equipments[1] = "갑옷", equipments[2] = "투구" 이라고 설정해둠.
    [SerializeField]
    private List<AudioClip> audioclip;
    [SerializeField]
    public string skillname;
    Animator animator;
    #endregion

    #region 캐릭터 속성 접근
    public string Access_prefabname
    {
        get { return this.prefabname; }
        set { this.prefabname = value; }
    }

    public string Access_charactername
    {
        get { return this.character_name; }
        set { this.character_name = value; }
    }

    public AttackType Access_atktype
    {
        get { return this.atktype; }
        set { this.atktype = value; }
    }

    public float Access_atkpower
    {
        get { return this.atkpower; }
        set { this.atkpower = value; }
    }

    public float Access_atkspeed
    {
        get { return this.atkspeed; }
        set { this.atkspeed = value; }
    }

    public float Access_defpower
    {
        get { return this.defpower; }
        set { this.defpower = value; }
    }

    public float Access_healthpoint
    {
        get { return this.healthpoint; }
        set { this.healthpoint = value; }
    }

    public float Access_maxhealthpoint
    {
        get { return maxhealthpoint; }
        set { this.maxhealthpoint = value; }
    }

    public int Access_level
    {
        get { return level; }
        set { this.level = value; }
    }

    public int Access_exp
    {
        get { return exp; }
        set { this.exp = value; }
    }

    public CharacterState Access_State
    {
        get { return this.state; }
        set { this.state = value; }
    }

    public Class Access_Class
    {
        get { return this.character_class; }
        set { this.character_class = value; }
    }

    public Animator Access_animator
    {
        get { return this.animator; }
        set { this.animator = value; }
    }

    public int Access_faceimage
    {
        get { return faceimage; }
        set { this.faceimage = value; }
    }

    public int Access_skillimage
    {
        get { return skillimage; }
        set { this.skillimage = value; }
    }

    public List<EquipmentData> Access_equipments
    {
        get { return this.equipments; }
        set { this.equipments = value; }
    }

    public EquipmentData Access_equipment(int idx)
    {
        return this.equipments[idx];
    }

    public void Access_equipment(int idx, EquipmentData data)
    {
        this.equipments[idx] = data;
    }

    public List<AudioClip> Access_audioclips
    {
        get { return this.audioclip; }
        set { this.audioclip = value; }
    }

    public AudioClip Access_audioclip(int idx)
    {
        return audioclip[idx];
    }

    public void Access_audioclip(int idx, AudioClip data)
    {
        audioclip[idx] = data;
    }

    public string Access_skillname
    {
        get { return skillname; }
        set { skillname = value; }
    }
    #endregion

    protected void AttackTypeSetting()
    {
        if (Access_atktype == AttackType.Melee && !GetComponent<MeleeAttack>())
            gameObject.AddComponent<MeleeAttack>();
        else if (Access_atktype == AttackType.Range)
        {
            if (!GetComponent<RangeAttack>())
                gameObject.AddComponent<RangeAttack>();
            GetComponent<RangeAttack>().Setting();
        }
    }
}
