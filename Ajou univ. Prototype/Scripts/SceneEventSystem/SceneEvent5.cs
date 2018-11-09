using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SceneEvent5 : SceneEvent
{
    // 다섯 번째 씬 이벤트 스크립트.
    GameObject tv;
    GameObject cellPhone;
    GameObject enemies;
    List<GameObject> enemieList;

    public override IEnumerator Init()
    {
        yield return null;
        cellPhone = GameObject.Find("CellPhone");
        tv = GameObject.Find("TVset_example");
        enemies = GameObject.Find("Enemies");
        enemieList = new List<GameObject>();
    }

    public override IEnumerator Execute()
    {
        yield return StartCoroutine(SceneDialog("SceneEvent5-1"));
        yield return StartCoroutine(TurnOnMarker(tv));
        yield return StartCoroutine(PlayMode());
        yield return StartCoroutine(InteractWithTV());
        yield return StartCoroutine(TurnOffMarker());
        yield return StartCoroutine(DirectMode());

        yield return StartCoroutine(FadeOut());
        sceneEventSystem.Player.transform.position = new Vector3(-3.5f, 0, 0.5f);
        yield return StartCoroutine(FadeIn());

        yield return StartCoroutine(SceneDirect1());
        yield return StartCoroutine(PlayMode());

        for (int i = 0; i < enemieList.Count; i++)
        {
            enemieList[i].GetComponent<Enemy>().enabled = true;
            enemieList[i].GetComponent<Enemy>().StartFSM();
        }
        yield return StartCoroutine(TurnOnMarker(cellPhone));
        yield return StartCoroutine(InteractWithCellPhone());
        yield return StartCoroutine(TurnOffMarker());
        yield return StartCoroutine(DirectMode());
        yield return StartCoroutine(SceneDialog("SceneEvent5-2"));
        yield return StartCoroutine(PlayMode());
    }

    public override IEnumerator Restore()
    {
        yield return null;
    }

    IEnumerator InteractWithCellPhone()
    {
        while (cellPhone)
        {
            yield return null;
        }
    }

    IEnumerator InteractWithTV() // TV와 상호작용 할 때 까지의 이벤트
    {
        yield return new WaitUntil(() => tv.GetComponent<TV>().IsOff == false);
        tv.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator SceneDirect1() // TV로 좀비들이 몰려드는 연출
    {
        Camera.main.GetComponent<CameraMove>().SetTarget(new Vector3(-1.3f, 0, 2));

        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            enemieList.Add(enemies.transform.GetChild(i).gameObject);
        }
        // 좀비들 tv로 돌진
        StartCoroutine(SplitSound("ZombieWalk", 3, 0.2f, 2));
        for (int i = 0; i < enemieList.Count; i++)
        {
            enemieList[i].GetComponent<Enemy>().StopAllCoroutines();
            enemieList[i].GetComponent<Enemy>().enabled = false;
            enemieList[i].transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
            enemieList[i].transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 1);
            enemieList[i].transform.position = new Vector3(2.5f + 0.6f * i, 0, 0.5f + 0.6f * i);
            enemieList[i].GetComponent<NavMeshAgent>().speed = 2;
            enemieList[i].GetComponent<NavMeshAgent>().SetDestination(tv.transform.position);
        }

        yield return new WaitForSeconds(3.5f);

        for (int i = 0; i < enemieList.Count; i++)
        {
            yield return new WaitForSeconds(0.4f);
            enemieList[i].GetComponent<NavMeshAgent>().speed = 0.5f;
            enemieList[i].transform.GetChild(0).GetComponent<Animator>().SetInteger("AnimState", 2);
            AudioManager.Instance.PlaySoundLoop("ZombieAttack", 1);
        }
        //뉴스가 꺼지고 노이즈 켜짐
        tv.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        tv.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 5; i++) // tv 깜빡거리는 연출
        {
            tv.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
            tv.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = Color.black;
            yield return new WaitForSeconds(0.1f);
        }
        Camera.main.GetComponent<CameraMove>().SetTarget(sceneEventSystem.Player);
    }
}