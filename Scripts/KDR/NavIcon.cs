using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NavIcon : MonoBehaviour
{
    private Quest targetQuest = null;
    private RectTransform rectTrm;
    private Camera mainCam;
    private Image image;
    [SerializeField]
    private GameObject questBar;
    private TextMeshProUGUI questName;
    private TextMeshProUGUI questDis;

    private float scaledPixelWidth;
    private float scaledPixelHeight;
    private float halfScaledPixelWidth;
    private float halfScaledPixelHeight;


    private void Awake()
    {
        questName = questBar.transform.Find("Q_Text").GetComponent<TextMeshProUGUI>();
        questDis = questBar.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        mainCam = Camera.main;
        rectTrm = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.enabled = false;

        scaledPixelWidth = mainCam.scaledPixelWidth;
        scaledPixelHeight = mainCam.scaledPixelHeight;
        halfScaledPixelWidth = scaledPixelWidth / 2;
        halfScaledPixelHeight = scaledPixelHeight / 2;
    }

    private void Start()
    {
        questBar.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance.currentQuest != targetQuest)
        {
            targetQuest = GameManager.Instance.currentQuest;
        }
        if (targetQuest != null)
        {
            if (!image.enabled)
            {
                questBar.SetActive(true);
                image.enabled = true;
            }
            questName.text = targetQuest.questInpo;
            questDis.text = $"°Å¸® : {(int)Vector3.Distance(GameManager.Instance._move.transform.position, targetQuest.transform.position)}m";

            Vector3 pos =
                mainCam.WorldToScreenPoint(targetQuest.transform.position) -
                new Vector3(scaledPixelWidth, scaledPixelHeight, 0) / 2;
            pos.z = 0;

            pos.y = Mathf.Clamp(pos.y, -halfScaledPixelHeight + rectTrm.rect.height / 2, halfScaledPixelHeight - rectTrm.rect.height / 2);

            Vector3 dir = (targetQuest.transform.position - mainCam.transform.position).normalized;
            float dirDot = Vector3.Dot(mainCam.transform.forward, dir);
            if (Mathf.Abs(pos.x) > halfScaledPixelWidth - rectTrm.rect.height / 2)
            {
                pos.y *= -1;
            }
            pos.x = Mathf.Clamp(pos.x, -halfScaledPixelWidth + rectTrm.rect.width / 2, halfScaledPixelWidth - rectTrm.rect.width / 2);

            if (dirDot < 0)
            {
                pos.y = -halfScaledPixelHeight + rectTrm.rect.height / 2;
                pos.x *= -1;
            }

            rectTrm.anchoredPosition = pos;
        }
        else
        {
            if (image.enabled)
            {
                questBar.SetActive(false);
                image.enabled = false;
            }
        }
    }
}
