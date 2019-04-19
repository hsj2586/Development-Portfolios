using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainScene : MonoBehaviour
{
    [SerializeField]
    Image fadeImage;
    private Touch tempTouchs;
    private Vector3 touchedPos;
    bool is_touched;

    void Awake()
    {
        Application.targetFrameRate = 60;
        AudioManager.Instance.PlayMainSound("SoundTrack1");
        is_touched = false;
    }

    void Update()
    {
        if(Input.anyKeyDown)
        {
            is_touched = true;
            StartCoroutine(StartButtonClick());
        }

        if (Input.touchCount > 0 && !is_touched)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                tempTouchs = Input.GetTouch(i);
                if (tempTouchs.phase == TouchPhase.Began)
                {
                    is_touched = true;
                    StartCoroutine(StartButtonClick());
                    break;
                }
            }
        }
    }

    IEnumerator StartButtonClick()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, 1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }
}
