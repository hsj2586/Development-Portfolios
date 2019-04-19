using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowUIBackground : MonoBehaviour, IPointerDownHandler
{
    // UI창 이벤트 기능을 하는 스크립트.
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.parent.gameObject.SetActive(false);
    }
}
