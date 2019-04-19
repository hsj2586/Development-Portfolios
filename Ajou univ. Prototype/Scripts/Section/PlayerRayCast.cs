using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCast : MonoBehaviour
{
    // 플레이어 기준에서 Raycast를 던지고 현재 자신이 위치한 섹션을 알아내는 기능.
    Vector3 down;
    RaycastHit hit;
    [SerializeField]
    SectionRendering renderer_;

    public Transform GetSection
    {
        get { return hit.transform.parent; }
    }

    void Awake()
    {
        down = new Vector3(0, -1, 0);
        StartCoroutine(update());
    }

    IEnumerator update()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);
        while (true)
        {
            yield return waitTime;
            if (Physics.Raycast(transform.position - down, down, out hit, 1, 1 << 9)) // hit할 경우 section 검색 성공.
            {
                renderer_.Render(hit.transform.parent.GetComponent<Section>()); // 렌더러에게 해당 섹션을 기준으로 그리라고 요청.
            }
        }
    }
}
