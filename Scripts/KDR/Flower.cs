using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Flower : MonoBehaviour
{
    [SerializeField]
    private bool startBloom = true;
    [SerializeField]
    private float bloomDelay = 5;
    private float currentBloomDelay = -1;

    [SerializeField]
    [ColorUsage(false, true)]
    private Color color;

    [SerializeField]
    private Material flowerMat;
    [SerializeField]
    private Material inMat;

    [SerializeField]
    private bulletType type;

    private SelectBullet _sb;
    private Gun_UI _gu;
    [SerializeField]
    private int bulletCount = 0;

    private MeshRenderer meshRenderer;
    private VisualEffect eventEffect;
    private SphereCollider sphereCollider;

    public bool isbloom = false;

    private void Awake()
    {
        _sb = FindObjectOfType<SelectBullet>();
        _gu = FindObjectOfType<Gun_UI>();
        eventEffect = transform.Find("EventEffect").GetComponent<VisualEffect>();
        sphereCollider = transform.GetChild(2).GetComponent<SphereCollider>();
        if (startBloom) Bloom();
    }

    private void Update()
    {
        if (startBloom)
        {
            if (currentBloomDelay > 0)
            {
                currentBloomDelay -= Time.deltaTime;
            }
            else if (currentBloomDelay != -1)
            {
                currentBloomDelay = -1;
                Bloom();
            }
        }
    }

    public void Harvest()
    {
        isbloom = false;
        meshRenderer.materials[1].color = new Color32(161, 161, 161, 255);
        transform.GetChild(0).gameObject.SetActive(false);
        eventEffect.Play();
        currentBloomDelay = bloomDelay;
        sphereCollider.enabled = false;
        GameManager.Instance._move.GetComponent<InteractionUI>().On();
        AddBullet();
    }

    public void Bloom()
    {
        isbloom = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = inMat;
        meshRenderer = transform.GetChild(1).GetComponent<MeshRenderer>();
        transform.GetChild(0).gameObject.SetActive(true);
        meshRenderer.materials[1] = flowerMat;
        meshRenderer.materials[1].color = color;
        sphereCollider.enabled = true;
    }

    private void AddBullet()
    {
        int num = 0;
        switch (type)
        {
            case bulletType.GRice_Bullet:
                num = 0;
                break;
            case bulletType.Water_Bullet:
                num = 1;
                break;
            case bulletType.Dango_Bullet:
                num = 2;
                break;
            case bulletType.Sparkle_Bullet:
                num = 3;
                break;
        }
        _sb.totalBullet[num] += bulletCount;
        if (_sb.bulletNumber == num)
            _gu.UpdateTotalBullet(num);

    }


}
