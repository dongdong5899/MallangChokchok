using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q_ShootAndHit : Quest
{
    [Header("목표 설정")]
    [SerializeField]
    private HitCountObject target;
    [SerializeField]
    private int maxCount;

    public override void InputAction()
    {
        GameManager.Instance.currentQuest = this;
        target.InEvent();
        target.outEvent = base.OutAction;
        target.maxCount = maxCount;
    }
}
