using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lobby_Stalactite : MonoBehaviour
{
    [SerializeField]
    float speed;
    IEnumerator temp;

    void Awake()
    {
        temp = update();
        StartCoroutine(temp);
    }

    IEnumerator update()
    {
        while (true)
        {
            yield return null;
            RaycastHit2D col = Physics2D.Raycast(transform.position, Vector2.down, 10, 1 << LayerMask.NameToLayer("Player"));
            Debug.DrawRay(transform.position, Vector2.down);
            if (col)
            {
                transform.DOShakePosition(0.8f, 0.3f, 20, 90);
                yield return new WaitForSeconds(0.8f);
                while (true)
                {
                    yield return null;
                    transform.Translate(new Vector2(0, -Time.deltaTime * speed));
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Map")))
        {
            StopCoroutine(temp);
        }
        if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            col.GetComponent<PlayerBehaviour>().BeAttacked(1, gameObject);
        }
    }
}
