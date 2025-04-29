using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Gun_UI : MonoBehaviour
{
    public int[] bulletCount;
    public int[] bulletData;
    public float reloadingTime;
    public float currentReloadingTime;
    public float shootingSpeed;


    public bool isReloading;
    bool emptyTotalBullet; // 남아있는 총알이 있는지 확인.
    bool isFull;
    bool isShoot;
    bool isShooting; // 연사...

    [SerializeField] TextMeshProUGUI bulletCountText;
    [SerializeField] TextMeshProUGUI TotalBulletText;
    [SerializeField] private Image aim;
    private Image reloadMark;

    [SerializeField]
    private CameraControl _cc;
    private PlayerMove _pm;
    private SelectBullet _sb;

    private void Awake()
    {
        reloadMark = aim.transform.Find("Reloading_Image").GetComponent<Image>();
        _pm = _cc.GetComponent<PlayerMove>();
        _sb = GetComponent<SelectBullet>();
        for (int i = 0; i < bulletCount.Length; i++)
            bulletCount[i] = bulletData[i];
    }

    private void Start()
    {
        StartCoroutine(Reloading(_sb.bulletSwapTime, _sb.bulletNumber, false));
    }

    private void Update()
    {
        FullCheck(_sb.bulletNumber);

        if (!isShooting && _sb.totalBullet[_sb.bulletNumber] <= 0f) emptyTotalBullet = true;
        else emptyTotalBullet = false;

        if (CameraManager.Instance._zoom && _pm.GunOn)
        {
            aim.color = new Color(1, 1, 1, 1);
            if (Input.GetMouseButtonDown(0) && !isReloading)
            {
                if (_sb.isShooting)
                    StartCoroutine(RepeaterRoutine(_sb.bulletNumber));
                else if (!isShoot)
                {
                    MarkingBullet(_sb.bulletNumber);
                }
            }
        }
        else
        {
            aim.color = new Color(1, 1, 1, 0);
        }

        if(Input.GetMouseButtonDown(0) && _sb.isShooting)
        {
            isShooting = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }

        if (Input.GetKeyDown(KeyCode.R) && !isFull && !emptyTotalBullet)
        {
            StartCoroutine(Reloading(reloadingTime, _sb.bulletNumber, true));
            _pm._animator.SetTrigger("Reload");
        }

    }
    IEnumerator RepeaterRoutine(int num)
    {
        while(true)
        {
            if (bulletData[num] <= 0) break;
            bulletData[num]--;
            bulletCountText.text = bulletData[num] + "/" + bulletCount[num];
            yield return new WaitForSeconds(shootingSpeed);
            if (!isShooting) break;
        }
        yield return null;
    }

    void FullCheck(int num) // 총알 가득찼는지 확인.
    {
        if (bulletData[num] == bulletCount[num]) isFull = true;
        else isFull = false;
    }

    void MarkingBullet(int num) // 총알 표시 함수.
    {
        if (bulletData[num] <= 0) return;
        isShoot = true;
        bulletData[num]--;
        bulletCountText.text = bulletData[num] + "/" + bulletCount[num];
        isShoot = false;
    }

    public IEnumerator Reloading(float time, int num, bool isShootingReload) // 재장전 코루틴.
    {
        currentReloadingTime = time;
        reloadMark.gameObject.SetActive(true);

        isReloading = true;

        yield return new WaitForSeconds(time);
        reloadMark.gameObject.SetActive(false);
        if(isShootingReload)
        {
            if (!_sb.isShooting)
            {
                int minusBullet = (bulletCount[num] - bulletData[num]);
                if (minusBullet > _sb.totalBullet[num]) minusBullet = _sb.totalBullet[num];
                _sb.totalBullet[num] -= minusBullet;
                bulletData[num] += minusBullet;

            }
            else
            {
                bulletData[num] = bulletCount[num];
            }
            
        }
        bulletCountText.text = bulletData[num] + "/" + bulletCount[num];
        UpdateTotalBullet(num);

        isShoot = false;
        isReloading = false;
        _sb.isChange = false;
    }

    public void UpdateTotalBullet(int num)
    {
        if (num < 4) TotalBulletText.text = _sb.totalBullet[num].ToString();
        else TotalBulletText.text = "∞";
    }
}
