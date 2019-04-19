using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    // 전투 종료후 생성되는 텍스트의 애니메이션 기능을 담당하는 스크립트.

    [SerializeField]
    float movespeeed;
    float elapstime;
    void OnEnable()
    {
        elapstime = 0;
        StartCoroutine(update());
    }

    IEnumerator update()
    {
        while (true)
        {
            yield return null;
            elapstime += Time.deltaTime * movespeeed;
            GetComponent<RectTransform>().anchoredPosition3D = new Vector3(190, 55 + 2 * Mathf.Sin(elapstime), 0);
        }
    }
}
