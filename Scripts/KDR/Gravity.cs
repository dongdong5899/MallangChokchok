using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    new protected Rigidbody rigidbody;
    protected float defaultGravity = 9.8f;
    private float currentGravity = 9.8f;
    protected float groundPower = 1;
    private float interval = 0.01f;
    private float force;

    protected void SetGravity(float value)
    {
        currentGravity = value;
    }

    protected void FixedUpdate()
    {
        force = currentGravity * rigidbody.mass * groundPower;
        Debug.Log(force);
        rigidbody.AddForce(Vector3.down * force, ForceMode.Force);
        if (Physics.CapsuleCast(transform.position + Vector3.down * 0.49f * transform.localScale.x,
            transform.position + Vector3.down * 0.49f * transform.localScale.x, 0.5f * transform.localScale.x, 
            Vector3.down, interval, 1 << LayerMask.NameToLayer("Floor")))
        {
            //rigidbody.AddForce(Vector3.up * force, ForceMode.Force);
        }
    }
}
