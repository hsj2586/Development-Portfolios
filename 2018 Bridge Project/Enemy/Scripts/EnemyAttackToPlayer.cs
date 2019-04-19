using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackToPlayer : MonoBehaviour {
	private EnemyProperty enemyProperty;

	void Start()
	{
		enemyProperty = GetComponent<EnemyProperty>();
	}
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player" && !enemyProperty.IsDead)
		{
			collider.gameObject.SendMessage("BeAttacked", enemyProperty);
		}
	}
}
