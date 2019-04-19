using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    // 투사체의 오브젝트 풀 기능을 하는 스크립트.

    List<GameObject> projectilePool;
    Transform temp;
    int i, j;

    void Awake()
    {
        projectilePool = new List<GameObject>();
        temp = GameObject.FindGameObjectWithTag("Temp").transform;
        for (i = 0; i < temp.childCount; i++)
        {
            projectilePool.Add(temp.GetChild(i).gameObject);
        }
    }

    public Transform ProjectilePoolPop(List<GameObject> projectilePoolList)
    {
        Transform obj;
        GameObject obj2;
        if (projectilePoolList.Count == 0)
            return null;
        else
        {
            obj = projectilePool[0].transform;
            projectilePool.RemoveAt(0);
            for (i = 0; i < projectilePoolList.Count; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    obj2 = Instantiate(projectilePoolList[i]);
                    obj2.transform.parent = obj.GetChild(i);
                    obj2.SetActive(false);
                }
                obj.GetChild(i).gameObject.SetActive(true);
            }
            obj.gameObject.SetActive(true);
            return obj;
        }
    }
}
