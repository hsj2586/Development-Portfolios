using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    // 전투 시에 BattleManager와 연동해 UI기능을 총괄하는 스크립트.

    Queue<GameObject> allyQueue = new Queue<GameObject>();
    Queue<GameObject> enemyQueue = new Queue<GameObject>();
    GameObject[] allies;
    GameObject[] enemies;
    [SerializeField]
    Transform canvas;
    [SerializeField]
    Transform canvas2;
    [SerializeField]
    Transform floatingTextPool; // floating text(데미지 표시 텍스트)를 담고 있는 풀리스트
    private List<GameObject> floatingLabel_list;
    [SerializeField]
    Button button;
    [SerializeField]
    Image fade;
    [SerializeField]
    AudioClip battle_button_clip;
    Account account;

    public GameObject Sliderpool_Pop(GameObject obj) // 슬라이더 오브젝트 할당
    {
        if (obj.tag == "Ally")
            return allyQueue.Dequeue();
        else
            return enemyQueue.Dequeue();
    }

    public void Labelpool_Push(GameObject obj) // 데미지 레이블 pool 할당
    {
        obj.GetComponent<FloatingLabelAnimation>().parent_obj = floatingTextPool;
        obj.SetActive(false);
        floatingLabel_list.Add(obj);
    }

    public GameObject Labelpool_Pop(Transform param, float damage)
    {
        GameObject temp = floatingLabel_list[0];
        temp.GetComponent<FloatingLabelAnimation>().parent_obj = param;
        temp.GetComponent<FloatingLabelAnimation>().damage = damage;
        temp.SetActive(true);
        floatingLabel_list.RemoveAt(0);
        return temp;
    }

    public void Init()
    {
        GameObject temp;
        allies = GameObject.FindGameObjectsWithTag("Ally");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        account = FileManager.AccountDataLoad("SaveFile/AccountData.txt");

        int i;

        floatingLabel_list = new List<GameObject>();
        button.onClick.AddListener(() => StartCoroutine(SceneConvert(5)));
        button.GetComponent<Image>().raycastTarget = false;

        if (floatingLabel_list.Count == 0)
        {
            for (i = 0; i < floatingTextPool.childCount; i++)
            {
                floatingLabel_list.Add(floatingTextPool.GetChild(i).gameObject);
            }
        }

        for (i = 0; i < allies.Length; i++)
        {
            temp = canvas.GetChild(0).GetChild(i).gameObject;
            temp.SetActive(true);
            allyQueue.Enqueue(temp);
        }
        for (i = 0; i < enemies.Length; i++)
        {
            temp = canvas.GetChild(1).GetChild(i).gameObject;
            temp.SetActive(true);
            enemyQueue.Enqueue(temp);
        }
    }

    public IEnumerator EndGame_Animation(bool value, int[,] param_data, List<EquipmentData> reward_list) // 게임이 종료됐을 경우 작동하는 코루틴, 유저가 이길 경우 value = true, 졌을 경우 value = false; 
    {
        int i, j;
        account = FileManager.AccountDataLoad("SaveFile/AccountData.txt");
        List<GameObject> characterlist = account.Access_characters;
        List<int> exp_data = FileManager.ListDataLoad<int>("SaveFile/ExpData.txt"); // 경험치 차트 로드
        Transform end_ui = canvas2.GetChild(3);
        Transform message = end_ui.GetChild(0);
        Transform characters = end_ui.GetChild(1);
        Transform rewards = end_ui.GetChild(2);
        Transform rewards_list = rewards.GetChild(0);
        Transform Button = end_ui.GetChild(3);
        Transform temp_transform;
        AllyCharacter temp_component;
        float temp_calc;

        yield return StartCoroutine(EndGameEffect());

        for (i = 0; i < allies.Length; i++) // 각 캐릭터에 승리 애니메이션 작동
        {
            if (allies[i].GetComponent<AllyCharacter>().Access_State != CharacterState.Dead)
                allies[i].GetComponent<AllyCharacter>().Access_animator.SetInteger("State", 3);
        }

        if (value)
        {
            end_ui.gameObject.SetActive(true);
            for (i = 0; i < allies.Length; i++) // panel에 담길 정보 setting
            {
                temp_transform = characters.GetChild(i);
                temp_component = characterlist[i].GetComponent<AllyCharacter>();
                temp_transform.gameObject.SetActive(true);
                temp_transform.GetChild(0).GetComponent<Text>().text = "LV." + param_data[i, 0].ToString() + " " + temp_component.Access_charactername;
                temp_transform.GetChild(1).GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprite/" + temp_component.Access_faceimage.ToString());
                temp_calc = (float)param_data[i, 1] / exp_data[param_data[i, 0]];
                temp_transform.GetChild(2).GetChild(1).GetComponent<Slider>().value = temp_calc;
                temp_transform.GetChild(2).GetChild(2).GetComponent<Text>().text = (temp_calc * 100).ToString("F1") + " %";

                rewards_list.GetChild(0).GetChild(1).GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprite/coin");
                rewards_list.GetChild(0).GetChild(2).GetComponent<Text>().text = "골드";
                rewards_list.GetChild(0).gameObject.SetActive(true);
                for (j = 0; j < reward_list.Count; j++)
                {
                    temp_transform = rewards_list.GetChild(j + 1);
                    temp_transform.GetChild(1).GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprite/" + reward_list[j].equipmentname);
                    switch (reward_list[j].grade)
                    {
                        case EquipmentGrade.common:
                            temp_transform.GetChild(2).GetComponent<Text>().text = reward_list[j].equipmentname;
                            break;
                        case EquipmentGrade.rare:
                            temp_transform.GetChild(2).GetComponent<Text>().text = "<color=cyan>" + reward_list[j].equipmentname + "</color>";
                            break;
                        case EquipmentGrade.unique:
                            temp_transform.GetChild(2).GetComponent<Text>().text = "<color=magenta>" + reward_list[j].equipmentname + "</color>";
                            break;
                        case EquipmentGrade.legend:
                            temp_transform.GetChild(2).GetComponent<Text>().text = "<color=orange>" + reward_list[j].equipmentname + "</color>";
                            break;
                    }
                    temp_transform.gameObject.SetActive(true);
                }
            }
            message.GetComponentInChildren<Text>().text = "승 리 !";
            yield return new WaitForSeconds(2);

            for (i = 0; i <= 400; i += 6) // panel 이동 애니메이션
            {
                message.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, i, 0);
                yield return null;
            }
            for (i = 0; i <= 300; i += 4)
            {
                characters.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(i * 3 - 1300, 0, 0);
                rewards.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1300 - i * 3, 0, 0);
                Button.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, i * 0.67f - 600, 0);
                yield return null;
            }

            for (i = 0; i < allies.Length; i++) // panel 이동이 완료된 이후 UI애니메이션 (캐릭터 당 1개의 코루틴 동작)
            {
                StartCoroutine(EndGame_SliderAnimation(i, param_data[i, 0], param_data[i, 1], characters.GetChild(i), exp_data));
            }
            button.GetComponent<Image>().raycastTarget = true;
        }
        else
        {
            end_ui.gameObject.SetActive(true);
            message.GetComponentInChildren<Text>().text = "패 배 !";
            for (i = 0; i <= 380; i += 4)
            {
                Button.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, i - 620, 0);
                yield return null;
            }
            button.onClick.AddListener(() => StartCoroutine(SceneConvert(0)));
            button.GetComponent<Image>().raycastTarget = true;
        }
    }

    public IEnumerator EndGameEffect() // 전투 종료 조건에 발동하는 효과를 다루는 코루틴
    {
        float elapstime;
        fade.gameObject.SetActive(true);
        Time.timeScale = 0.2f; // 일시적으로 슬로우 모션
        for (elapstime = 0; elapstime <= 0.6f;)
        {
            elapstime += 0.02f;
            yield return new WaitForSecondsRealtime(0.01f);
            fade.color = new Color(1, 1, 1, elapstime);
        }
        for (; elapstime >= 0;)
        {
            elapstime -= 0.02f;
            yield return new WaitForSecondsRealtime(0.01f);
            fade.color = new Color(1, 1, 1, elapstime);
        }
        fade.GetComponent<Image>().sprite = null;
        Time.timeScale = 1;
        fade.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
    }

    IEnumerator EndGame_SliderAnimation(int character_idx, int level, int exp, Transform characterUI, List<int> exp_data) // 배틀 종료 후 패널의 UI의 애니메이션 기능을 하는 코루틴
    {
        AllyCharacter temp_character = allies[character_idx].GetComponent<AllyCharacter>();
        Slider slider = characterUI.GetChild(2).GetChild(1).GetComponent<Slider>();
        Text percent_exp = characterUI.GetChild(2).GetChild(2).GetComponent<Text>();
        Text character_level = characterUI.GetChild(0).GetComponent<Text>();
        int temp_level = level;
        float exp_ratio = (float)temp_character.Access_exp / exp_data[temp_character.Access_level];

        while ((temp_level != temp_character.Access_level) || Mathf.Abs(exp_ratio - slider.value) > 0.01f)
        {
            yield return null;
            slider.value += 0.01f;
            percent_exp.text = (slider.value * 100).ToString("F0") + " %";
            if (slider.value == 1)
            {
                characterUI.GetChild(3).gameObject.SetActive(true); // 레벨업 마크 표시
                slider.value = 0;
                percent_exp.text = "0.0 %";
                temp_level += 1;
                character_level.text = "LV." + temp_level.ToString() + " " + temp_character.Access_charactername;
            }
        }
    }

    IEnumerator SceneConvert(int value)
    {
        float elapstime = 0;
        SoundManager.Instance.PlaySound(battle_button_clip);
        fade.gameObject.SetActive(true);
        for (elapstime = 0; elapstime <= 1; elapstime += 0.025f)
        {
            yield return null;
            fade.color = new Color(0, 0, 0, elapstime);
        }
        SceneManager.LoadScene(value);
    }
}
