using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Quest : MonoBehaviour
{
    [Header("=========퀘스트 기본설정=========")]
    public UnityEvent OutAction;
    public string questInpo;

    public abstract void InputAction();
}
