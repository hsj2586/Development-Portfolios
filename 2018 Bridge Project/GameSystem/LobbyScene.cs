using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LobbyScene : MonoBehaviour
{
    bool onClick;
    bool SceneIdentifier; // false = LobbyScene, true = SelectScene
    public int selectNumber;
    public Text nameText;
    public GameObject lobbyUI;
    public GameObject stageSelectUI;
    public GameObject selectedUI;
    public GameObject readyUI;
    public GameObject optionUIPanel;
    public GameObject butlerUIPanel;
    public GameObject reinforceSkillUIPanel;
    public GameObject ListOfConstellation;
    public GameObject recoverSunUIPanel;
    public GameObject WarningMessagePanel;
    public GameObject planets;
    public Slider volume;
    public Button playButton;
    public Image fadeImage;
    const int numOfPlanets = 4;

    private Touch tempTouchs;
    private Vector3 touchedPos;
    private bool touchOn;
    RaycastHit hit;

    void Awake()
    {
        Application.targetFrameRate = 60;
        selectNumber = 1;
        onClick = false;
        SceneIdentifier = false;
        StartCoroutine(StartLobby());
    }

    private void Update()
    {
        touchOn = false;
        //if (Input.touchCount > 0)  모바일 버젼 조작
        //{
        //    for (int i = 0; i < Input.touchCount; i++)
        //    {
        //        tempTouchs = Input.GetTouch(i);
        //        if (tempTouchs.phase == TouchPhase.Began)
        //        {
        //            touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);//get world position.
        //            touchOn = true;
        //            Ray ray = Camera.main.ScreenPointToRay(touchedPos);
        //            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 14))
        //            {
        //                Debug.Log("test");
        //                break;
        //            }
        //        }
        //    }
        //}

        if (Input.GetMouseButtonDown(0)) // PC 버젼 조작
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 14))
            {
                int temp = int.Parse(hit.transform.name);
                Camera.main.GetComponent<Animator>().SetInteger("State", 2 + temp);
                ChangeUI(temp);
                stageSelectUI.SetActive(false);
                selectedUI.SetActive(true);
            }
        }
    }

    IEnumerator StartLobby()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.GetComponent<Image>().DOFade(0, 1);
        yield return new WaitForSeconds(1);
        fadeImage.gameObject.SetActive(false);
    }

    #region 스테이지 선택 버튼 관련
    public void ConvertScene()
    {
        StartCoroutine(ConvertScene_());
    }

    IEnumerator ConvertScene_()
    {
        if (!SceneIdentifier)
        {
            Camera.main.GetComponent<Animator>().SetInteger("State", 1);
            lobbyUI.SetActive(false);
            yield return new WaitForSeconds(1f);
            stageSelectUI.SetActive(true);
            SceneIdentifier = true;
        }
        else
        {
            Camera.main.GetComponent<Animator>().SetInteger("State", 2);
            stageSelectUI.SetActive(false);
            yield return new WaitForSeconds(1f);
            SceneIdentifier = false;
            lobbyUI.SetActive(true);
        }
    }
    #endregion

    #region 설정 창 관련
    public void OpenOptionWindow()
    {
        StartCoroutine(OpenOptionWindow_());
    }

    IEnumerator OpenOptionWindow_()
    {
        RectTransform optionUIPanelChild = optionUIPanel.transform.GetChild(0).GetComponent<RectTransform>();
        optionUIPanel.SetActive(true);
        optionUIPanelChild.DOSizeDelta(new Vector2(900, 750), 0.5f).SetEase(Ease.OutBack);
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

    public void NewGameButton()
    {
        WarningMessagePanel.SetActive(true);
    }

    public void StartNewGame()
    {
        LocalAccount.Instance.NewAccount();
        StartCoroutine(StartNewGame_());
    }

    IEnumerator StartNewGame_()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.GetComponent<Image>().DOFade(1, 1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }

    public void CloseWarningMessagePanel()
    {
        WarningMessagePanel.SetActive(false);
    }

    public void GoToLobbyButton()
    {
        StartCoroutine(GoToLobby());
    }

    IEnumerator GoToLobby()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.GetComponent<Image>().DOFade(1, 1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
    }

    public void VolumeValueChanged()
    {
        AudioManager.Instance.SetBgmVolume(volume.value);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
    #endregion

    #region 집사 창 관련
    public void CallButler()
    {
        StartCoroutine(CallButler_());
    }

    IEnumerator CallButler_()
    {
        RectTransform butlerUIPanelChild = butlerUIPanel.transform.GetChild(2).GetComponent<RectTransform>();
        butlerUIPanel.SetActive(true);
        butlerUIPanelChild.DOSizeDelta(new Vector2(1800, 500), 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < butlerUIPanelChild.childCount; i++)
        {
            butlerUIPanelChild.GetChild(i).gameObject.SetActive(true);
        }
        butlerUIPanel.transform.GetChild(0).gameObject.SetActive(true);
        butlerUIPanel.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void CloseButler()
    {
        StartCoroutine(CloseButler_());
    }

    IEnumerator CloseButler_()
    {
        RectTransform butlerUIPanelChild = butlerUIPanel.transform.GetChild(2).GetComponent<RectTransform>();
        for (int i = 0; i < butlerUIPanelChild.childCount; i++)
        {
            butlerUIPanelChild.GetChild(i).gameObject.SetActive(false);
        }
        butlerUIPanel.transform.GetChild(0).gameObject.SetActive(false);
        butlerUIPanel.transform.GetChild(1).gameObject.SetActive(false);
        butlerUIPanelChild.DOSizeDelta(new Vector2(50, 50), 0.3f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.3f);
        butlerUIPanel.SetActive(false);
    }

    public void OpenReinforceSkill()
    {
        StartCoroutine(OpenReinforceSkill_());
    }

    IEnumerator OpenReinforceSkill_()
    {
        RectTransform ReinforceSkillPanelChild = reinforceSkillUIPanel.transform.GetChild(0).GetComponent<RectTransform>();
        reinforceSkillUIPanel.SetActive(true);
        ReinforceSkillPanelChild.DOSizeDelta(new Vector2(1500, 900), 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < ReinforceSkillPanelChild.childCount; i++)
        {
            ReinforceSkillPanelChild.GetChild(i).gameObject.SetActive(true);
        }

        if (LocalAccount.Instance.ListOfConstellation != null)
        {
            for (int i = 0; i < LocalAccount.Instance.ListOfConstellation.Count; i++)
            {
                ConstellationDictionary tempConst = LocalAccount.Instance.ListOfConstellation[i];
                Transform tempConst_ = ListOfConstellation.transform.GetChild(i);

                tempConst_.GetChild(0).GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("SkillImage/" + tempConst.constellationName);
                tempConst_.GetChild(1).GetChild(0).GetComponent<Text>().text = tempConst.contellationContent;
                if (tempConst.isLocked)
                {
                    tempConst_.GetChild(3).GetComponent<Text>().text = string.Format("Lv." + tempConst.GetLevelOfSkill().ToString());
                    tempConst_.GetChild(4).GetComponent<Slider>().value = tempConst.GetRatioExpOfLevel();
                    tempConst_.GetChild(5).GetComponent<Text>().text = string.Format((tempConst.GetRatioExpOfLevel() * 100).ToString("N2") + "%");
                }
                else
                {
                    tempConst_.GetChild(6).gameObject.SetActive(true);
                }
                tempConst_.gameObject.SetActive(true);
            }
        }
        yield return null;
        ListOfConstellation.transform.parent.parent.GetChild(1).GetComponent<Scrollbar>().value = 0;
    }

    public void CloseReinforceSkill()
    {
        StartCoroutine(CloseReinforceSkill_());
    }

    IEnumerator CloseReinforceSkill_()
    {
        RectTransform ReinforceSkillPanelChild = reinforceSkillUIPanel.transform.GetChild(0).GetComponent<RectTransform>();
        for (int i = 0; i < ReinforceSkillPanelChild.childCount; i++)
        {
            ReinforceSkillPanelChild.GetChild(i).gameObject.SetActive(false);
        }
        ReinforceSkillPanelChild.DOSizeDelta(new Vector2(50, 50), 0.3f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.3f);
        reinforceSkillUIPanel.SetActive(false);
    }

    public void OpenRecoverSun()
    {
        StartCoroutine(OpenRecoverSun_());
    }

    IEnumerator OpenRecoverSun_()
    {
        RectTransform recoverSunUIPanelChild = recoverSunUIPanel.transform.GetChild(0).GetComponent<RectTransform>();
        recoverSunUIPanel.SetActive(true);
        for (int i = 0; i < recoverSunUIPanelChild.childCount; i++)
        {
            recoverSunUIPanelChild.GetChild(i).gameObject.SetActive(false);
        }
        recoverSunUIPanelChild.DOSizeDelta(new Vector2(1250, 800), 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.5f);
        recoverSunUIPanelChild.GetChild(3).GetComponent<Text>().text =
            string.Format("태양 소녀 레벨 : " + LocalAccount.Instance.GetLevelOfSun());
        recoverSunUIPanelChild.GetChild(4).GetComponent<Slider>().value = LocalAccount.Instance.GetRatioExpOfLevel();
        recoverSunUIPanelChild.GetChild(4).GetChild(2).GetComponent<Text>().text =
            string.Format(LocalAccount.Instance.GetRatioExpOfLevel().ToString("N2") + "%");
        for (int i = 0; i < recoverSunUIPanelChild.childCount; i++)
        {
            recoverSunUIPanelChild.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void CloseRecoverSun()
    {
        StartCoroutine(CloseRecoverSun_());
    }

    IEnumerator CloseRecoverSun_()
    {
        RectTransform recoverSunUIPanelChild = recoverSunUIPanel.transform.GetChild(0).GetComponent<RectTransform>();
        for (int i = 0; i < recoverSunUIPanelChild.childCount; i++)
        {
            recoverSunUIPanelChild.GetChild(i).gameObject.SetActive(false);
        }
        recoverSunUIPanelChild.DOSizeDelta(new Vector2(50, 50), 0.3f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.3f);
        recoverSunUIPanel.SetActive(false);
    }
    #endregion

    #region 스테이지 선택 관련
    public void BackToStageSelect()
    {
        StartCoroutine(BackToStageSelect_());
    }

    IEnumerator BackToStageSelect_()
    {
        selectedUI.SetActive(false);
        Camera.main.GetComponent<Animator>().SetInteger("State", 1);
        yield return new WaitForSeconds(2);
        stageSelectUI.SetActive(true);
    }

    public void ChangeUI(int planetIndex)
    {
        Planet selectedPlanet = planets.transform.GetChild(planetIndex - 1).GetComponent<Planet>();
        nameText.text = selectedPlanet.planetName;
        if (!selectedPlanet.isLocked)
        {
            playButton.GetComponent<Button>().enabled = true;
            playButton.transform.GetChild(0).GetComponent<Text>().text = "플레이";
        }
        else
        {
            playButton.GetComponent<Button>().enabled = false;
            playButton.transform.GetChild(0).GetComponent<Text>().text = "잠금";
        }
    }
    #endregion

    #region 게임 시작 관련
    public void ReadyGameButton()
    {
        StartCoroutine(ReadyGameButton_());
    }

    IEnumerator ReadyGameButton_()
    {
        selectedUI.SetActive(false);
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, 1f);
        Camera.main.transform.DOMove(new Vector3(0, 0.176f, -2.791f), 1f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(1f);
        readyUI.SetActive(true);
        fadeImage.DOFade(0, 1f);
        yield return new WaitForSeconds(1f);
        fadeImage.gameObject.SetActive(false);
    }

    public void BackToSelectStageButton()
    {
        StartCoroutine(BackToSelectStageButton_());
    }

    IEnumerator BackToSelectStageButton_()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, 1f);
        yield return new WaitForSeconds(1f);
        readyUI.SetActive(false);
        selectedUI.SetActive(false);
        stageSelectUI.SetActive(true);
        Camera.main.transform.position = new Vector3(0, 0.5f, -4);
        fadeImage.DOFade(0, 1f);
        yield return new WaitForSeconds(1f);
        fadeImage.gameObject.SetActive(false);
    }

    public void StartButtonClick()
    {
        StartCoroutine(StartButtonClick_());
    }

    IEnumerator StartButtonClick_()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, 1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(3);
    }
    #endregion
}
