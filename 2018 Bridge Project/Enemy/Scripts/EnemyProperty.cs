using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperty : MonoBehaviour
{
    [SerializeField]
    int life;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float knockBackDist;
    [SerializeField]
    GameObject onLand; // 어느 땅을 밟고 있는지
    [SerializeField]
    int chargingDamage; // 플레이어와 물리적으로 충돌했을 때 주는 데미지
    //[SerializeField]
    bool isDead;
    //[SerializeField]
    bool beAttacked;
    
    public int Life
    {
        get { return life; }
        set { life = value; }
    }
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public float KnockBackDist
    {
        get { return knockBackDist; }
        set { knockBackDist = value; }
    }

    public GameObject OnLand
    {
        get { return onLand; }
        set { onLand = value; }
    }

    public int ChargingDamage
    {
        get { return chargingDamage; }
        set { chargingDamage = value; }
    }

    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    public bool BeAttacked
    {
        get { return beAttacked; }
        set { beAttacked = value; }
    }
}
