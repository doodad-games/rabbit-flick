using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider))]
public class CarrotSpawnZone : MonoBehaviour
{
    public static CarrotSpawnZone I { get; private set; }

    static readonly Vector3 s_spawnVariance = new(0.25f, 0.05f, 0.25f);


    public Vector2Int NumRowsColumns;

    BoxCollider _box;
    GameObject _carrotPrefab;

    public void Awake()
    {
#if UNITY_EDITOR
        Assert.IsTrue(NumRowsColumns.x > 1);
        Assert.IsTrue(NumRowsColumns.y > 1);
#endif

        _box = GetComponent<BoxCollider>();
        _carrotPrefab = Resources.Load<GameObject>(Constants.Resources.CARROT_PREFAB);
    }

    public void OnEnable() =>
        I = this;

    public void OnDisable() =>
        I = null;

    public void SpawnCarrots()
    {
        var bounds = _box.bounds;
        var boundsSize = new Vector2(
            bounds.size.x - s_spawnVariance.x * 2,
            bounds.size.z - s_spawnVariance.z * 2
        );
        var spaceBetweenCarrots = new Vector2(
            boundsSize.x / (NumRowsColumns.x - 1),
            boundsSize.y / (NumRowsColumns.y - 1)
        );

        var bottomLeft = bounds.min + s_spawnVariance;
        for (var row = 0; row != NumRowsColumns.x; ++row)
        {
            for (var col = 0; col != NumRowsColumns.y; ++col)
            {
                var spawnPos = bottomLeft + new Vector3(
                    spaceBetweenCarrots.x * col + s_spawnVariance.x * Random.Range(-1f, 1f),
                    s_spawnVariance.y * Random.Range(-1f, 1f),
                    spaceBetweenCarrots.y * row + s_spawnVariance.z * Random.Range(-1f, 1f)
                );
                Instantiate(_carrotPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}
