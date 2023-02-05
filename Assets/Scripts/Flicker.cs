using UnityEngine;
using UnityEngine.InputSystem;

public class Flicker : MonoBehaviour
{
    const float FLICK_INTERVAL = 0.2f;


    bool _isFlicking;
    float _nextFlickAfter;


    public void OnEnable() =>
        Bunny.OnHoveredBunnyChanged += HandleHoveredBunnyChanged;
    public void OnDisable() =>
        Bunny.OnHoveredBunnyChanged -= HandleHoveredBunnyChanged;

    public void Update()
    {
        if (_isFlicking)
        {
            _nextFlickAfter -= Time.unscaledDeltaTime;
            while (_nextFlickAfter < 0f)
            {
                _nextFlickAfter += FLICK_INTERVAL;
                Flick();
            }
        }
    }

    public void HandleFlickChanged(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        var startFlicking = ctx.ReadValue<float>() == 1f;
        if (startFlicking)
            StartFlicking();
        else
            StopFlicking();
    }

    void HandleHoveredBunnyChanged()
    {
        /*
        if (_isFlicking)
            Flick();
        */
    }

    void StartFlicking()
    {
        if (_isFlicking)
            return;

        Flick();
        _isFlicking = true;
        _nextFlickAfter = FLICK_INTERVAL;
    }

    void StopFlicking()
    {
        if (!_isFlicking)
            return;

        _isFlicking = false;
    }

    void Flick() =>
        Bunny.CurrentlyHovered?.TakeFlick();
}