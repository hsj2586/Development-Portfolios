using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TutoControl : MonoBehaviour {

    public InputField TutorialSet;
    public void setTuto()
    {
        DataManager.tutorial.isTuto = true;
        DataManager.tutorial.Tuto_stage = Convert.ToInt32(TutorialSet.text);
        DataManager.inst().AllSave();
        Debug.Log(DataManager.tutorial.Tuto_stage);
    }

    private void Awake()
    {
        DataManager.inst().AllLoad();
        TutorialSet.text = DataManager.tutorial.Tuto_stage.ToString();
    }

}
