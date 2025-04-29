
using UnityEngine;

public interface ICustomBackground
{
    public System.Action<GameObject> onEnterEvent { get; set; }
    public System.Action<GameObject> onOutEvent { get; set; }
}
