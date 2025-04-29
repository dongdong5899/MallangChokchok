using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StartStoryManager : MonoBehaviour
{
    
    public static StartStoryManager Instance;


    [Space(15)]
    [Header("BoardInfos")]
    public BoardScriptManageSO boardScripts;

    public BoardInfoSO currentBoardInfo
    {
        get => boardScripts.boards[currentBoardIdx];
    }

    public int currentBoardIdx = 0;
    public GameObject backgroundPuttingPanel;
        
    [Space(15)]
    [Header("Debugs")]
    public RectTransform trm;
    
    public BackgroundRoutine board;
    public string boardType;
    
    public DefaultBackground background;
    public string backgroundType;

    public void Awake()
    {
        Instance = this;
        trm = GetComponent<RectTransform>();

        // 해당 Board만 지워지도록 나중에 수정
        #region BoardDestroyCheck

        Transform[] childTrms = transform.GetComponentsInChildren<Transform>();

        for (int i = 1; i < childTrms.Length; i++)
        {
            Destroy(childTrms[i].gameObject);
        }
        
        // 최적화를 위해 나중에 pool로 바꿀 예정
        
        #endregion
        
        #region BoardInitialize 

        
        boardType = PlayerPrefs.GetString(key: "BoardType", defaultValue: "BackgroundRoutine");
        backgroundType = PlayerPrefs.GetString(key: "BackgroundType", defaultValue: "DefaultBackground");
        
        RectTransform boardTrm = new GameObject(boardType).AddComponent<RectTransform>();
        board = boardTrm.gameObject.AddComponent(Type.GetType(boardType)) as BackgroundRoutine;
        Image img = boardTrm.gameObject.AddComponent<Image>();
        img.color = Color.clear;
        boardTrm.SetParent(trm);

        // Stretch 상태로 바꾸기
        BoardStretchPanelSetting(boardTrm, Vector2.zero, Vector2.one, 0f);
        

        RectTransform backgroundTrm = new GameObject(backgroundType).AddComponent<RectTransform>();
        background = backgroundTrm.gameObject.AddComponent(Type.GetType(backgroundType)) as DefaultBackground;
        backgroundTrm.SetParent(trm);
        
        BoardStretchPanelSetting(backgroundTrm, Vector2.zero, Vector2.one, 0.1f);
        
        // 여기서 자식 panel의 위치를 정해주는 게임 오브젝트들을 생성해주는 작업을 해줘야 함.

        int startX =  !currentBoardInfo.isSortFromRight ? 0 : currentBoardInfo.boardSize.x - 1; 
        int addingX = !currentBoardInfo.isSortFromRight ? +1 : -1;
        
        int startY =  !currentBoardInfo.isSortFromUp ? 0 : currentBoardInfo.boardSize.y - 1; 
        int addingY = !currentBoardInfo.isSortFromUp ? +1 : -1;
        
        if (!currentBoardInfo.isReadPriorityYFirst)
        {
            for (int y = startY; y != currentBoardInfo.boardSize.y  && y >= 0; y += addingY)
            {
                for (int x = startX; x != currentBoardInfo.boardSize.x && x >= 0; x += addingX)
                {
                    
                    RectTransform makingPanelTrm = new GameObject($"({x},{y})").AddComponent<RectTransform>();
                    Image panelImg = makingPanelTrm.gameObject.AddComponent<Image>();
                    panelImg.color = Color.clear;
                    makingPanelTrm.SetParent(backgroundTrm);
                    
                    BoardStretchPanelSetting(makingPanelTrm, 
                        new Vector2((float)x/currentBoardInfo.boardSize.x, (float)y/currentBoardInfo.boardSize.y),
                        new Vector2((float)(x+1)/currentBoardInfo.boardSize.x, (float)(y+1)/currentBoardInfo.boardSize.y)
                            , currentBoardInfo.backgroundStroke);
                }
            }
        }
        else
        {
            for (int x = startX; x != currentBoardInfo.boardSize.x && x >= 0; x += addingX)
            {
                for (int y = startY; y != currentBoardInfo.boardSize.y && y >= 0; y += addingY)
                {
                    RectTransform makingPanelTrm = new GameObject($"({x},{y})").AddComponent<RectTransform>();
                    Image panelImg = makingPanelTrm.gameObject.AddComponent<Image>();
                    panelImg.color = Color.clear;
                    makingPanelTrm.SetParent(backgroundTrm);
                    
                    BoardStretchPanelSetting(makingPanelTrm, 
                        new Vector2((float)x/currentBoardInfo.boardSize.x, (float)y/currentBoardInfo.boardSize.y),
                        new Vector2((float)(x+1)/currentBoardInfo.boardSize.x, (float)(y+1)/currentBoardInfo.boardSize.y)
                        , currentBoardInfo.backgroundStroke);
                }
            }
        }
        
                    
        #endregion  
        
        // 클릭을 할 때마다 board가 다음 것을 호출하도록 코드를 짠다.
        // 모든 board가 끝났을 때는 자신을 Destroy 시킨다. 

        board.boardInfo = currentBoardInfo;
        board.background = background;
        board.panelPrefab = backgroundPuttingPanel;

    }
    
    private void BoardStretchPanelSetting(RectTransform rectTrm, Vector2 anchorMin, Vector2 anchorMax, float stroke = 0.01f)
    {
        rectTrm.anchoredPosition3D = Vector3.zero; // 포지션 중심으로 지정
        rectTrm.transform.localScale = Vector3.one * (1f - stroke); // 크기 기본값 지정

        // size 초기화
        rectTrm.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        rectTrm.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

        // anchor 설정

        float pixelWidth = Camera.main.pixelWidth;
        float pixelHeight = Camera.main.pixelHeight;
        
        
        rectTrm.anchorMin = anchorMin;
        rectTrm.anchorMax = anchorMax;
        
    }
    
    
    
}

