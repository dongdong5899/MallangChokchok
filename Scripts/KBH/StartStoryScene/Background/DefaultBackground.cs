using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DefaultBackground : MonoBehaviour, ICustomBackground
{
    public Action<GameObject> onEnterEvent { get; set; }
    public Action<GameObject> onOutEvent { get; set; }
    
    

    private List<GameObject> currentPanelList = new List<GameObject>();
    

    private int? currentChildPos = null;
    
    public void MoveNextPage(BackGroundInfo info, int childPos = 0, GameObject prefab = null, bool isFixedPosition = false)
    {
        if (currentChildPos is null)
        {
            currentChildPos = childPos;
            
            Transform goalTrm = transform.GetChild(currentChildPos.Value);
            
            Transform panelTrm = Instantiate(prefab, transform).transform;
            currentPanelList.Add(panelTrm.gameObject);
            
            onEnterEvent?.Invoke(panelTrm.gameObject);
            
            panelTrm.rotation = quaternion.Euler(new Vector3(0, 0, info.rotateScaleOnEnter));
            panelTrm.localScale = transform.localScale * info.startScalingScaleOnEnter;
            
            DOTween.Sequence()

                .Append(
                    panelTrm
                        .DOLocalRotate(new Vector3(0, 0, 0), info.enterRoutineTime)
                        .SetEase(info.turnCurveOnEnter))
                
                
                // panelTrm.GetComponent<Image>()
                //     .DOColor(info.colorCurveOnEnter.Evaluate(0), info.enterRoutineTime);

                
                .Join(
                    panelTrm
                        .DOScale(transform.localScale * info.endScalingScaleOnEnter, info.enterRoutineTime)
                        .SetEase(info.scalingCurveOnEnter))
                .AppendCallback(() => StyleController.Instance.UnFocus());
            
            StartCoroutine(ColorRoutine(panelTrm, info.colorCurveOnEnter, info.sprite, info.enterRoutineTime));
            
        }
        else
        {
            Transform goalTrm = transform.GetChild(currentChildPos.Value);
            Transform panelTrm = currentPanelList.Last().transform;
            
            onOutEvent?.Invoke(panelTrm.gameObject);
            
            
            Vector2 randomVector0
                = new Vector2(Random.Range(-1f, 1f) * (Camera.main.aspect/1f), Random.Range(-1f, 1f) * Camera.main.aspect) * 0.25f;


            DOTween.Sequence()

                .Append(
                    panelTrm
                        .DOLocalRotate
                        (panelTrm.eulerAngles - Vector3.forward * info.rotateScaleOnSubtract,
                            info.subtractRoutineTime)
                        .SetEase(info.turnCurveOnSubtract))


                // panelTrm.GetComponent<Image>()
                //     .DOColor(info.colorCurveOnEnter.Evaluate(0), info.enterRoutineTime);

                .Join(
                    panelTrm
                        .DOScale(goalTrm.localScale * info.endScalingScaleOnSubtract, info.subtractRoutineTime)
                        .SetEase(info.scalingCurveOnSubtract))

                .Join(
                    panelTrm.GetComponent<RectTransform>().DOAnchorMin(

                            isFixedPosition
                                ? new Vector2(0.2f, 0.2f)
                                : goalTrm.GetComponent<RectTransform>().anchorMin + randomVector0 * 0.5f,

                            info.subtractRoutineTime

                        )

                        .SetEase(info.moveCurveOnSubtract))
                .Join(
                    panelTrm.GetComponent<RectTransform>().DOAnchorMax(

                            isFixedPosition
                                ? new Vector2(0.8f, 0.8f)
                                : goalTrm.GetComponent<RectTransform>().anchorMax + randomVector0 * 0.5f


                            , info.subtractRoutineTime

                        )
                        .SetEase(info.moveCurveOnSubtract))
                .AppendCallback(() =>
                {
                    StyleController.Instance.Focus
                    ((panelTrm.GetComponent<RectTransform>().anchorMin +
                      panelTrm.GetComponent<RectTransform>().anchorMax) / 2);
                    
                    if(isFixedPosition)
                        StyleController.Instance.EndEffect();
                    
                });
            
            
            
            
            StartCoroutine(ColorRoutine(panelTrm, info.colorCurveOnSubtract, info.sprite, info.subtractRoutineTime));
            
            currentChildPos = null;
            
        }
        
        
    }

    private IEnumerator ColorRoutine(Transform panelTrm, Gradient infoColorCurveOnEnter, Sprite sprite, float infoEnterRoutineTime)
    {
        
        Image panelImage = panelTrm.GetComponent<Image>();
        panelImage.sprite = sprite;
        
        float percent = 0;
        
        
        while (percent <= 1)
        {
            panelImage.color = infoColorCurveOnEnter.Evaluate(percent);

            percent += Time.deltaTime;
            yield return null;
        }
        
    }


    public void EraseAll()
    {
        
        
    }

    
    
}
