using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreManager : MonoBehaviour
{
    // 상점 창에서의 모든 기능을 담당하는 스크립트.

    [SerializeField]
    Transform buttons;
    [SerializeField]
    AudioClip main_clip;
    [SerializeField]
    AudioClip main_button_clip;
    List<EquipmentData> store_itemList; // 상점의 판매 아이템 정보
    List<Button> store_itemListButtons;
    List<EquipmentData> inventory_itemList; // 유저의 인벤토리 정보
    List<Button> inventory_itemListButtons;

    [SerializeField]
    GameObject storewindow;
    [SerializeField]
    Transform storewindow_itemlist;
    [SerializeField]
    GameObject informationwindow;
    [SerializeField]
    GameObject inventorywindow;
    [SerializeField]
    Transform inventorywindow_itemlist;
    [SerializeField]
    GameObject messagewindow;
    [SerializeField]
    Transform panel;
    [SerializeField]
    Image fade;
    Account account;

    Image colorChange_temp; // 버튼을 눌렀을 때 색변화를 위한 임시 변수
    List<EquipmentData> tempList;
    Transform infolist;
    Image temp_infoImage;

    int idx_temp; // 현재 선택한 아이템의 인덱스 변수
    int type_temp; // 현재 선택한 아이템의 타입(store or inventory) 변수

    void Awake()
    {
        SoundManager.Instance.PlayMainSound(main_clip);
        account = FileManager.AccountDataLoad("SaveFile/AccountData.txt");
        store_itemList = FileManager.ListDataLoad<EquipmentData>("SaveFile/StoreData/StoreItemList.txt"); // 상점의 정보를 load해 적용.
        inventory_itemList = FileManager.ListDataLoad<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt"); // 인벤토리 정보를 load해 적용.
        infolist = informationwindow.transform.Find("InfoList");
        temp_infoImage = informationwindow.transform.Find("Image").transform.GetChild(0).GetComponent<Image>();

        idx_temp = -1;

        ButtonSetting();
        PanelSetting();
        ItemListSetting(1);
        ItemListSetting(2);
    }

    void ButtonSetting()
    {
        store_itemListButtons = new List<Button>();
        inventory_itemListButtons = new List<Button>();
        buttons.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ExecuteCommand(0, 0)); // 로비 버튼 세팅
        buttons.GetChild(1).GetComponent<Button>().onClick.AddListener(() => ExecuteCommand(1, 0)); // 로비 버튼 세팅
        buttons.GetChild(2).GetComponent<Button>().onClick.AddListener(() => ExecuteCommand(2, 0)); // 로비 버튼 세팅
        messagewindow.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() => ExecuteCommand(3, 0)); // 경고 메세지 창 '확인' 버튼 세팅
    }

    void ExecuteCommand(int value, int type) // type 0 = 일반 버튼, type 1 = 상점 아이템 버튼, type 2 = 인벤토리 아이템 버튼
    {
        if (type == 0)
        {
            switch (value)
            {
                case 0: // 로비 버튼
                    FileManager.AccountDataGenerate("SaveFile/AccountData.txt", account);
                    StartCoroutine(Button_Anim(1));
                    break;
                case 1: // 구입 버튼
                    PurchaseItem();
                    break;
                case 2: // 판매 버튼
                    SellItem();
                    break;
                case 3: // 확인 버튼 (메세지 창)
                    messagewindow.SetActive(false);
                    break;
            }
        }
        else
            ClickButton(value, type);
    }

    void PanelSetting()
    {
        panel.GetChild(0).GetChild(0).GetComponent<Text>().text = account.Access_name;
        panel.GetChild(1).GetChild(0).GetComponent<Text>().text = account.Access_gold.ToString();
        panel.GetChild(2).GetChild(0).GetComponent<Text>().text = "LV " + account.Access_level.ToString();
    }

    void ItemListSetting(int type) // 1 = 상점 아이템 리스트, 2 = 인벤토리 아이템 리스트
    {
        Transform temp_obj;

        if (type == 1)
        {
            for (int i = 0; i < store_itemList.Count; i++)
            {
                temp_obj = storewindow_itemlist.GetChild(i);
                temp_obj.gameObject.SetActive(true);
                temp_obj.GetChild(0).GetComponent<Text>().text = store_itemList[i].equipmentname;
                temp_obj.GetChild(1).GetChild(0).GetComponent<Image>().overrideSprite =
                    Resources.Load<Sprite>("Sprite/" + store_itemList[i].equipmentname);

                store_itemListButtons.Add(temp_obj.GetComponent<Button>());
                int temp = store_itemListButtons.Count - 1;
                int temp2 = i;
                store_itemListButtons[temp].onClick.RemoveAllListeners();
                store_itemListButtons[temp].onClick.AddListener(() => ExecuteCommand(temp2, 1));
                if (store_itemList.Count >= 6)
                    storewindow_itemlist.GetComponent<RectTransform>().sizeDelta = new Vector2(425, store_itemList.Count * 90.5f + 20);
                else
                    storewindow_itemlist.GetComponent<RectTransform>().sizeDelta = new Vector2(425, 600);
            }
        }
        else
        {
            for (int i = 0; i < inventory_itemList.Count; i++)
            {
                temp_obj = inventorywindow_itemlist.GetChild(i);
                temp_obj.gameObject.SetActive(true);
                temp_obj.GetChild(0).GetComponent<Text>().text = inventory_itemList[i].equipmentname;
                temp_obj.GetChild(1).GetChild(0).GetComponent<Image>().overrideSprite =
                    Resources.Load<Sprite>("Sprite/" + inventory_itemList[i].equipmentname);

                inventory_itemListButtons.Add(temp_obj.GetComponent<Button>());
                int temp = inventory_itemListButtons.Count - 1;
                int temp2 = i;
                inventory_itemListButtons[temp].onClick.RemoveAllListeners();
                inventory_itemListButtons[temp].onClick.AddListener(() => ExecuteCommand(temp2, 2));
                if (inventory_itemList.Count >= 6)
                    inventorywindow_itemlist.GetComponent<RectTransform>().sizeDelta = new Vector2(425, inventory_itemList.Count * 90.5f + 20);
                else
                    inventorywindow_itemlist.GetComponent<RectTransform>().sizeDelta = new Vector2(425, 600);
            }
        }
    }

    void ClickButton(int idx, int type) // type 1 = 상점 아이템 버튼, type else = 인벤토리 아이템 버튼
    {
        if (colorChange_temp != null)
            colorChange_temp.color = new Color(48 / 255f, 51 / 255f, 51 / 255f, 60 / 255f);

        if (type == 1)
        {
            tempList = store_itemList;
            buttons.GetChild(1).GetComponent<Button>().gameObject.SetActive(true);
            buttons.GetChild(2).GetComponent<Button>().gameObject.SetActive(false);
            colorChange_temp = storewindow_itemlist.GetChild(idx).GetComponent<Image>();
            colorChange_temp.color = new Color(1, 1, 1, 60 / 255f);
        }
        else
        {
            tempList = inventory_itemList;
            buttons.GetChild(1).GetComponent<Button>().gameObject.SetActive(false);
            buttons.GetChild(2).GetComponent<Button>().gameObject.SetActive(true);
            colorChange_temp = inventorywindow_itemlist.GetChild(idx).GetComponent<Image>();
            colorChange_temp.color = new Color(1, 1, 1, 60 / 255f);
        }
        informationwindow.SetActive(true);
        idx_temp = idx;
        type_temp = type;
        temp_infoImage.overrideSprite = Resources.Load<Sprite>("Sprite/" + tempList[idx].equipmentname);
        infolist.GetChild(0).GetComponent<Text>().text = tempList[idx].equipmentname;
        infolist.GetChild(1).GetComponent<Text>().text = "공격력 : " + tempList[idx].atkpower.ToString();
        infolist.GetChild(2).GetComponent<Text>().text = "공격속도 : " + tempList[idx].atkspeed.ToString();
        infolist.GetChild(3).GetComponent<Text>().text = "방어력 : " + tempList[idx].defpower.ToString();
        infolist.GetChild(4).GetComponent<Text>().text = "가격 : " + tempList[idx].cost.ToString();
    }

    IEnumerator Button_Anim(int value)
    {
        float elapsTime;
        SoundManager.Instance.PlaySound(main_button_clip);
        fade.gameObject.SetActive(true);
        for (elapsTime = 0; elapsTime <= 1; elapsTime += 0.025f)
        {
            yield return null;
            fade.color = new Color(0, 0, 0, elapsTime);
        }
        SceneManager.LoadScene(value);
    }

    void PurchaseItem()
    {
        if (idx_temp != -1 && type_temp == 1 && account.Access_gold >= store_itemList[idx_temp].cost)
        {
            print(store_itemList[idx_temp].equipmentname + " 구입");
            account.Access_gold -= store_itemList[idx_temp].cost;
            inventory_itemList.Add(store_itemList[idx_temp]);
            FileManager.ListDataGenerate<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt", inventory_itemList);
            informationwindow.SetActive(false);
            buttons.GetChild(1).GetComponent<Button>().gameObject.SetActive(false);
            colorChange_temp.color = new Color(48 / 255f, 51 / 255f, 51 / 255f, 60 / 255f);
            PanelSetting();
            ItemListSetting(2);
        }
        else
        {
            print("골드가 모자릅니다.");
        }
    }

    void SellItem()
    {
        inventory_itemList = FileManager.ListDataLoad<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt"); // 인벤토리 정보를 load해 적용.
        if (idx_temp != -1 && type_temp == 2 && !inventory_itemList[idx_temp].isMounted)
        {
            print(inventory_itemList[idx_temp].equipmentname + " 판매");
            account.Access_gold += inventory_itemList[idx_temp].cost * 1 / 2;
            inventorywindow_itemlist.transform.GetChild(inventory_itemList.Count - 1).gameObject.SetActive(false);
            inventory_itemList.RemoveAt(idx_temp);
            FileManager.ListDataGenerate<EquipmentData>("SaveFile/InventoryData/InventoryItemList.txt", inventory_itemList);
            idx_temp = -1;
            informationwindow.SetActive(false);
            buttons.GetChild(2).GetComponent<Button>().gameObject.SetActive(false);
            colorChange_temp.color = new Color(48 / 255f, 51 / 255f, 51 / 255f, 60 / 255f);
            PanelSetting();
            ItemListSetting(2);
        }
        else if (inventory_itemList[idx_temp].isMounted)
        {
            messagewindow.SetActive(true);
        }
    }
}
