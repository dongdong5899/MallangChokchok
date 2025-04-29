using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWaterBallSpown_Kbh : MonoBehaviour
{

   [SerializeField] Vector3 startDir;
   [SerializeField] float speed;

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.K))
      {
         WaterBall waterball = PoolManager.Instance.Pop_NotInit("WaterBall", transform.position) as WaterBall;
         waterball.startDir = startDir;
         waterball.startSpeed = speed;
         waterball.Init();

      }

      if (Input.GetKeyDown(KeyCode.L))
      {
         GriceBall gricBall = PoolManager.Instance.Pop_NotInit("GRiceBall", transform.position) as GriceBall;
         gricBall.startDir = startDir;
         gricBall.startSpeed = speed;
         gricBall.Init();

      }

   }

}
