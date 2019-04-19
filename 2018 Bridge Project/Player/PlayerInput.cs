using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    MoveSlider moveSlider;
    PlayerProperty playerProperty;
    PlayerBehaviour playerBehaviour;
    [SerializeField]
    Constellation constellation;
    [SerializeField]
    GameObject model;
    IEnumerator playerUpdate;
    [SerializeField]
    int jumpCount;

    bool jumpConst = true; // 일정 시간내에 점프를 다시 시도할 경우를 제한
    WaitForSeconds waitJumpConst = new WaitForSeconds(0.3f);

    WaitForFixedUpdate waitForFixedUpdate;

    public MoveSlider MoveSlider
    {
        get
        {
            return moveSlider;
        }

        set
        {
            moveSlider = value;
        }
    }

    public int JumpCount
    {
        get
        {
            return jumpCount;
        }

        set
        {
            jumpCount = value;
        }
    }

    void Start()
    {
        playerProperty = GetComponent<PlayerProperty>();
        playerBehaviour = GetComponent<PlayerBehaviour>();
        constellation = GetComponent<Constellation>();
        moveSlider = GameObject.Find("MoveSlider").GetComponent<MoveSlider>();
        waitForFixedUpdate = new WaitForFixedUpdate();
        model = transform.GetChild(6).gameObject;
        JumpCount = 0;
        playerUpdate = PlayerUpdate();
        StartCoroutine(playerUpdate);
    }

    IEnumerator PlayerUpdate()
    {
        while (!playerProperty.IsDead)
        {
            yield return waitForFixedUpdate;
            float sliderValue = MoveSlider.GetSliderValue();

            if (!GetComponent<Rigidbody2D>().constraints.Equals(RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation))
                transform.Translate(new Vector2(sliderValue * playerProperty.MoveSpeed * Time.deltaTime, 0));

            ConstellationUpdate();
            //AnimationUpdate(sliderValue); // 모바일 입력 코드
            Test_KeyboardInput(); // PC(테스팅) 임시 입력 코드
        }
    }

    public void ConstellationUpdate() // 별자리 업데이트
    {
        constellation = GetComponent<Constellation>();
    }

    void AnimationUpdate(float sliderValue) // 애니메이션 업데이트
    {
        if (playerProperty.IsDead)
        {
            playerBehaviour.Animation(PlayerAnimation.Death);
            return;
        }

        if (!playerProperty.Attackable)
        {
            playerBehaviour.Animation(PlayerAnimation.Attack);
        }
        else if (sliderValue != 0 && !playerProperty.IsJump && !playerProperty.IsDead) // 슬라이더를 누르고 있을 때
        {
            playerBehaviour.Animation(PlayerAnimation.Walk);
        }
        else if (!playerProperty.IsJump && playerProperty.Attackable) // 점프 상태가 아니고 공격중이 아니라면
        {
            playerProperty.MoveState = MoveState.Idle;
            playerBehaviour.Animation(0);
        }
        else if (!playerProperty.IsDead && playerProperty.IsJump) // 점프 상태이면
        {
            playerBehaviour.Animation(PlayerAnimation.Jump);
        }
    }

    void DirectionUpdate(float sliderValue) // 방향 업데이트
    {
        playerProperty.MoveState = sliderValue > 0 ? MoveState.Right : MoveState.Left;
        if (sliderValue != 0)
            playerProperty.Direction = playerProperty.MoveState == MoveState.Right ? false : true;

        if (!playerProperty.Direction)
        {
            model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            model.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    public void Jump() // 점프 버튼 클릭 콜백
    {
        if (JumpCount >= 1 || playerProperty.IsDead || playerProperty.BeAttacked)
            return;
        
        if (jumpConst)
        {
            JumpCount++;
            jumpConst = false;
            StartCoroutine(ConstraintJump());
            playerProperty.IsJump = true;
            transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerProperty.JumpSensitivity, ForceMode2D.Impulse);
        }
    }

    public void Attack() // 공격 버튼 클릭 콜백
    {
        if (!playerProperty.IsDead)
            constellation.Attack();

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, playerProperty.AttackDistance, 1 << 15);
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                float temp = cols[i].transform.position.x - transform.position.x;
                if ((playerProperty.Direction && (temp < 0)) || (!playerProperty.Direction && (temp > 0)))
                {
                    cols[i].GetComponent<IInteractionObject>().DoInteraction();
                }
            }
        }
    }

    public void Skill() // 스킬 버튼 클릭 콜백
    {
        if (!playerProperty.IsDead)
            constellation.Skill();
    }

    public void Test_KeyboardInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (!GetComponent<Rigidbody2D>().constraints.Equals(RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation))
            transform.Translate(new Vector2(horizontal * playerProperty.MoveSpeed * Time.deltaTime, 0));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Skill();
        }
        AnimationUpdate(horizontal);
        DirectionUpdate(horizontal);
    }

    IEnumerator ConstraintJump()
    {
        yield return waitJumpConst;
        jumpConst = true;
    }

    public void StartUpdate()
    {
        StartCoroutine(playerUpdate);
    }

    public void StopUpdate()
    {
        StopCoroutine(playerUpdate);
    }
}
