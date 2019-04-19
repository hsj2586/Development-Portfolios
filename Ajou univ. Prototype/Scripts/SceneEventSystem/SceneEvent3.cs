using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SceneEvent3 : SceneEvent
{
    // 세 번째 씬 이벤트 스크립트.
    GameObject tv;
    GameObject teachers;
    GameObject camMarkerPos;

    public override IEnumerator Init()
    {
        yield return null;
        tv = GameObject.Find("TVset_example");
        teachers = GameObject.Find("Teachers");
        yield return StartCoroutine(TurnOnMarker(tv));
        sceneEventSystem.Player.GetComponent<PlayerInput>().enabled = true;
    }

    public override IEnumerator Execute()
    {
        yield return StartCoroutine(InteractWithTV());
        yield return StartCoroutine(TurnOffMarker());
        StartCoroutine(FadeOut());
        yield return StartCoroutine(DirectMode());
        sceneEventSystem.Player.transform.position = new Vector3(-2, 0, 2);
        yield return StartCoroutine(FadeIn());
        yield return StartCoroutine(SceneDialog("SceneEvent3-1"));
        yield return StartCoroutine(SceneDirect1());
        yield return StartCoroutine(SceneDirect2());
        yield return StartCoroutine(SceneDialog("SceneEvent3-2"));
    }

    private IEnumerator InteractWithTV() // TV와 상호작용 할때까지의 이벤트
    {
        while (!tv.GetComponent<TV>().IsOff)
        {
            yield return null;
        }
        
        tv.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(0.7f);
    }

    private IEnumerator SceneDirect1() // 승연이 숨는 연출 이벤트
    {
        sceneEventSystem.Player.GetComponent<PlayerInput>().enabled = false;
        Vector3 targetPos = new Vector3(-4.647f, 0f, -2.004f);
        NavMeshAgent nma = sceneEventSystem.Player.GetComponent<NavMeshAgent>();
        nma.enabled = true;
        sceneEventSystem.Player.transform.Find("Model").GetComponent<SpriteRenderer>().flipX = true;
        nma.SetDestination(targetPos);
        sceneEventSystem.Player.transform.GetChild(0).GetComponent<Animator>().SetInteger("State", 2);
        AudioManager.Instance.PlayStepSound("Run1");
        while ((sceneEventSystem.Player.transform.position - targetPos).sqrMagnitude > 0.01f)
        {
            yield return null;
        }
        AudioManager.Instance.StopStepSound();
        sceneEventSystem.Player.transform.GetChild(0).GetComponent<Animator>().SetInteger("State", 0);
        nma.enabled = false;

    }

    private IEnumerator SceneDirect2() // TV와 상호작용 후 교장, 교감이 들어오는 연출 이벤트
    {
        AudioManager.Instance.MuteMainSound(); // BGM 뮤트
        teachers.transform.GetChild(0).gameObject.SetActive(true);
        teachers.transform.GetChild(1).gameObject.SetActive(true);
        Camera.main.GetComponent<CameraMove>().SetTarget(teachers.transform.GetChild(0).gameObject);
        Animator vicePrincipalAnimator = teachers.transform.GetChild(0).GetComponent<Animator>();
        Animator zombieTeacherAnimator = teachers.transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        StartCoroutine(SplitSound("Walk1", 2.5f, 0.2f, 2));

        while ((teachers.transform.GetChild(0).position - teachers.transform.GetChild(2).position).sqrMagnitude > 0.4f)
        {
            yield return null;
            vicePrincipalAnimator.SetInteger("State", 1);
            zombieTeacherAnimator.SetInteger("State", 1);
            teachers.transform.GetChild(0).Translate(new Vector3(-0.01f, 0, 0));
            teachers.transform.GetChild(1).Translate(new Vector3(-0.01f, 0, 0));
        }
        vicePrincipalAnimator.SetInteger("State", 0);
        zombieTeacherAnimator.SetInteger("State", 0);

        teachers.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;

        yield return null;
    }

    public override IEnumerator Restore()
    {
        yield return null;

        sceneEventSystem.Player.GetComponent<PlayerInput>().enabled = true;
    }
}
