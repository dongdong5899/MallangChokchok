using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class SelectStage : MonoBehaviour
{
    [SerializeField] private GameObject stageSelect_Panel;
    [SerializeField] private TextMeshProUGUI stage_Text;
    [SerializeField] private TextMeshProUGUI stage_Info_Text;

    private int stageNum;
    private bool isStageSelected = false;
    private string[] stage_Info = { "", "마을", "끈적숲", "화산", "연구소" };
    public void Select_Stage(int stageNumber) 
    {
        if(!isStageSelected)
        {
            isStageSelected = true;
            stageSelect_Panel.SetActive(true);
            stageSelect_Panel.transform.DOLocalMoveX(910, 0.5f);
            stageNum = stageNumber;
            stage_Text.text = "스테이지 " + stageNumber;
            stage_Info_Text.text = stage_Info[stageNumber];
        }
    }
    public void Start_Stage()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage_1");
    }
    public void ShutDown_Stage()
    {
        stageSelect_Panel.transform.DOMoveX(2540, 1);
        StartCoroutine(StageShutDown_Routine());
    }

    IEnumerator StageShutDown_Routine()
    {
        yield return new WaitForSeconds(1f);
        stageSelect_Panel.SetActive(false);
        stage_Text.text = "";
        isStageSelected = false;
    }

    public void Exit_StageScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start Scene");
    }

}
