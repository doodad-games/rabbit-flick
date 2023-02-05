using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace DefaultNamespace
{
    public class SlowTime : MonoBehaviour
    {
        [SerializeField] float timeSlowMultiplier = 0.4f;
        [SerializeField] float timeResumeDuration = 0.8f;
        [SerializeField] GameObject slowCanvas;

        public static int HowManySlowTimesStacked = 0;

        void Awake()
        {
            Assert.IsNotNull(slowCanvas);
            slowCanvas.SetActive(false);
        }

        public void StartSlowTime()
        {
            StartCoroutine(Wait1FrameBeforeSlowTime());
        }

        IEnumerator Wait1FrameBeforeSlowTime()
        {
            yield return null;
            slowCanvas.SetActive(true);
            HowManySlowTimesStacked++;
            Time.timeScale = timeSlowMultiplier;
        }

        public void EndSlowTime()
        {
            HowManySlowTimesStacked--;
            if (HowManySlowTimesStacked <= 0 && StopTime.HowManyStopTimesStacked <= 0)
            {
                HowManySlowTimesStacked = 0;
                StartCoroutine(SlowlyResume());
            }
            else
            {
                slowCanvas.gameObject.SetActive(false);
            }
        }
        IEnumerator SlowlyResume()
        {
            CanvasGroup fadeOutCanvasGroup = slowCanvas.gameObject.AddComponent<CanvasGroup>();
            fadeOutCanvasGroup.interactable = false;
            fadeOutCanvasGroup.blocksRaycasts = false;
            fadeOutCanvasGroup.alpha = 1;
            float time = 0;
            while (time < timeResumeDuration)
            {
                Time.timeScale = Mathf.Lerp(timeSlowMultiplier, 1, time / timeResumeDuration);
                fadeOutCanvasGroup.alpha = Mathf.Lerp(1, 0, time / timeResumeDuration);
                time += Time.deltaTime;
                yield return null;
            }
            Time.timeScale = 1;
            fadeOutCanvasGroup.alpha = 0;
            slowCanvas.gameObject.SetActive(false);
        }
    }
}