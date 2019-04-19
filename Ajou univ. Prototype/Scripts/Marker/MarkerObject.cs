using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerObject : MonoBehaviour
{
    // 마커 오브젝트의 기능을 하는 스크립트.

    [SerializeField]
    GameObject targetObject; // 마커의 타겟이 될 오브젝트
    [SerializeField]
    GameObject nearbyMarker; // 타겟 오브젝트가 가까이 있을 경우에 active되는 마커
    [SerializeField]
    GameObject distantMarker; // 타겟 오브젝트가 멀리있을 경우에 active되는 마커
    bool objectInCamera; // 타겟이 카메라에 잡혔는지 여부
    
    void OnEnable()
    {
        StartCoroutine(RenderUpdate());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator RenderUpdate()
    {
        while (true)
        {
            yield return null;
            if (targetObject)
            {
                Vector3 pos = Camera.main.WorldToViewportPoint(targetObject.transform.position);
                if (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f)
                {
                    if (objectInCamera) // 카메라 안에 타겟이 있을 때
                    {
                        objectInCamera = false;
                        ChangeShowedMarker();
                    }
                }
                else
                {
                    if (!objectInCamera) // 카메라 안에 타겟이 없을 때
                    {
                        objectInCamera = true;
                        ChangeShowedMarker();
                    }
                }
            }
        }
    }

    void ChangeShowedMarker() // 마커 표시를 변경
    {
        if (objectInCamera)
        {
            nearbyMarker.SetActive(false);
            distantMarker.SetActive(true);
        }
        else
        {
            nearbyMarker.SetActive(true);
            distantMarker.SetActive(false);
        }
    }

    public void SetTarget(GameObject target) // 타겟 오브젝트를 설정
    {
        targetObject = target;
        nearbyMarker.GetComponent<NearbyMarker>().SetTarget(target);
        distantMarker.GetComponent<DistantMarker>().SetTarget(target);
    }
}
