using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameObject player;
    [SerializeField]
    float speedSensitivity;

    void Awake()
    {
        player = GameObject.Find("PlayerTest");
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(0, 0, -20), Time.deltaTime * speedSensitivity);
    }
}
