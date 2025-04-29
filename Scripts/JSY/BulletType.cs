using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum bulletType
{
    GRice_Bullet, // Âý½ÒÅº
    Water_Bullet, // ¹°Åº
    Dango_Bullet, // °æÅº
    Sparkle_Bullet // Åº»êÅº
}
public class BulletType : MonoBehaviour
{
    public bulletType type;

    private bool isPlayer;
    private SelectBullet _sb;
    private Gun_UI _gu;

    private void Awake()
    {
        _sb = FindObjectOfType<SelectBullet>();
        _gu = FindObjectOfType<Gun_UI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isPlayer)
        {
            isPlayer = true;
            AddBullet();
        }
    }

    private void AddBullet()
    {
        int num = 0;
        switch(type)
        {
            case bulletType.GRice_Bullet:
                num = 0;
                break;
            case bulletType.Water_Bullet:
                num = 1;
                break;
            case bulletType.Dango_Bullet:
                num = 2;
                break;
            case bulletType.Sparkle_Bullet:
                num = 3;
                break;
        }
        print("NUmber : " + _sb.bulletNumber + "\nNUm : " + num);
        _sb.totalBullet[num] += _gu.bulletCount[num];
        if(_sb.bulletNumber == num)
            _gu.UpdateTotalBullet(num);

    }


}
