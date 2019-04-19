using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MapState { idle, move }

public class MapManager : MonoBehaviour
{
    // 맵에서의 기능을 총괄하는 스크립트.

    [SerializeField]
    Transform panel;
    [SerializeField]
    AudioClip main_clip;
    [SerializeField]
    AudioClip button_clip;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject maincamera;
    [SerializeField]
    Transform buttons;
    [SerializeField]
    Transform message_buttons;
    [SerializeField]
    Image fade;
    [SerializeField]
    Transform messageWindow;
    [SerializeField]
    Transform waypoint;
    Account account;

    MapState state;
    Ray ray;
    RaycastHit hit;
    Transform temp_hitobject;

    string current_stage;
    Stage temp_stage;

    public MapState Access_mapstate
    {
        get { return state; }
        set { state = value; }
    }

    void Awake()
    {
        state = MapState.idle;
        SoundManager.Instance.PlayMainSound(main_clip);
        account = FileManager.AccountDataLoad("SaveFile/AccountData.txt");
        panel.GetChild(0).GetChild(0).GetComponent<Text>().text = account.Access_name;
        panel.GetChild(1).GetChild(0).GetComponent<Text>().text = account.Access_gold.ToString();
        panel.GetChild(2).GetChild(0).GetComponent<Text>().text = "LV " + account.Access_level.ToString();

        ButtonSetting();
        current_stage = "Stage" + account.Access_currentstage.ToString();
        player.transform.position = GameObject.Find(current_stage).transform.position - new Vector3(0, 0.5f, 0);
        for (int i = 0; i < account.Access_currentstage; i++) // 이미 완료한 스테이지 비활성화
        {
            waypoint.GetChild(i).tag = "Untagged";
            waypoint.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }

        StartCoroutine(Idle());
    }

    void ButtonSetting()
    {
        for (int i = 0; i < buttons.transform.childCount; i++)
        {
            int temp = i;
            buttons.GetChild(i).GetComponent<Button>().onClick.AddListener(() => ExecuteCommand(temp, 0));
        }

        for (int i = 0; i < message_buttons.transform.childCount; i++)
        {
            int temp = i;
            message_buttons.GetChild(i).GetComponent<Button>().onClick.AddListener(() => ExecuteCommand(temp, 1));
        }
    }

    public IEnumerator Idle()
    {
        while (state == MapState.idle)
        {
            yield return new WaitForFixedUpdate();
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Enemy") && hit.transform.name != current_stage)
                {
                    temp_hitobject = hit.transform;
                    messageWindow.gameObject.SetActive(true);
                    temp_stage = FileManager.StageDataLoad("SaveFile/StageData/" + temp_hitobject.name + ".txt");
                    if (temp_stage.Access_grade == StageGrade.Boss)
                    {
                        messageWindow.GetChild(2).GetComponent<Text>().text = "<color=red> 보스 출현! </color>\n\n전투를 하시겠습니까?";
                    }
                    else
                        messageWindow.GetChild(2).GetComponent<Text>().text = "<color=yellow>" + hit.transform.name + "</color>\n\n전투를 하시겠습니까?";
                }
            }
        }
    }

    void ExecuteCommand(int value, int type) // type 0 = 일반 버튼, type 1 = 메세지창 버튼
    {
        switch (type)
        {
            case 0:
                StartCoroutine(Button_Anim(1)); // 로비로 씬이동
                break;
            case 1:
                ExecuteMessage(value);
                break;
            default:
                break;
        }
    }

    void ExecuteMessage(int value)
    {
        switch (value)
        {
            case 0: // 전투로 씬이동
                SoundManager.Instance.PlaySound(button_clip);
                state = MapState.move;
                messageWindow.gameObject.SetActive(false);
                StartCoroutine(player.GetComponent<PlayerMove>().Move(temp_hitobject.position));
                break;
            case 1: // 취소 버튼
                messageWindow.gameObject.SetActive(false);
                break;
        }
    }

    public IEnumerator Button_Anim(int value)
    {
        float elapsTime;
        if (value != 2)
            SoundManager.Instance.PlaySound(button_clip);

        for (elapsTime = 0; elapsTime <= 1; elapsTime += 0.025f)
        {
            yield return null;
            fade.gameObject.SetActive(true);
            fade.color = new Color(0, 0, 0, elapsTime);
        }
        SceneManager.LoadScene(value);
    }
}
