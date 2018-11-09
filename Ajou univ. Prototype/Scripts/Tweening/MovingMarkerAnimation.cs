using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMarkerAnimation : MonoBehaviour
{
    // 마커의 애니메이션 Tweening 기능을 하는 스크립트.
    RectTransform transform_;
    [SerializeField]
    float amplifyX;
    [SerializeField]
    float amplifyY;

    void OnDisable()
    {
        iTween.Stop(gameObject);
    }

    void OnEnable()
    {
        transform_ = GetComponent<RectTransform>();
        iTween.ValueTo(gameObject, iTween.Hash("from", new Vector2(amplifyX * -1, amplifyY * -1)
            , "to", new Vector2(amplifyX * 1, amplifyY * 1)
            , "looptype", iTween.LoopType.pingPong, "onUpdate", "Floating", "time", 0.4f, "easetype", iTween.EaseType.easeInOutQuad));
    }

    void Floating(Vector2 param)
    {
        transform_.anchoredPosition = param;
    }
}
