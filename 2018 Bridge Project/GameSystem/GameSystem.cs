using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class GameSystem : MonoBehaviour
{
    // 게임에서 기능적으로 시스템역할을 하는 스크립트.
    [SerializeField]
    MoveSlider moveSlider;
    [SerializeField]
    GameObject UICanvas;
    [SerializeField]
    GameObject DirectCanvas;
    PlayerInput player;
    [SerializeField]
    GameObject buttons;
    [SerializeField]
    GameObject fade;
    [SerializeField]
    GameObject npcWindow;
    [SerializeField]
    GameObject attackButton;
    [SerializeField]
    GameObject InteractionButton;
    [SerializeField]
    Image SaveMessage;
    public GameObject optionUIPanel;

    void Awake()
    {
        Application.targetFrameRate = 60;
        player = GameObject.Find("PlayerTest").GetComponent<PlayerInput>();
        Physics.gravity = new Vector3(0, -50, 0);
        GameStart();
    }

    public void EnableToCallNpc()
    {
        attackButton.SetActive(false);
        InteractionButton.SetActive(true);
    }

    public void DisableToCallNpc()
    {
        attackButton.SetActive(true);
        InteractionButton.SetActive(false);
    }

    public void CallNpc() // NPC를 Call하고 창을 띄우는 메소드
    {
        Time.timeScale = 0;
        UICanvas.SetActive(false);
        DirectCanvas.SetActive(true);
        npcWindow.SetActive(true);
    }

    public void ShutNpcWindow() // NPC 창을 끄는 메소드
    {
        Time.timeScale = 1;
        UICanvas.SetActive(true);
        DirectCanvas.SetActive(false);
        npcWindow.SetActive(false);
    }

    public void GameStart() // 게임 시작
    {
        StartCoroutine(GameStart_());
    }

    public void GameOver() // 게임 오버
    {
        StartCoroutine(GameOver_());
    }

    IEnumerator GameStart_() // 게임 시작 초기화
    {
        yield return StartCoroutine(DirectCanvas.GetComponent<SubCanvas>().GameStart());
        UICanvas.SetActive(true);
        DirectCanvas.SetActive(false);
        player.enabled = true;
        moveSlider.enabled = true;
    }

    IEnumerator GameOver_() // 게임 오버 초기화
    {
        UICanvas.SetActive(false);
        DirectCanvas.SetActive(true);
        player.enabled = false;
        moveSlider.enabled = false;
        yield return StartCoroutine(DirectCanvas.GetComponent<SubCanvas>().GameOver());

        yield return new WaitForSeconds(2);
        buttons.SetActive(true);
    }

    public void Retry() // 재도전
    {
        StartCoroutine(Retry_());
    }

    IEnumerator Retry_()
    {
        fade.SetActive(true);
        fade.GetComponent<Image>().DOFade(1, 1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }

    public void Quit() // 어플리케이션 종료
    {
        Application.Quit();
    }

    public void PushSaveMessage() // 저장 메세지 띄움
    {
        if (!SaveMessage.gameObject.activeInHierarchy)
        {
            SaveMessage.gameObject.SetActive(true);
            StartCoroutine(PushSaveMessage_());
        }
    }

    IEnumerator PushSaveMessage_()
    {
        float alpha = 0;
        WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(0.025f);
        Text text = SaveMessage.transform.GetChild(0).GetComponent<Text>();

        while (alpha <= 0.7843f)
        {
            yield return waitTime;
            SaveMessage.color = new Color(1, 1, 1, alpha);
            text.color = new Color(1, 1, 1, alpha);
            alpha += 0.01f;
        }
        while (text.color.a != 0)
        {
            yield return waitTime;
            SaveMessage.color = new Color(1, 1, 1, alpha);
            text.color = new Color(1, 1, 1, alpha);
            alpha = Mathf.Clamp(alpha - 0.005f, 0, 1);
        }
        yield return new WaitForSecondsRealtime(1.5f);
        SaveMessage.gameObject.SetActive(false);
    }

    #region 설정 창 관련
    public void OpenOptionWindow()
    {
        StartCoroutine(OpenOptionWindow_());
    }

    IEnumerator OpenOptionWindow_()
    {
        RectTransform optionUIPanelChild = optionUIPanel.transform.GetChild(0).GetComponent<RectTransform>();
        optionUIPanel.SetActive(true);
        optionUIPanelChild.DOSizeDelta(new Vector2(850, 700), 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < optionUIPanelChild.childCount; i++)
        {
            optionUIPanelChild.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void CloseOptionWindow()
    {
        StartCoroutine(CloseOptionWindow_());
    }

    IEnumerator CloseOptionWindow_()
    {
        RectTransform optionUIPanelChild = optionUIPanel.transform.GetChild(0).GetComponent<RectTransform>();
        for (int i = 0; i < optionUIPanelChild.childCount; i++)
        {
            optionUIPanelChild.GetChild(i).gameObject.SetActive(false);
        }
        optionUIPanelChild.DOSizeDelta(new Vector2(50, 50), 0.3f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.3f);
        optionUIPanel.SetActive(false);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
    #endregion

    public void MoveStage() // 스테이지 이동 연출
    {
        StartCoroutine(MoveStage_());
    }

    IEnumerator MoveStage_()
    {
        UICanvas.SetActive(false);
        player.GetComponent<PlayerInput>().StopUpdate();
        DirectCanvas.SetActive(true);
        Transform fadeImage = DirectCanvas.transform.GetChild(3);
        fadeImage.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        fadeImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1920, 0);
        fadeImage.gameObject.SetActive(true);
        fadeImage.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 1).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(1.5f);
        fadeImage.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1920, 0), 1).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(1);
        player.GetComponent<PlayerInput>().MoveSlider.SetSliderValue(0.5f);
        fadeImage.gameObject.SetActive(false);
        DirectCanvas.SetActive(false);
        player.GetComponent<PlayerInput>().StartUpdate();
        UICanvas.SetActive(true);
    }
}
