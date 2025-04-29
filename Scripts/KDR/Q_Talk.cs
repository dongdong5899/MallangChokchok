using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Q_Talk : Quest
{
    [Header("=========��ȭ ����=========")]
    [SerializeField]
    private InteractionProfile talker;

    [SerializeField]
    private TalkInfo[] talkInfo;

    public override void InputAction()
    {
        talker.talkInfo = talkInfo;
        talker.outEvent = base.OutAction;
        GameManager.Instance.currentQuest = this;
    }
}
