using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingSceneToIngame : MonoBehaviour
{
    public GameObject model;
    public GameObject fadeImage;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        model.transform.GetChild(0).GetComponent<Animator>().SetInteger("State", 1);
        fadeImage.GetComponent<Image>().DOFade(0, 1);
        StartCoroutine(GoToLobby());
    }

    IEnumerator GoToLobby()
    {
        yield return new WaitForSeconds(3);
        fadeImage.GetComponent<Image>().DOFade(1, 1);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(4);
    }
}
