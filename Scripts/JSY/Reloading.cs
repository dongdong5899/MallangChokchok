using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reloading : MonoBehaviour
{
    [SerializeField] private Image reloadMark;

    float reloadingTime;
    private void OnEnable()
    {
        reloadMark.fillAmount = 0f;
        reloadingTime = FindObjectOfType<Gun_UI>().currentReloadingTime * 0.02f;
        StartCoroutine(MarkReloading());
    }
    IEnumerator MarkReloading() // 재장전 표시 코루틴.
    {
        while (reloadMark.fillAmount <= 1f)
        {
            reloadMark.fillAmount += 0.02f;
            yield return new WaitForSeconds(reloadingTime);
        }
    }
}
