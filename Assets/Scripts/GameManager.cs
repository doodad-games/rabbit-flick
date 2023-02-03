using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const float WAVE_TRANSITION_DURATION = 4f;

    enum State
    {
        Menu,
        WaitingForSpawnCompletion,
        WaitingForWaveCompletion,
        TransitioningBetweenWaves,
    }


    int _waveNum = 0;
    State _curState = State.Menu;

    public void Start() =>
        StartNewGame();

    public void Update()
    {
        if (_curState is State.Menu or State.TransitioningBetweenWaves)
            return;

        if (_curState == State.WaitingForSpawnCompletion)
        {
            CheckForLoseState();
            CheckIfFinishedSpawning();
        }
        else if (_curState == State.WaitingForWaveCompletion)
        {
            CheckForLoseState();
            CheckForWaveCompletion();
        }
    }

    public void StartNewGame() =>
        StartNextWave();

    public void StartNextWave()
    {
        ++_waveNum;
        _curState = State.WaitingForSpawnCompletion;

        var thingsToSpawn = GenerateThingsToSpawn();
        BunnySpawnZone.I.Spawn(thingsToSpawn);
    }

    List<BunnySpawnZone.SpawnInstruction> GenerateThingsToSpawn()
    {
        var thingsToSpawn = new List<BunnySpawnZone.SpawnInstruction>();

        var numBunnies = 12 + _waveNum * 2;
        var totalSpawnDuration = 12 + _waveNum * 0.5f;
        var bunnySpawnInterval = totalSpawnDuration / numBunnies;

        var bunnyPool = new WeightedPool<GameObject>();
        bunnyPool.Add(
            weight: 1f,
            Resources.Load<GameObject>(Constants.Resources.BUNNY_REGULAR_PREFAB)
        );

        for (var i = 0; i != numBunnies; ++i)
            thingsToSpawn.Add(new BunnySpawnZone.SpawnInstruction
            {
                SpawnTime = bunnySpawnInterval * i,
                BunnyPrefab = bunnyPool.PickRandom(),
            });

        return thingsToSpawn;
    }

    void CheckForLoseState()
    {
        // TODO
    }

    void CheckIfFinishedSpawning()
    {
        if (!BunnySpawnZone.I.IsSpawningInProgress)
            _curState = State.WaitingForWaveCompletion;
    }

    void CheckForWaveCompletion()
    {
        if (Bunny.All.Count == 0)
            StartCoroutine(TransitionToNextWave());
    }

    IEnumerator TransitionToNextWave()
    {
        _curState = State.TransitioningBetweenWaves;

        yield return new WaitForSeconds(WAVE_TRANSITION_DURATION);

        StartNextWave();
    }
}
