using System;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    public static readonly List<Carrot> All = new();
    public static int NumDestroyedThisWave { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    public static void HandleWaveChanged() =>
        GameManager.OnWaveStarted += () => NumDestroyedThisWave = 0;

    public void OnEnable() =>
        All.Add(this);

    public void OnDisable()
    {
        ++NumDestroyedThisWave;
        All.Remove(this);
    }
}
