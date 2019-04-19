using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IObjectPoolable
{

    enum EnemyState { Left, Right }

    SpriteRenderer renderer_;
    ArrowProperty arrowProperty;
    PlayerProperty playerProperty;

    int penetrateCount = 0;      //관통 한 횟수 
    int maxPenetrate = 1;   //최대 관통 가능 횟수

    void Awake()
    {
        renderer_ = GetComponent<SpriteRenderer>();
        arrowProperty = GetComponent<ArrowProperty>();
        playerProperty = GameObject.Find("PlayerTest").GetComponent<PlayerProperty>();
    }

    void OnEnable()
    {
        StartCoroutine(update());
    }

    IEnumerator update()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / arrowProperty.MoveSpeed);

            if (renderer_.flipX && renderer_.flipY)
            {
                transform.Translate(Vector3.left * 0.3f);
            }
            else
            {
                transform.Translate(Vector3.right * 0.3f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Map"))
        {
            transform.parent.GetComponent<ObjectPool>().ObjectPoolPush(gameObject);
        }
        else if (col.CompareTag("Enemy"))
        {
            if (penetrateCount < 1)
            {

            }
            else
            {
                playerProperty.Lux -= 1;
            }

            penetrateCount += 1;

            col.SendMessage("BeAttacked", playerProperty, SendMessageOptions.DontRequireReceiver);

            if (penetrateCount >= maxPenetrate)
            {
                transform.parent.GetComponent<ObjectPool>().ObjectPoolPush(gameObject);
            }
        }

    }

    public void PoolObjectInit(params object[] list)
    {
        penetrateCount = 0;
        if (playerProperty.chargingTime >= 3f)
        {
            maxPenetrate = 3;
        }
        else
        {
            maxPenetrate = (int)(playerProperty.chargingTime / 1f);
        }
        playerProperty.chargingTime = 0f;


        transform.position = playerProperty.transform.position;

        if (playerProperty.Direction)
        {
            renderer_.flipX = true;
            renderer_.flipY = true;

        }
        else
        {
            renderer_.flipX = false;
            renderer_.flipY = false;
        }
    }

    public IEnumerator Dispose()
    {
        yield return null;
        StopAllCoroutines();
    }
}
