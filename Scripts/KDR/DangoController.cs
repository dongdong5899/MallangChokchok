using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DangoController : PoolableMono
{
    [SerializeField]
    private float speed = 10;

    [SerializeField]
    private VisualEffect boomEffect;

    private float boomTime = 0.6f;

    new private Rigidbody[] rigidbody;

    private Vector3[] dir;

    private Material material;

    private VisualEffect visualEffect;

    private Vector3[] startPos;

    private Transform[] child;

    private int childCount = 0;

    private float currentTime;
    [SerializeField]
    private float dieTime = 2f;
    [SerializeField]
    private float splitTime = 0.4f;

    private bool isDie = false;

    public override void Init()
    {
        isDie = false;
        currentTime = 0;
        Debug.Log(transform.position);
        StopCoroutine("OffsetRigid");
        StartCoroutine("OffsetRigid");
    }

    IEnumerator OffsetRigid()
    {
        Vector3 targetDir = Camera.main.transform.forward;
        Vector3 target_ = Camera.main.transform.position + targetDir * 10;
        for (int i = 0; i < childCount; i++)
        {
            child[i].GetComponent<MeshRenderer>().enabled = true;
            child[i].GetComponent<SphereCollider>().enabled = true;
            child[i].localPosition = startPos[i];
            rigidbody[i].velocity = (target_ - transform.position).normalized * speed;
        }
        yield return new WaitForSeconds((target_ - transform.position).magnitude / speed);
        for (int i = 0; i < childCount; i++)
        {
            rigidbody[i].velocity = targetDir.normalized * speed;
        }
        Invoke("Split", splitTime);
    }

    private void Update()
    {
        if (currentTime < dieTime)
        {
            currentTime += Time.deltaTime;
        }
        else if (!isDie)
        {
            isDie = true;
            for (int i = 0; i < childCount; i++)
            {
                Debug.Log(child[i]);
                child[i].GetComponent<MeshRenderer>().enabled = false;
                child[i].GetComponent<SphereCollider>().enabled = false;
            }
            Invoke("Die", 2f);
        }
    }

    private void Die()
    {
        PoolManager.Instance.Push("DangoBullet", this);
    }

    private void Awake()
    {
        childCount = transform.childCount - 1;

        rigidbody = new Rigidbody[childCount];
        child = new Transform[childCount];
        startPos = new Vector3[childCount];
        dir = new Vector3[childCount];
        for (int i = 0; i < childCount; i++)
        {
            child[i] = transform.GetChild(i);
            startPos[i] = child[i].localPosition;
            rigidbody[i] = child[i].GetComponent<Rigidbody>();
            Vector4 vector4 = child[i].GetComponent<MeshRenderer>().material.GetColor("_Color");
            child[i].GetChild(0).GetComponent<VisualEffect>().SetVector4("Color", vector4 * 4);
        }
    }

    private void Split()
    {
        int cnt = 0;
        for (int i = 0; i < childCount; i++)
        {
            if (child[i].GetComponent<MeshRenderer>().enabled)
            {
                boomEffect.transform.position = child[i].position;
                boomEffect.Play();
                cnt++;
                break;
            }
        }
        if (cnt == 0)
        {
            return;
        }
        for (int i = 0; i < childCount; i++)
        {
            Vector3 rPos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector3 splitDir = (rigidbody[i].velocity.normalized * 6 + rPos).normalized;
            rigidbody[i].velocity = splitDir * speed;
        }
    }
}
