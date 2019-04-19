using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloor : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerProperty playerProperty;

    void Awake()
    {
        playerInput = transform.parent.GetComponent<PlayerInput>();
        playerProperty = transform.parent.GetComponent<PlayerProperty>();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            playerProperty.IsJump = false;
            playerInput.JumpCount = 0;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        playerProperty.IsJump = true;
        playerInput.JumpCount = 0;
    }
}
