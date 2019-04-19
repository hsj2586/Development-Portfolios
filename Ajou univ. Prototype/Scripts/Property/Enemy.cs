using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { Idle, Stroll, Move, Run, Attack }


public class Enemy : MonoBehaviour
{
    // 적의 AI 및 행동 기능을 하는 스크립트.
    [SerializeField]
    GameObject player;
    Animator animator;
    SpriteRenderer spriteRenderer;
    NavMeshAgent nav;
    Transform target; // 목표물 변수
    Vector3 targetPos; // 목표지점 변수
    float weightOfSound; // 소리의 가중치 변수, (소리의 크기/ 소음의 근원지까지의 거리)로 계산
    IEnumerator routine; // FSM 제어용 변수
    float elapsTime; // Stroll 이동을 위한 변수
    float strollTime; // stroll 시간 변수
    Vector3 tempPos; // Idle 이동을 위한 임시 변수
    Transform exclamationMark; // 느낌표 마크 오브젝트
    EnemyProperty property;

    void Awake()
    {
        property = GetComponent<EnemyProperty>();
        property.State = EnemyState.Idle;
        tempPos = transform.position;
        nav = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        exclamationMark = transform.GetChild(1);
        weightOfSound = 0;
        elapsTime = 0;

        StartFSM();
    }

    public GameObject Player
    {
        set
        {
            this.player = value;
        }
    }

