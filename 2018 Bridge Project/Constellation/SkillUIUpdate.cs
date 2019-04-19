using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIUpdate : MonoBehaviour
{
    [SerializeField]
    GameObject cooltimeImage;
    [SerializeField]
    Text coolTimeText;
    Image tempImage;
    Image skillImage;

    public void UIOn()
    {
        tempImage = cooltimeImage.GetComponent<Image>();
        tempImage.fillAmount = 1;
        cooltimeImage.SetActive(true);
    }

    public void UIOff()
    {
        cooltimeImage.SetActive(false);
    }

    public void UIUpdate(float totalCooltime, float remainCooltime)
    {
        float cooltimeRatio = (remainCooltime / totalCooltime);
        coolTimeText.text = ((int)remainCooltime).ToString();
        tempImage.fillAmount = cooltimeRatio;
    }
}
