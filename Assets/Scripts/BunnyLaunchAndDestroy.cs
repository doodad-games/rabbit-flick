using UnityEngine;

public class BunnyLaunchAndDestroy : MonoBehaviour
{
    Bunny _bunny;

    [SerializeField] float _explosionForce = 14.1f;
    [SerializeField] float _disappearAfter = 2.1f;

    public void OnEnable()
    {
        _bunny = GetComponent<Bunny>();
        _bunny.OnDestroyed += LaunchSelf;
    }
    public void OnDisable() =>
        _bunny.OnDestroyed -= LaunchSelf;

    void LaunchSelf() =>
        gameObject.AddComponent<LaunchAndDestroy>()
            .ExecuteLaunchAndDestroy(_explosionForce, _disappearAfter);
}