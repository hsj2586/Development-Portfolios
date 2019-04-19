using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    // 메인 창에서의 모든 기능을 담당하는 스크립트.

    [SerializeField]
    Transform buttons;
    [SerializeField]
    GameObject main_text;
    [SerializeField]
    AudioClip main_clip;
    [SerializeField]
    AudioClip main_button_clip;
    [SerializeField]
    Image fade;

    void Start()
    {
        SoundManager.Instance.PlayMainSound(main_clip);
        ButtonSetting();
    }

    void ButtonSetting()
    {
        for (int i = 0; i < buttons.childCount; i++)
        {
            int num = i;
            buttons.GetChild(num).GetComponent<Button>().onClick.AddListener(() => ExecuteCommand(num));
        }
    }

    void ExecuteCommand(int value)
    {
        switch (value)
        {
            case 0:
                StartCoroutine(Button_Anim(1));
                break;
            case 1:
                Application.Quit();
                break;
            default:
                break;
        }
    }

    IEnumerator Button_Anim(int value)
    {
        SoundManager.Instance.PlaySound(main_button_clip);
        fade.gameObject.SetActive(true);
        float elapsTime;
        for (elapsTime = 0; elapsTime <= 1; elapsTime += 0.025f)
        {
            yield return null;
            fade.color = new Color(0, 0, 0, elapsTime);
        }
        SceneManager.LoadScene(value);
    }
}
