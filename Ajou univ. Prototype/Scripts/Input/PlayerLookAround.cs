using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLookAround : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // '둘러보기' 기능을 하는 스크립트.

    GameObject player;
    Transform cam;
    Vector3 tempCamPos;
    Vector2 prevPos = Vector2.zero;
    [SerializeField]
    float dragRange;
    [SerializeField]
    public float dragSensitivity;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main.transform;
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        if (prevPos == Vector2.zero)
        {
            tempCamPos = player.transform.position + new Vector3(0, 2.625f, -1.5f); // 플레이어를 기준으로 카메라의 상대 위치
            prevPos = ped.position;
        }
        cam.GetComponent<CameraMove>().StopCamMove();

        Vector2 dir = (ped.position - prevPos);
        Vector3 vec = new Vector3(dir.x, 0, dir.y).normalized;
        Vector3 temp = cam.position - vec * dragSensitivity * Time.deltaTime;

        float x = Mathf.Clamp(temp.x, tempCamPos.x - dragRange, tempCamPos.x + dragRange);
        float z = Mathf.Clamp(temp.z, tempCamPos.z - dragRange, tempCamPos.z + dragRange);
        cam.position = new Vector3(x, cam.position.y, z);

        prevPos = ped.position;
    }
    
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }
    
    public virtual void OnPointerUp(PointerEventData ped)
    {
        prevPos = Vector2.zero;
        cam.GetComponent<CameraMove>().StartCamMove();
    }
}
