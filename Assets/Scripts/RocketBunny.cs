using System.Collections;
using UnityEngine;

public class RocketBunny : MonoBehaviour
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
        StartCoroutine(LifeCycle());
    }
    
    IEnumerator LifeCycle()
    {
        float timer = rocketLaunchTime;
        GameObject parentObject = new GameObject();
        transform.parent = parentObject.transform;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            transform.Translate(-transform.right * launchSpeed * Time.deltaTime);
            if (timer > rocketLaunchTime * 0.7f)
            {
                parentObject.transform.Rotate(parentObject.transform.forward * 2.8f);
            }
            else
            {
                parentObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,90));
                transform.Translate(-transform.right * launchSpeed * Time.deltaTime);
            }
            yield return null;
        }
        Destroy(transform.parent.gameObject);
    }
}
