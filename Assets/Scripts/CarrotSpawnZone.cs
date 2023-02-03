using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider))]
public class CarrotSpawnZone : MonoBehaviour
{
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

    public void Start() =>
        SpawnCarrots();

    public void SpawnCarrots()
    {
        var bounds = _box.bounds;
        var boundsSize = new Vector2(bounds.size.x, bounds.size.z);
        var spaceBetweenCarrots = new Vector2(
            boundsSize.x / (NumRowsColumns.x - 1),
            boundsSize.y / (NumRowsColumns.y - 1)
        );

        var bottomLeft = bounds.min;
        for (var row = 0; row != NumRowsColumns.x; ++row)
        {
            for (var col = 0; col != NumRowsColumns.y; ++col)
            {
                var spawnPos = bottomLeft + new Vector3(
                    spaceBetweenCarrots.x * col,
                    0f,
                    spaceBetweenCarrots.y * row
                );
                Instantiate(_carrotPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}
