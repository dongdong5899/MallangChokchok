
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject canvas;
    private GameObject esc_Panel;
    private GameManager gameManager;

    static public UIManager Instance;

    bool isEsc;

    private void Awake()
    {
        Instance = this;
        gameManager = FindObjectOfType<GameManager>();
        esc_Panel = canvas.transform.Find("Option_Panel").gameObject;
    }

    public void Option_Button() // 왼쪽 상단 옵션 버튼.
    {
        if (!isEsc) PanelOn();
        else PanelOff();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isEsc)
        {
            PanelOn();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isEsc)
        {
            PanelOff();
        }
    }

    private void PanelOff()
    {
        isEsc = false;
        esc_Panel.SetActive(false);
        FindObjectOfType<CameraControl>().onRotation = true;
        gameManager.MouseOff();
        Time.timeScale = 1f;
    }

    private void PanelOn()
    {
        isEsc = true;
        esc_Panel.SetActive(true);
        gameManager.MouseOn();
        FindObjectOfType<CameraControl>().onRotation = false;
        Time.timeScale = 0f;
    }
    
    public void Continue_Button()
    {
        PanelOff();
    }

    public void Exit_Button()
    {
        print("씬 이동해야되는디 어디로 가야하오");
        // 대충 씬 이동 하는 코드 작성할듯.
    }
}
