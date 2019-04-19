using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapA_Treasure : MonoBehaviour, IInteractionObject
{
    public void DoInteraction()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 1.5f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().PushInventory("Treasure");
    }
}
