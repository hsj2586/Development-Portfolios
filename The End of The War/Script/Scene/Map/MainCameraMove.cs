using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMove : MonoBehaviour
{
    // 맵씬에서 카메라 이동을 위한 스크립트.

    [SerializeField]
    Transform player;
    Vector3 distance;

    void Start()
    {
        distance = new Vector3(-0.06f, 7.3f, -2.7f);
        StartCoroutine(Follow());
    }

    IEnumerator Follow()
    {
        while (true)
        {
            yield return null;
            transform.position = player.position + distance;
        }
    }
}
