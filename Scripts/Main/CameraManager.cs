using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    public bool _zoom = false;

    [SerializeField]
    private CinemachineVirtualCamera _playerCameraVC;
    private CinemachineBasicMultiChannelPerlin _playerCameraVC_B;
    [SerializeField]
    private CinemachineVirtualCamera _zoomCameraVC;
    private CinemachineBasicMultiChannelPerlin _zoomCameraVC_B;
    [SerializeField]
    private CinemachineVirtualCamera _talkCameraVC;
    private CinemachineBasicMultiChannelPerlin _talkCameraVC_B;

    public CinemachineVirtualCamera _currentCamera;

    private Sequence _shakeSeq;

    public static CameraManager Instance;

    private void Awake()
    {
        Instance = this;
        _playerCameraVC_B = _playerCameraVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _zoomCameraVC_B = _zoomCameraVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _talkCameraVC_B = _talkCameraVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _currentCamera = _playerCameraVC;
    }


    public void ZoomView()
    {
        _zoom = true;
        CameraChange(_zoomCameraVC);
    }
    public void PlayerView()
    {
        _zoom = false;
        CameraChange(_playerCameraVC);
    }
    public void TalkView()
    {
        _zoom = false;
        CameraChange(_talkCameraVC);
    }

    public void CameraShake(float amplitude, float frequency, float time)
    {
        if (_shakeSeq != null && _shakeSeq.IsActive()) _shakeSeq.Kill();
        _shakeSeq = DOTween.Sequence();
        _shakeSeq.Append(DOTween.To(() => amplitude, value => _playerCameraVC_B.m_AmplitudeGain = value, 0, time));
        _shakeSeq.Join(DOTween.To(() => frequency, value => _playerCameraVC_B.m_FrequencyGain = value, 0, time));
        _shakeSeq.Join(DOTween.To(() => amplitude, value => _zoomCameraVC_B.m_AmplitudeGain = value, 0, time));
        _shakeSeq.Join(DOTween.To(() => frequency, value => _zoomCameraVC_B.m_FrequencyGain = value, 0, time));
    }

    private void CameraChange(CinemachineVirtualCamera changeCamera)
    {
        _currentCamera.Priority = 10;
        changeCamera.Priority = 11;
        _currentCamera = changeCamera;
    }
}
