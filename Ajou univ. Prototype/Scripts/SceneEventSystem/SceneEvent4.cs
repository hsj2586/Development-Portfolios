using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SceneEvent4 : SceneEvent
{
    // 네 번째 씬 이벤트 스크립트.
    GameObject teacher; // 여선생
    GameObject teachers; // 교장, 담임 선생
    GameObject vicePrincipal;
    GameObject zombieTeacher;
    Animator zombieTeacherAnimator;
    GameObject cellPhone;

    public override IEnumerator Init()
    {
        yield return null;
        teacher = GameObject.Find("Teacher");
        teachers = GameObject.Find("Teachers");
        cellPhone = GameObject.Find("CellPhone");
        vicePrincipal = teachers.transform.GetChild(0).gameObject;
        zombieTeacher = teachers.transform.GetChild(1).gameObject;
    }

    public override IEnumerator Execute()
    {
        yield return StartCoroutine(SceneDirect1());
        yield return StartCoroutine(SceneDirect2());
    }

    public override IEnumerator Restore()
    {
        yield return null;
        teacher.SetActive(false);
        teachers.SetActive(false);
        GameObject.Find("IngameScene").transform.Find("Enemies").gameObject.SetActive(true);
    }

    private IEnumerator SceneDirect1() // 좀비 여선생이 교장을 공격하는 연출
    {
        yield return new WaitForSeconds(2);
        AudioManager.Instance.PlayMainSound("BGM002");
        AudioManager.Instance.MuteMainSound();
        Animator vicePrincipalAnimator = vicePrincipal.GetComponent<Animator>();

        // 좀비화 연출
        AudioManager.Instance.PlaySoundLoop("ZombieWalk", 2);
        zombieTeacher.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        zombieTeacher.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        zombieTeacher.transform.GetChild(1).transform.gameObject.SetActive(true);
        zombieTeacherAnimator = zombieTeacher.transform.GetChild(1).GetComponent<Animator>();
        vicePrincipalAnimator.SetInteger("State", 0);
        zombieTeacherAnimator.SetInteger("AnimState", 0);

        yield return new WaitForSeconds(1);

        // 좀비 공격
        AudioManager.Instance.PlaySoundLoop("ZombieAttack", 2);
        zombieTeacherAnimator.SetInteger("AnimState", 2);
        yield return new WaitForSeconds(1f);
        zombieTeacherAnimator.SetInteger("AnimState", 0);
        yield return StartCoroutine(SceneDialog("SceneEvent4-1"));
        // 교장 쓰러짐 연출
        zombieTeacherAnimator.SetInteger("AnimState", 2);
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySoundOneShot("MaleDeath");
        vicePrincipalAnimator.SetInteger("State", 2);
        yield return null;
    }

    private IEnumerator SceneDirect2()
    {
        yield return new WaitForSeconds(2);
        teacher.GetComponent<SpriteRenderer>().flipX = false;
        Camera.main.GetComponent<CameraMove>().SetTarget(teacher);

        // 벨소리 울림 구현 필요.
        yield return StartCoroutine(SceneDialog("SceneEvent4-2"));
        yield return new WaitForSeconds(2);

        Camera.main.GetComponent<CameraMove>().SetTarget(zombieTeacher);
        yield return StartCoroutine(SceneDialog("SceneEvent4-3"));
        zombieTeacher.GetComponent<NavMeshAgent>().SetDestination(teacher.transform.position);
        zombieTeacherAnimator.SetInteger("AnimState", 1);
        zombieTeacher.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);

        // 담임이 여선생에게 돌진
        AudioManager.Instance.PlaySoundLoop("ZombieWalk", 3);
        while ((teacher.transform.position - zombieTeacher.transform.position).sqrMagnitude > 0.55f) { yield return null; }

        // 담임이 여선생에게 공격
        AudioManager.Instance.PlaySoundLoop("ZombieAttack", 2);
        zombieTeacherAnimator.SetInteger("AnimState", 2);
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySoundOneShot("FemaleDeath");

        // 여선생 쓰러짐 연출
        teacher.GetComponent<Animator>().SetInteger("State", 1);
        StartCoroutine(cellPhoneMove());
        yield return new WaitForSeconds(2);

        Camera.main.GetComponent<CameraMove>().SetTarget(sceneEventSystem.Player);
    }

    IEnumerator cellPhoneMove()
    {
        Vector3 startPos = new Vector3(-1.5f, 0.15f, 0);
        Vector3 endPos = new Vector3(-2.5f, 0.15f, 0);
        Vector3 lerpTemp;
        Transform cellPhoneSprite = cellPhone.transform.GetChild(0);
        cellPhoneSprite.gameObject.SetActive(true);
        cellPhone.transform.position = startPos;

        while ((cellPhone.transform.position - endPos).sqrMagnitude > 0.1f)
        {
            yield return null;
            lerpTemp = Vector3.Lerp(cellPhone.transform.position, endPos, Time.deltaTime * 2);
            cellPhone.transform.position = lerpTemp;
            cellPhoneSprite.transform.eulerAngles = new Vector3(60, 0, lerpTemp.sqrMagnitude * 180);
        }
        cellPhoneSprite.transform.eulerAngles = new Vector3(60, 0, 0);
    }
}