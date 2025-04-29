using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_UI : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int enemyDamage;
    [SerializeField] private Image hp_Bar;
    public int currentHp;
    float hp_Decimalpoint = 1f; // 소수점 구하는 용도에용용

    void Start()
    {
        currentHp = hp;
        GetHp();
    }

    private void GetHp()
    {
        hp = (int)Math.Floor(Math.Log10(hp));
        for (int i = 0; i < hp; i++)
        {
            hp_Decimalpoint *= 0.1f;
        }
        print(hp_Decimalpoint);
    }
    public void HitDamage()
    {
        currentHp -= enemyDamage;
        Hp_Update();
    }

    public void Hp_Update()
    {
        hp_Bar.fillAmount = currentHp * hp_Decimalpoint;
    }
}
