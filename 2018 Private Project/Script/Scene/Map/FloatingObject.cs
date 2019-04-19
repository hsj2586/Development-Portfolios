using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    // 맵 씬에서의 마커 오브젝트 애니메이션을 위한 스크립트.

    float elapstime;
    [SerializeField]
    float floatingSpeed;
    float value;

    void Start()
    {
        elapstime = 0;
        StartCoroutine((Floating()));
    }

    IEnumerator Floating()
    {
        while (true)
        {
            yield return null;
            elapstime += Time.deltaTime;
            value = 0.2f * Mathf.Sin(elapstime * floatingSpeed) + 2.5f;
            transform.localPosition = new Vector3(0, value, 0);
        }
    }
}
