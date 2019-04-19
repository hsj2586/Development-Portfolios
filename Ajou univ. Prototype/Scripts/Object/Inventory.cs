using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Inventory : MonoBehaviour
{
    // 인벤토리 기능을 하는 스크립트.

    [SerializeField]
    List<Item> inventory = new List<Item>();

    public List<Item> GetInventory
    {
        get { return inventory; }
    }

    public void AddItem(Item item) // 아이템을 인벤토리에서 추가한다.
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == "") // 인벤토리에서 빈 공간을 찾아 아이템 push.
            {
                inventory[i] = item;
                return;
            }
        }
    }

    public void RemoveItem(int itemNumber) // 아이템을 인벤토리에서 제거한다.
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemID == itemNumber)
            {
                inventory[i] = new Item();
                return;
            }
        }
    }

    public void SwapItem(int idx, int idx2) // 인벤토리 내에서 아이템 스왑
    {
        Item temp = inventory[idx];
        inventory[idx] = inventory[idx2];
        inventory[idx2] = temp;
    }
}