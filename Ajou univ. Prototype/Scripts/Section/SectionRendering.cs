using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionRendering : MonoBehaviour
{
    // 실시간으로 현재 플레이어 위치에 기반한 렌더링(Active)을 결정하고 가시화해주는 스크립트.
    // 섹션, 해당 섹션 위의 오브젝트들을 렌더링.
    List<Section> tempList;
    List<Section> tempList2;
    [Header("모든 섹션 리스트")]
    [SerializeField]
    List<Section> sectionList;

    void Awake()
    {
        tempList2 = sectionList.GetRange(0, sectionList.Count);
    }

    public void Render(Section section)
    {
        tempList = section.GetLinkedSection();
        for (var i = 0; i < tempList.Count; i++)
        {
            StartCoroutine(DoRender(tempList[i].gameObject));
            tempList2.Remove(tempList[i]);
        }
        for (var i = 0; i < tempList2.Count; i++)
        {
            StartCoroutine(DonotRender(tempList2[i].gameObject));
        }
        tempList2 = sectionList.GetRange(0, sectionList.Count);
    }

    public void ObjectsRender(Section section, bool value) // 현재 섹션 안에 있는 오브젝트들을 렌더링 활성화.
    {
        foreach (var item in section.GetObjectsInSection)
        {
            item.transform.GetChild(0).gameObject.SetActive(value);
        }
    }

    IEnumerator DoRender(GameObject obj) // 섹션을 그리는 메소드
    {
        yield return new WaitForSeconds(0.2f);
        if (!obj.activeSelf)
        {
            obj.SetActive(true);
            ObjectsRender(obj.GetComponent<Section>(), true);
        }
    }

    IEnumerator DonotRender(GameObject obj) // 섹션을 그리지 않는 메소드
    {
        yield return new WaitForSeconds(0.3f);
        if (obj.activeSelf)
        {
            obj.SetActive(false);
            ObjectsRender(obj.GetComponent<Section>(), false);
        }
    }
}
