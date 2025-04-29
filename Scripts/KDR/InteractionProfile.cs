using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct TalkInfo
{
    public string talker;
    [TextArea]
    public string[] talks;
}

public class InteractionProfile : MonoBehaviour
{
    public TalkInfo[] talkInfo;
    public UnityEvent outEvent;
}
