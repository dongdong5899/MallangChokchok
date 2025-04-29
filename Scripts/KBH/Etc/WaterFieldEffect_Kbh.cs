using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaterFieldEffect_Kbh : PoolableMono
{

   Material _material;
   [SerializeField] float lifeTime = 5f;
   [SerializeField] float dissolveTime_Mul = 5f;

   readonly int _EmissionColorHash  = Shader.PropertyToID("_ColorEmission");
   readonly int _ColorPowerHash     = Shader.PropertyToID("_ColorPower");
   readonly int _ColorMulHash       = Shader.PropertyToID("_ColorMul");


   private void Awake()
   {
      _material = GetComponent<MeshRenderer>().material;
      _material.SetFloat(_ColorPowerHash, 20f);
      _material.SetFloat(_ColorMulHash, 0f);
   }

   public override void Init()
   {

      GameManager.Instance.fields.Add(this);

      DOTween.Sequence()
         .Append(DOTween.To(() => _material.GetFloat(_ColorPowerHash), value => _material.SetFloat(_ColorPowerHash, value), 2.6f, 1f))
         .Join(DOTween.To(() => _material.GetFloat(_ColorMulHash), value => _material.SetFloat(_ColorMulHash, value), 3f, 1f))
         .Join(DOTween.To(() => _material.GetColor(_EmissionColorHash), value => _material.SetColor(_EmissionColorHash, value),
            Color.white/5, 1f))

         .Append(DOTween.To(() => _material.GetFloat(_ColorPowerHash), value => _material.SetFloat(_ColorPowerHash, value), 3f, 2f))
         .Join(DOTween.To(() => _material.GetColor(_EmissionColorHash), value => _material.SetColor(_EmissionColorHash, value),
            new Color(84f / 255f, (12f*16+9) /255f, (14f*16 +14)/255f, 1), 2f));

      transform.GetChild(0).gameObject.SetActive(false);
      transform.GetChild(0).gameObject.SetActive(true);

      Invoke("SetDestroy", lifeTime);

   }


   public void SetDestroy()
   {
      DOTween.Sequence()
         .Append(DOTween.To(() => _material.GetFloat(_ColorPowerHash), value => _material.SetFloat(_ColorPowerHash, value), 20f, 1f * dissolveTime_Mul))
         .Join(DOTween.To(() => _material.GetFloat(_ColorMulHash), value => _material.SetFloat(_ColorMulHash, value), 0, 1f * dissolveTime_Mul))
         .Join(DOTween.To(() => _material.GetColor(_EmissionColorHash), value => _material.SetColor(_EmissionColorHash, value),
            Color.clear, 1f * dissolveTime_Mul));

      Transform trm = transform.GetChild(0);

      DOTween.Sequence()
         .Append(trm.DORotate(Vector3.up * 360, dissolveTime_Mul, RotateMode.FastBeyond360))
         .Join(trm.DOScale(0, dissolveTime_Mul))
         .AppendCallback(() => { trm.gameObject.SetActive(false); trm.DOScale(1, 0); });

        GameManager.Instance.fields.Remove(this);

   }



}
