using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapC_Rocks : MonoBehaviour, IInteractionObject
{
    public void DoInteraction()
    {
        GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 1);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
