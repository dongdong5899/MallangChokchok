using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangoPool : PoolableMono
{
    private float currentTime;
    [SerializeField]
    private float dieTime = 2f;

    public override void Init()
    {
        currentTime = 0;
    }

    private void Update()
    {
        if (currentTime < dieTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            PoolManager.Instance.Push("DangoBullet", this);
        }
    }

    //안민수 *전3성 메리스 보유자
}
