using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // 전투 시에 투사체의 기능적인 부분을 담당하는 스크립트.

    Vector3 dir;
    Vector3 target_pos;
    GameObject target;
    float movespeed;
    ProjectilePoolList poolist_parent;

    public void targetSetting(GameObject obj, float movespeed, ProjectilePoolList poolist_parent)
    {
        target = obj;
        target_pos = target.transform.position + new Vector3(0, 1, 0);
        this.movespeed = movespeed;
        this.poolist_parent = poolist_parent;
    }

    public IEnumerator move()
    {
        transform.LookAt(target_pos);
        dir = (target_pos - transform.position).normalized;
        while (Vector3.Distance(transform.position, target_pos) >= 0.1f)
        {
            yield return null;
            transform.Translate(dir * movespeed, Space.World);
        }
        poolist_parent.ProjectilePoolPush(gameObject);
    }
}
