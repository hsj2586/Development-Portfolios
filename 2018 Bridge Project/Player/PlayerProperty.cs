using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveState { Idle, Left, Right }

public class PlayerProperty : MonoBehaviour
{
    [SerializeField]
    int life; // 생명, 목숨
    [SerializeField]
    int maxLife; // 최대 생명, 목숨
    [SerializeField]
    float moveSpeed; // 이동 속도
    [SerializeField]
    float jumpSensitivity; // 점프 감도
    [SerializeField]
    float attackSpeed; // 공격 속도
    [SerializeField]
    float attackDistance; // 사거리
    [SerializeField]
    float knockBackDist; // 넉백 거리
    [SerializeField]
    float immortalTime; // 무적 시간, 피격 당하지 않는 지속시간
    [SerializeField]
    bool direction; // 플레이어가 바라보는 방향, true는 오른쪽 false는 왼쪽 방향.
    [SerializeField]
    bool beAttacked;
    [SerializeField]
    int lux; // 럭스 수치
    MoveState moveState;
    [SerializeField]
    bool isJump;
    bool attackable;
    bool isDead;
	public float chargingTime;
    List<string> constellationList;
    Constellation constellation;

    void Awake()
    {
        // 별자리 클래스 리스트 초기화
        constellationList = new List<string>();
        constellationList.Add("Aries");
		constellationList.Add("Sagittarius");
    }

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

    public float JumpSensitivity
    {
        get
        {
            return jumpSensitivity;
        }

        set
        {
            jumpSensitivity = value;
        }
    }

    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }

        set
        {
            attackSpeed = value;
        }
    }

    public float AttackDistance
    {
        get
        {
            return attackDistance;
        }

        set
        {
            attackDistance = value;
        }
    }

    public MoveState MoveState
    {
        get
        {
            return moveState;
        }

        set
        {
            moveState = value;
        }
    }

    public bool IsJump
    {
        get
        {
            return isJump;
        }

        set
        {
            isJump = value;
        }
    }

    public bool Attackable
    {
        get
        {
            return attackable;
        }

        set
        {
            attackable = value;
        }
    }

    public bool IsDead
    {
        get
        {
            return isDead;
        }

        set
        {
            isDead = value;
        }
    }

    public List<string> ConstellationList
    {
        get
        {
            return constellationList;
        }

        set
        {
            constellationList = value;
        }
    }

    public Constellation Constellation
    {
        get
        {
            return constellation;
        }

        set
        {
            constellation = value;
        }
    }

    public int Lux
    {
        get
        {
            return lux;
        }

        set
        {
            lux = value;
        }
    }

    public int Life
    {
        get
        {
            return life;
        }

        set
        {
            life = value;
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

    public float ImmortalTime
    {
        get
        {
            return immortalTime;
        }

        set
        {
            immortalTime = value;
        }
    }

    public int MaxLife
    {
        get
        {
            return maxLife;
        }

        set
        {
            maxLife = value;
        }
    }

    public bool BeAttacked
    {
        get
        {
            return beAttacked;
        }

        set
        {
            beAttacked = value;
        }
    }

    public bool Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }
}
