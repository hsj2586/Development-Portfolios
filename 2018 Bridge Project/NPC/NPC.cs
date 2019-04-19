using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    GameObject player;
    [SerializeField]
    float interactionDist;
    GameSystem gameSystem;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
    }

    protected void CheckDistanceToPlayer()
    {
        float distanceToPlayer = (transform.position - player.transform.position).sqrMagnitude;
        if(distanceToPlayer < interactionDist)
        {
            gameSystem.EnableToCallNpc();
        }
        else
        {
            gameSystem.DisableToCallNpc();
        }
    }
}
