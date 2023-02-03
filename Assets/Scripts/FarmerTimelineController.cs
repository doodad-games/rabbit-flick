using System;
using UnityEngine;
using UnityEngine.Playables;

public class FarmerTimelineController : MonoBehaviour
{
    [SerializeReference] PlayableDirector _waveTransitionDirector;
    [SerializeReference] PlayableDirector _loseDirector;

    public void OnEnable()
    {
        GameManager.OnWaveCompleted += HandleWaveCompleted;
        GameManager.OnLoseGame += HandleLoseGame;
    }

    public void OnDisable()
    {
        GameManager.OnWaveCompleted -= HandleWaveCompleted;
        GameManager.OnLoseGame -= HandleLoseGame;
    }

    void HandleWaveCompleted() =>
        _waveTransitionDirector.Play();

    void HandleLoseGame() =>
        _loseDirector.Play();
}