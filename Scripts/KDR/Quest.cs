using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Quest : MonoBehaviour
{
    [Header("=========����Ʈ �⺻����=========")]
    public UnityEvent OutAction;
    public string questInpo;

    public abstract void InputAction();
}
