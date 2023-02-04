using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    const float SCALE_UP_TIME = 1.3f;
    const float SCALE_UP_TIME_VARIANCE = 0.3f;
    const float SHAKE_RANDOM_POSITION_INTERVAL = 0.02f;
    const float SHAKE_DISTANCE = 0.015f;
    const float SCALE_DOWN_TIME = 0.3f;


    public static readonly List<Carrot> All = new();
    public static int NumDestroyedThisWave { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    public static void HandleWaveChanged() =>
        GameManager.OnWaveStarted += () => NumDestroyedThisWave = 0;


    [SerializeField] GameObject _visuals;

    public void Awake() =>
        _visuals.AddComponent<ScaleUpAndShake>()
            .Use(
                SCALE_UP_TIME,
                SCALE_UP_TIME_VARIANCE,
                SHAKE_RANDOM_POSITION_INTERVAL,
                SHAKE_DISTANCE
            );

    public void OnEnable() =>
        All.Add(this);

    public void OnDisable()
    {
        ++NumDestroyedThisWave;
        All.Remove(this);
    }

    public void Destroy()
    {
        _visuals.AddComponent<ScaleDownAndDestroy>()
            .Use(SCALE_DOWN_TIME, destroyTarget: gameObject);

        Destroy(this);
    }
}
