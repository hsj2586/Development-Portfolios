using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRayCast : MonoBehaviour
{
    // 적 기준에서 Raycast를 던지고 현재 자신이 위치한 섹션을 알아내는 기능.
    // 추가로 동적으로 섹션에 오브젝트 리스트에 자신을 추가 및 제거하는 기능.
    RaycastHit hit;
    GameObject temp;

    public Transform GetSection
    {
        get { return hit.transform.parent; }
    }

    void Awake()
    {
        StartCoroutine(update());
        temp = null;
    }

    IEnumerator update()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1, 1 << 9))
            {
                if (temp != hit.transform.parent.gameObject)
                {
                    if (temp != null)
                        temp.GetComponent<Section>().RemoveObjectInSection(gameObject);
                    temp = hit.transform.parent.gameObject;
                    temp.GetComponent<Section>().AddObjectInSection(gameObject);
                }
            }
        }
    }
}
