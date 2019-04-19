using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 맵씬에서 이동을 위한 애니메이션 기능을 담당하는 스크립트.

    [SerializeField]
    MapManager mapmanager;

    [SerializeField]
    float movespeed;

    public IEnumerator Move(Vector3 dest)
    {
        transform.GetChild(1).gameObject.SetActive(false);

        Vector3 dir = (dest - (transform.position + new Vector3(0, 0.5f, 0))).normalized;
        while (mapmanager.Access_mapstate == MapState.move)
        {
            yield return null;
            transform.Translate(dir * movespeed);
            if (Vector3.Distance(transform.position, dest) <= 0.51f)
            {
                mapmanager.Access_mapstate = MapState.idle;
                StartCoroutine(mapmanager.Button_Anim(2));
            }
        }
    }
}
