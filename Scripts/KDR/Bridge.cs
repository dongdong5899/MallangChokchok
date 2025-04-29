using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public void OnBrdige()
    {
        transform.GetChild(2).gameObject.SetActive(true);
    }
}
