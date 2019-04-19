using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lux : MonoBehaviour, IObjectPoolable
{
    [Header("럭스 수치")]
    [SerializeField]
    int luxValue;
    [SerializeField]
    Transform luxUIPosition;
    [SerializeField]
    float moveSpeed;
    Transform temp_Player;
    Transform temp_Parent;

    public IEnumerator Dispose()
    {
        yield return new WaitForSeconds(0.5f);
        temp_Player.gameObject.SendMessage("UpdateLux", temp_Player.GetComponent<PlayerProperty>().Lux + luxValue);
        GameObject.Find("LuxPoint").SendMessage("UpdateUI");
        transform.parent = temp_Parent;
        transform.parent.GetComponent<ObjectPool>().ObjectPoolPush(gameObject);
        transform.localScale = new Vector3(3, 1, 3);
        StopAllCoroutines();
    }

    public void PoolObjectInit(params object[] list)
    {
        transform.position = (Vector3)list[0];
        luxValue = (int)list[1];
        transform.DOLocalMoveY(transform.position.y + 0.5f, 0.3f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            temp_Player = col.transform;
            transform.localScale = new Vector3(2.5f, 1f, 2.5f);
            transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InBack);
            StartCoroutine(Dispose());
            StartCoroutine(MoveToLuxUI());
        }
    }

    IEnumerator MoveToLuxUI()
    {
        temp_Parent = transform.parent;
        transform.parent = temp_Player;

        while (true)
        {
            yield return null;
            transform.Translate((luxUIPosition.position - transform.position) * moveSpeed, Space.Self);
        }
    }
}
