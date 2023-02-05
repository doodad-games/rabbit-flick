using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(EXEC_ORDER)]
public class GameManager : MonoBehaviour
{
    [Serializable]
    public enum State
    {
        Menu,
        WaitingForSpawnCompletion,
        WaitingForWaveCompletion,
        TransitioningBetweenWaves,
    }


    const int EXEC_ORDER = -100;
    // WARNING: Changing this has side-effects on the timeline animation -
    // the number on the sign may change if the sign is thrown before or after this duration!
    const float WAVE_TRANSITION_DURATION = 2f;


    public static event Action OnNewGameStarted;
    public static event Action OnWaveCompleted;
    public static event Action OnWaveStarted;
    public static event Action OnLoseGame;

    public static GameManager I { get; private set; }


    public State CurState { get; private set; } = State.Menu;
    public int WaveNum { get; private set; } = 0;

    public void OnEnable() =>
        I = this;

    public void OnDisable() =>
        I = null;

    public void Start() =>
        CarrotTitle.I.Show();

    public void Update()
    {
        if (CurState is State.Menu or State.TransitioningBetweenWaves)
            return;

        if (CurState == State.WaitingForSpawnCompletion)
        {
            if (CheckForLoseState())
                return;
            CheckIfFinishedSpawning();
        }
        else if (CurState == State.WaitingForWaveCompletion)
        {
            if (CheckForLoseState())
                return;
            CheckForWaveCompletion();
        }
    }

    public void StartNewGame()
    {
        CarrotTitle.I.Hide();
        CarrotSpawnZone.I.SpawnCarrots();
        WaveNum = 0;
        StartCoroutine(TransitionToNextWave());
    }

    public void StartNextWave()
    {
        CurState = State.WaitingForSpawnCompletion;

        var thingsToSpawn = GenerateThingsToSpawn();
        BunnySpawnZone.I.Spawn(thingsToSpawn);

        OnWaveStarted?.Invoke();
    }

    List<BunnySpawnZone.SpawnInstruction> GenerateThingsToSpawn()
    {
        var thingsToSpawn = new List<BunnySpawnZone.SpawnInstruction>();

        var numBunnies = 12 + WaveNum;
        var totalSpawnDuration = 9 + Mathf.Log(WaveNum / 10f + 1) * 10f;
        var bunnySpawnInterval = totalSpawnDuration / numBunnies;

        var bunnyPool = new WeightedPool<GameObject>();
        bunnyPool.Add(
            weight: ((float)Math.E - Mathf.Log(2 * WaveNum)) * 10 + 50,
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
        bunnyPool.Add(
            weight: GraduallyIncreasingWeight(waveOffset: 0, logMultiplier: 1f, constantMultiplier: 0.02f),
            Resources.Load<GameObject>(Constants.Resources.BUNNY_ROCKET_PREFAB)
        );

        for (var i = 0; i != numBunnies; ++i)
            thingsToSpawn.Add(new BunnySpawnZone.SpawnInstruction
            {
                SpawnTime = bunnySpawnInterval * i,
                BunnyPrefab = bunnyPool.PickRandom(),
            });

        return thingsToSpawn;


        float GraduallyIncreasingWeight(int waveOffset, float logMultiplier, float constantMultiplier) =>
            Mathf.Log(WaveNum + waveOffset) * logMultiplier + (WaveNum + waveOffset) * constantMultiplier;
    }

    bool CheckForLoseState()
    {
        if (Carrot.All.Count == 0)
        {
            CurState = State.Menu;
            OnLoseGame?.Invoke();

            StartCoroutine(ShowTitleSoon());

            return true;
        }

        return false;
    }

    void CheckIfFinishedSpawning()
    {
        if (!BunnySpawnZone.I.IsSpawningInProgress)
            CurState = State.WaitingForWaveCompletion;
    }

    void CheckForWaveCompletion()
    {
        if (Bunny.All.Count == 0)
            StartCoroutine(TransitionToNextWave());
    }

    IEnumerator TransitionToNextWave()
    {
        CurState = State.TransitioningBetweenWaves;

        if (WaveNum == 0)
            OnNewGameStarted?.Invoke();
        else OnWaveCompleted?.Invoke();

        if (WaveNum == 0)
            ++WaveNum;
        else WaveNum += Mathf.Min(
            Mathf.Max(1, 12 - Carrot.NumDestroyedThisWave),
            Carrot.All.Count
        );

        yield return new WaitForSeconds(WAVE_TRANSITION_DURATION);

        StartNextWave();
    }

    IEnumerator ShowTitleSoon()
    {
        yield return new WaitForSeconds(3f);

        if (CurState != State.Menu)
            yield break;

        CarrotTitle.I.Show();
    }
}
