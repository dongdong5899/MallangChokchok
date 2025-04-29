using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<WaterFieldEffect_Kbh> fields;
    public PlayerMove _move;

    public Quest currentQuest = null;

    private void Awake()
    {
        Instance = this;
        _move = FindAnyObjectByType<PlayerMove>();
        fields = new List<WaterFieldEffect_Kbh>();
        MouseOff();
    }

    private void Update()
    {
        
    }

    public void MouseOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void MouseOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
