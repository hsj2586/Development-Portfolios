using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour, IObjectPoolable
{
    private ParticleSystem ps;

    public IEnumerator Dispose()
    {
        throw new System.NotImplementedException();
    }

    public void PoolObjectInit(params object[] list)
    {
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        var velocity = ps.velocityOverLifetime;

        if ((bool)list[0])
        {
            main.startRotationY = Mathf.PI;
            velocity.x = -2;
        }
        else
        {
            main.startRotationY = 0;
            velocity.x = 2;
        }
    }
}
