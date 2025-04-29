using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q_Harvest : Quest
{
    [Header("수집물 설정")]
    [SerializeField]
    private Flower flower;

    public override void InputAction()
    {
        GameManager.Instance.currentQuest = this;
        flower.Bloom();
        StartCoroutine("UpDate_c");
    }

    IEnumerator UpDate_c()
    {
        if (flower.isbloom)
        {
            yield return null;
            StartCoroutine("UpDate_c");
        }
        else
        {
            base.OutAction?.Invoke();
        }
    }
}
