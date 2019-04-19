using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : MonoBehaviour
{
    [SerializeField]
    GameObject targetPos;
    GameSystem gameSystem;

    void Awake()
    {
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            gameSystem.MoveStage();
            StartCoroutine(MoveStage(col));
        }
    }

    IEnumerator MoveStage(Collider2D col)
    {
        yield return new WaitForSeconds(1);
        col.transform.position = targetPos.transform.position;
    }
}
