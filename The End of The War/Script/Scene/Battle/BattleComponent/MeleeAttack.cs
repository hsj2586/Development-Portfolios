using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, AttackBehaviour
{
    // 근접 공격의 동작, 애니메이션 기능을 담당하는 스크립트.

    Character temp;
    Vector3 origin;
    Vector3 dest;
    Vector3 dir;
    float movespeed;

    void Awake()
    {
        movespeed = 0.08f;
    }

    public IEnumerator AttackAnim(GameObject target, float damage)
    {
        origin = transform.position;

        if (CompareTag("Ally"))
        {
            temp = GetComponent<AllyCharacter>();
            dest = target.transform.position + new Vector3(-1.5f, 0, 0);
        }
        else
        {
            temp = GetComponent<EnemyCharacter>();
            dest = target.transform.position + new Vector3(1.5f, 0, 0);
        }
        dir = (dest - transform.position).normalized;

        temp.Access_animator.SetInteger("State", 5); // 앞으로 전진
        yield return new WaitForSeconds(0.1f);
        while (Vector3.Distance(transform.position, dest) >= 0.1f)
        {
            yield return null;
            transform.Translate(dir * movespeed, Space.World);
        }
        
        temp.Access_animator.SetInteger("State", 2); // 공격 모션
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlaySound(GetComponent<Character>().Access_audioclip(0));

        if (gameObject.CompareTag("Ally"))
            temp.GetComponent<AllyCharacter>().AttackTrigger(target, damage);
        else
            temp.GetComponent<EnemyCharacter>().AttackTrigger(target, damage);

        yield return new WaitForSeconds(0.5f);
        temp.Access_animator.SetInteger("State", 5);  // 뒤로 후진
        transform.forward *= -1;
        while (Vector3.Distance(transform.position, origin) >= 0.1f)
        {
            yield return null;
            transform.Translate(-dir * movespeed, Space.World);
        }
        temp.Access_animator.SetInteger("State", 0);
        yield return new WaitForSeconds(0.2f);
        transform.forward *= -1;
    }
}
