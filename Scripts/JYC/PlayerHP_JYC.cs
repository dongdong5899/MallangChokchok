using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP_JYC : MonoBehaviour
{
    private HP_UI hP;
    private GameManager gameManager;
    private bool canTakeDamage = true; 
    private float cooldownDuration = 0.42f;
    private float detectionRadius = 0.5f;
    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private GameObject _diePanel;


    private void Awake()
    {
        Time.timeScale = 1f;
        hP = FindObjectOfType<HP_UI>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        foreach (Collider collider in colliders)
        {
            if (canTakeDamage && collider.CompareTag("Enemy"))
            {
                hP.HitDamage();
                Debug.Log("가나다라");
                StartCoroutine(DamageCooldown());
            }
        }

        if (hP.currentHp == 0)
        {
            gameManager.MouseOn();
            _diePanel.SetActive(true);
            GetComponent<CameraControl>().onRotation = false;
            Time.timeScale = 0f;
            Debug.Log(Cursor.lockState);
        }
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false; 
        yield return new WaitForSeconds(cooldownDuration);
        canTakeDamage = true; 
    }

    public void Retry()
    {
        SceneManager.LoadScene("JYC");
        
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartScene");
        Debug.Log("dlehd");
    }
}