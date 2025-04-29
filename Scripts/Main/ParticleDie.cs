using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleDie : PoolableMono
{
    public override void Init()
    {

    }

    private VisualEffect ve;

    private void Awake()
    {
        ve = GetComponent<VisualEffect>();
        ve.Reinit();
    }

    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine("Die");
    }


    IEnumerator Die()
    {
        yield return new WaitForSeconds(1f);
        PoolManager.Instance.Push(gameObject.name, this);
    }
}
