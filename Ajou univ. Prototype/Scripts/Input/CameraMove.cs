using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // 카메라 이동을 위한 스크립트.
    [SerializeField]
    GameObject target;
    Vector3 targetPos;
    [SerializeField]
    float sensitivity;
    Vector3 temp;
    IEnumerator routine;

    public float angle;

    public void StopCamMove()
    {
        StopCoroutine(routine);
    }

    public void StartCamMove()
    {
        routine = update();
        StartCoroutine(routine);
    }

    void Start()
    {
        routine = update();
        StartCoroutine(routine);
    }

    public void SetTarget(Vector3 targetPos)
    {
        target = null;
        this.targetPos = targetPos;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    IEnumerator update()
    {
        while (true)
        {
            yield return null;
            if (target)
            {
                temp = target.transform.position + new Vector3(0, 1.7f * angle, -angle);
            }
            else
            {
                temp = targetPos + new Vector3(0, 1.7f * angle, -angle);
            }
            transform.position = Vector3.Lerp(transform.position, temp, Time.deltaTime * sensitivity);
        }
    }
}
