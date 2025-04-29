using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoomTest_Kbh : MonoBehaviour
{
   Material _mat;
   readonly int _matTimeScale = Shader.PropertyToID("_TimeScale");
   readonly int _powerValue = Shader.PropertyToID("_PowerValue");
   readonly int _colorPower = Shader.PropertyToID("_ColorPower");
   readonly int _MatCurrentTime = Shader.PropertyToID("_CurrentTime");

   public AnimationCurve _currentTimeCurve;

   private void Awake()
   {
      _mat = GetComponent<MeshRenderer>().material;
      BoomTest();
      
   }

   public void Update()
   {
      if (Input.GetKeyDown(KeyCode.Q))
      {
         BoomTest();
      }
   }
      
   public void BoomTest()
   {
      DOTween.Sequence()

         .Append(DOTween.To(() => _mat.GetFloat(_MatCurrentTime), x => _mat.SetFloat(_MatCurrentTime, x), 5, 0.4f)).SetEase(_currentTimeCurve)
         .Join(DOTween.To(() => _mat.GetFloat(_matTimeScale), x => _mat.SetFloat(_matTimeScale, x), 30, 0.4f)).SetEase(_currentTimeCurve)
         .Join(DOTween.To(() => _mat.GetFloat(_powerValue), x => _mat.SetFloat(_powerValue, x), 2, 0.4f)).SetEase(_currentTimeCurve)
         .Insert(0, transform.DOScale(5, 2.5f)).SetEase(Ease.InCirc)

         .Append(DOTween.To(() => _mat.GetFloat(_MatCurrentTime), x => _mat.SetFloat(_MatCurrentTime, x), 1, 0.7f)).SetEase(_currentTimeCurve)
         .Join(DOTween.To(() => _mat.GetFloat(_matTimeScale), x => _mat.SetFloat(_matTimeScale, x), 5, 0.7f)).SetEase(_currentTimeCurve)
         .Join(DOTween.To(() => _mat.GetFloat(_powerValue), x => _mat.SetFloat(_powerValue, x), 6, 0.7f)).SetEase(_currentTimeCurve)

         .Append(transform.DOScale(2, 2.5f));

   }
}
