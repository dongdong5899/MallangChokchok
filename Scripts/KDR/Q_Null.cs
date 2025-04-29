using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q_Null : Quest
{
    public override void InputAction()
    {
        GameManager.Instance.currentQuest = null;
    }
}
