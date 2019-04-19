using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // UI들에게 메시지를 보내 UI를 갱신하도록 요청.
    [SerializeField]
    List<GameObject> UIlist;

    // Update is called once per frame
    void Update()
    {
        foreach (var UIComponent in UIlist)
        {
            UIComponent.GetComponent<UIComponent>().UpdateUI();
        }
    }
}
