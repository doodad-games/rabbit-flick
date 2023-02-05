using MyLibrary;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
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

    public void PlayFarmerWaveCompleteSound() =>
        SoundController.Play(
            Carrot.NumDestroyedThisWave < 3
                ? "Farmer Happy Grunts"
                : "Farmer Displeased Grunts"
        );

    public void PlaySound(string soundName) =>
        SoundController.Play(soundName);

    void HandleWaveTransition() =>
        _waveTransitionDirector.Play();

    void HandleLoseGame() =>
        _loseDirector.Play();
}