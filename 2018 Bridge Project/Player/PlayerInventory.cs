using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    List<string> inventory;

    void Awake()
    {
        inventory = new List<string>();
    }

    public void PushInventory(string ItemName) // 인벤토리에 아이템을 추가
    {
        if (!inventory.Contains(ItemName))
            inventory.Add(ItemName);
    }

    public void PopInventory(string ItemName) // 인벤토리에서 아이템을 제거
    {
        if (inventory.Contains(ItemName))
            inventory.Remove(ItemName);
    }

    public bool CheckItem(string ItemName) // 아이템이 있는지에 따라 boolean 반환
    {
        return inventory.Contains(ItemName) ? true : false;
    }
}
