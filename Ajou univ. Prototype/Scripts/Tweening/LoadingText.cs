using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    // 로딩 텍스트 애니메이션 Tweening 기능을 하는 스크립트.
    Text text_;

    void OnDisable()
    {
        iTween.Stop(gameObject);
    }

    void Awake()
    {
        text_ = GetComponent<Text>();
        iTween.ValueTo(gameObject, iTween.Hash("from", new Color(1, 1, 1, 0), "to", new Color(1, 1, 1, 1), "onUpdate", "Change", "looptype", iTween.LoopType.pingPong, "time", 0.5f));
    }

    void Change(Color color)
    {
        text_.color = color;
    }
}
