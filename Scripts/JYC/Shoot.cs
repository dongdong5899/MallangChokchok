using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shoot : MonoBehaviour
{
    public GameObject childObject;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shooter();
        }
    }


    void Shooter()
    {

        Vector3 raycastOrigin = transform.position;
        Vector3 raycastDirection = transform.forward;


        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit))
        {

            Debug.DrawLine(raycastOrigin, hit.point, Color.red);
            ShootPosition_JYC childShooter = childObject.GetComponent<ShootPosition_JYC>();
            if (childShooter != null)
            {
                childShooter.Fire();
            }
        }
    }
}
