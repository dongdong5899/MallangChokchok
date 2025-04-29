using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_KDR : MonoBehaviour
{
    [SerializeField]
    private float damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Enemy"))
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy_JYC>().HPMinus(damage);
            }
            if (other.CompareTag("Wall"))
            {
                other.GetComponent<HitCountObject>().CountUP();
            }
            PoolManager.Instance.Pop("HitEffect", transform.position);
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
