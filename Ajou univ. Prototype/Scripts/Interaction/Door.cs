using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, BehavioralObject
{
    // '문' 오브젝트 스크립트.

    bool isOpen;
    Animator animator;
    [Header("연결 섹션")]
    [SerializeField]
    List<Section> sectionList = new List<Section>();

    public bool IsOpen
    {
        get { return this.isOpen; }
    }

    void Start()
    {
        isOpen = false;
        animator = GetComponent<Animator>();
    }

    public void BehaviorByInteraction()
    {
        if (!isOpen)
        {
            animator.SetInteger("Trigger", 1);
            isOpen = true;
        }
        else
        {
            animator.SetInteger("Trigger", 2);
            isOpen = false;
        }
        callSection();
    }

    void callSection() // 각 섹션에게 변경사항을 알리는 메소드
    {
        if (sectionList.Count != 0)
        {
            for (var i = 0; i < 2; i++)
            {
                sectionList[i].changeBoolList(sectionList[1 - i], isOpen);
            }
        }
    }
}
