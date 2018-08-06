using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarAnimation : MonoBehaviour
{
    // hp바의 애니메이션 기능을 담당하는 스크립트.
    Slider hpslider;
    Slider hpslider2;
    Slider attackslider;

    public float AccessAttacksliderValue
    {
        get
        {
            return attackslider.value;
        }
        set
        {
            attackslider.value = value;
        }
    }

    public float AccessHpsliderValue
    {
        get
        {
            return hpslider.value;
        }
        set
        {
            hpslider.value = value;
        }
    }

    public float AccessHpslider2Value
    {
        get
        {
            return hpslider2.value;
        }
        set
        {
            hpslider2.value = value;
        }
    }

    public void Init(Slider sl1, Slider sl2, Slider sl3)
    {
        this.hpslider = sl1;
        this.hpslider2 = sl2;
        this.attackslider = sl3;
        hpslider.value = 1;
        hpslider2.value = 1;
        attackslider.value = 0;
    }  // 초기화

    public IEnumerator HpSliderAnimation(float hpratio)
    {
        yield return null;
        hpslider.value = hpratio;
        while (hpslider2.value >= hpratio)
        {
            yield return null;
            hpslider2.value -= 0.003f;
        }
    } // hp바가 깎이는 애니메이션 코루틴
}
