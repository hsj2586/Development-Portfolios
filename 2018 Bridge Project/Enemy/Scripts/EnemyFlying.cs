using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
	private PlayerProperty 	playerProperty;
	private EnemyProperty	enemyProperty;

	public LayerMask 	playerLayer;
	public float 		playerRange;
	public bool 		playerInRange;

	public bool			moveRight;
	
	void Start() {
		playerProperty  = GameObject.Find("PlayerTest").GetComponent<PlayerProperty>();
		enemyProperty 	= GetComponent<EnemyProperty>();
	}

	void Update()
	{
		playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);

		
        if (playerInRange)
        {
			if (transform.position.x < playerProperty.transform.position.x)
			{
				moveRight = true;
			}
			else
			{
				moveRight = false;
			}

            if (moveRight)
            {
                transform.localScale = new Vector3(-4f, 4f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(4f, 4f, 1f);
            }
            transform.position = Vector3.MoveTowards(transform.position, playerProperty.transform.position, enemyProperty.MoveSpeed * Time.deltaTime);
        }
	}

	void OndrawGizmosSelected()
	{
		Gizmos.DrawSphere(transform.position, playerRange);
	}
}
