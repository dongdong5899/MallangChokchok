using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript_Kbh : MonoBehaviour
{

   [SerializeField] WaterBall _waterBallA;
   

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.W))
         _waterBallA.Init();

      if(Input.GetKeyDown(KeyCode.Q))
         _waterBallA.gameObject.SetActive(!_waterBallA.gameObject.activeSelf);


   }





}
