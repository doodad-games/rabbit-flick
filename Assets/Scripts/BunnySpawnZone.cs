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

    Vector3 _boundsBottomLeft;
    float _boundsHeight;

    public void Awake()
    {
        var box = GetComponent<BoxCollider>();
        var bounds = box.bounds;
        _boundsBottomLeft = bounds.min;
        _boundsHeight = bounds.size.z;
    }

    public void Start()
    {
        var bunnyRegularPrefab = Resources.Load<GameObject>(Constants.Resources.BUNNY_REGULAR_PREFAB);

        var dummySpawnInstructions = new[] {
            new SpawnInstruction { SpawnTime = 0.5f, BunnyPrefab = bunnyRegularPrefab },
            new SpawnInstruction { SpawnTime = 1f, BunnyPrefab = bunnyRegularPrefab },
            new SpawnInstruction { SpawnTime = 1.5f, BunnyPrefab = bunnyRegularPrefab },
            new SpawnInstruction { SpawnTime = 2f, BunnyPrefab = bunnyRegularPrefab },
            new SpawnInstruction { SpawnTime = 2.5f, BunnyPrefab = bunnyRegularPrefab },
            new SpawnInstruction { SpawnTime = 3f, BunnyPrefab = bunnyRegularPrefab },
            new SpawnInstruction { SpawnTime = 3.5f, BunnyPrefab = bunnyRegularPrefab },
            new SpawnInstruction { SpawnTime = 4f, BunnyPrefab = bunnyRegularPrefab },
            new SpawnInstruction { SpawnTime = 4.5f, BunnyPrefab = bunnyRegularPrefab },
        };
        Spawn(dummySpawnInstructions);
    }

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

            foreach (var spawnInstruction in spawnInstructions)
            {
                var nextSpawnAfter = spawnInstruction.SpawnTime - curTime;
                yield return new WaitForSeconds(nextSpawnAfter);

                curTime = spawnInstruction.SpawnTime;

                ActuallySpawn(spawnInstruction.BunnyPrefab);
            }
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
