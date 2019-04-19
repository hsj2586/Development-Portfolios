using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PlayerAnimation { Idle, Walk, Run, Jump, Attack, Death } // 플레이어 애니메이션 변수

public class PlayerBehaviour : MonoBehaviour
{
    // 플레이어의 행동을 정의하는 클래스
    PlayerInput playerInput;
    PlayerProperty playerProperty;
    Animator animator;
    SpriteRenderer spriteRenderer;
    GameObject lifePointPool;
    List<GameObject> lifePointGauge;

    void Awake()
    {
        spriteRenderer = transform.Find("Model").GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        playerProperty = GetComponent<PlayerProperty>();
        animator = transform.Find("Model").GetComponent<Animator>();
        lifePointPool = GameObject.Find("LifePointPool");
        lifePointGauge = new List<GameObject>();

        for (int i = 0; i < lifePointPool.transform.childCount; i++)
        {
            lifePointGauge.Add(lifePointPool.transform.GetChild(i).gameObject);
        }
    }

    void FixedUpdate()
    {
        //BottomsCollisionCheck();
        ForwardCollisionCheck();
    }

    //void BottomsCollisionCheck() // 바닥 충돌체 체크
    //{
    //    RaycastHit2D hit = Physics2D.BoxCast(transform.position - new Vector3(0, 0.6f, 0), new Vector2(0.4f, 0.01f), 0, Vector2.down, 0.2f, 1 << LayerMask.NameToLayer("Map"));
    //    if (hit)
    //    {
    //        playerInput.JumpCount = 0;
    //        playerProperty.IsJump = false;
    //    }
    //    else
    //    {
    //        playerProperty.IsJump = true;
    //    }
    //}

    void ForwardCollisionCheck() // 캐릭터가 바라보는 정면 유격 체크
    {
        Vector2 temp = playerProperty.Direction ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.1f, 1.2f), 0, temp, 0.4f, 1 << LayerMask.NameToLayer("Map"));

        if (hit)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void Animation(PlayerAnimation anim)
    {
        animator.SetInteger("State", (int)anim);
    }

    public void HealthRecovery(float value) // 체력 회복 메소드
    {
    }

    public void UpdateLux(int luxValue) // 럭스를 갱신하는 메소드
    {
        playerProperty.Lux = Mathf.Clamp(luxValue, 0, int.MaxValue);
    }

    public void UpdateLife(int lifeValue) // 생명 갱신하는 메소드
    {
        playerProperty.Life = Mathf.Clamp(lifeValue, 0, int.MaxValue);
        for (int i = 0; i < playerProperty.MaxLife; i++)
        {
            if (i < playerProperty.Life)
                lifePointGauge[i].transform.GetChild(1).gameObject.SetActive(true);
            else
                lifePointGauge[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void BeAttacked(EnemyProperty enemyProperty) // 적에게 공격을 받았을 때 메소드
    {
        if (!playerProperty.BeAttacked && !playerProperty.IsDead)
        {
            UpdateLife(playerProperty.Life - enemyProperty.ChargingDamage);
            Camera.main.transform.DOShakeRotation(0.5f, 0.45f, 30, 90);
            spriteRenderer.DOColor(new Color(255, 0, 0), 0.15f).SetEase(Ease.InOutQuad).SetLoops(6, LoopType.Yoyo);
            if (playerProperty.Life == 0)
            {
                Death();
                GameObject.Find("GameSystem").SendMessage("GameOver");
            }
            else
                StartCoroutine(KnockBack(enemyProperty));
        }
    }

    public void BeAttacked(int damage, GameObject target) // 중립 대상에게 공격을 받았을 때 메소드
    {
        if (!playerProperty.BeAttacked && !playerProperty.IsDead)
        {
            UpdateLife(playerProperty.Life - damage);
            Camera.main.transform.DOShakeRotation(0.5f, 0.45f, 30, 90);
            spriteRenderer.DOColor(new Color(255, 0, 0), 0.15f).SetEase(Ease.InOutQuad).SetLoops(6, LoopType.Yoyo);
            if (playerProperty.Life == 0)
            {
                Death();
                GameObject.Find("GameSystem").SendMessage("GameOver");
            }
            else
                StartCoroutine(KnockBack(target));
        }
    }

    public void Death() // 죽음 행동
    {
        playerProperty.IsDead = true;
        StopAllCoroutines();
        transform.DOKill();
    }

    IEnumerator KnockBack(EnemyProperty enemyProperty) // 적으로부터 뒤로 밀리는 행동(넉백) 메소드
    {
        transform.GetComponent<Rigidbody2D>().AddForce(transform.up * playerProperty.JumpSensitivity * 0.35f, ForceMode2D.Impulse);

        playerProperty.BeAttacked = true;
        if ((transform.position.x - enemyProperty.transform.position.x) > 0)
            transform.DOMoveX(transform.position.x + playerProperty.KnockBackDist, 0.15f).SetEase(Ease.InOutQuad);
        else
            transform.DOMoveX(transform.position.x - playerProperty.KnockBackDist, 0.15f).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(playerProperty.ImmortalTime);
        playerProperty.BeAttacked = false;
    }

    IEnumerator KnockBack(GameObject target) // 중립대상으로부터 뒤로 밀리는 행동(넉백) 메소드
    {
        transform.GetComponent<Rigidbody2D>().AddForce(transform.up * playerProperty.JumpSensitivity * 0.35f, ForceMode2D.Impulse);

        playerProperty.BeAttacked = true;
        if ((transform.position.x - target.transform.position.x) > 0)
            transform.DOMoveX(transform.position.x + playerProperty.KnockBackDist, 0.15f).SetEase(Ease.InOutQuad);
        else
            transform.DOMoveX(transform.position.x - playerProperty.KnockBackDist, 0.15f).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(playerProperty.ImmortalTime);
        playerProperty.BeAttacked = false;
    }
}
