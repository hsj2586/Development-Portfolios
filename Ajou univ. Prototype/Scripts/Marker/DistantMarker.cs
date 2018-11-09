using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistantMarker : MonoBehaviour
{
    // 시야 밖의 오브젝트의 마커 표시 기능을 하는 스크립트.

    GameObject markerPos;
    GameObject player;
    Vector3 temp;
    GameObject markerImage;

    void Awake()
    {
        markerImage = transform.GetChild(0).gameObject;
    }

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        temp = Camera.main.WorldToScreenPoint(player.transform.position);
        StartCoroutine(MoveDistantMarker());
    }

    void OnDisable()
    {
        markerImage.SetActive(false);
        StopAllCoroutines();
    }

    IEnumerator MoveDistantMarker()
    {
        float rad;
        RaycastHit hit;

        while (true)
        {
            if (markerPos)
            {
                yield return new WaitForEndOfFrame();
                temp = Camera.main.WorldToScreenPoint(player.transform.position);
                Vector3 markerToScreen = Camera.main.WorldToScreenPoint(markerPos.transform.position);
                Vector2 dir = new Vector2(markerToScreen.x - temp.x, markerToScreen.y - temp.y).normalized;
                Vector2 forward = new Vector2(1, 0);

                if(!markerImage.activeSelf)
                    markerImage.SetActive(true);

                if (Physics.Raycast(temp, dir, out hit, 10000)) // 레이캐스팅
                {
                    transform.position = hit.point;
                }

                if (dir.y > 0) // 마커 회전
                    rad = Mathf.Acos(dir.x * forward.x + dir.y * forward.y);
                else
                {
                    rad = -Mathf.Acos(dir.x * forward.x + dir.y * forward.y);
                }

                float degree = rad * 180 / Mathf.PI;
                transform.eulerAngles = new Vector3(0, 0, degree);
            }
            else
                StopAllCoroutines();
        }
    }

    public void SetTarget(GameObject obj)
    {
        markerPos = obj;
    }
}
