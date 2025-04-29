using UnityEngine;
using System.Collections;

public class ShootPosition_JYC : MonoBehaviour
{
    [SerializeField]
    private CameraControl _cc;
    [SerializeField]
    private SelectBullet _sb;
    private Gun_UI _gu;

    [Space(50)]
    [Header("하드코딩의 흔적")]

    public GameObject griceballPrefab;

    private void Awake()
    {
        _gu = _sb.transform.GetComponent<Gun_UI>();
    }


    private void Update()
    {
        if (_gu.bulletData[_sb.bulletNumber] > 0 && !_gu.isReloading && GameManager.Instance._move.GunOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
            if ((Input.GetMouseButtonUp(0) && _sb.bulletNumber == 4))
            {
                StopCoroutine("AutoShoot");
            }
        }
        else
        {
            StopCoroutine("AutoShoot");
        }
    }

    public void Fire()
    {

        IEnumerator ForceRoutine(Rigidbody rigid, Vector3 dir, float force)
        {

            float time = 0.5f;
            float percent = 0;

            while(percent <= 1)
            {

                rigid.AddForce(dir * force / (Time.deltaTime / time));

                percent += Time.deltaTime / time;
                yield return null;
            }
            

        }

        if (CameraManager.Instance._zoom)
        {
            switch (_sb.bulletNumber)
            {
                case 0:
                    CameraManager.Instance.CameraShake(8, 5, 0.2f);
                    GameObject obj = Instantiate(griceballPrefab, transform.position, Quaternion.identity);

                    obj.transform.eulerAngles = transform.eulerAngles;
                    obj.transform.position += transform.forward * 2;
                    StartCoroutine(ForceRoutine(obj.GetComponent<Rigidbody>(), transform.forward, 1f));

                    break;
                case 1:
                    CameraManager.Instance.CameraShake(8, 5, 0.2f);
                    PoolableMono waterBullet = PoolManager.Instance.Pop("WaterGunEffect", transform.position);
                    waterBullet.transform.eulerAngles = transform.eulerAngles;
                    WaterBall waterGunEffect = PoolManager.Instance.Pop("WaterBall", transform.position) as WaterBall;

                    waterGunEffect.startDir = transform.forward;
                    waterGunEffect.startSpeed = 20f;
                    waterGunEffect.forceTime = 1.2f;
                    waterGunEffect.Init();

                    waterGunEffect.transform.eulerAngles = transform.eulerAngles;
                    break;
                case 2:
                    CameraManager.Instance.CameraShake(5, 3, 0.2f);
                    PoolableMono dangoBullet = PoolManager.Instance.Pop("DangoBullet", transform.position);
                    dangoBullet.transform.eulerAngles = transform.eulerAngles;
                    dangoBullet.transform.position = transform.position;
                    dangoBullet.Init();
                    break;
                case 3:
                    CameraManager.Instance.CameraShake(10, 6, 0.3f);
                    PoolableMono sparkleBullet = PoolManager.Instance.Pop("SparkleBullet", transform.position);
                    sparkleBullet.transform.eulerAngles = transform.eulerAngles;
                    sparkleBullet.transform.position = transform.position;
                    sparkleBullet.Init();
                    break;
                case 4:
                    StartCoroutine("AutoShoot");
                    break;
            }
        }
    }


    IEnumerator AutoShoot()
    {
        while (true)
        {
            CameraManager.Instance.CameraShake(4, 3, 0.2f);
            PoolableMono bullet = PoolManager.Instance.Pop("Bullet", transform.position);
            bullet.transform.eulerAngles = transform.eulerAngles;
            PoolableMono gunEffect = PoolManager.Instance.Pop("DefaultGunEffect", transform.position);
            gunEffect.transform.eulerAngles = transform.eulerAngles;
            yield return new WaitForSeconds(_gu.shootingSpeed);
        }
    }
}
