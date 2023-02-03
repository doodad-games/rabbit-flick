using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider))]
public class BunnySpawnZone : MonoBehaviour
{
    public struct SpawnInstruction
    {
        public float SpawnTime;
        public GameObject BunnyPrefab;
    }


    public static BunnySpawnZone I;


    public bool IsSpawningInProgress { get; private set; }

    Vector3 _boundsBottomLeft;
    float _boundsHeight;

    public void Awake()
    {
        var box = GetComponent<BoxCollider>();
        var bounds = box.bounds;
        _boundsBottomLeft = bounds.min;
        _boundsHeight = bounds.size.z;
    }

    public void OnEnable() =>
        I = this;

    public void OnDisable() =>
        I = null;

    public void Spawn(IList<SpawnInstruction> spawnInstructions)
    {
#if UNITY_EDITOR
        var prevSpawnTime = float.MinValue;
        foreach (var spawnInstruction in spawnInstructions)
        {
            Assert.IsTrue(spawnInstruction.SpawnTime >= prevSpawnTime);
            prevSpawnTime = spawnInstruction.SpawnTime;
        }
#endif

        StartCoroutine(SpawnCoroutine());

        return;


        IEnumerator SpawnCoroutine()
        {
            var curTime = 0f;
            IsSpawningInProgress = true;

            foreach (var spawnInstruction in spawnInstructions)
            {
                var nextSpawnAfter = spawnInstruction.SpawnTime - curTime;
                yield return new WaitForSeconds(nextSpawnAfter);

                curTime = spawnInstruction.SpawnTime;

                ActuallySpawn(spawnInstruction.BunnyPrefab);
            }

            IsSpawningInProgress = false;
        }
    }

    void ActuallySpawn(GameObject prefab)
    {
        var zOffset = Random.Range(0f, _boundsHeight);
        var spawnPoint = new Vector3(
            _boundsBottomLeft.x,
            _boundsBottomLeft.y,
            _boundsBottomLeft.z + zOffset
        );

        Instantiate(prefab, spawnPoint, Quaternion.identity);
    }
}
