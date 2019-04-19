using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUiEvent : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    // 인벤토리의 아이템과 UI기능을 연결 시켜주는 스크립트.
    [SerializeField]
    Inventory inventory;
    [SerializeField]
    InventoryUI inventoryUI;
    [SerializeField]
    GraphicRaycaster graphicRaycaster;
    [SerializeField]
    GameObject ItemInfo;
    PlayerInteraction player;
    GameObject interactionButton;
    int maxDragtime; // 드래그 최대 시간 변수
    int elapstime; // 드래그 시간 체크 변수
    Vector2 defaultPos; // 초기 위치 변수
    bool itemInfoWindow; // 아이템 정보 창을 띄울 것인지에 대한 변수

    void Awake()
    {
        interactionButton = GameObject.FindGameObjectWithTag("TouchCanvas").transform.Find("InteractionButton").gameObject;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        maxDragtime = 5;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (elapstime > maxDragtime)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StopAllCoroutines();
        if (elapstime != maxDragtime)
        {
            int count = 0;
            List<RaycastResult> raycastList = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, raycastList);

            for (int i = 0; i < raycastList.Count; i++) // 레이캐스팅해 Slot tag를 가진 오브젝트 카운트
            {
                if (raycastList[i].gameObject.CompareTag("Slot")) count++;
            }

            if (count != 0) // Slot tag 오브젝트가 있을 경우
            {
                for (int i = 0; i < raycastList.Count; i++)
                {
                    if (raycastList[i].gameObject.CompareTag("Slot"))
                    {
                        GameObject temp = raycastList[i].gameObject;
                        if (transform.parent.name != temp.name) // 자기 자신의 슬롯이 아니라면
                        {
                            inventory.SwapItem(int.Parse(transform.parent.name), int.Parse(temp.name));
                            transform.position = defaultPos;
                            inventoryUI.DrawInventory();
                        }
                        else // 자기 자신의 슬롯이라면
                        {
                            transform.position = defaultPos;
                        }
                        break;
                    }
                }
            }
            else // Slot tag 오브젝트가 없을 경우
            {
                transform.position = defaultPos;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        itemInfoWindow = true;
        defaultPos = transform.position;
        StartCoroutine(Dragging());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        itemInfoWindow = false;
        StopAllCoroutines();
        if (elapstime < maxDragtime)
        {
            transform.position = defaultPos;
            ItemInfoSetting();
            ItemInfo.SetActive(true);
        }
    }

    IEnumerator Dragging()
    {
        elapstime = 0;
        while (true)
        {
            yield return null;
            elapstime += 1;
            if (itemInfoWindow)
            {
                if ((transform.parent.position - transform.position).sqrMagnitude > 100)
                {
                    itemInfoWindow = false;
                }
            }
        }
    }

    void ItemInfoSetting()
    {
        Item temp = inventory.GetInventory[int.Parse(transform.parent.name)];
        ItemInfo.transform.GetChild(2).GetComponent<Image>().sprite = ResourceManager.Instance.GetItemIcon(temp.itemName); 
        ItemInfo.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = temp.itemName;
        ItemInfo.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = temp.itemDes;

        switch (temp.itemType)
        {
            case Item.ItemType.Equipment:
                ItemInfo.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "착용하기";
                ItemInfo.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => EquipItem(temp));
                ItemInfo.transform.GetChild(4).gameObject.SetActive(true);
                break;
        }
    }

    void EquipItem(Item item)
    {
        player.EquipedItem = item;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>().EquipedItem = item;
        interactionButton.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.GetItemIcon(item.itemName); 
        interactionButton.transform.GetChild(0).gameObject.SetActive(true);
        ItemInfo.SetActive(false);
    }
}
