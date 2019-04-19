using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ConstellationManager : MonoBehaviour
{
    [SerializeField]
    Constellation constellation; // 별자리 클래스
    Text testText; // 별자리 이름 표시용 테스트 변수
    List<string> constellationList;
    int constellationIdx; // 별자리 리스트 인덱스
    PlayerProperty playerProperty;

    void Start()
    {
        playerProperty = GetComponent<PlayerProperty>();
        testText = GameObject.Find("ChangeClassButton").transform.GetChild(1).GetComponent<Text>();
        constellationList = playerProperty.ConstellationList;
        constellationIdx = 0;
        gameObject.AddComponent(Type.GetType(constellationList[constellationIdx]));
        constellation = GetComponent<Constellation>();
        playerProperty.Constellation = constellation;
        TestText();
    }


    public void ChangeConstellation() // 별자리 변경 콜백
    {
        playerProperty.Constellation = constellation;
        constellationIdx++;
        int nextIdx = constellationIdx % constellationList.Count;
        Destroy(GetComponent<Constellation>());
        Destroy(GetComponent<Skill>());
        Type type = Type.GetType(constellationList[nextIdx]);
        gameObject.AddComponent(type);
        constellation = GetComponent(type) as Constellation;
        playerProperty.Constellation = constellation;
        TestText();
    }

    void TestText() // 테스트용 텍스트 표시 코드
    {
        switch (constellation.GetType().Name)
        {
            case "Aries":
                testText.text = string.Format("현재 클래스 : 양자리");
                break;
		    case "Sagittarius":
				testText.text = string.Format ("현재 클래스 : 사수 자리");
				break; 
        }
    }
}
