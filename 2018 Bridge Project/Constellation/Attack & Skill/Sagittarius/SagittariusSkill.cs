using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SagittariusSkill : Skill
{
    private ObjectPool objectPool;
    private PlayerBehaviour playerBehaviour;

    public void Init(PlayerBehaviour playerBehaviour, GameObject arrowObjectPool)
    {
        SkillImage = Resources.Load<Sprite>("SkillImage/" + GetType().Name.Substring(0, GetType().Name.Length - 5));
        GameObject.Find("SkillButton").transform.GetChild(0).GetComponent<Image>().overrideSprite = SkillImage;
        this.objectPool = arrowObjectPool.GetComponent<ObjectPool>();
        this.playerBehaviour = playerBehaviour;
    }

    // 사수자리 클래스의 스킬을 구현
    public override void DoSkill()
    {
        if (CheckSkillAvailability())
        {
            playerBehaviour.Animation(PlayerAnimation.Attack);
            objectPool.ObjectPoolPop();
            Debug.Log("사수자리의 스킬 발동!");
            StartCoroutine(SkillActivation());
        }
    }
}
