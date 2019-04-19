using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour, BehavioralObject
{
    // TV 오브젝트 스크립트.

    bool isOff;
    GameObject player;

    public bool IsOff
    {
        get
        {
            return isOff;
        }
        set
        {
            isOff = value;
        }
    }

    private void Start()
    {
        isOff = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void BehaviorByInteraction()
    {
        if (player.GetComponent<PlayerInteraction>().EquipedItem != null && player.GetComponent<PlayerInteraction>().EquipedItem.itemName == "remoteController")
        {
            if (!isOff)
                isOff = true;
            else
                isOff = false;
        }
    }
}
