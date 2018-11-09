using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneEvent1 : SceneEvent
{
    // 첫 번째 씬 이벤트 스크립트.
    GameObject teacher;

    public override IEnumerator Init()
    {
        yield return null;
        teacher = GameObject.Find("Teacher");
        sceneEventSystem.Player.GetComponent<PlayerInput>().enabled = false;
    }

    public override IEnumerator Execute()
    {
        yield return StartCoroutine(ChapterIntroduce("1장", "좀비 사태"));
        yield return StartCoroutine(DirectMode()); // 디렉트 모드 실행
        yield return StartCoroutine(SceneDirect());
        yield return StartCoroutine(SceneDialog("SceneEvent1"));
        yield return StartCoroutine(PlayMode());
    }

    public override IEnumerator Restore()
    {
        yield return null;
        sceneEventSystem.Player.GetComponent<PlayerInput>().enabled = true;
    }

    IEnumerator SceneDirect()
    {
        Animator animator = sceneEventSystem.Player.transform.Find("Model").GetComponent<Animator>();
        animator.SetInteger("State", 1);
        AudioManager.Instance.PlayStepSound("Walk1");
        while ((sceneEventSystem.Player.transform.position - teacher.transform.position).sqrMagnitude > 0.4f)
        {
            yield return null;
            sceneEventSystem.Player.transform.Translate(new Vector3(0.01f, 0, 0));
        }
        AudioManager.Instance.StopStepSound();
        animator.SetInteger("State", 0);
    }
}
