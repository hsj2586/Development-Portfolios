using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Interaction2 : MonoBehaviour, IInteractionObject
{
    // 시작 스테이지의 상호작용 물체1
    [SerializeField]
    GameObject movingPlatform;
    bool isSwitched;

    void Awake()
    {
        isSwitched = false;
    }

    public void DoInteraction()
    {
        movingPlatform.GetComponent<Animator>().enabled = isSwitched = isSwitched ? false : true;
    }
}
