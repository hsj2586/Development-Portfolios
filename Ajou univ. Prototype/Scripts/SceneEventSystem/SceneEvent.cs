using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class SceneEvent : MonoBehaviour, IPointerDownHandler
{
    // SceneEventSystem의 기본 단위가 되는 이벤트의 최소 단위를 정의하는 부모 클래스
    protected SceneEventSystem sceneEventSystem;
    public abstract IEnumerator Init(); // 초기화 메서드
    public abstract IEnumerator Execute(); // 이벤트 진행 여부 확인 메서드
    public abstract IEnumerator Restore(); // 초기화 상태 복원 메서드
    bool isTouched;

    public void OnPointerDown(PointerEventData eventData) // 터치 인식
    {
        List<RaycastResult> raycastList = new List<RaycastResult>();
        sceneEventSystem.DirectCanvas.GetComponent<GraphicRaycaster>().Raycast(eventData, raycastList);
        if (raycastList.Count != 0)
            isTouched = true;
    }

    void Awake()
    {
        isTouched = false;
        sceneEventSystem = GameObject.Find("IngameScene").GetComponent<SceneEventSystem>();
    }

    public IEnumerator FadeOut() // 화면 어두워짐 효과
    {
        GameObject chapterDirect = sceneEventSystem.DirectCanvas.transform.Find("ChapterDirect").gameObject;
        chapterDirect.transform.GetChild(0).gameObject.SetActive(false);
        chapterDirect.transform.GetChild(1).gameObject.SetActive(false);
        chapterDirect.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        chapterDirect.SetActive(true);
        Image fadeImage = chapterDirect.GetComponent<Image>();
        float elapsTime = 0;

        while (fadeImage.color.a <= 0.99f)
        {
            yield return null;
            elapsTime += 0.01f;
            fadeImage.color = new Color(0, 0, 0, elapsTime);
        }
    }

    public IEnumerator FadeIn() // 화면 밝아짐 효과
    {
        GameObject chapterDirect = sceneEventSystem.DirectCanvas.transform.Find("ChapterDirect").gameObject;
        chapterDirect.transform.GetChild(0).gameObject.SetActive(false);
        chapterDirect.transform.GetChild(1).gameObject.SetActive(false);
        chapterDirect.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        chapterDirect.SetActive(true);
        Image fadeImage = chapterDirect.GetComponent<Image>();
        float elapsTime = 1;

        while (fadeImage.color.a >= 0.01f)
        {
            yield return null;
            elapsTime -= 0.01f;
            fadeImage.color = new Color(0, 0, 0, elapsTime);
        }
    }

    public IEnumerator DirectMode() // 연출 모드 전환
    {
        GameObject directCanvas = sceneEventSystem.DirectCanvas;
        RectTransform image = directCanvas.transform.Find("UpperImage").GetComponent<RectTransform>();
        RectTransform image2 = directCanvas.transform.Find("LowerImage").GetComponent<RectTransform>();
        Image image_ = image.GetComponent<Image>();
        Image image2_ = image2.GetComponent<Image>();

        sceneEventSystem.TouchCanvas.SetActive(false);

        for (int i = 0; i < 40; i++)
        {
            yield return null;
            image.anchoredPosition = new Vector2(0, 300 - i * 2);
            image2.anchoredPosition = new Vector2(0, i * 4 - 350);
            image_.color = new Color(0, 0, 0, 0.025f * i);
            image2_.color = new Color(0, 0, 0, 0.025f * i);
        }
        image.anchoredPosition = new Vector2(0, 220);
        image2.anchoredPosition = new Vector2(0, -190);
        image_.color = new Color(0, 0, 0, 1);
        image2_.color = new Color(0, 0, 0, 1);
    }

    public IEnumerator PlayMode() // 플레이 모드 전환
    {
        GameObject directCanvas = sceneEventSystem.DirectCanvas;
        RectTransform image = directCanvas.transform.GetChild(0).GetComponent<RectTransform>();
        RectTransform image2 = directCanvas.transform.GetChild(1).GetComponent<RectTransform>();
        Image image_ = image.GetComponent<Image>();
        Image image2_ = image2.GetComponent<Image>();
        for (int i = 0; i < 80; i++)
        {
            yield return null;
            image.anchoredPosition = new Vector2(0, 220 + i * 2);
            image2.anchoredPosition = new Vector2(0, -190 - i * 4);
            image_.color = new Color(0, 0, 0, 1 - 0.025f * i);
            image2_.color = new Color(0, 0, 0, 1 - 0.025f * i);
        }
        image.anchoredPosition = new Vector2(0, 300);
        image2.anchoredPosition = new Vector2(0, -350);
        image_.color = new Color(0, 0, 0, 0);
        image2_.color = new Color(0, 0, 0, 0);

        sceneEventSystem.TouchCanvas.SetActive(true);
    }

    public IEnumerator SceneDialog(string _fileName) // 대화 이벤트 발생
    {
        yield return new WaitForSeconds(0.5f);

        List<string> temp;
        GameObject dialog = sceneEventSystem.Dialog;
        temp = ResourceManager.Instance.GetDialog(_fileName);
        dialog.transform.GetChild(1).GetComponent<Text>().text = null;
        dialog.SetActive(true);

        for (int i = 0; i < temp.Count; i += 2)
        {
            int j = 0;
            string tempString;
            dialog.transform.GetChild(0).GetComponent<Text>().text = temp[i];
            for (; j < temp[i + 1].Length + 1; j++)
            {
                yield return null;
                if (Input.GetKey(KeyCode.Space) || isTouched)
                {
                    dialog.transform.GetChild(1).GetComponent<Text>().text = temp[i + 1];
                    isTouched = false;
                    break;
                }
                tempString = temp[i + 1].Substring(0, j);
                dialog.transform.GetChild(1).GetComponent<Text>().text = tempString;
            }
            while (true)
            {
                yield return null;
                if (Input.GetKey(KeyCode.Space) || isTouched)
                {
                    isTouched = false;
                    break;
                }
            }
        }
        dialog.SetActive(false);
    }

    public IEnumerator TurnOnMarker(GameObject target) // 마커 표시하기
    {
        yield return null;
        GameObject camMarkerPos = Camera.main.transform.GetChild(0).gameObject;
        camMarkerPos.GetComponent<MarkerObject>().SetTarget(target);
        camMarkerPos.SetActive(true);
        sceneEventSystem.MarkerCanvas.SetActive(true);
    }

    public IEnumerator TurnOffMarker() // 마커 지우기
    {
        yield return null;
        Camera.main.transform.GetChild(0).gameObject.SetActive(false);
        sceneEventSystem.MarkerCanvas.SetActive(false);
    }

    public IEnumerator ChapterIntroduce(string chapter, string chapterName) // 챕터 소개 연출
    {
        sceneEventSystem.TouchCanvas.SetActive(false);

        GameObject chapterDirect = sceneEventSystem.DirectCanvas.transform.Find("ChapterDirect").gameObject;
        Text text1 = chapterDirect.transform.GetChild(0).GetComponent<Text>();
        Text text2 = chapterDirect.transform.GetChild(1).GetComponent<Text>();
        text1.text = chapter;
        text2.text = chapterName;
        chapterDirect.SetActive(true);

        for (int i = 0; i < 100; i++)
        {
            yield return null;
            text1.color = new Color(1, 1, 1, 0.01f * i);
            text2.color = new Color(1, 1, 1, 0.01f * i);
        }

        for (int i = 0; i < 100; i++)
        {
            yield return null;
            text1.color = new Color(1, 1, 1, 1 - 0.01f * i);
            text2.color = new Color(1, 1, 1, 1 - 0.01f * i);
        }

        text1.color = new Color(1, 1, 1, 0);
        text2.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(1);

        chapterDirect.SetActive(false);
    }

    public IEnumerator SplitSound(string clip, float durationTime, float timeInterval, int count) // 다중 사운드 발생
    {
        for (int i = 0; i < count; i++)
        {
            AudioManager.Instance.PlaySoundLoop(clip, durationTime);
            yield return new WaitForSeconds(timeInterval);
        }
    }
}
