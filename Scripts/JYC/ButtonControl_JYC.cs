using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonControl_JYC : MonoBehaviour
{
    // �ݼձ� �Դٰ�
    [SerializeField] private GameObject _setPanel, _fadeImg, _diePanel;

    private float _fadeTime = 2f;


    public void StartGame()
    {
        _fadeImg.SetActive(true);
        StartCoroutine(FadeAndStart());
    }

    public void Setting()
    {
        _setPanel.SetActive(true);
    }

    public void Back()
    {
        _setPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("������");
    }

    IEnumerator FadeAndStart()
    {
        float elapsedTime = 0f;
        Color startColor = _fadeImg.GetComponent<Image>().color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f); 

        while (elapsedTime < _fadeTime)
        {
            _fadeImg.GetComponent<Image>().color = Color.Lerp(startColor, targetColor, elapsedTime / _fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //SceneManager.LoadScene();
        SceneManager.LoadScene("StartStoryScene");
    }
}
