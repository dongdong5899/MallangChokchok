using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct PoolSetting
{
    public PoolableMono pool;
    public string type;
    public int count;
}


[CreateAssetMenu(menuName = "SO/PoolListSO")]
public class PoolListSO : ScriptableObject
{
    [SerializeField]
    public PoolSetting[] poolSetting;

    

}
