using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSlider : MonoBehaviour
{
    bool pointerClick;
    Slider slider;

    void Awake()
    {
        pointerClick = false;
        slider = GetComponent<Slider>();
    }

    public float GetSliderValue()
    {
        if (pointerClick)
            return (slider.value - 0.5f) * 2;
        else
        {
            slider.value = 0.5f;
            return 0;
        }
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }

    public void PointerUp()
    {
        pointerClick = false;
    }

    public void PointerDown()
    {
        pointerClick = true;
    }
}
