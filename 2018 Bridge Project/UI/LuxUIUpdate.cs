using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LuxUIUpdate : MonoBehaviour, UIComponent
{
    PlayerProperty playerProperty;
    Text numOfLuxText;

    void Awake()
    {
        playerProperty = GameObject.Find("PlayerTest").GetComponent<PlayerProperty>();
        numOfLuxText = GetComponent<Text>();
    }

    public void UpdateUI()
    {
        numOfLuxText.text = playerProperty.Lux.ToString();
        numOfLuxText.fontSize = 65; //럭스 획득 텍스트 애니메이션
        numOfLuxText.transform.DOScale(1.4f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
}
