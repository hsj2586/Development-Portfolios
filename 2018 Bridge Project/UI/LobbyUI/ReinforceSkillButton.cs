using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReinforceSkillButton : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => ReinforceSkill());
    }

    public void ReinforceSkill()
    {
        if (LocalAccount.Instance.UpdateLux(-100)) // 소유 럭스 100을 소비했을 때,
        {
            int reinforceSkillIndex = int.Parse(transform.parent.name[transform.parent.name.Length - 1].ToString());
            LocalAccount.Instance.ListOfConstellation[reinforceSkillIndex].totalSkillExp += 100;
            UIUpdate(reinforceSkillIndex);
        }
        else // 소유 럭스가 100 미만일 때,
        {
            Debug.Log("소유 럭스 부족!");
        }
    }

    void UIUpdate(int reinforceSkillIndex)
    {
        transform.parent.GetChild(3).GetComponent<Text>().text =
            string.Format("Lv." + LocalAccount.Instance.ListOfConstellation[reinforceSkillIndex].
            GetLevelOfSkill().ToString());
        transform.parent.GetChild(4).GetComponent<Slider>().value =
            LocalAccount.Instance.ListOfConstellation[reinforceSkillIndex].GetRatioExpOfLevel();
        transform.parent.GetChild(5).GetComponent<Text>().text =
            string.Format((LocalAccount.Instance.ListOfConstellation[reinforceSkillIndex].GetRatioExpOfLevel()
            * 100).ToString("N2") + "%");
    }
}
