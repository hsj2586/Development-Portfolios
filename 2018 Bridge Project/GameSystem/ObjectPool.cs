using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPoolable
{
    void PoolObjectInit(params object[] list); // 풀오브젝트 초기화
    IEnumerator Dispose(); // 풀오브젝트 메모리 해제
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    Queue<GameObject> objectPool;

    public void Awake()
    {
        objectPool = new Queue<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            objectPool.Enqueue(transform.GetChild(i).gameObject);
        }
    }

    public void ObjectPoolPush(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }

    public GameObject ObjectPoolPop(params object[] list)
    {
        if (objectPool.Count != 0)
        {
            GameObject obj;
            obj = objectPool.Dequeue();
            obj.SetActive(true);
            obj.GetComponent<IObjectPoolable>().PoolObjectInit(list);
            return obj;
        }
        else
        {
            Debug.Log("There's no Object in ObjectPool!"); // 예외 처리 필요
            return null;
        }
    }
}
