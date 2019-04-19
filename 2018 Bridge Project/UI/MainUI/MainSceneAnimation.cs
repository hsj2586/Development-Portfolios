using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainSceneAnimation : MonoBehaviour
{
    Text touchText;

    private void Awake()
    {
        touchText = GetComponent<Text>();
        touchText.DOFade(1, 1.5f).SetEase(Ease.InOutQuad).SetLoops(int.MaxValue, LoopType.Yoyo);
    }
}
