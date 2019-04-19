using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Interaction1 : MonoBehaviour, IInteractionObject
{
    // 시작 스테이지의 상호작용 물체1
    [SerializeField]
    GameObject movingPlatform;
    SpriteRenderer spriteRenderer;
    [SerializeField]
    List<Sprite> spriteList;
    bool isSwitched;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isSwitched = false;
    }

    public void DoInteraction()
    {
        movingPlatform.GetComponent<Animator>().enabled = isSwitched = isSwitched ? false : true;
        spriteRenderer.sprite = movingPlatform.GetComponent<Animator>().enabled ? spriteList[0] : spriteList[1];
    }
}
