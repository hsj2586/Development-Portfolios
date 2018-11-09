using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    // 플레이어의 상호작용을 기능을 담당하는 스크립트.

    [SerializeField]
    Button interactionButton;
    [SerializeField]
    Item equipedItem; // 장착중인 장비 아이템
    GameObject lookDir; // 플레이어가 바라보고 있는 방향을 가리키는 오브젝트 변수.
    Inventory inventory;
    PlayerProperty property;
    bool isAlive;

    public Item EquipedItem
    {
        get { return equipedItem; }
        set { equipedItem = value; }
    }

    void Start()
    {
        equipedItem = null;
        property = GetComponent<PlayerProperty>();
        inventory = GetComponent<Inventory>();
        lookDir = transform.Find("LookDir").gameObject;
        isAlive = true;
        interactionButton.onClick.AddListener(() => ButtonClick());
    }

    void ButtonClick()
    {
        GameObject target = FindInteractiveObject();
        if (target != null)
        {
            switch (target.tag)
            {
                case "BehavioralObject": // 행동형 오브젝트
                    target.SendMessage("BehaviorByInteraction");
                    break;
                case "ItemObject": // 아이템형 오브젝트
                    Item item = target.GetComponent<ItemObject>().GetItem();
                    inventory.AddItem(item);
                    break;
            }
        }
    }

    public void BeAttacked(GameObject enemy)
    {
        property.HealthPoint -= enemy.GetComponent<EnemyProperty>().Damage;
        property.HealthPoint = Mathf.Clamp(property.HealthPoint, 0, int.MaxValue);
        if (property.HealthPoint <= 0)
        {
            StartCoroutine(Dead());
        }
    }

    GameObject FindInteractiveObject()
    {
        Collider[] colliders = Physics.OverlapSphere(lookDir.transform.position, property.InteractionRange, 1 << 8); // 레이어가 Interactive인 오브젝트 탐색
        List<GameObject> targets = new List<GameObject>();
        int minIdx = -1;
        float minDist = float.MaxValue;

        for (int i = 0; i < colliders.Length; i++) // 그중 시야각에 들어온 오브젝트 탐색
        {
            Vector3 dir = (colliders[i].transform.position - lookDir.transform.position).normalized;
            Vector3 forward = lookDir.transform.forward.normalized;
            float angle = Mathf.Acos(dir.x * forward.x + dir.y * forward.y + dir.z * forward.z) * 180 / Mathf.PI;
            if (angle <= property.InteractionRadius)
            {
                targets.Add(colliders[i].gameObject);
            }
        }

        for (int i = 0; i < targets.Count; i++) // 그중 가장 가까운 오브젝트 탐색
        {
            float temp = (transform.position - targets[i].transform.position).sqrMagnitude;
            if ((temp <= minDist))
            {
                minIdx = i;
                minDist = temp;
            }
        }

        return minIdx == -1 ? null : targets[minIdx].gameObject;
    }

    IEnumerator Dead() // 죽음에 대한 후처리(메모리 해제, 좀비행동, 시각적인 처리 등)는 나중에 한꺼번에 고민해보기. 플레이어가 아닌 게임 매니져에서 다뤄야 할것.
    {
        if (isAlive)
        {
            isAlive = false;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Enemy>().Player = null;
            }
            transform.GetChild(0).GetComponent<Animator>().SetInteger("State", 3);
            AudioManager.Instance.StopStepSound();
            Camera.main.SendMessage("StopCamMove");
            GetComponent<PlayerInput>().enabled = false;
            yield return null;
        }
    }
}
