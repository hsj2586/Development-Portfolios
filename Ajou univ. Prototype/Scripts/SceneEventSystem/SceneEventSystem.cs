using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

public class SceneEventSystem : MonoBehaviour
{
    // 각 각의 SceneEvent를 순차적으로 재생시키는 기능을 하는 시스템 기능의 스크립트.
    SceneEvent currentEvent;
    List<string> eventStringList;

    #region 씬 이벤트에서 빈번하게 참조하는 변수들
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject touchCanvas;
    [SerializeField]
    GameObject dragSection;
    [SerializeField]
    GameObject dialog;
    [SerializeField]
    GameObject joystick;
    [SerializeField]
    GameObject interactionButton;
    [SerializeField]
    GameObject systemButton;
    [SerializeField]
    GameObject inventoryButton;
    [SerializeField]
    GameObject directCanvas;
    [SerializeField]
    GameObject markerCanvas;

    public GameObject Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public GameObject TouchCanvas
    {
        get
        {
            return touchCanvas;
        }

        set
        {
            touchCanvas = value;
        }
    }

    public GameObject DragSection
    {
        get
        {
            return dragSection;
        }

        set
        {
            dragSection = value;
        }
    }

    public GameObject Dialog
    {
        get
        {
            return dialog;
        }

        set
        {
            dialog = value;
        }
    }

    public GameObject Joystick
    {
        get
        {
            return joystick;
        }

        set
        {
            joystick = value;
        }
    }

    public GameObject InteractionButton
    {
        get
        {
            return interactionButton;
        }

        set
        {
            interactionButton = value;
        }
    }

    public GameObject SystemButton
    {
        get
        {
            return systemButton;
        }

        set
        {
            systemButton = value;
        }
    }

    public GameObject InventoryButton
    {
        get
        {
            return inventoryButton;
        }

        set
        {
            inventoryButton = value;
        }
    }

    public GameObject DirectCanvas
    {
        get
        {
            return directCanvas;
        }

        set
        {
            directCanvas = value;
        }
    }

    public GameObject MarkerCanvas
    {
        get
        {
            return markerCanvas;
        }

        set
        {
            markerCanvas = value;
        }
    }
    #endregion

    void Start()
    {
        eventStringList = new List<string>();
        //eventStringList = FileManager.ListDataLoad<string>("추후에 씬 이름 리스트 json파일 경로넣기.");
        eventStringList.Add("SceneEvent1");
        eventStringList.Add("SceneEvent2");
        eventStringList.Add("SceneEvent3");
        eventStringList.Add("SceneEvent4");
        eventStringList.Add("SceneEvent5");
        eventStringList.Add("SceneEvent6");
        StartCoroutine(update());
    }

    IEnumerator update()
    {
        foreach (var sceneEvent in eventStringList)
        {
            Type temp = Type.GetType(sceneEvent);
            gameObject.AddComponent(temp);
            currentEvent = gameObject.GetComponent<SceneEvent>();
            currentEvent.enabled = true;
            yield return StartCoroutine(currentEvent.Init()); // 이벤트 초기화
            yield return StartCoroutine(currentEvent.Execute()); // 이벤트 실행
            yield return StartCoroutine(currentEvent.Restore()); // 초기화 복원
            DestroyImmediate(currentEvent);
        }
        print("씬이벤트 종료");
    }
}
