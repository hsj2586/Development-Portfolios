using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
    // 아이템 정보를 파일화 하기 위한 클래스.
    public string itemName;
    public int itemID;
    public string itemDes;      // 아이템의 설명
    
    public ItemType itemType;   //아이템의 속성 설정


    public enum ItemType
    {
        Equipment,  // 착용 아이템류
        Costume,    // 방어구류
        Quest,      // 퀘스트 아이템류
        Use         // 소모품류
    }
    
    public Item()
    {
    }

    public Item(string itemName, int itemID, string itemDes, ItemType itemType)
    {
        this.itemName = itemName;
        this.itemID = itemID;
        this.itemDes = itemDes;        
        this.itemType = itemType;
    }
}
