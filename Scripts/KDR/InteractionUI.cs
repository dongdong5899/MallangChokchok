using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    private GameObject intUI;
    private Transform intKey;
    private RectTransform intTextBar;
    private TextMeshProUGUI intText;
    private Image intKeyImage;
    private Image intTextBarImage;
    private TextMeshProUGUI intKeyText;

    private Sequence intUISeq;

    private bool isOn = false;

    [SerializeField]
    private float radius = 1.75f;
    [SerializeField]
    private float actionTime = 0.3f;
    [SerializeField]
    private float onTime = 0.2f;

    RaycastHit lastHit = new RaycastHit();



    private void Start()
    {
        intUI = UIManager.Instance.canvas.transform.Find("Interaction").gameObject;
        intKey = intUI.transform.Find("InteractionKey");
        intTextBar = intUI.transform.Find("InteractionTextBar").GetComponent<RectTransform>();
        intText = intTextBar.transform.Find("Mask/Text").GetComponent<TextMeshProUGUI>();
        intKeyImage = intKey.GetComponent<Image>();
        intTextBarImage = intTextBar.GetComponent<Image>();
        intKeyText = intKey.transform.Find("KeyText").GetComponent<TextMeshProUGUI>();
        intKeyImage.color = new Color(1, 1, 1, 0.6f);
        intTextBarImage.color = new Color(1, 1, 1, 0.6f);
        
        intKey.localScale = Vector3.zero;
        intTextBar.sizeDelta = new Vector2(0, 50);

        intUISeq = DOTween.Sequence();
    }

    private void Update()
    {
        RaycastHit[] currentHit = 
            Physics.SphereCastAll(transform.position, radius, transform.forward, 0, 1 << LayerMask.NameToLayer("Interaction"));
        if (currentHit.Length == 0 && lastHit.transform != new RaycastHit().transform)
        {
            lastHit = new RaycastHit();

            if (intUISeq != null && intUISeq.IsActive()) intUISeq.Kill();
            intUISeq = DOTween.Sequence();
            intUISeq
                .Append(DOTween.To(() => intTextBar.sizeDelta.x,
                    value => intTextBar.sizeDelta = new Vector2(value, intTextBar.sizeDelta.y),
                    0, 0.2f)).SetEase(Ease.InQuint)
                .Append(intKey.DOScale(Vector3.zero, 0.1f));
            isOn = false;
        }
        float minDis = 10;
        RaycastHit minDisHit = new RaycastHit();
        for (int i = 0; i < currentHit.Length; i++)
        {
            if (currentHit[i].transform.GetComponent<Interaction>())
            {
                float dis = Vector3.Distance(transform.position, currentHit[i].transform.position);
                if (minDis > dis)
                {
                    minDis = dis;
                    minDisHit = currentHit[i];
                }
            }
        }
        if (isOn && Input.GetKeyDown(KeyCode.F) && transform.GetComponent<PlayerMove>().OnGround)
        {
            minDisHit.transform.GetComponent<Interaction>().eventFun.Invoke();
            Active();
        }

        if (minDisHit.transform != lastHit.transform) lastHit = minDisHit;
        else return;

        Debug.Log("¾È³ç");
        intText.text = minDisHit.transform.GetComponent<Interaction>().eventName;

        intKeyImage.color = new Color(1, 1, 1, 0);
        intTextBarImage.color = new Color(1, 1, 1, 0);
        intText.alpha = 0;
        intKeyText.alpha = 0;

        if (intUISeq != null && intUISeq.IsActive()) intUISeq.Kill();
        intUISeq = DOTween.Sequence();
        intUISeq
            .Append(intKey.DOScale(new Vector3(-1, -1, 1), onTime/2))
            .Append(DOTween.To(() => intTextBar.sizeDelta.x,
                value => intTextBar.sizeDelta = new Vector2(value, intTextBar.sizeDelta.y),
                intText.preferredWidth + 30, onTime)).SetEase(Ease.OutQuint)
            .Join(intKeyImage.DOFade(0.6f, onTime))
            .Join(intTextBarImage.DOFade(0.6f, onTime))
            .Join(intText.DOFade(1f, onTime))
            .Join(intKeyText.DOFade(1f, onTime))
            .AppendCallback(() =>
            {
                isOn = true;
            });
    }

    public void Active()
    {
        isOn = false;
        intKeyImage.color = Color.yellow;
        intTextBarImage.color = Color.yellow;

        if (intUISeq != null && intUISeq.IsActive()) intUISeq.Kill();
        intUISeq = DOTween.Sequence();
        intUISeq
            .Append(intKeyImage.DOColor(Color.white, actionTime))
            .Join(intKeyImage.DOFade(0, actionTime))
            .Join(intTextBarImage.DOColor(Color.white, actionTime))
            .Join(intTextBarImage.DOFade(0, actionTime))
            .Join(intText.DOFade(0, actionTime))
            .Join(intKeyText.DOFade(0, actionTime))
            .AppendCallback(() =>
            {
                intUI.SetActive(false);

                intKeyImage.color = new Color(1, 1, 1, 0.6f);
                intTextBarImage.color = new Color(1, 1, 1, 0.6f);
                intText.alpha = 1;
                intKeyText.alpha = 1;
                intKey.localScale = Vector3.zero;
                intTextBar.sizeDelta = new Vector2(0, 50);
            });
    }

    public void On()
    {
        intUI.SetActive(true);
        intKeyImage.color = new Color(1, 1, 1, 0.6f);
        intTextBarImage.color = new Color(1, 1, 1, 0.6f);

        intKey.localScale = Vector3.zero;
        intTextBar.sizeDelta = new Vector2(0, 50);

        lastHit = new RaycastHit();
    }
}
