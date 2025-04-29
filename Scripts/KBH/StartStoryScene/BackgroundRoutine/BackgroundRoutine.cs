using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class BackgroundRoutine : MonoBehaviour, ICustomBoard
{
    public BoardInfoSO boardInfo;
    public DefaultBackground background;
    
    public GameObject panelPrefab { get; set; }
    
    public int currentPos = 0;
    public int endPos = 0;

    public bool isWaiting = false;

    private bool? defaultBool = true;
    private IEnumerator<bool?> IsPanelRoutineEnd;

    private void Awake()
    {
        IsPanelRoutineEnd = WaitRoutine(2f);
    }

    private void Update()
    {
        bool isMouseButtonDownAndWaitEnd = Input.GetButtonDown("Fire1") && (IsPanelRoutineEnd.Current ?? defaultBool).Value;

        
        if (isMouseButtonDownAndWaitEnd)
        {
            IsPanelRoutineEnd = WaitRoutine(2f);
            StartCoroutine(IsPanelRoutineEnd);
        }
        
        
        if (isMouseButtonDownAndWaitEnd && currentPos < boardInfo.backGroundInfos.Count)
        {
            if (!isWaiting)
            {
                if (currentPos == boardInfo.backGroundInfos.Count)
                    currentPos = 0;
                
                NextPage(false);
                isWaiting = true;
            }
            else
            {

                if (currentPos == boardInfo.backGroundInfos.Count - 1)
                {
                    NextPage(true);
                }
                else
                    NextPage(false);
                
                currentPos++;
                isWaiting = false;
            }
        }
        else if(isMouseButtonDownAndWaitEnd)
        {
            StyleController.Instance.MoveNextScene();
        }

        
    }


    private void NextPage(bool isCenter)
    {
        background.MoveNextPage(boardInfo.backGroundInfos[currentPos], currentPos, panelPrefab, isCenter);
    }


    private IEnumerator<bool?> WaitRoutine(float interval)
    {
        float percent = 0;

        while (percent <= 1)
        {
            yield return false;
            percent += Time.deltaTime / interval;
        }

        yield return true;
    }

}
    