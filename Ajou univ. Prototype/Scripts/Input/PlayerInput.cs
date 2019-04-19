using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Idle, Sneak, Walk, Run }

public class PlayerInput : MonoBehaviour
{
    // 입력에 따른 플레이어의 행동 세팅을 조작하는 스크립트.

    GameObject lookDir;
    [SerializeField]
    Joystick joystick; //조이스틱 오브젝트
    Animator animator;
    SpriteRenderer spriteRenderer;
    PlayerProperty property;
    float movespeed; // 이동 속도
    private Vector3 moveVector; //회전,이동 방향

    void Start()
    {
        property = GetComponent<PlayerProperty>();
        animator = transform.Find("Model").GetComponent<Animator>();
        lookDir = transform.Find("LookDir").gameObject;
        spriteRenderer = transform.Find("Model").GetComponent<SpriteRenderer>();
        property.State = PlayerState.Idle;
    }

    void SetMovementSpeed()
    {
        switch (property.State)
        {
            case PlayerState.Idle:
                {
                    SoundGenerator.SoundTransmission(new Sound(transform.position, 0f, 1f), this.transform);
                    movespeed = 0;
                    break;
                }
            case PlayerState.Sneak:
                {
                    SoundGenerator.SoundTransmission(new Sound(transform.position, property.SneakVolume, property.SneakRange), this.transform);
                    movespeed = property.SneakSpeed;
                    break;
                }
            case PlayerState.Walk:
                {
                    SoundGenerator.SoundTransmission(new Sound(transform.position, property.WalkVolume, property.WalkRange), this.transform);
                    movespeed = property.WalkSpeed;
                    break;
                }
            case PlayerState.Run:
                {
                    SoundGenerator.SoundTransmission(new Sound(transform.position, property.RunVolume, property.RunRange), this.transform);
                    movespeed = property.RunSpeed;
                    break;
                }
            default:
                break;
        }
    }

    void SetState()
    {
        if (moveVector.magnitude == 0.0f)
        {
            animator.SetInteger("State", 0);
            AudioManager.Instance.StopStepSound();
            property.State = PlayerState.Idle;
        }
        else if (moveVector.magnitude < 0.3f)
        {
            animator.SetInteger("State", 1);
            AudioManager.Instance.PlayStepSound("Sneak");
            property.State = PlayerState.Sneak;
        }
        else if (moveVector.magnitude < 0.7f)
        {
            animator.SetInteger("State", 1);
            AudioManager.Instance.PlayStepSound("Walk1");
            property.State = PlayerState.Walk;
        }
        else
        {
            animator.SetInteger("State", 2);
            AudioManager.Instance.PlayStepSound("Run1");
            property.State = PlayerState.Run;
        }
    }

    void Rotation()
    {
        if (moveVector != Vector3.zero)
            lookDir.transform.forward = moveVector;
    }

    void Move()
    {
        transform.Translate(moveVector.normalized * movespeed * Time.smoothDeltaTime);
    }

    void SetAnimation()
    {
        float angle = lookDir.transform.localEulerAngles.y;
        if (angle < 180)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    Vector3 GetMoveInput()
    {
        float h = joystick.GetHorizontalValue();
        float v = joystick.GetVerticalValue();

        Vector3 moveDir = new Vector3(h, 0, v);

        return moveDir;
    }

    void Update()
    {
        moveVector = GetMoveInput();
        Move();
        SetState();
        SetMovementSpeed();
        SetAnimation();
        Rotation();
    }
}
