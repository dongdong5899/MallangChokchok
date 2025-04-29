using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : PoolableMono
{
    [SerializeField]
    private float dieTime = 3f;
    private float currentTime = 0;

    private bool isPool = false;

    public override void Init()
    {
        currentTime = 0;
        isPool = true;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > dieTime)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isPool) 
            PoolManager.Instance.Push(gameObject.name, this);
        else
        {
            Destroy(gameObject);
        }
    }
}
