using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    static public PoolManager Instance;

    [SerializeField]
    private PoolListSO poolingSO;

    private Dictionary<string, Stack<PoolableMono>> Pools;

    private void Awake()
    {
        Instance = this;
        Pools = new Dictionary<string, Stack<PoolableMono>>();
        StartPoolingSetting();


    }

    private void Update()
    {

    }

    private void StartPoolingSetting()
    {
        foreach (PoolSetting poolSetting in poolingSO.poolSetting)
        {
            Stack<PoolableMono> Pooling = new Stack<PoolableMono>();
            for (int i = 0; i < poolSetting.count; i++)
            {
                Pooling.Push(Create(poolSetting.pool));
            }
            Pools.Add(poolSetting.type, Pooling);
        }
    }

    private PoolableMono Create(PoolableMono prefab)
    {
        Debug.Log(prefab);
        PoolableMono obj = Instantiate(prefab, transform);
        obj.name = obj.name.Replace("(Clone)", "");
        obj.gameObject.SetActive(false);

        return obj;
    }

    public PoolableMono Pop(string name, Vector3 pos)
    {
        while (Pools[name].Count <= 0)
        {
            Debug.Log("=================================");
            foreach (PoolSetting poolSetting in poolingSO.poolSetting)
            {
                if (poolSetting.type == name)
                {
                    PoolableMono newObj = Create(poolSetting.pool);
                    Pools[name].Push(newObj);
                    break;
                }
            }
        }

        PoolableMono obj = Pools[name].Pop();

        obj.transform.localPosition = pos;
        obj.gameObject.SetActive(true);
        obj.Init();
        return obj;
    }

    public PoolableMono Pop_NotInit(string name, Vector3 pos)
    {
       while (Pools[name].Count <= 0)
       {
          Debug.Log("=================================");
          foreach (PoolSetting poolSetting in poolingSO.poolSetting)
          {
             if (poolSetting.type == name)
             {
                PoolableMono newObj = Create(poolSetting.pool);
                Pools[name].Push(newObj);
                break;
             }
          }
       }
    
       PoolableMono obj = Pools[name].Pop();
    
       obj.transform.localPosition = pos;
       obj.gameObject.SetActive(true);
       return obj;
    }

   public void Push(string name, PoolableMono obj)
    {
        obj.gameObject.SetActive(false);
        Pools[name].Push(obj);
    }
}
