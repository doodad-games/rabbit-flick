using System.Collections;
using UnityEngine;

public class LaunchRocketBunnyOnFlicked : MonoBehaviour
{
    [SerializeField] float rocketLaunchTime = 1.2f;
    [SerializeField] float launchSpeed = 24f;
    Bunny _bunny;

    public void OnEnable()
    {
        _bunny = GetComponent<Bunny>();
        _bunny.OnFlicked += LaunchRocket;
    }
    public void OnDisable() =>
        _bunny.OnFlicked -= LaunchRocket;

    void LaunchRocket()
    {
        StartCoroutine(LaunchCycle());
    }

    IEnumerator LaunchCycle()
    {
        float timer = rocketLaunchTime;
        var tfm = transform;
        var euler = tfm.rotation.eulerAngles;
        tfm.rotation = Quaternion.Euler(-90f, euler.y, euler.z);
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            tfm.position += Vector3.up * (launchSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
