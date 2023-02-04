using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class PowerUp : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] float _powerUpDuration = 3.5f;
        [SerializeField] GameObject _visuals;
        [SerializeField] UnityEvent _startPowerUpEvent;
        [SerializeField] UnityEvent _endPowerUpEvent;

        public void Awake() =>
            gameObject.layer = LayerMask.NameToLayer("Click Colliders");

        public void OnEnable() =>
            GameManager.OnLoseGame += DestroyPowerUp;
        public void OnDisable() =>
            GameManager.OnLoseGame -= DestroyPowerUp;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!enabled)
                return;
            StartCoroutine(ActivatePowerUp());
        }

        IEnumerator ActivatePowerUp()
        {
            enabled = false;
            DoDestructionAnimation();
            _startPowerUpEvent?.Invoke();
            yield return new WaitForSecondsRealtime(_powerUpDuration);
            _endPowerUpEvent?.Invoke();
            yield return EventuallySelfDestruct();
        }

        IEnumerator EventuallySelfDestruct()
        {
            yield return new WaitForSecondsRealtime(10f); // This is to make sure any other effects are lost.
            Destroy(gameObject);
        }

        void DestroyPowerUp()
        {
            enabled = false;
            DoDestructionAnimation();
            StartCoroutine(EventuallySelfDestruct());
        }

        void DoDestructionAnimation() =>
            _visuals.AddComponent<ScaleDownAndDestroy>()
                .Use(scaleDownTime: 0.5f, destroyTarget: _visuals, useScaledTime: false);
    }
}