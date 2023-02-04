using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    protected bool HasTarget { get; private set; }
    public Vector3 Target =>
        _targetTransform == null ? _targetPoint : _targetTransform.position;

    Transform _targetTransform;
    Vector3 _targetPoint;

    public void SetTarget(Transform target)
    {
        HasTarget = target != null;
        _targetTransform = target;
    }

    public void SetTarget(Vector3 target)
    {
        HasTarget = true;
        _targetTransform = null;
        _targetPoint = target;
    }
}