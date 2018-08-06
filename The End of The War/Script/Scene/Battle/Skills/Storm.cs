using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : Skill
{
    // '마법사'의 스킬 Storm

    GameObject[] enemies;
    GameObject particle;
    GameObject[] skilleffect_particles;
    int i;

    void Awake()
    {
        particle = Instantiate(Resources.Load<GameObject>("Prefabs/Storm"), transform);
        particle.SetActive(false);
        particle.transform.localPosition = new Vector3(0, 6, 0);
        transform.localRotation = Quaternion.Euler(45, 0, 0);
    }

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        skilleffect_particles = new GameObject[enemies.Length];
        for (i = 0; i < enemies.Length; i++)
        {
            skilleffect_particles[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Frost"), enemies[i].transform);
            skilleffect_particles[i].SetActive(false);
            skilleffect_particles[i].transform.localPosition = new Vector3(0, 0.7f, 0);
        }
    }

    protected override IEnumerator ActiveSkill()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length != 0)
        {
            particle.SetActive(true);
            SetAnimation(4);
            PlaySound(2);
            yield return new WaitForSeconds(2);
            for (i = 0; i < skilleffect_particles.Length; i++)
            {
                if (skilleffect_particles[i].transform.parent.GetComponent<Character>().Access_State != CharacterState.Dead)
                {
                    GetComponent<AllyCharacter>().AttackTrigger(skilleffect_particles[i].transform.parent.gameObject, GetComponent<AllyCharacter>().Access_atkpower * 2);
                    skilleffect_particles[i].SetActive(true);
                }
            }
            SetAnimation(0);
            yield return new WaitForSeconds(2);
            for (i = 0; i < skilleffect_particles.Length; i++)
            {
                if (skilleffect_particles[i].activeInHierarchy)
                {
                    skilleffect_particles[i].SetActive(false);
                }
            }
            particle.SetActive(false);
            yield return StartCoroutine(InitializeSkill());
        }
    }
}
