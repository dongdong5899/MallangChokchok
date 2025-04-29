using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject option;

    public void StageScene()
    {
        SceneManager.LoadScene("Stage_1");
        Time.timeScale = 1;
    }
    public void StageSelectScene()
    {
        SceneManager.LoadScene("JSY_StageSelectScene");
        Time.timeScale = 1;
    }
    public void OffOption()
    {
        Time.timeScale = 1;
        GameManager.Instance._move.transform.GetComponent<CameraControl>().onRotation = true;
        GameManager.Instance.MouseOff();
        option.SetActive(false);
    }
}
