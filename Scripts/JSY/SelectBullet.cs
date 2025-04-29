using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectBullet : MonoBehaviour
{
    [SerializeField] private Image[] bullet; // 탄 종류
    [SerializeField] public int[] totalBullet;

    public bool isChange;
    public bool isShooting; // 연사... 영어로 어케쓰는지 몰라...
    public float bulletSwapTime;
    public int bulletNumber;

    /*private void Awake()
    {
        totalBullet = new int[5];
    }*/

    private void Start()
    {
        ChangeBulletImage(4); // 시작할 때 1번 탄 선택.
        isShooting = true;
    }
    void Update()
    {
        Bullet_Select();
    }

    void Bullet_Select() // 탄 교체 함수.
    {
        if(!isChange) // 탄 교체할 때 다른 탄으로 교체하는 것을 방지.
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeBulletImage(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeBulletImage(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeBulletImage(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangeBulletImage(3);
            }
        }
    }

    void ChangeBulletImage(int num) // UI 이미지로 어떤 탄을 선택하였는지 보이게끔 함.
    {
        if (num == bulletNumber)
        {
            for (int i = 0; i < bullet.Length; i++)
            {
                bullet[i].color = new Color(1f, 1f, 1f, 1f);
            }
            isShooting= true;
            bulletNumber = 4;
            StartCoroutine(GetComponent<Gun_UI>().Reloading(bulletSwapTime, bulletNumber, false));
            return;
        }
        isChange = true;
        for (int i = 0; i < bullet.Length; i++)
        {
            bullet[i].color = new Color(1f, 1f, 1f, 1f);
        }
        bulletNumber = num;
        if (bulletNumber < bullet.Length) bullet[bulletNumber].color = new Color(0.8f, 0.8f, 0.8f, 1f);
        StartCoroutine(GetComponent<Gun_UI>().Reloading(bulletSwapTime, bulletNumber, false));
        isShooting = false;
    }
}
