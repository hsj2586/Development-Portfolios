using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommonBehaviour : MonoBehaviour
{
    private EnemyProperty enemyProperty;
    private Animator animator;

    public GameObject luxPool;

    void OnEnable()
    {
        enemyProperty = GetComponent<EnemyProperty>();
        animator = GetComponent<Animator>();
    }

    public void BeAttacked(PlayerProperty playerProperty)
    {
        if (!enemyProperty.BeAttacked)
        {
            enemyProperty.Life = Mathf.Clamp(enemyProperty.Life - 1, 0, 3);
            Animation(4);
            StartCoroutine(KnockBack(playerProperty));

            if (enemyProperty.Life == 0)
            {
                enemyProperty.IsDead = true;
                StartCoroutine(Death());
            }
        }
    }
    //KnockBack 부분 수정
    IEnumerator KnockBack(PlayerProperty playerProperty)
    {
        enemyProperty.BeAttacked = true;
        if (transform.localScale.x < 0)
        {
            transform.Translate(Vector3.left * enemyProperty.KnockBackDist);
        }
        else
        {
            transform.Translate(Vector3.right * enemyProperty.KnockBackDist);
        }
        yield return new WaitForSeconds(0.3f);
        enemyProperty.BeAttacked = false;
        if(!enemyProperty.IsDead)
        {
            Animation(1);
        }
    }

    public void Animation(int enemyAnimation)
    {
        animator.SetInteger("State", enemyAnimation);
    }

    IEnumerator Death()
    {
        enemyProperty.MoveSpeed = 0;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<EnemyPatrol>().enabled = false;
        Animation(3);

        luxPool.GetComponent<ObjectPool>().ObjectPoolPop(transform.position, 1);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        //transform.parent.GetComponent<ObjectPool>().ObjectPoolPush(gameObject);
    }
}
