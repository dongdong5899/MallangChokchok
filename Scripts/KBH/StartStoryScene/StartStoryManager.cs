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

        // �ش� Board�� ���������� ���߿� ����
        #region BoardDestroyCheck

        Transform[] childTrms = transform.GetComponentsInChildren<Transform>();

        for (int i = 1; i < childTrms.Length; i++)
        {
            Destroy(childTrms[i].gameObject);
        }
        
        // ����ȭ�� ���� ���߿� pool�� �ٲ� ����
        
        #endregion
        
        #region BoardInitialize 

        
        boardType = PlayerPrefs.GetString(key: "BoardType", defaultValue: "BackgroundRoutine");
        backgroundType = PlayerPrefs.GetString(key: "BackgroundType", defaultValue: "DefaultBackground");
        
        RectTransform boardTrm = new GameObject(boardType).AddComponent<RectTransform>();
        board = boardTrm.gameObject.AddComponent(Type.GetType(boardType)) as BackgroundRoutine;
        Image img = boardTrm.gameObject.AddComponent<Image>();
        img.color = Color.clear;
        boardTrm.SetParent(trm);

        // Stretch ���·� �ٲٱ�
        BoardStretchPanelSetting(boardTrm, Vector2.zero, Vector2.one, 0f);
        

        RectTransform backgroundTrm = new GameObject(backgroundType).AddComponent<RectTransform>();
        background = backgroundTrm.gameObject.AddComponent(Type.GetType(backgroundType)) as DefaultBackground;
        backgroundTrm.SetParent(trm);
        
        BoardStretchPanelSetting(backgroundTrm, Vector2.zero, Vector2.one, 0.1f);
        
        // ���⼭ �ڽ� panel�� ��ġ�� �����ִ� ���� ������Ʈ���� �������ִ� �۾��� ����� ��.

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
        
        // Ŭ���� �� ������ board�� ���� ���� ȣ���ϵ��� �ڵ带 §��.
        // ��� board�� ������ ���� �ڽ��� Destroy ��Ų��. 

        board.boardInfo = currentBoardInfo;
        board.background = background;
        board.panelPrefab = backgroundPuttingPanel;

    }
    
    private void BoardStretchPanelSetting(RectTransform rectTrm, Vector2 anchorMin, Vector2 anchorMax, float stroke = 0.01f)
    {
        rectTrm.anchoredPosition3D = Vector3.zero; // ������ �߽����� ����
        rectTrm.transform.localScale = Vector3.one * (1f - stroke); // ũ�� �⺻�� ����

        // size �ʱ�ȭ
        rectTrm.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        rectTrm.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

        // anchor ����

        float pixelWidth = Camera.main.pixelWidth;
        float pixelHeight = Camera.main.pixelHeight;
        
        
        rectTrm.anchorMin = anchorMin;
        rectTrm.anchorMax = anchorMax;
        
    }
    
    
    
}

