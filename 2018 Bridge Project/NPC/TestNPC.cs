﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNPC : NPC
{
    void Update()
    {
        CheckDistanceToPlayer();
    }
}
