using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    // '섹션'이라는 개념을 사용한 스크립트.
    // 각 하나의 공간을 섹션으로 잡아 플레이어를 기준으로 시야 처리를 결정하고,
    // 적과의 상호작용에서도 사용할 수 있도록 구현.

    [Header("인접 섹션 리스트")]
    [SerializeField]
    private List<Section> sectionList;
    [Header("인접 섹션 개방성")]
    [SerializeField]
    private List<bool> sectionBoolList;
    [SerializeField]
    private List<GameObject> objectsInSection; // 해당 섹션에 있는 플레이어 or 적 오브젝트들.
    private List<Section> tempList;

    public List<GameObject> GetObjectsInSection
    {
        get
        {
            return objectsInSection;
        }
    }

    void Awake()
    {
        objectsInSection = new List<GameObject>();
        tempList = new List<Section>();
    }

    public void changeBoolList(Section sec, bool isOpen) // 섹션이 가지고 있는 개방성 정보를 변경.
    {
        int i;
        for (i = 0; i < sectionList.Count; i++)
        {
            if (sectionList[i] == sec)
                break;
        }
        sectionBoolList[i] = isOpen;
    }

    public List<Section> GetLinkedSection() // 자신의 섹션을 기준으로 개방된 섹션들의 리스트를 반환.
    {
        tempList.Clear();
        tempList.Add(this); // 자신까지 포함
        for (var i = 0; i < sectionList.Count; i++)
        {
            if (sectionBoolList[i])
            {
                tempList.Add(sectionList[i]);
            }
        }
        return tempList;
    }

    public void AddObjectInSection(GameObject obj)
    {
        if (!objectsInSection.Contains(obj))
            objectsInSection.Add(obj);
    }

    public void RemoveObjectInSection(GameObject obj)
    {
        if (objectsInSection.Contains(obj))
            objectsInSection.Remove(obj);
    }
}
