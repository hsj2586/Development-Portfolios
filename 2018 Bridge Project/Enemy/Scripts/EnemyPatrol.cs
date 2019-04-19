using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyPatrol : MonoBehaviour
{
    public LayerMask whatIsWall;
    public Transform edgeCheck;
    public Transform wallCheck;
    public float wallCheeckRadius;
    public bool moveRight;

    public LayerMask playerLayer;
    public float playerRange;
    public bool playerInRange;

    private Animator animator;
    private bool hittingWall;
    private bool atEdge;

    private EnemyProperty enemyProperty;
    private PlayerProperty playerProperty;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyProperty = GetComponent<EnemyProperty>();
        playerProperty = GameObject.Find("PlayerTest").GetComponent<PlayerProperty>();

        animator.SetInteger("State", 1);
    }

    void Update()
    {
        hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheeckRadius, whatIsWall);
        atEdge = Physics2D.OverlapCircle(edgeCheck.position, wallCheeckRadius, whatIsWall);

        if (hittingWall || atEdge)
            moveRight = !moveRight;

        playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);

        if (playerInRange)
        {
            if ((transform.position.x < playerProperty.transform.position.x) && Mathf.Abs(transform.position.y - playerProperty.transform.position.y) < 5)
            {
                moveRight = true;
            }
            else
            {
                moveRight = false;
            }
        }

        if (moveRight)
        {
            //transform.localScale = new Vector3(-5f, 5f, 1f);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            //transform.localScale = new Vector3(5f, 5f, 1f);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        transform.Translate(new Vector2(-1, 0) * enemyProperty.MoveSpeed);
    }

    void OndrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, playerRange);
    }
}
