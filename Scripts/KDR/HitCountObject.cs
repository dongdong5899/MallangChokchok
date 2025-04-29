using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitCountObject : MonoBehaviour
{
    public int maxCount;
    private int currentCount;

    public UnityEvent outEvent;

    private bool on = false;

    public void InEvent()
    {
        on = true;
    }

    public void CountUP()
    {
        Debug.Log("¸ÂÀ½!");
        if (on)
        {
            currentCount++;
            if (currentCount == maxCount)
            {
                outEvent?.Invoke();
                on = false;
            }
        }
    }
}
