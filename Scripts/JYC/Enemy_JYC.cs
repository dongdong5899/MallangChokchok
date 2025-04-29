using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using System.Collections;
using System.Collections.Generic;
using System;

public class Enemy_JYC : MonoBehaviour
{
    public Transform target;
    [SerializeField] private GameObject _bullet;

    [SerializeField]
    private float _enemySpeed = 9f;
    [SerializeField]
    private float _rotationSpeed = 9f;
    [SerializeField]
    private float _life = 10f;
    [SerializeField]
    private float _maxChaseDistance = 20f;
    [SerializeField]
    private float _pushForce = 1f;
    [SerializeField]
    private float _jumpForce = 2f;
    [SerializeField]
    private float _jumpDuration = 1f;

    [SerializeField] private float _delayTime = 0;
    private bool _isJumping = false;


    private Rigidbody _playerRb;
    private Rigidbody _myRb;
    private NavMeshAgent _navMeshAgent;
    private CapsuleCollider _cc;

    private Animator _anime;

    [SerializeField] Material dissolveMat; 
    private readonly int _dissolveHash = Shader.PropertyToID("_DissolveAmount");
    private readonly int _colorHash    = Shader.PropertyToID("_Color");

    List<int> matNums;
    List<Material> _Materials;
    SkinnedMeshRenderer[] renders;

    [SerializeField]
    private GameObject dyingEffect;

    private bool isDie;

    void Awake()
    {
        _anime = GetComponent<Animator>();
        _myRb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        renders = GetComponentsInChildren<SkinnedMeshRenderer>();
        _cc = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        MaterialSetting();
        isDie = false;
    }

    private void FixedUpdate()
    {
        EnemyFollow();
    }

    void Update()
    {
        if (isDie)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            //DissolveAction();
            Debug.Log("¾ö");
        }

        if (!isDie)
        {
            if (GameManager.Instance.fields.Count != 0)
            {
                WaterFieldEffect_Kbh nearWaterField = GameManager.Instance.fields[0];
                float minDis = Vector3.Distance(nearWaterField.transform.position, transform.position);
                foreach (WaterFieldEffect_Kbh item in GameManager.Instance.fields)
                {
                    float currentDis = Vector3.Distance(item.transform.position, transform.position);
                    if (minDis > currentDis)
                    {
                        minDis = currentDis;
                        nearWaterField = item;
                    }
                }
                if (minDis <= _maxChaseDistance)
                {
                    target = nearWaterField.transform;
                }
            }
            else
            {
                target = GameManager.Instance._move.transform;
            }

        }

        if (target != null)
        {
            _navMeshAgent.SetDestination(target.position);
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            if (distanceToPlayer > _maxChaseDistance)
            {
                _enemySpeed = 0f;
                _navMeshAgent.speed = 0;
                _myRb.velocity = Vector3.zero;
                _anime.SetBool("IsMove", false);
            }
            else
            {
                _enemySpeed = 0.5f;
                _navMeshAgent.speed = 2.5f;
                _anime.SetBool("IsMove", true);
            }
        }
    }

    private void OnCollisionEnter(Collision collsion)

    {

        if (collsion.gameObject.tag == "Player" && !_isJumping)
        {
            Vector3 pushDirection = (collsion.transform.position - transform.position).normalized;
            _playerRb = collsion.transform.GetComponent<Rigidbody>();

            if (_playerRb != null && _delayTime == 0)
            {
                _playerRb.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
                _playerRb.AddForce(Vector3.up * _pushForce, ForceMode.Impulse);
                _isJumping = true;
                _delayTime = 3f;
            }
            StartCoroutine("CancelJump");
        }
    }

    IEnumerator CancelJump()
    {
        yield return new WaitForSeconds(_jumpDuration);

        if(_playerRb != null)
        {
            _playerRb.velocity = new Vector3(_playerRb.velocity.x, 0, _playerRb.velocity.z);
            _isJumping = false;
        }
    }

    public void HPMinus(float damage)
    {
        if (isDie) return;

        _life -= damage;

        if (_life <= 0)
        {
            isDie = true;
            target = null;

            _cc.enabled = false;
            _navMeshAgent.enabled = false;
            _myRb.velocity = Vector3.zero;
            _myRb.angularVelocity = Vector3.zero;

            DissolveAction();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    void EnemyFollow()
    {
        if (target == null) return;


        Vector3 targetDirection = target.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _rotationSpeed * Time.deltaTime, 0.0f);
        //newDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(newDirection);


        if (targetDirection.magnitude > 1f)
        {
            Vector3 newPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, _enemySpeed * Time.deltaTime);
        }
    }

    private void EnemyShooting()
    {
        
    }

    private void MaterialSetting()
    {
        for (int i = 0; i < renders.Length; i++)
        {
            //matNums.Add(renders[i].materials.Length);
            for (int j = 0; j < renders[i].materials.Length; j++)
            {
                renders[i].materials[j] = dissolveMat;
                renders[i].materials[j].SetFloat(_dissolveHash, 0);
            }
        }
    }

    private void DissolveAction()
    {
        
        DOTween.Sequence()
            .AppendInterval(UnityEngine.Random.Range(0f, 0.1f))
            .AppendCallback(() => { _anime.SetTrigger("IsDie"); })
            .AppendInterval(UnityEngine.Random.Range(1.4f, 1.6f))
            .AppendCallback(() =>
            {
                VisualEffect ve = Instantiate(dyingEffect, transform.position, Quaternion.identity).GetComponent<VisualEffect>();
                //ve.transform.eulerAngles = new Vector3(-70, transform.eulerAngles.y, 0);
                ve.SetVector3("SpawnShape_transform_angles", new Vector3(-70, transform.eulerAngles.y, 0));
            })
            .AppendInterval(0.15f)
            .Append(DOTween.To(
                 () => 0,
                 Dissolve,
                 1f, 2f))
            .AppendCallback(Die);


        //for (int i = 0; i < renders.Length; i++)
        //{
        //    //matNums.Add(renders[i].materials.Length);
        //    for (int j = 0; j < renders[i].materials.Length; j++)
        //    {
        //        //renders[i].material.SetFloat(_dissolveHash, 0.5f);

        //        Debug.Log(renders[i].materials[j].name);

        //        //DOTween.Sequence()
        //        //    .Append(renders[i].material.DOColor(new Color(0.3f, 0.3f, 0.3f, 1f), 1f))
        //        //    .Append(DOTween.To(
        //        //         () => renders[i].material.GetFloat(_dissolveHash),
        //        //         x => renders[i].material.SetFloat(_dissolveHash, x),
        //        //         0f, 2f));

        //        //_Materials.Add(renders[i].materials[j]);

        //    }
        //}
        /*
            if (_delayTime != 0)
            {
                _delayTime -= Time.deltaTime;

                if (_delayTime <= 0)
                {
                    _delayTime = 0;
                }
            }*/
    }

    private void Dissolve(float x)
    {
        for (int i = 0; i < renders.Length; i++)
        {
            for (int j = 0; j < renders[i].materials.Length; j++)
            {
                renders[i].materials[j].SetFloat(_dissolveHash, x);
            }
        }
    }
}
