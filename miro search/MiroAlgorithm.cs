using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiroAlgorithm : MonoBehaviour
{
    enum dir { up = 1, right = 2, down = 3, left = 4 };

    Dictionary<Vector3, int> roadInfo; // 맵의 정보 저장
    Vector3 exitCondition; // 탈출 조건
    Vector3 nowPos; // 현재 위치
    dir direction; // 현재의 방향 정보
    Vector3 front, left, right; // 자신이 바라보는 기준의 방향 정보들
    public float movespeed = 0.1f; // 이동속도

    void Awake()
    {
        roadInfo = new Dictionary<Vector3, int>();
        exitCondition = new Vector3(4, 0, 0);
        direction = dir.up;
        nowPos = transform.position;
        setDefault();
        move();
    }

    void setDefault() // 맵 정보 초기화
    {
        roadInfo.Add(new Vector3(0, 0, 0), 1);
        roadInfo.Add(new Vector3(0, 1, 0), 1);
        roadInfo.Add(new Vector3(1, 1, 0), 1);
        roadInfo.Add(new Vector3(2, 1, 0), 1);
        roadInfo.Add(new Vector3(3, 1, 0), 1);
        roadInfo.Add(new Vector3(4, 1, 0), 1);
        roadInfo.Add(new Vector3(4, 0, 0), 1);
        roadInfo.Add(new Vector3(0, -1, 0), 0);
        roadInfo.Add(new Vector3(-1, 0, 0), 0);
        roadInfo.Add(new Vector3(1, 0, 0), 0);
        roadInfo.Add(new Vector3(-1, 1, 0), 0);
        roadInfo.Add(new Vector3(0, 2, 0), 0);
        roadInfo.Add(new Vector3(1, 2, 0), 0);
        roadInfo.Add(new Vector3(2, 2, 0), 0);
        roadInfo.Add(new Vector3(2, 0, 0), 0);
        roadInfo.Add(new Vector3(3, 0, 0), 0);
        roadInfo.Add(new Vector3(3, 2, 0), 0);
        roadInfo.Add(new Vector3(4, -1, 0), 0);
        roadInfo.Add(new Vector3(4, 2, 0), 0);
        roadInfo.Add(new Vector3(5, 0, 0), 0);
        roadInfo.Add(new Vector3(5, 1, 0), 0);
    }

    void directionSet() // 방향 초기화
    {
        switch (direction)
        {
            case dir.up:
                {
                    front = nowPos + new Vector3(0, 1, 0);
                    left = nowPos + new Vector3(-1, 0, 0);
                    right = nowPos + new Vector3(1, 0, 0);
                    break;
                }
            case dir.down:
                {
                    front = nowPos + new Vector3(0, -1, 0);
                    left = nowPos + new Vector3(1, 0, 0);
                    right = nowPos + new Vector3(-1, 0, 0);
                    break;
                }
            case dir.left:
                {
                    front = nowPos + new Vector3(-1, 0, 0);
                    left = nowPos + new Vector3(0, -1, 0);
                    right = nowPos + new Vector3(0, 1, 0);
                    break;
                }
            case dir.right:
                {
                    front = nowPos + new Vector3(1, 0, 0);
                    left = nowPos + new Vector3(0, 1, 0);
                    right = nowPos + new Vector3(0, -1, 0);
                    break;
                }
        }
    }

    void move() // 이동 알고리즘
    {
        print("from " + nowPos + " move");
        directionSet();
        if (roadInfo[right] == 1)
        {
            direction = (dir)(((int)direction + 1) % 4);
            directionSet();
            StartCoroutine(forward());
        }
        else if (roadInfo[front] == 1)
        {
            StartCoroutine(forward());
        }
        else if (roadInfo[left] == 1)
        {
            direction = (dir)(((int)direction - 1) % 4);
            directionSet();
            StartCoroutine(forward());
        }
        else
        {
            direction = (dir)(((int)direction + 2) % 4);
            directionSet();
            StartCoroutine(forward());
        }
    }

    IEnumerator forward() // 오브젝트 이동
    {
        while (Vector3.Distance(transform.position, front) > 0.01f)
        {
            yield return new WaitForFixedUpdate();
            transform.Translate((front - transform.position).normalized * movespeed, Space.World);
        }
        nowPos = front;
        checkInMiro();
    }

    void checkInMiro() // 탈출 조건 확인
    {
        if (exitCondition == nowPos)
        {
            print("탈출!");
            Destroy(this.gameObject);
        }
        else
            move();
    }
}
