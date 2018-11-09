using System.Collections;
using UnityEngine;

public class SceneEvent2 : SceneEvent
{
    // 두 번째 씬 이벤트 스크립트.
    GameObject camMarkerPos; // 카메라 마커 투영 오브젝트
    GameObject remoteController; // 리모콘 오브젝트

    public override IEnumerator Init()
    {
        yield return null;
        remoteController = GameObject.Find("RemoteController");
        yield return StartCoroutine(TurnOnMarker(remoteController));
        sceneEventSystem.Player.GetComponent<PlayerInput>().enabled = true;
        sceneEventSystem.TouchCanvas.SetActive(true);
    }

    public override IEnumerator Execute()
    {
        while (remoteController)
        {
            yield return null;
        }
    }

    public override IEnumerator Restore()
    {
        yield return null;
        sceneEventSystem.MarkerCanvas.SetActive(false);
        Camera.main.transform.GetChild(0).gameObject.SetActive(false);
    }
}
