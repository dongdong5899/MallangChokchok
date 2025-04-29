using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q_KillEnemy : Quest
{
    [Header("=========적 설정=========")]
    [SerializeField]
    private GameObject[] enemys;
    private string baseQuestInpo;

    public override void InputAction()
    {
        StartCoroutine("Q_Update");
        GameManager.Instance.currentQuest = this;
        foreach (GameObject item in enemys)
        {
            item.SetActive(true);
        }
    }

    private void Awake()
    {
        foreach (GameObject item in enemys)
        {
            item.SetActive(false);
        }

        baseQuestInpo = base.questInpo;
    }

    int count = 0;

    private IEnumerator Q_Update()
    {
        Debug.Log("지금 퀘스트 진행중");

        yield return null;
        count = 0;
        foreach (GameObject item in enemys)
        {
            if (item != null)
            {
                count += 1;
            }
        }
        base.questInpo = $"{baseQuestInpo} ({count} / {enemys.Length})";
        if (count == 0)
        {
            GameManager.Instance.currentQuest = null;
            OutAction?.Invoke();
        }
        else
        {
            StartCoroutine("Q_Update");
        }
    }
}
