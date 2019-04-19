using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyLuxUIUpdate : MonoBehaviour, UIComponent
{
    Text numOfLuxText;

    void Awake()
    {
        numOfLuxText = GetComponent<Text>();
    }

    public void UpdateUI()
    {
        if (gameObject.activeInHierarchy.Equals(true))
        {
            //numOfLuxText.text = string.Format("남은 럭스 : {0}  태양 레벨 : {1} ( {2}% )",
            //                LocalAccount.Instance.NumOfLux.ToString(), LocalAccount.Instance.GetLevelOfSun(),
            //                (LocalAccount.Instance.GetRatioExpOfLevel() * 100).ToString("N2"));
            numOfLuxText.text = string.Format("{0}", LocalAccount.Instance.NumOfLux.ToString());
        }
    }
}
