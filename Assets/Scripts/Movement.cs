using System;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    protected event Action OnTargetChanged;

    protected bool HasTarget { get; private set; }
    public Vector3 Target => _targetTransform == null ? _targetPoint : _targetTransform.position;

    Transform _targetTransform;
    Vector3 _targetPoint;

    public void SetTarget(Transform target)
    {
        HasTarget = target != null;
        _targetTransform = target;

        OnTargetChanged?.Invoke();
    }

    public void SetTarget(Vector3 target)
    {
        HasTarget = true;
        _targetTransform = null;
        _targetPoint = target;

        OnTargetChanged?.Invoke();
    }
}