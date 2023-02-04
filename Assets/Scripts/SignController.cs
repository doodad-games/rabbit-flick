using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class SignController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject _playButtonObject;
    [SerializeField] GameObject _waveNumberObject;
    [SerializeField] TextMeshPro _waveNumberText;

    bool _canBePressed;

    public void OnEnable() =>
        RefreshSignage();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_canBePressed)
            return;

#if UNITY_EDITOR
        Assert.IsTrue(GameManager.I.CurState == GameManager.State.Menu);
#endif

        GameManager.I.StartNewGame();
        _canBePressed = false;
    }

    public void RefreshSignage()
    {
        var showPlayButton = GameManager.I.CurState == GameManager.State.Menu;

        if (!showPlayButton)
            _waveNumberText.text = $"{GameManager.I.WaveNum}";

        _canBePressed = showPlayButton;
        _playButtonObject.SetActive(showPlayButton);
        _waveNumberObject.SetActive(!showPlayButton);
    }
}
