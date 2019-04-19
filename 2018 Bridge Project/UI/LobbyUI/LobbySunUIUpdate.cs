using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySunUIUpdate : MonoBehaviour, UIComponent
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
            numOfLuxText.text = string.Format("{0} ({1}%)",
                LocalAccount.Instance.GetLevelOfSun(), 
                (LocalAccount.Instance.GetRatioExpOfLevel() * 100).ToString("N2"));
        }
    }
}
