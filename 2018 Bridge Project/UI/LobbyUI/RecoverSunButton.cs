using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecoverSunButton : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => RecoverSun());
    }

    public void RecoverSun()
    {
        if (LocalAccount.Instance.UpdateLux(-100)) // 소유 럭스 100을 소비했을 때, (경험치 10 증가)
        {
            LocalAccount.Instance.TotalExperienceOfSunGirl += 10;
            UIUpdate();
        }
        else // 소유 럭스가 100 미만일 때,
        {
            Debug.Log("소유 럭스 부족!");
        }
    }

    void UIUpdate()
    {
        transform.parent.GetChild(3).GetComponent<Text>().text =
            string.Format("태양 소녀 레벨 : " + LocalAccount.Instance.GetLevelOfSun().ToString());
        transform.parent.GetChild(4).GetComponent<Slider>().value = LocalAccount.Instance.GetRatioExpOfLevel();
        transform.parent.GetChild(4).GetChild(2).GetComponent<Text>().text =
            string.Format((LocalAccount.Instance.GetRatioExpOfLevel() * 100).ToString("N2") + "%");
    }
}
