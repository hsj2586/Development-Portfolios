using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class LobbyManager : MonoBehaviour
{
    // 로비창에서의 모든 기능을 담당하는 스크립트.

    [SerializeField]
    GameObject buttons;
    [SerializeField]
    GameObject panel;
    [SerializeField]
    Image fade;
    [SerializeField]
    AudioClip lobby_main_clip;
    [SerializeField]
    AudioClip lobby_button_clip;
    Account account;

    void Awake()
    {
        ButtonSetting();
    }

    void Start()
    {
        SoundManager.Instance.PlayMainSound(lobby_main_clip);
        account = FileManager.AccountDataLoad("SaveFile/AccountData.txt"); // 계정 정보를 load해 적용.

        panel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = account.Access_name;
        panel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = account.Access_gold.ToString();
        panel.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "LV " + account.Access_level.ToString();
    }

    void ButtonSetting()
    {
        for (int i = 0; i < buttons.transform.childCount; i++)
        {
            int num = i;
            buttons.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => ExecuteCommand(num));
        }
    }

    void ExecuteCommand(int value)
    {
        switch (value)
        {
            case 0:
                {
                    StartCoroutine(Button_Anim(5)); // 전투준비 맵으로 씬이동
                    break;
                }
            case 1:
                StartCoroutine(Button_Anim(0)); // 메인으로 씬이동
                break;
            case 2:
                StartCoroutine(Button_Anim(3)); // 상점으로 씬이동
                break;
            case 3:
                StartCoroutine(Button_Anim(4)); // 영웅으로 씬이동
                break;
            default:
                break;
        }
    }

    IEnumerator Button_Anim(int value)
    {
        float elapstime;
        SoundManager.Instance.PlaySound(lobby_button_clip);
        fade.gameObject.SetActive(true);
        for (elapstime = 0; elapstime <= 1; elapstime += 0.025f)
        {
            yield return null;
            fade.color = new Color(0, 0, 0, elapstime);
        }
        SceneManager.LoadScene(value);
    }
}
