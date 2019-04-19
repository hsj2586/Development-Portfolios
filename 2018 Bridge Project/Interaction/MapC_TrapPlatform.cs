using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapC_TrapPlatform : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.transform.parent.CompareTag("Player"))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.transform.GetChild(i).GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 1.5f);
            }
            Invoke("DestroyObject", 0.75f);
        }
    }

    void DestroyObject()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
