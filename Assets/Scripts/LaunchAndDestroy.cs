using System.Collections;
using UnityEngine;

public class LaunchAndDestroy : MonoBehaviour
{
    public void ExecuteLaunchAndDestroy(float explosionForce, float disappearAfter) =>
        StartCoroutine(LifeCycle(explosionForce, disappearAfter, upwardsModifier: explosionForce));
    public void ExecuteLaunchAndDestroy(float explosionForce, float disappearAfter, float upwardsModifier) =>
        StartCoroutine(LifeCycle(explosionForce, disappearAfter, upwardsModifier));

    IEnumerator LifeCycle(float explosionForce, float disappearAfter, float upwardsModifier)
    {
        transform.parent = null;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        var armourRigidbody = gameObject.AddComponent<Rigidbody>();
        armourRigidbody.AddExplosionForce(
            explosionForce,
             new Vector3(
                 Random.Range(1.0f,-1.0f),
                 1,
                 Random.Range(1.0f,-1.0f)
            ),
            explosionForce,
            upwardsModifier,
            ForceMode.Impulse
        );

        yield return new WaitForSeconds(disappearAfter);

        while (transform.localScale.sqrMagnitude > 0.01f)
        {
            var tfm = transform;
            var scale = tfm.localScale;
            var newScale  = new Vector3(scale.x, scale.y, scale.z) * (1 - Time.deltaTime * 5);
            tfm.localScale = newScale;
            yield return null;
        }

        Destroy(gameObject);
    }
}