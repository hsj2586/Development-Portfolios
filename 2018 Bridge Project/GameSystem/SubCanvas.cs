using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SubCanvas : MonoBehaviour
{
    public IEnumerator GameStart()
    {
        GameObject temp = transform.GetChild(0).gameObject;
        temp.SetActive(true);
        temp.GetComponent<Image>().DOFade(0, 2);

        yield return new WaitForSeconds(0.5f);

        GameObject temp2 = temp.transform.GetChild(0).gameObject;
        temp2.SetActive(true);
        temp2.GetComponent<Text>().DOFade(1, 2);
        yield return new WaitForSeconds(2f);
        temp2.SetActive(false);
    }

    public IEnumerator GameOver()
    {
        GameObject temp = transform.GetChild(0).gameObject;
        temp.SetActive(true);
        temp.GetComponent<Image>().DOFade(1, 2);

        yield return new WaitForSeconds(0.3f);

        GameObject temp2 = temp.transform.GetChild(1).gameObject;
        Camera.main.DOShakePosition(1, 1, 20, 90);
        temp2.SetActive(true);
        temp2.GetComponent<Text>().DOFade(1, 1);
    }
}
