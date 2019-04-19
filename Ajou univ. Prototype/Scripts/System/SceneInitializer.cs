using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneInitializer : MonoBehaviour
{
    // 모든 씬의 초기화를 담당하는 스크립트.
    private static SceneInitializer sceneInitializer = null;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (sceneInitializer == null)
        {
            sceneInitializer = this;
        }
        else if (sceneInitializer != this)
            Destroy(gameObject);
    }

    public void SceneChanged()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    public void SceneDestroyed()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0:
                TitleSceneInit();
                break;
            case 1:
                StartCoroutine(LoadingSceneInit());
                break;
            case 2:
                IngameSceneInit();
                break;
        }
    }

    IEnumerator GoToScene(int num)
    {
        // num을 매개변수로 받아 해당 씬으로 이동하는 코루틴
        yield return null;
        SceneManager.LoadScene(num);
    }

    void TitleSceneInit()
    {
        // TitleScene 초기화

        // 변수 초기화
        GameObject canvas = GameObject.Find("Canvas");
        Transform buttons = canvas.transform.Find("Buttons");
        Transform optionWindow = canvas.transform.GetChild(3);
        Button systemInfo = canvas.transform.GetChild(3).GetChild(3).GetComponent<Button>();

        // 컴포넌트 초기화
        if (!GetComponent<OptionInitializer>())
            gameObject.AddComponent<OptionInitializer>();
        GetComponent<OptionInitializer>().Init(canvas.transform.GetChild(3));

        // 리소스 매니저 초기화
        if (!GetComponent<ResourceManager>())
            gameObject.AddComponent<ResourceManager>();

        // 화면 초기화
        Screen.orientation = ScreenOrientation.LandscapeRight;

        // 버튼 초기화
        buttons.GetChild(0).GetComponent<Button>().onClick.AddListener(() => StartCoroutine(GoToScene(1)));
        buttons.GetChild(1).GetComponent<Button>().onClick.AddListener(() => StartCoroutine(GoToScene(1)));
        buttons.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { optionWindow.gameObject.SetActive(true); });
        buttons.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { Application.Quit(); });
        systemInfo.onClick.AddListener(() => { print("게임 정보 출력"); });

        // 오디오 재생
        AudioManager.Instance.PlayMainSound("BGM001");
    }

    IEnumerator LoadingSceneInit()
    {
        // LoadingScene 초기화
        // 컴포넌트 초기화
        if (!GetComponent<ResourceManager>())
            gameObject.AddComponent<ResourceManager>();

        yield return new WaitForSeconds(2);
        StartCoroutine(GoToScene(2));
    }

    void IngameSceneInit()
    {
        // IngameScene 초기화

        // 변수 초기화
        Transform touchCanvas = GameObject.Find("TouchCanvas").transform;
        GameObject systemWindow = touchCanvas.Find("SystemWindow").gameObject;
        GameObject InventoryWindow = touchCanvas.GetChild(6).gameObject;

        // 컴포넌트 초기화
        if (!GetComponent<OptionInitializer>())
            gameObject.AddComponent<OptionInitializer>();
        GetComponent<OptionInitializer>().Init(touchCanvas.transform.GetChild(5));
        if (!GetComponent<ResourceManager>())
            gameObject.AddComponent<ResourceManager>();

        // 화면 초기화
        Screen.orientation = ScreenOrientation.LandscapeRight;

        // 버튼 초기화
        touchCanvas.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { systemWindow.SetActive(true); });
        touchCanvas.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { InventoryWindow.SetActive(true); });
        touchCanvas.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { Application.Quit(); });
        systemWindow.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { print("게임 정보 출력"); });

        // 오디오 재생
        AudioManager.Instance.PlayMainSound("BGM001");
    }
}
