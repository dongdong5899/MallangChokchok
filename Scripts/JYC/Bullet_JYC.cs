using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet_JYC : PoolableMono
{
    [SerializeField]
    private float damage = 1;

    private Vector3 moveDir;
    private SphereCollider sphereCollider;

    public override void Init()
    {
        timer = 0;
        StopCoroutine("OffsetRigid");
        StartCoroutine("OffsetRigid");
    }

    public bool isDead = true;
    private bool isHit = false;

    public float _speed = 10f;

    public float lifeTime = 2f;

    private float timer = 0f;

    private Rigidbody _rigid;


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    IEnumerator OffsetRigid()
    {
        Vector3 targetDir = Camera.main.transform.forward;
        Vector3 target_ = Camera.main.transform.position + targetDir * 5;
        moveDir = (target_ - transform.position).normalized;
        yield return new WaitForSeconds((target_ - transform.position).magnitude / _speed);
        transform.position = target_;
        moveDir = targetDir.normalized;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {

        // 총알의 수명이 지나면 파괴
        if (isDead)
        {
            timer += Time.deltaTime;
            if (timer >= lifeTime)
            {
                PoolManager.Instance.Push(gameObject.name, this);
            }
        }
    }

    private void Move()
    {
        RaycastHit raycastHit;
        if (Physics.SphereCast(transform.position + sphereCollider.center, sphereCollider.radius, moveDir, out raycastHit, _speed * Time.deltaTime, 1 << LayerMask.NameToLayer("Floor")))
        {
            Debug.Log("야발");
            transform.position += moveDir * raycastHit.distance;
            Hit(raycastHit);
        }
        else
        {
            transform.position += moveDir * _speed * Time.deltaTime;
        }


    }

    private void Hit(RaycastHit raycastHit)
    {
        if (raycastHit.transform.CompareTag("Enemy"))
        {
            raycastHit.transform.GetComponent<Enemy_JYC>().HPMinus(damage);
        }
        if (raycastHit.transform.CompareTag("Wall") && raycastHit.transform.GetComponent<HitCountObject>())
        {
            raycastHit.transform.GetComponent<HitCountObject>().CountUP();
        }
        PoolManager.Instance.Pop("HitEffect", transform.position);
        PoolManager.Instance.Push(gameObject.name, this);
    }
}
