using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SceneEvent6 : SceneEvent
{
    // 여섯 번째 씬 이벤트 스크립트.
    GameObject exitDoor;

    public override IEnumerator Init()
    {
        yield return null;
        exitDoor = GameObject.Find("RightDoor");
        yield return StartCoroutine(TurnOnMarker(exitDoor));
    }

    public override IEnumerator Execute()
    {
        yield return StartCoroutine(InteractWithDoor());
        yield return StartCoroutine(TurnOffMarker());
        yield return StartCoroutine(SceneDirect());
    }

    public override IEnumerator Restore()
    {
        yield return null;
    }

    IEnumerator InteractWithDoor() // 문과 상호작용 하기를 기다림
    {
        while (!exitDoor.GetComponent<Door>().IsOpen)
        {
            yield return null;
        }
    }

    IEnumerator SceneDirect() // 문이 열린 후 걸어가는 연출
    {
        yield return new WaitForSeconds(1);
        Vector3 targetPos = new Vector3(6, 0, 0);
        sceneEventSystem.TouchCanvas.SetActive(false);
        sceneEventSystem.Player.GetComponent<PlayerInput>().enabled = false;

        NavMeshAgent nma = sceneEventSystem.Player.GetComponent<NavMeshAgent>();
        nma.enabled = true;
        sceneEventSystem.Player.transform.Find("Model").GetComponent<SpriteRenderer>().flipX = false;
        nma.speed = 1;
        nma.SetDestination(targetPos);
        sceneEventSystem.Player.transform.GetChild(0).GetComponent<Animator>().SetInteger("State", 1);
        AudioManager.Instance.PlayStepSound("Walk1");
        StartCoroutine(FadeOut());
        while ((sceneEventSystem.Player.transform.position - targetPos).sqrMagnitude > 0.3f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2);
    }
}