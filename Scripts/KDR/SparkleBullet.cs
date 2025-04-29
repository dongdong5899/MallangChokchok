using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleBullet : PoolableMono
{
    [SerializeField]
    private float speed = 25f;

    [SerializeField]
    private GameObject boomEffect;

    [SerializeField]
    private float damage = 7;

    public override void Init()
    {
        StopCoroutine("OffsetRigid");
        StartCoroutine("OffsetRigid");
    }

    IEnumerator OffsetRigid()
    {
        Vector3 targetDir = Camera.main.transform.forward;
        Vector3 target_ = Camera.main.transform.position + targetDir * 10;
        rigidbody.velocity = (target_ - transform.position).normalized * speed;
        Debug.Log("--------------------------------------------------------");
        yield return new WaitForSeconds((target_ - transform.position).magnitude / speed);
        Debug.Log("========================================================");
        Vector3 force = targetDir.normalized * speed;
        force.y = rigidbody.velocity.y;
        rigidbody.velocity = force;
    }

    new private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Enemy"))
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, 5, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (Collider item in colls)
            {
                //Rigidbody enemyRigid = item.GetComponent<Rigidbody>();
                //enemyRigid.AddExplosionForce(200, transform.position, 50);

                item.GetComponent<Enemy_JYC>().HPMinus(damage);
            }
            Collider[] f_colls = Physics.OverlapSphere(transform.position, 5, 1 << LayerMask.NameToLayer("Floor"));
            foreach (Collider item in f_colls)
            {
                if (item.GetComponent<HitCountObject>())
                {
                    item.GetComponent<HitCountObject>().CountUP();
                }
            }
            GameObject obj = Instantiate(boomEffect, PoolManager.Instance.transform);
            obj.transform.position = transform.position;
            PoolManager.Instance.Push(gameObject.name, this);
        }
    }
}