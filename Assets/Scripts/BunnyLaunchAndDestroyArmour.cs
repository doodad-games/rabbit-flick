using UnityEngine;
using UnityEngine.Serialization;

public class BunnyLaunchAndDestroyArmour : MonoBehaviour
{
    Bunny _bunny;

    [FormerlySerializedAs("armour")] [SerializeField] GameObject _armour;
    [SerializeField] float _explosionForce = 14.1f;
    [SerializeField] float _disappearAfter = 2.1f;

    public void OnEnable()
    {
        _bunny = GetComponentInParent<Bunny>();
        _bunny.OnReachedLowLife += LaunchArmour;
    }
    public void OnDisable() =>
        _bunny.OnReachedLowLife -= LaunchArmour;

    public void LaunchArmour()
    {
        if (_armour != null)
        {
            _armour.AddComponent<LaunchAndDestroy>()
                .ExecuteLaunchAndDestroy(_explosionForce, _disappearAfter);
        }
    }
}