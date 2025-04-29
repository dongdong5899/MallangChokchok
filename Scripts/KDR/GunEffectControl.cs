using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunEffectControl : PoolableMono
{
    public override void Init()
    {

    }

    private float currentTime = 0;
    private float splashDieTime = 0.05f;
    private float dieTime = 1f;

    private GameObject gunEffectSplash;
    private VisualEffect visualEffect;


    private void Awake()
    {
        gunEffectSplash = transform.Find("GunEffectSplash").gameObject;
        visualEffect = transform.Find("Visual Effect").GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        gunEffectSplash.SetActive(true);
        currentTime = 0;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > splashDieTime)
        {
            gunEffectSplash.SetActive(false);
        }

        if (currentTime > dieTime)
        {
            PoolManager.Instance.Push("DefaultGunEffect", this);
        }
    }

    
}
