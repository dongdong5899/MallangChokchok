using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundEffectParticle : PoolableMono
{
    public override void Init()
    {
        Invoke("DestroyRoutine",3f); 
    }

    public void DestroyRoutine()
    {
        PoolManager.Instance.Push("BackgroundEffectParticle", this);
    }
    
    
}