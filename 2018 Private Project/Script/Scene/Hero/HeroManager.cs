using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeroManager : MonoBehaviour
{
    // 영웅 관리 창에서의 모든 기능을 담당하는 스크립트.

    [SerializeField]
    Transform buttons;
    [SerializeField]
    AudioClip hero_clip;
    [SerializeField]
    AudioClip hero_button_clip;
    [SerializeField]
    GameObject skill_image;
    [SerializeField]
    GameObject skill_manual;
    [SerializeField]
    Transform characterList;
    [SerializeField]
    Transform panel;
    int selectIndex; // 현재 선택한 캐릭터의 인덱스 저장 변수
    [SerializeField]
    Transform InventoryWindow;
    Transform InventoryWindow_itemlist;
    List<int> exp_data; // 경험치 차트 로드
    [SerializeField]
    Image fade;
    Account account;

    Transform paneltemp_faceImage;
    Transform paneltemp_equipment;
    Transform paneltemp_textlist;
    List<EquipmentData> templist;
    List<EquipmentData> templist2;

    float temp_hp; // panel에 표시할 추가 능력치 저장 변수
    float temp_atkpower;
    float temp_atkspeed;
    float temp_defpower;

    void Awake()
    {
        Transform temp;
        paneltemp_equipment = panel.Find("Equipment");
        paneltemp_faceImage = panel.Find("faceImage");
        paneltemp_textlist = panel.Find("textList");
        InventoryWindow_itemlist = InventoryWindow.Find("List").GetChild(0);
        exp_data = FileManager.ListDataLoad<int>("SaveFile/ExpData.txt");
        account = FileManager.AccountDataLoad("SaveFile/AccountData.txt");

        for (int i = 0; i < account.Access_numofcharacter; i++)
        {
            temp = characterList.GetChild(i);
            temp.gameObject.SetActive(true);
            InfoSetting(i, temp);
        }

        ButtonSetting();
    }

    void ButtonSetting()
    {
        for (int i = 0; i < buttons.childCount; i++)
        {
            int temp = i;
            buttons.GetChild(i).GetComponent<Button>().onClick.AddListener(() => ExecuteCommand(temp, 0)); // 일반 버튼('뒤로' 버튼 ... etc) 세팅
        }

        for (int i = 0; i < characterList.childCount; i++)
        {
            if (characterList.GetChild(i).gameObject.activeInHierarchy)
            {
                int temp = i;
                characterList.GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => ExecuteCommand(temp, 1)); // 영웅 버튼 세팅
            }
        }

        for (int i = 0; i < paneltemp_equipment.childCount; i++)
        {
            int temp = i;
            paneltemp_equipment.GetChild(i).GetComponent<Button>().onClick.AddListener(() => ExecuteCommand(temp, 2)); // 장비 버튼 세팅
        }

        InventoryWindow.Find("back").GetComponent<Button>().onClick.AddListener(() => ExecuteCommand(0, 3)); // 장비 창 '뒤로' 버튼 세팅
    }

    void ExecuteCommand(int value, int type) // type 0 = 일반 버튼, type 1 = 영웅 버튼, type 2 = 장비 버튼, type 3 = 인벤토리창 버튼
    {
        switch (type)
        {
            case 0:
                StartCoroutine(Button_Anim(1));
                break;
            case 1:
                HeroClickExecute(value);
                break;
            case 2:
                EquipmentClickExecute(value);
                break;
            case 3:
                InventoryWindowBack();
                break;
        }
    }

    void Start()
    {
        SoundManager.Instance.PlayMainSound(hero_clip);
    }

    void InfoSetting(int idx, Transform obj)
    {
        obj.GetChild(0).GetComponent<Image>().overrideSprite =
            Resources.Load<Sprite>("Sprite/" + account.Access_character(idx).GetComponent<AllyCharacter>().Access_faceimage.ToString());
    }

    void HeroClickExecute(int idx) // 영웅 버튼 클릭시 콜백
    {
        if (!panel.gameObject.activeInHierarchy) panel.gameObject.SetActive(true);

        selectIndex = idx;
        AllyCharacter temp = account.Access_character(selectIndex).GetComponent<AllyCharacter>();
        temp.Access_equipments = FileManager.ListDataLoad<EquipmentData>("SaveFile/UserData/" + temp.Access_prefabname + "EquipList.txt");
        temp_atkpower = temp_atkspeed = temp_defpower = temp_hp = 0;
        for (int i = 0; i < temp.Access_equipments.Count; i++)
        {
            temp_atkpower += temp.Access_equipment(i).atkpower;
            temp_atkspeed += temp.Access_equipment(i).atkspeed;
            temp_defpower += temp.Access_equipment(i).defpower;
        }
        paneltemp_faceImage.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprite/" + temp.Access_faceimage.ToString());
        paneltemp_textlist.GetChild(0).GetComponent<Text>().text = temp.Access_charactername;
        paneltemp_textlist.GetChild(1).GetComponent<Text>().text =
            "LV : " + temp.Access_level.ToString() + " <color=lime> ( " + ((float)temp.Access_exp / exp_data[temp.Access_level] * 100).ToString("F1") + " % )</color>";
        paneltemp_textlist.GetChild(2).GetComponent<Text>().text = "HP : " + temp.Access_maxhealthpoint + AdditionalText(temp_hp);
        paneltemp_textlist.GetChild(3).GetComponent<Text>().text = "공격력 : " + temp.Access_atkpower.ToString() + AdditionalText(temp_atkpower);
        paneltemp_textlist.GetChild(4).GetComponent<Text>().text = "공격속도 : " + temp.Access_atkspeed.ToString("F2") + AdditionalText(temp_atkspeed);
        paneltemp_textlist.GetChild(5).GetComponent<Text>().text = "방어력 : " + temp.Access_defpower.ToString() + AdditionalText(temp_defpower);
        skill_image.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprite/skillImage" + temp.Access_skillimage.ToString());
        skill_manual.GetComponent<Text>().text = temp.Access_skillname + "\n" +
           ((Skill)temp.GetComponent(temp.Access_skillname)).Access_skill_manual;
        paneltemp_equipment.GetChild(0).GetChild(0).GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprite/" + temp.Access_equipment(0).equipmentname);
        paneltemp_equipment.GetChild(1).GetChild(0).GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprite/" + temp.Access_equipment(1).equipmentname);
        paneltemp_equipment.GetChild(2).GetChild(0).GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprite/" + temp.Access_equipment(2).equipmentname);
    }

    string AdditionalText(float value) // 추가 능력치를 나타내는 메소드
    {
        if (value != 0)
        {
            return " + <color=lime>" + value.ToString() + "</color>";
        }
        else
            return null;
    }

    void EquipmentClickExecute(int idx) // panel 상에서 장비 버튼 클릭시 콜백
    {
        InventoryWindow.gameObject.SetActive(true);
        List<EquipmentData> tempList = FileManager.ListDataLoad<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt");
        AllyCharacter tempCharacter = account.Access_character(selectIndex).GetComponent<AllyCharacter>();

        for (int i = 0; i < tempList.Count; i++)
        {
            if (ClassToSubtype((EquipmentMainType)idx, tempCharacter.Access_Class, tempList[i].subtype) && !tempList[i].isMounted)
            {
                Transform temp = InventoryWindow_itemlist.GetChild(i);
                int temp_idx = i;
                temp.gameObject.SetActive(true);
                temp.GetComponent<Button>().onClick.RemoveAllListeners();
                temp.GetComponent<Button>().onClick.AddListener(() => ClickEquipmentOnInventory(idx, temp_idx, tempCharacter));
                temp.GetChild(0).GetComponent<Text>().text = tempList[i].equipmentname;
                temp.GetChild(1).GetChild(0).GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprite/" + tempList[i].equipmentname);
            }
        }
    }

    bool ClassToSubtype(EquipmentMainType maintype, Class param_class, EquipmentSubType param_subtype)
    {
        switch (maintype)
        {
            case EquipmentMainType.Head:
                if (param_class == Class.Brute || param_class == Class.Knight)
                {
                    if (param_subtype == EquipmentSubType.Helm)
                        return true;
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (param_subtype == EquipmentSubType.Hat)
                        return true;
                    else
                        return false;
                }
            case EquipmentMainType.Weapon:
                if (param_class == Class.Brute)
                {
                    if (param_subtype == EquipmentSubType.Axe)
                        return true;
                    else
                        return false;
                }
                else if (param_class == Class.Knight)
                {
                    if (param_subtype == EquipmentSubType.Sword)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (param_class == Class.Rogue)
                {
                    if (param_subtype == EquipmentSubType.Dagger)
                        return true;
                    else
                        return false;
                }
                else if (param_class == Class.Archer)
                {
                    if (param_subtype == EquipmentSubType.Bow)
                        return true;
                    else
                        return false;
                }
                else if (param_class == Class.Wizard)
                {
                    if (param_subtype == EquipmentSubType.Staff)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (param_subtype == EquipmentSubType.Staff)
                        return true;
                    else
                        return false;
                }
            case EquipmentMainType.Armour:
                if (param_class == Class.Brute || param_class == Class.Knight)
                {
                    if (param_subtype == EquipmentSubType.Plate)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (param_subtype == EquipmentSubType.Cloth)
                        return true;
                    else
                        return false;
                }
            case EquipmentMainType.Accessorie:
                if (param_subtype == EquipmentSubType.Neckless || param_subtype == EquipmentSubType.Ring)
                    return true;
                else
                    return false;
            default:
                return false;
        }
    } // 클래스에 따른 장비 선택 분류 메소드

    void ClickEquipmentOnInventory(int param_maintype, int param_idx, AllyCharacter param_character) // 인벤토리 창에서 장비를 선택했을 때 콜백
    {
        templist = FileManager.ListDataLoad<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt"); // 인벤토리 아이템 리스트 로드
        for (int i = 0; i < templist.Count; i++)
        {
            if (templist[i].isMounted && templist[i].whoMounted == param_character.Access_charactername && templist[i].maintype == (EquipmentMainType)param_maintype)
            {
                templist[i].isMounted = false;
            }
        }
        templist[param_idx].isMounted = true;
        templist[param_idx].whoMounted = param_character.Access_charactername;

        param_character.Access_equipment(param_maintype, templist[param_idx]);
        FileManager.ListDataGenerate<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt", templist);

        templist2 = FileManager.ListDataLoad<EquipmentData>("SaveFile/UserData/" + param_character.Access_prefabname + "EquipList.txt"); // 캐릭터 아이템 리스트 로드
        templist2[param_maintype] = templist[param_idx];
        FileManager.ListDataGenerate<EquipmentData>("SaveFile/UserData/" + param_character.Access_prefabname + "EquipList.txt", templist2);

        for (int i = 0; i < InventoryWindow_itemlist.childCount; i++)
        {
            InventoryWindow_itemlist.GetChild(i).gameObject.SetActive(false);
        }
        InventoryWindow.gameObject.SetActive(false);
        HeroClickExecute(selectIndex);
    }

    void InventoryWindowBack() // 인벤토리 창에서 뒤로가기 버튼 눌렀을 때 콜백
    {
        for (int i = 0; i < InventoryWindow_itemlist.childCount; i++)
        {
            InventoryWindow_itemlist.GetChild(i).gameObject.SetActive(false);
        }
        InventoryWindow.gameObject.SetActive(false);
    }

    IEnumerator Button_Anim(int value)
    {
        float elapsTime;
        SoundManager.Instance.PlaySound(hero_button_clip);
        fade.gameObject.SetActive(true);
        for (elapsTime = 0; elapsTime <= 1; elapsTime += 0.025f)
        {
            yield return null;
            fade.color = new Color(0, 0, 0, elapsTime);
        }
        SceneManager.LoadScene(value);
    }
}
