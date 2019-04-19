using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolList : MonoBehaviour
{
    // 투사체의 실재 오브젝트 풀리스트 스크립트.
    List<GameObject> projectilePool;
    int i;

    void Awake()
    {
        projectilePool = new List<GameObject>();
        for (i = 0; i < transform.childCount; i++)
        {
            projectilePool.Add(transform.GetChild(i).gameObject);
        }
    }

    public void ProjectilePoolPush(GameObject obj)
    {
        obj.SetActive(false);
        projectilePool.Add(obj);
    }

    public GameObject ProjectilePoolPop(Vector3 self_pos)
    {
        if (projectilePool.Count == 0)
            return null;

        GameObject obj;
        obj = projectilePool[0];
        projectilePool.RemoveAt(0);
        obj.transform.position = self_pos;
        obj.SetActive(true);
        return obj;
    }
}
