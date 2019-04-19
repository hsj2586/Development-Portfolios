using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject2 : MonoBehaviour, IObjectPoolable
{
    public IEnumerator Dispose()
    {
        throw new System.NotImplementedException();
    }

    public void PoolObjectInit(params object[] list)
    {
        float temp;
        transform.position = (Vector3)list[1] + new Vector3(0,-1,0);

        if ((bool)list[0])
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            temp = 180;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            temp = 0;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            var main = transform.GetChild(i).GetComponent<ParticleSystem>().main;
            main.startRotationY = temp;
        }
    }
}
