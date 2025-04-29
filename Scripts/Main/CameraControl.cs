using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Vector2 _mouseSpeed = new Vector2(5f, 3f);

    private float _xMouse = 0;
    private float _yMouse = 0;

    private float _mouseWheel = 0;
    private GameObject _rot;
    private GameObject _cameraRot;
    private Transform _playerCamera;
    private Transform _zoomCamera;
    private CinemachineVirtualCamera _playerCameraVC;
    private CinemachineBasicMultiChannelPerlin _playerCameraVC_B;
    private CinemachineVirtualCamera _zoomCameraVC;
    private CinemachineBasicMultiChannelPerlin _zoomCameraVC_B;
    [SerializeField]
    private Transform _chastBone;

    private Sequence _shakeSeq;

    public bool onRotation = true;

    private void Awake()
    {
        _rot = transform.Find("Rot").gameObject;
        _cameraRot = _rot.transform.Find("CameraRot").gameObject;
        _playerCamera = _cameraRot.transform.Find("PlayerCam");
        _zoomCamera = _cameraRot.transform.Find("ZoomCam");
        _playerCameraVC = _playerCamera.GetComponent<CinemachineVirtualCamera>();
        _zoomCameraVC = _zoomCamera.GetComponent<CinemachineVirtualCamera>();
        _playerCameraVC_B = _playerCameraVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _zoomCameraVC_B = _zoomCameraVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        Zoom();
    }

    private void LateUpdate()
    {
        if (!TalkManager.Instance.isReading && onRotation)
            Rotation();
    }

    private void Rotation()
    {
        _xMouse = Input.GetAxis("Mouse X");
        _yMouse = Input.GetAxis("Mouse Y");

        float rotX = (_cameraRot.transform.eulerAngles.x - _yMouse * _mouseSpeed[0]);
        float rotY = (_rot.transform.eulerAngles.y + _xMouse * _mouseSpeed[1]);
        rotX = Mathf.Clamp(rotX > 180 ? rotX - 360 : rotX, -90f, 45f);

        _rot.transform.eulerAngles = new Vector3(0, rotY, 0);

        if (CameraManager.Instance._zoom)
        {
            Vector3 rott = _chastBone.transform.eulerAngles;
            rott.x = rotX;
            _chastBone.transform.eulerAngles = rott;
        }
        _cameraRot.transform.eulerAngles = new Vector3(rotX, rotY, 0);
    }

    private void Zoom()
    {
        _mouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");
        _playerCamera.localPosition += Vector3.forward * _mouseWheel;

        if (Input.GetMouseButtonDown(1))
        {
            CameraManager.Instance.ZoomView();
            GameManager.Instance._move.IsRun = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            CameraManager.Instance.PlayerView();
        }
    }
}
