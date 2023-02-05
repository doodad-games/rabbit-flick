using UnityEngine;

public class BunnyDropPowerupOnFlicked : MonoBehaviour
{
    const float BASE_DROP_CHANCE = 0.05f;
    const float GLOBAL_DROP_CHANCE_TAPERING_FACTOR = 0.8f;

    static float s_globalDropChanceMultiplier;
    static WeightedPool<GameObject> s_powerupPool;

    [RuntimeInitializeOnLoadMethod]
    public static void PopulatePowerupPool()
    {
        s_powerupPool = new WeightedPool<GameObject>();

        s_powerupPool.Add(75f, Resources.Load<GameObject>(Constants.Resources.POWERUP_CRITICAL_FLICK_PREFAB));
        s_powerupPool.Add(100f, Resources.Load<GameObject>(Constants.Resources.POWERUP_SLOW_TIME_PREFAB));
        s_powerupPool.Add(30f, Resources.Load<GameObject>(Constants.Resources.POWERUP_STOP_TIME_PREFAB));
        s_powerupPool.Add(100f, Resources.Load<GameObject>(Constants.Resources.POWERUP_GROW_CARROTS_PREFAB));
    }

    [RuntimeInitializeOnLoadMethod]
    public static void ResetGlobalDropChanceMultiplier() =>
        GameManager.OnWaveStarted += () => s_globalDropChanceMultiplier = 1f;


    [SerializeField] float _dropChanceMultiplier = 1f;

    Bunny _bunny;

    public void OnEnable()
    {
        _bunny = GetComponent<Bunny>();
        _bunny.OnFlicked += MaybeDropPowerup;
    }

    public void OnDisable() =>
        _bunny.OnFlicked -= MaybeDropPowerup;

    void MaybeDropPowerup()
    {
        var dropChance = BASE_DROP_CHANCE * _dropChanceMultiplier * s_globalDropChanceMultiplier;
        if (Random.value > dropChance)
            return;
        s_globalDropChanceMultiplier *= GLOBAL_DROP_CHANCE_TAPERING_FACTOR;

        var prefab = s_powerupPool.PickRandom();
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}