using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScaleUpAndShake : MonoBehaviour
{
    public void Use(
        float baseScaleUpTime,
        float scaleUpTimeVariance,
        float shakeRandomPositionInterval,
        float shakeDistance,
        Action finishedCallback = null
    )
    {
        StartCoroutine(ScaleUpAndShake());
        return;


        IEnumerator ScaleUpAndShake()
        {
            var tfm = transform;
            tfm.localScale = Vector3.zero;

            var scaleUpTime = baseScaleUpTime + scaleUpTimeVariance * Random.Range(-1f, 1f);

            var timeElapsed = 0f;
            var nextShakeAfter = 0f;
            while (timeElapsed <= scaleUpTime)
            {
                tfm.localScale = Vector3.one * timeElapsed / scaleUpTime;

                if (nextShakeAfter <= 0f)
                {
                    nextShakeAfter += shakeRandomPositionInterval;
                    tfm.localPosition = Random.insideUnitSphere * shakeDistance;
                }

                yield return null;
                timeElapsed += Time.deltaTime;
                nextShakeAfter -= Time.deltaTime;
            }

            tfm.localPosition = Vector3.zero;
            tfm.localScale = Vector3.one;

            finishedCallback?.Invoke();
        }
    }
}