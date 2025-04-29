using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    private GameObject enemyCanvas;
    private GameObject target;
    private void Awake()
    {
        enemyCanvas = transform.GetChild(0).gameObject;
        target = FindObjectOfType<PlayerMove>().gameObject;
    }
    private void Update()
    {
        Vector3 dir = target.transform.position - enemyCanvas.transform.position;
        dir.y = 0f;

        Quaternion rot = Quaternion.LookRotation(dir.normalized);

        enemyCanvas.transform.rotation = rot;
    }
}