    public IEnumerator Dead()
    {
        StopCoroutine(routine);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    IEnumerator FSM()
    {
        yield return null;
        switch (property.State)
        {
            case EnemyState.Idle:
                {
                    if (animator.gameObject.activeSelf)
                        animator.SetInteger("AnimState", 0);
                    if (player)
                    {
                        if ((transform.position - player.transform.position).sqrMagnitude <= property.ToRunRadius) // Idle 도중에 플레이어가 시야 안에 들어왔을 경우
                        {
                            elapsTime = 0;
                            property.State = EnemyState.Run;
                            exclamationMark.gameObject.SetActive(true);
                            SetTarget(player.transform);
                        }
                        else if (elapsTime <= property.IdleDurationTime) // Idle 도중에 Idle 지속시간이 지나지 않은 경우
                        {
                            elapsTime += Time.deltaTime;
                        }
                        else // Idle 도중에 Idle 지속시간이 지난 경우
                        {
                            elapsTime = 0;
                            Vector3 newDir;
                            property.State = EnemyState.Stroll;
                            strollTime = property.StrollDistance / property.StrollSpeed;
                            if ((transform.position - tempPos).sqrMagnitude <= property.StrollRange) // 제한동선 범위 내에 있을 경우
                            {
                                newDir = transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                            }
                            else // 제한동선 범위 외에 있을 경우
                            {
                                newDir = transform.position + (tempPos - transform.position).normalized;
                            }
                            SetMovepoint(newDir);
                        }

                        StartFSM();
                        break;
                    }
                    else
                        break;
                }

            case EnemyState.Stroll:
                {
                    if (animator.gameObject.activeSelf)
                        animator.SetInteger("AnimState", 1);
                    if (player)
                    {
                        float dist = (transform.position - player.transform.position).sqrMagnitude;

                        if (dist <= property.ToRunRadius) // Stroll 도중에 플레이어가 시야 안에 들어왔을 경우
                        {
                            elapsTime = 0;
                            property.State = EnemyState.Run;
                            exclamationMark.gameObject.SetActive(true);
                            SetTarget(player.transform);
                        }
                        else if (elapsTime <= strollTime) // Stroll 도중에 아직 목표 거리에 다다르지 못했을 경우
                        {
                            elapsTime += Time.deltaTime;
                        }
                        else // Stroll 도중에 목표 거리에 도착했을 경우
                        {
                            elapsTime = 0;
                            property.State = EnemyState.Idle;
                        }

                        StartFSM();
                        break;
                    }
                    else
                    {
                        property.State = EnemyState.Idle;
                        tempPos = transform.position;
                        StartFSM();
                        break;
                    }
                }

            case EnemyState.Move:
                {
                    if (animator.gameObject.activeSelf)
                        animator.SetInteger("AnimState", 1);
                    if (player)
                    {
                        float dist = (transform.position - player.transform.position).sqrMagnitude;

                        if (dist <= property.ToRunRadius) // Move 도중에 플레이어가 시야 안에 들어왔을 경우
                        {
                            property.State = EnemyState.Run;
                            exclamationMark.gameObject.SetActive(true);
                            SetTarget(player.transform);
                        }
                        else if ((transform.position - targetPos).sqrMagnitude <= 0.5f) // Move 도중에 소음의 근원지에 도착했을 경우
                        {
                            property.State = EnemyState.Idle;
                            tempPos = transform.position;
                        }
                        else
                            SetMovepoint(targetPos);

                        StartFSM();
                        break;
                    }
                    else
                    {
                        property.State = EnemyState.Idle;
                        tempPos = transform.position;
                        StartFSM();
                        break;
                    }
                }

            case EnemyState.Run:
                {
                    if (animator.gameObject.activeSelf)
                        animator.SetInteger("AnimState", 1);
                    if (player)
                    {
                        float dist = (transform.position - player.transform.position).sqrMagnitude;
                        if (dist <= property.RunToAttackRadius) // 플레이어가 공격 사거리 안에 있을 경우
                        {
                            property.State = EnemyState.Attack;
                        }
                        else if (dist <= property.RunToIdleRadius) // 플레이어가 포착 시야 안에 있을 경우
                        {
                            SetTarget(player.transform);
                            SetMovepoint(target.position);
                        }
                        else // 플레이어가 포착 시야 안에서 벗어날 경우
                        {
                            property.State = EnemyState.Idle;
                            exclamationMark.gameObject.SetActive(false);
                            tempPos = transform.position;
                        }
                        StartFSM();
                        break;
                    }
                    else
                    {
                        property.State = EnemyState.Idle;
                        exclamationMark.gameObject.SetActive(false);
                        tempPos = transform.position;
                        StartFSM();
                        break;
                    }
                }

            case EnemyState.Attack:
                {
                    if (animator.gameObject.activeSelf)
                        animator.SetInteger("AnimState", 2);
                    yield return new WaitForSeconds(0.5f);
                    player.SendMessage("BeAttacked", gameObject, SendMessageOptions.DontRequireReceiver);
                    // 공격 사운드 발생, 애니메이션 등 효과를 추가할 것.
                    property.State = EnemyState.Idle;
                    tempPos = transform.position;
                    StartFSM();
                    break;
                }
        }
    }

    public void StartFSM()
    {
        routine = FSM();
        StartCoroutine(routine);
    }

    public void SetTarget(Transform target) // 목표물 설정, 외부의 이벤트 상호 작용에 의해 호출됨
    {
        this.target = target;
    }

    void SetMovepoint(Vector3 pos)
    {
        if ((pos - transform.position).x > 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
        nav.SetDestination(pos);
    }

    public void ListeningToSounds(Sound sound) // 소리를 듣는 콜백, 가중치를 실시간으로 계산해 행동을 결정함 
    {
        float tempSound = sound.Volume / (transform.position - sound.Source).sqrMagnitude;
        if (property.State != EnemyState.Run && property.State != EnemyState.Attack && weightOfSound < tempSound) // Run, Attack 상태가 아니면서, 최근에 들린 소리의 가중치가 기존 가중치보다 클 경우
        {
            weightOfSound = tempSound;
            targetPos = sound.Source;
            property.State = EnemyState.Move;
        }
    }

    void Update() // 상태에 따라 속성을 조정하는 업데이트
    {
        switch (property.State)
        {
            case EnemyState.Idle:
                weightOfSound = 0;
                nav.speed = 0;
                break;
            case EnemyState.Stroll:
                nav.speed = property.StrollSpeed;
                break;
            case EnemyState.Move:
                nav.speed = property.MoveSpeed;
                break;
            case EnemyState.Run:
                nav.speed = property.RunSpeed;
                break;
        }
    }
}
