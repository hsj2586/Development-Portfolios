using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingLabelAnimation : MonoBehaviour
{
    // 전투 시에 데미지 레이블에 대한 텍스트 애니메이션 기능을 담당하는 스크립트.

    BattleUIManager battle_uimanager;
    float elapstime;
    public Transform parent_obj;
    public float damage;

    void OnEnable()
    {
        battle_uimanager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleUIManager>();
        transform.position = (parent_obj.GetChild(0).position + new Vector3(0, 0.2f, 0));
        GetComponent<Text>().fontSize = 65;
        GetComponent<Text>().color = new Color(230 / 255f, 150 / 255f, 0, 1);
        GetComponent<Text>().text = damage.ToString("F0");
        elapstime = 0;
        StartCoroutine(FontSizing());
        StartCoroutine(FontPositioning());
    }

    IEnumerator FontSizing()
    {
        while (true)
        {
            yield return null;
            GetComponent<Text>().fontSize += 2;
            if (GetComponent<Text>().fontSize >= 85)
                break;
        }
        StartCoroutine(FontColoring());
        while (true)
        {
            yield return null;
            GetComponent<Text>().fontSize -= 2;
            if (GetComponent<Text>().fontSize <= 65)
                break;
        }
    }

    IEnumerator FontPositioning()
    {
        while (true)
        {
            yield return null;
            elapstime += 1 / 60f;
            transform.position += new Vector3(0, 1 / 90f, 0);
            if (elapstime >= 2)
            {
                battle_uimanager.Labelpool_Push(this.gameObject);
                break;
            }
        }
    }

    IEnumerator FontColoring()
    {
        while (true)
        {
            yield return null;
            GetComponent<Text>().color -= new Color(0, 0, 0, 0.025f);
            if (GetComponent<Text>().color.a == 0)
                yield break;
        }
    }
}
