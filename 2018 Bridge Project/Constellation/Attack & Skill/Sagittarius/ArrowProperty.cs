using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProperty : MonoBehaviour
{
	[SerializeField]
	float moveSpeed;
	[SerializeField]
	float knockBackDist;
	[SerializeField]
	int chargingDamage; // 플레이어와 물리적으로 충돌했을 때 주는 데미지
	[SerializeField]
	bool isCrushed;


	public float MoveSpeed
	{
		get
		{
			return moveSpeed;
		}

		set
		{
			moveSpeed = value;
		}
	}

	public float KnockBackDist
	{
		get
		{
			return knockBackDist;
		}

		set
		{
			knockBackDist = value;
		}
	}

	public int ChargingDamage
	{
		get
		{
			return chargingDamage;
		}

		set
		{
			chargingDamage = value;
		}
	}

	public bool IsCrushed
	{
		get
		{
			return isCrushed;
		}

		set
		{
			isCrushed = value;
		}
	}

}
