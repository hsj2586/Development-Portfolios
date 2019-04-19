using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovedPlatform : MonoBehaviour
{
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.transform.parent = transform;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.transform.parent = null;
        }
    }
}
