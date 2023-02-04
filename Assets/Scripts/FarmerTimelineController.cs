using UnityEngine;
using UnityEngine.Playables;

public class FarmerTimelineController : MonoBehaviour
{
    [SerializeReference] PlayableDirector _waveTransitionDirector;
    [SerializeReference] PlayableDirector _loseDirector;

    public void OnEnable()
    {
        GameManager.OnNewGameStarted += HandleWaveTransition;
        GameManager.OnWaveCompleted += HandleWaveTransition;
        GameManager.OnLoseGame += HandleLoseGame;
    }

    public void OnDisable()
    {
        GameManager.OnNewGameStarted -= HandleWaveTransition;
        GameManager.OnWaveCompleted -= HandleWaveTransition;
        GameManager.OnLoseGame -= HandleLoseGame;
    }

    void HandleWaveTransition() =>
        _waveTransitionDirector.Play();

    void HandleLoseGame() =>
        _loseDirector.Play();
}