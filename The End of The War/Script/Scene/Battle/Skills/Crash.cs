using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crash : Skill
{
    // '기사'의 스킬 Crash

    GameObject[] enemies;
    AllyCharacter temp;
    Vector3 origin;
    Vector3 dest;
    Vector3 dir;
    float movespeed;

    void Awake()
    {
        movespeed = 0.08f;
    }

    protected override IEnumerator ActiveSkill()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length != 0)
        {
            temp = GetComponent<AllyCharacter>();
            origin = transform.position;
            dest = enemies[0].transform.position + new Vector3(-1.5f, 0, 0);
            dir = (dest - transform.position).normalized;

            SetAnimation(5); // 앞으로 전진
            yield return new WaitForSeconds(0.1f);
            while (Vector3.Distance(transform.position, dest) >= 0.1f)
            {
                yield return null;
                transform.Translate(dir * movespeed, Space.World);
            }

            SetAnimation(4); // 공격 모션
            yield return new WaitForSeconds(0.3f);
            AttackTrigger(enemies[0], temp.Access_atkpower);
            PlaySound(0);
            yield return new WaitForSeconds(0.37f);
            AttackTrigger(enemies[0], temp.Access_atkpower);
            PlaySound(0);
            yield return new WaitForSeconds(0.37f);
            AttackTrigger(enemies[0], temp.Access_atkpower);
            PlaySound(0);
            yield return new WaitForSeconds(0.37f);

            SetAnimation(5);  // 뒤로 후진
            transform.forward *= -1;
            while (Vector3.Distance(transform.position, origin) >= 0.1f)
            {
                yield return null;
                transform.Translate(-dir * movespeed, Space.World);
            }
            SetAnimation(0);
            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(InitializeSkill());
            transform.forward *= -1;
        }
    }
}
