using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    public static readonly HashSet<Carrot> All = new();

    public void OnEnable() =>
        All.Add(this);

    public void OnDisable() =>
        All.Remove(this);
}
