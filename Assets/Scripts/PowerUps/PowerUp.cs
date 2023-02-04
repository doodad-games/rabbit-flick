using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class PowerUp : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] float _powerUpDuration = 3.5f;
        [SerializeField] UnityEvent _startPowerUpEvent;
        [SerializeField] UnityEvent _endPowerUpEvent;

        void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Click Colliders");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            StartCoroutine(ActivatePowerUp());
        }

        IEnumerator ActivatePowerUp()
        {
            TurnOffAllCollidersMeshesAndRigidbodies();
            _startPowerUpEvent?.Invoke();
            yield return new WaitForSecondsRealtime(_powerUpDuration);
            _endPowerUpEvent?.Invoke();
            Destroy(gameObject);
        }

        void TurnOffAllCollidersMeshesAndRigidbodies()
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

            foreach (var powerUpCollider in colliders)
            {
                powerUpCollider.enabled = false;
            }
            foreach (var powerUpMeshRender in meshRenderers)
            {
                powerUpMeshRender.enabled = false;
            }
        }
    }
}