using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationMarkMove : MonoBehaviour
{
    // 느낌표 표시의 애니메이션 Tweening 기능을 하는 스크립트.
    void OnDisable()
    {
        iTween.Stop(gameObject);
    }

    void OnEnable()
    {
        iTween.MoveAdd(gameObject, iTween.Hash("y", 0.05f, "looptype", iTween.LoopType.pingPong, "time", 0.5f, "easetype", iTween.EaseType.easeInOutQuad));
    }
}
