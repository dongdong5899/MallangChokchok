using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class TalkManager : MonoBehaviour
{
    static public TalkManager Instance;

    public Transform talkTrm;
    private RectTransform talkBar;
    private TextMeshProUGUI talkName;
    private TextMeshProUGUI talkText;
    private RectTransform talkTextRet;
    public bool isReading = false;
    public bool onTextbar = false;
    private bool isTalking = false;

    private TalkInfo[] talks;
    private int currentTalkInfoIdx = 0;
    private int currentTalkIdx = 0;
    private string text = "";

    [SerializeField]
    private float delay = 0.15f;

    Sequence seq;
    Sequence textSeq;


    UnityEvent outEvent;

    private InteractionProfile currentInteractionProfile;


    private void Awake()
    {
        Instance = this;
        seq = DOTween.Sequence();
        textSeq = DOTween.Sequence();
    }

    private void Start()
    {
        talkBar = UIManager.Instance.canvas.transform.Find("TalkBar").GetComponent<RectTransform>();
        talkBar.anchoredPosition = new Vector3(0, 110, 0);
        talkBar.sizeDelta = new Vector2(10f, 0f);
        talkBar.gameObject.SetActive(false);

        talkName = talkBar.transform.Find("Mask/Name").GetComponent<TextMeshProUGUI>();
        talkText = talkBar.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        talkTextRet = talkText.transform.GetComponent<RectTransform>();
        talkTextRet.sizeDelta = Vector2.zero;
    }

    public void Read()
    {
        if (onTextbar)
        {
            if (isTalking)
            {
                StopCoroutine("TextPrint");
                talkText.text = text;
                isTalking = false;
            }
            else
            {
                if (currentTalkInfoIdx >= talks.Length)
                {
                    onTextbar = false;
                    currentTalkInfoIdx++;
                    TalkEnd();
                    return;
                }


                talkName.text = talks[currentTalkInfoIdx].talker;
                talkText.text = talks[currentTalkInfoIdx].talks[currentTalkIdx];
                talkTextRet.sizeDelta = new Vector2(talkText.preferredWidth, talkText.preferredHeight);
                text = talkText.text;
                talkText.text = "";


                StartCoroutine("TextPrint");
                currentTalkIdx++;
                if (currentTalkIdx >= talks[currentTalkInfoIdx].talks.Length)
                {
                    currentTalkIdx = 0;
                    currentTalkInfoIdx++;
                }
            }
        }
    }

    IEnumerator TextPrint()
    {
        isTalking = true;
        int i = 0;

        while (i < text.Length)
        {
            talkText.text += text[i].ToString();
            i++;

            yield return new WaitForSeconds(delay);
        }
        isTalking = false;
    }

    private void Update()
    {
        if (isReading)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Read();
            }
        }
    }
    
    public void TalkStart(InteractionProfile nPCProfile)
    {
        currentInteractionProfile = nPCProfile;
        this.outEvent = nPCProfile.outEvent;
        if (!isReading)
        {
            onTextbar = true;
            isReading = true;
            GameManager.Instance._move.MoveOn = false;

            CameraManager.Instance.TalkView();

            currentTalkInfoIdx = 0;
            currentTalkIdx = 0;

            talkTrm = nPCProfile.transform;
            talks = nPCProfile.talkInfo;

            talkName.text = talks[currentTalkInfoIdx].talker;
            talkText.text = "";
            talkBar.gameObject.SetActive(true);


            if (seq != null && seq.IsActive()) seq.Kill();
            seq = DOTween.Sequence();
            seq.Append(DOTween.To(() => talkBar.sizeDelta.y,
                    value => talkBar.sizeDelta = new Vector2(talkBar.sizeDelta.x, value),
                    220, 0.4f).SetEase(Ease.OutQuint))
                .Join(DOTween.To(() => talkBar.anchoredPosition.y,
                    value => talkBar.anchoredPosition = new Vector3(0, value, 0),
                    0, 0.4f).SetEase(Ease.OutQuint))
                .Append(DOTween.To(() => talkBar.sizeDelta.x,
                    value => talkBar.sizeDelta = new Vector2(value, talkBar.sizeDelta.y),
                    1920, 1f).SetEase(Ease.OutCubic))
                .AppendCallback(() =>
                {
                    Read();
                });
        }
    }

    public void TalkEnd()
    {
        Invoke("PlayOn", 1f);
        outEvent?.Invoke();

        CameraManager.Instance.PlayerView();

        talkName.text = "";
        talkText.text = "";


        talkBar.anchoredPosition = new Vector3(0, 110, 0);
        talkBar.sizeDelta = new Vector2(10f, 0f);
        talkBar.gameObject.SetActive(false);

        talkTextRet.sizeDelta = Vector2.zero;

        currentInteractionProfile.outEvent = null;
    }

    private void PlayOn()
    {
        GameManager.Instance._move.GetComponent<InteractionUI>().On();
        isReading = false;
        GameManager.Instance._move.MoveOn = true;
    }
}
