using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyMarker : MonoBehaviour
{
    // 시야 안의 오브젝트의 마커 표시 기능을 하는 스크립트.

    GameObject markerPos;

    void OnEnable()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.localPosition = Camera.main.WorldToScreenPoint(markerPos.transform.position + new Vector3(0, 0.45f, 0.3f));
        StartCoroutine(ShowNearbyMarker());
    }

    void OnDisable()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        StopAllCoroutines();
    }

    IEnumerator ShowNearbyMarker()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (markerPos)
                transform.position = Camera.main.WorldToScreenPoint(markerPos.transform.position + new Vector3(0, 0.45f, 0.3f));
            else
                StopAllCoroutines();
        }
    }

    public void SetTarget(GameObject obj)
    {
        markerPos = obj;
    }
}
