using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour, AttackBehaviour
{
    // 원거리 공격의 동작, 애니메이션 기능을 담당하는 스크립트.

    Character temp;
    List<GameObject> projectiles; // 투사체 프리팹 불러오기용 변수
    Transform poollist; // 실제 게임에서 할당 받는 오브젝트 풀 부모 변수
    ProjectilePool projectilePool;
    GameObject missile;

    Class character_class;

    public void Setting()
    {
        projectiles = new List<GameObject>();
        projectilePool = GameObject.FindGameObjectWithTag("Temp").GetComponent<ProjectilePool>();
        character_class = GetComponent<Character>().Access_Class;
        switch (character_class)
        {
            case Class.Archer:
                projectiles.Add(Resources.Load<GameObject>("Prefabs/Arrow"));
                poollist = projectilePool.ProjectilePoolPop(projectiles);
                break;
            case Class.Wizard:
                projectiles.Add(Resources.Load<GameObject>("Prefabs/EnergyBolt"));
                poollist = projectilePool.ProjectilePoolPop(projectiles);
                break;
        }
    }

    public IEnumerator AttackAnim(GameObject target, float damage)
    {
        if (CompareTag("Ally"))
            temp = GetComponent<AllyCharacter>();
        else
            temp = GetComponent<EnemyCharacter>();

        temp.Access_animator.SetInteger("State", 2);
        yield return new WaitForSeconds(0.5f);
        missile = poollist.transform.GetChild(0).GetComponent<ProjectilePoolList>().ProjectilePoolPop(transform.position + new Vector3(0, 1, 0));
        missile.GetComponent<Projectile>().targetSetting(target, 0.15f, poollist.transform.GetChild(0).GetComponent<ProjectilePoolList>());
        temp.Access_animator.SetInteger("State", 0);
        SoundManager.Instance.PlaySound(GetComponent<Character>().Access_audioclip(0));
        yield return StartCoroutine(missile.GetComponent<Projectile>().move());

        if (gameObject.CompareTag("Ally"))
            temp.GetComponent<AllyCharacter>().AttackTrigger(target, damage);
        else
            temp.GetComponent<EnemyCharacter>().AttackTrigger(target, damage);
    }
}
