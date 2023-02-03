using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum State
    {
        Menu,
        WaitingForSpawnCompletion,
        WaitingForWaveCompletion,
        TransitioningBetweenWaves,
    }


    const float WAVE_TRANSITION_DURATION = 4f;


    public static event Action OnWaveCompleted;
    public static event Action OnWaveStarted;
    public static event Action OnLoseGame;


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
            if (CheckForLoseState())
                return;
            CheckIfFinishedSpawning();
        }
        else if (_curState == State.WaitingForWaveCompletion)
        {
            if (CheckForLoseState())
                return;
            CheckForWaveCompletion();
        }
    }

    public void StartNewGame() =>
        StartNextWave();

    public void StartNextWave()
    {
        if (_waveNum == 0)
            ++_waveNum;
        else _waveNum += Mathf.Max(1, 8 - Carrot.NumDestroyedThisWave);

        _curState = State.WaitingForSpawnCompletion;

        var thingsToSpawn = GenerateThingsToSpawn();
        BunnySpawnZone.I.Spawn(thingsToSpawn);

        OnWaveStarted?.Invoke();
    }

    List<BunnySpawnZone.SpawnInstruction> GenerateThingsToSpawn()
    {
        var thingsToSpawn = new List<BunnySpawnZone.SpawnInstruction>();

        var numBunnies = 12 + _waveNum;
        var totalSpawnDuration = 9 + Mathf.Log(_waveNum / 10f + 1) * 10f;
        var bunnySpawnInterval = totalSpawnDuration / numBunnies;

        var bunnyPool = new WeightedPool<GameObject>();
        bunnyPool.Add(
            weight: ((float)Math.E - Mathf.Log(2 * _waveNum)) * 10 + 50,
            Resources.Load<GameObject>(Constants.Resources.BUNNY_REGULAR_PREFAB)
        );
        bunnyPool.Add(
            weight: GraduallyIncreasingWeight(waveOffset: 0, logMultiplier: 5f, constantMultiplier: 0.05f),
            Resources.Load<GameObject>(Constants.Resources.BUNNY_ARMOURED_PREFAB)
        );
        bunnyPool.Add(
            weight: GraduallyIncreasingWeight(waveOffset: -5, logMultiplier: 1f, constantMultiplier: 0.02f),
            Resources.Load<GameObject>(Constants.Resources.BUNNY_HEAVY_PREFAB)
        );

        for (var i = 0; i != numBunnies; ++i)
            thingsToSpawn.Add(new BunnySpawnZone.SpawnInstruction
            {
                SpawnTime = bunnySpawnInterval * i,
                BunnyPrefab = bunnyPool.PickRandom(),
            });

        return thingsToSpawn;


        float GraduallyIncreasingWeight(int waveOffset, float logMultiplier, float constantMultiplier) =>
            Mathf.Log(_waveNum + waveOffset) * logMultiplier + (_waveNum + waveOffset) * constantMultiplier;
    }

    bool CheckForLoseState()
    {
        if (Carrot.All.Count == 0)
        {
            _curState = State.Menu;
            OnLoseGame?.Invoke();
            return true;
        }

        return false;
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
        OnWaveCompleted?.Invoke();

        yield return new WaitForSeconds(WAVE_TRANSITION_DURATION);

        StartNextWave();
    }
}
