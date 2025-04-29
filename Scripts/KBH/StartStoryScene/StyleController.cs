using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StyleController : MonoBehaviour
{
    public static StyleController Instance;
    
    private Volume _volume;

    private VolumeProfile _profile;

    private Vignette _vignette;
    private Bloom _bloom;

    [SerializeField] private TextMeshProUGUI touchToStart;
    [SerializeField] private Image whiteEffectImage;

    [SerializeField] private GameObject targetCanvas;

    public AnimationCurve endMoveTween;
    public float endMoveAmount;
    public float moveTime;
    
    private void Awake()
    {
        Instance = this;
        
        _volume = GetComponent<Volume>();
        _profile = _volume.profile;

        _profile.TryGet(out _vignette);
        _profile.TryGet(out _bloom);

    }

    public void Focus(Vector2 position)
    {
        DOTween.Sequence()
            .Append(DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, 0.5f, 0.5f))
            .Join(DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x, 3f, 0.2f))
            .Join(DOTween.To(() => _vignette.center.value, x => _vignette.center.value = x, position, 0.2f));


    }

    public void UnFocus()
    {
        DOTween.Sequence()
            .Append(DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, 0.3f, 1f))
            .Join(DOTween.To(() => _bloom.intensity.value, x => _bloom.intensity.value = x, 1f, 1f))
            .Join(DOTween.To(() => _vignette.center.value, x => _vignette.center.value = x, Vector2.one / 2, 1f));

    }


    public void EndEffect()
    {
        
        Transform targetBackgroundTrm = targetCanvas.transform.Find("DefaultBackground");

        DOTween.Sequence()
            .Append(targetBackgroundTrm
                .DOScale(Vector3.one * (targetBackgroundTrm.localScale[0] + endMoveAmount), moveTime)
                .SetEase(endMoveTween))

            .Append(DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, 0.4f, 0.4f))
            .Join(DOTween.To(() => _vignette.color.value, x => _vignette.color.value = x, Color.black, 0.4f))
            .Join(DOTween.To(() => _vignette.smoothness.value, x => _vignette.smoothness.value = x, 1f, 0.4f));

        DOTween.Sequence()
            .AppendInterval(moveTime)
            .Append(
                DOTween.Sequence()
                    .Append(touchToStart.DOColor(Color.white, 0.5f))
                    .Append(touchToStart.DOColor(Color.clear, 0.5f))
                .SetLoops(-1));

    }

    public void MoveNextScene()
    {
        Transform targetBackgroundTrm = targetCanvas.transform.Find("DefaultBackground");

        DOTween.Sequence()
            .Append(DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, 0f, 1f))
            .Join(whiteEffectImage.DOColor(Color.white, 1f))
            .Join(targetBackgroundTrm
                .DOScale(Vector3.one * 20, 1f))
            
            .AppendInterval(0.5f)
            .AppendCallback(() => SceneManager.LoadScene("JSY_StageSelectScene"));
    }
    
}
