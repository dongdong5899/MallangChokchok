using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Gravity
{
    public bool IsMove = false;
    public bool IsRun = false;
    public bool OnGround = true;
    public bool MoveOn = true;
    public bool GunOn = false;

    private float runAmount = 2f;

    private float _hMov = 0;
    private float _vMov = 0;
    private Vector3 _rotation;

    [SerializeField]
    private float _oriSpeed = 5f;
    private float _speed = 5f;
    [SerializeField]
    private float _oriJumpPawer = 5f;
    private float _jumpPawer = 5f;


    Vector3 _moveDir = Vector3.zero;

    public Animator _animator;
    private CameraControl _cc;
    private GameObject _rot;
    public GameObject _myRend;

    [SerializeField]
    private GameObject _gun;
    [SerializeField]
    private RuntimeAnimatorController _gunAni;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        _rot = transform.Find("Rot").gameObject;
        _myRend = transform.Find("PlayerRender").gameObject;
        _animator = _myRend.transform.Find("Rotation/Player").GetComponent<Animator>();
        _cc = transform.GetComponent<CameraControl>();
    }

    void Start()
    {
        _speed = _oriSpeed;
        _jumpPawer = _oriJumpPawer;
        _gun.SetActive(false);
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();


        if (MoveOn)
        {
            if (OnGround)
            {
                Move();
            }
        }
    }


    void Update()
    {

        if (MoveOn)
        {
            Jump();
            Rotation();

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                IsRun = !IsRun;
            }
        }
        else 
        {
            if (TalkManager.Instance.isReading)
            {
                rigidbody.velocity = Vector3.zero;

                _moveDir = TalkManager.Instance.talkTrm.position - transform.position;
                _rotation = _moveDir;

                IsMove = false;
            }
            IsRun = false;
        }
        Rotation();
        Renderer();



        _speed = _oriSpeed * (IsRun ? runAmount : 1);

    }


    private void Rotation()
    {
        if (CameraManager.Instance._zoom)
        {
            float angle = _rot.transform.eulerAngles.y;
            Vector3 rot = _myRend.transform.eulerAngles;
            float rotationSpeed = 0.1f;

            rot.y = Mathf.Lerp(angle - rot.y > 180 ? rot.y + 360 : rot.y, rot.y - angle > 180 ? angle + 360 : angle, rotationSpeed);
            //360도에서 0도, 0도에서 360도로 회전할때 반대방향으로 360도 도는걸 방지하는 코드
            //코드가 꼽다면 고쳐줘 머리 안돌아가

            _myRend.transform.eulerAngles = rot;
        }
        else
        {
            Vector3 dir = _rotation;
            float angle = (450 - Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg) % 360;
            Vector3 rot = _myRend.transform.eulerAngles;

            rot.y = Mathf.Lerp(angle - rot.y > 180 ? rot.y + 360 : rot.y, rot.y - angle > 180 ? angle + 360 : angle, 0.1f);
            //360도에서 0도, 0도에서 360도로 회전할때 반대방향으로 360도 도는걸 방지하는 코드
            //코드가 꼽다면 고쳐줘 머리 안돌아가

            _myRend.transform.eulerAngles = rot;
        }
    }

    public void OnGun()
    {
        _gun.SetActive(true);
        _animator.runtimeAnimatorController = _gunAni;
        GunOn = true;
    }

    private void Move()
    {
        _hMov = Input.GetAxisRaw("Horizontal");
        _vMov = Input.GetAxisRaw("Vertical");

        _moveDir = (_rot.transform.right * _hMov + _rot.transform.forward * _vMov).normalized;

        RaycastHit hit;
        if (Physics.CapsuleCast(transform.position + Vector3.down * 0.5f * transform.localScale.x,
            transform.position + Vector3.down * 0.5f * transform.localScale.x, 0.49f * transform.localScale.x,
            Vector3.down, out hit, 1f, 1 << LayerMask.NameToLayer("Floor")))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            if (angle != 0)
            {
                _moveDir = Vector3.ProjectOnPlane(_moveDir, hit.normal);
            }
        }

        Vector3 newVector3 = rigidbody.velocity;

        newVector3.x = _moveDir.x * _speed;
        newVector3.z = _moveDir.z * _speed;


        if (_moveDir == Vector3.zero)
        {
            if (IsMove)
            {
                IsMove = false;
                IsRun = false;
            }
        }
        else
        {
            if (!IsMove) IsMove = true;
            _rotation = _moveDir;
        }

        rigidbody.velocity = newVector3;


    }

    private void Jump()
    {
        bool cCast = Physics.CapsuleCast(transform.position + Vector3.down * 0.5f * transform.localScale.x,
            transform.position + Vector3.down * 0.5f * transform.localScale.x, 0.49f * transform.localScale.x, 
            Vector3.down, 0.2f, 1 << LayerMask.NameToLayer("Floor"));
        if (!OnGround && cCast)
        {
            OnGround = true;
            if (Input.GetMouseButton(1))
            {
                CameraManager.Instance.ZoomView();
            }
        }
        if (OnGround && !cCast)
        {
            OnGround = false;
            CameraManager.Instance.PlayerView();
        }

        Vector3 newVector3 = rigidbody.velocity;
        if (Input.GetKeyDown(KeyCode.Space) && OnGround)
        {
            newVector3.y = _jumpPawer;
        }

        rigidbody.velocity = newVector3;
    }

    private void Renderer()
    {
        _animator.SetBool("IsMove", IsMove);
        _animator.SetBool("OnGround", OnGround);
        _animator.SetBool("Zoom", CameraManager.Instance._zoom);
        _animator.SetBool("IsRun", IsRun);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Bullet"))
        Debug.Log("Sd");
    }
}
