using System.Collections;
using UnityEngine;

public class ScaleDownAndDestroy : MonoBehaviour
{
    public void Use(float scaleDownTime, GameObject destroyTarget, bool useScaledTime = true)
    {
        StartCoroutine(ScaleDownAndDestroy());
        return;


        IEnumerator ScaleDownAndDestroy()
        {
            var tfm = transform;

            var timeElapsed = 0f;
            while (timeElapsed <= scaleDownTime)
            {
                tfm.localScale = Vector3.one * (1f - timeElapsed / scaleDownTime);

                yield return null;
                timeElapsed += useScaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
            }

            Destroy(destroyTarget);
        }
    }
}