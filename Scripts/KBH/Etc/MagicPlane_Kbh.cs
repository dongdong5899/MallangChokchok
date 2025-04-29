using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagicPlane_Kbh : PoolableMono
{

   Material _material;
   readonly int _MapAreaHash       = Shader.PropertyToID("_MapArea");

   private void OnEnable()
   {
      _material = GetComponent<MeshRenderer>().material;
      Init();
   }

   public override void Init()
   {

      //DOTween.Sequence()
      //   .Append(DOTween.To(() => _material.GetFloat(_ColorPowerHash), value => _material.SetFloat(_ColorPowerHash, value), 2.6f, 1f))
      //   .Join(DOTween.To(() => _material.GetFloat(_ColorMulHash), value => _material.SetFloat(_ColorMulHash, value), 3f, 1f))
      //   .Join(DOTween.To(() => _material.GetColor(_EmissionColorHash), value => _material.SetColor(_EmissionColorHash, value),
      //      Color.white / 5, 1f))

      //   .Append(DOTween.To(() => _material.GetFloat(_ColorPowerHash), value => _material.SetFloat(_ColorPowerHash, value), 3f, 2f))
      //   .Join(DOTween.To(() => _material.GetColor(_EmissionColorHash), value => _material.SetColor(_EmissionColorHash, value),
      //      new Color(84f / 255f, (12f * 16 + 9) / 255f, (14f * 16 + 14) / 255f, 1), 2f));

      DOTween.To(() => _material.GetVector(_MapAreaHash), value => _material.SetVector(_MapAreaHash, value), new Vector4(0.31f, 0.4f), 1f);

   }

   private void OnDisable()
   {
      _material.SetVector(_MapAreaHash, new Vector4());
   }

}
