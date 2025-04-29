using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

[System.Serializable]
public struct BackGroundInfo
{
    public string Name;
    
    [Space]
    public Sprite sprite;
    public string effectName;

    [Space(15)] [Header("패널 등장 프로퍼티")]
    public AnimationCurve turnCurveOnEnter;
    public AnimationCurve scalingCurveOnEnter;
    public Gradient colorCurveOnEnter;

    [Space(15)] 
    public float rotateScaleOnEnter;
    
    [Space]
    public float startScalingScaleOnEnter;
    public float endScalingScaleOnEnter;
    
    [Space]
    public float enterRoutineTime;
    
    
    [Space(15)] [Header("패널 다운 프로퍼티")]
    public AnimationCurve moveCurveOnSubtract;
    public AnimationCurve turnCurveOnSubtract;
    public AnimationCurve scalingCurveOnSubtract;
    public Gradient colorCurveOnSubtract;

    [Space(15)]
    public float rotateScaleOnSubtract;
    
    [Space]
    public float startScalingScaleOnSubtract;
    public float endScalingScaleOnSubtract;

    [Space]
    public float subtractRoutineTime;

}


[CreateAssetMenu(menuName = "SO/BoardInfoSO")]
public class BoardInfoSO : ScriptableObject
{
    public Gradient gradientColor;
    [Range(0f, 1f)] public float boardStroke;
    [Range(0f, 1f)] public float backgroundStroke;

    [Space] public bool isReadPriorityYFirst;
    public bool isSortFromRight = false;
    public bool isSortFromUp = false;
    public Vector2Int boardSize = new Vector2Int(2,2);

    public float nextOverTime = 1f;
    
    
    [Space(60)] 
    public bool ___GetBoardActionListBUTTON___ = false;
    public List<string> boardActionList;
    [Space]
    public string setBoardAction;


    [Space(80)]
    public bool ___GetBackgroundActionListBUTTON___;
    public List<string> backgroundActionList;
    [Space(60)] public List<BackGroundInfo> backGroundInfos;

    
    
    private void OnValidate()
    {
        if (___GetBoardActionListBUTTON___)
        {
            ___GetBoardActionListBUTTON___ = false;
            
            boardActionList.Clear();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            
            var childClasses = types
                .Where(t => t.GetInterfaces().Contains(typeof(ICustomBoard)));

            foreach (Type type in childClasses)
            {
                boardActionList.Add(type.ToString());
            }
        }

        if (___GetBackgroundActionListBUTTON___)
        {
            ___GetBackgroundActionListBUTTON___ = false;
            
            backgroundActionList.Clear();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            var childClasses = types
                .Where(t => t.GetInterfaces().Contains(typeof(ICustomBackground)));

            foreach (Type type in childClasses)
            {
                backgroundActionList.Add(type.ToString());
            }
            
        }
        
    }
}
