using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player_JYC : MonoBehaviour
{
    Rigidbody _rb;
    MeshRenderer _meshRen;
    [SerializeField] GameObject _enemyBullet;
    //HP_UI _hpui;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _meshRen = GetComponent<MeshRenderer>();
        //_hpui = FindAnyObjectByType<HP_UI>();
    }

    private void Update()
    {
        if(_meshRen.material.color == Color.red && _enemyBullet.gameObject)
        {
            _meshRen.material.DOColor(Color.gray, duration: 1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            _rb.velocity = Vector3.zero;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            _meshRen.material.DOColor(Color.red, duration: 0.5f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_meshRen.material.color == Color.red && collision.gameObject.tag == "Enemy")
        {
            _meshRen.material.DOColor(Color.gray, duration: 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_enemyBullet.gameObject)
        {
            _meshRen.material.DOColor(Color.red, duration: 0.5f);
            //_hpui.HitDamage();
            Destroy(_enemyBullet);
        }
    }
}
