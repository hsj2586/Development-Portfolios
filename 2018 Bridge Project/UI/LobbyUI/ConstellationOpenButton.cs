using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstellationOpenButton : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => ConstellationOpen());
    }

    public void ConstellationOpen()
    {
        if (LocalAccount.Instance.UpdateLux(-100)) // 소유 럭스 100을 소비했을 때,
        {
            string tempName = transform.parent.parent.name;
            int openSkillIndex = int.Parse(tempName[tempName.Length - 1].ToString());
            LocalAccount.Instance.ListOfConstellation[openSkillIndex].isLocked = true;
            UIUpdate(openSkillIndex);
        }
        else // 소유 럭스가 100 미만일 때
        {
            Debug.Log("소유 럭스 부족!");
        }
    }

    void UIUpdate(int openSkillIndex)
    {
        Transform tempConstellation = transform.parent.parent;
        ConstellationDictionary tempConstellation_ = LocalAccount.Instance.ListOfConstellation[openSkillIndex];
        tempConstellation.GetChild(0).GetComponent<Image>().overrideSprite =
                    Resources.Load<Sprite>("SkillImage/" + tempConstellation_.constellationName);
        tempConstellation.GetChild(1).GetChild(0).GetComponent<Text>().text =
            tempConstellation_.contellationContent;
        tempConstellation.GetChild(3).GetComponent<Text>().text =
           string.Format("Lv." + tempConstellation_.GetLevelOfSkill().ToString());
        tempConstellation.GetChild(4).GetComponent<Slider>().value =
            tempConstellation_.GetRatioExpOfLevel();
        tempConstellation.GetChild(5).GetComponent<Text>().text =
           string.Format((tempConstellation_.GetRatioExpOfLevel() * 100).ToString("N2") + "%");
        tempConstellation.GetChild(6).gameObject.SetActive(false);
    }
}
