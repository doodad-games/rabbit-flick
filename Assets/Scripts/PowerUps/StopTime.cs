using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace DefaultNamespace
{

    public class StopTime : MonoBehaviour
    {
        [SerializeField] GameObject stopCanvas;
        [SerializeField] float timeResumeDuration = 0.08f;
        public static int HowManyStopTimesStacked = 0;

        void Awake()
        {
            Assert.IsNotNull(stopCanvas);
            stopCanvas.SetActive(false);
        }

        public void StartStopTime()
        {
            HowManyStopTimesStacked++;
            stopCanvas.SetActive(true);
            Time.timeScale = 0;
        }

        public void EndStopTime()
        {
            HowManyStopTimesStacked--;
            if (HowManyStopTimesStacked <= 0)
            {
                StartCoroutine(SlowlyResume());
            }
            else
            {
                stopCanvas.gameObject.SetActive(false);
            }
        }
        IEnumerator SlowlyResume()
        {
            CanvasGroup fadeOutCanvasGroup = stopCanvas.gameObject.AddComponent<CanvasGroup>();
            fadeOutCanvasGroup.interactable = false;
            fadeOutCanvasGroup.blocksRaycasts = false;
            fadeOutCanvasGroup.alpha = 1;
            float time = 0;
            while (time < timeResumeDuration)
            {
                Time.timeScale = Mathf.Lerp(0, 1, time / timeResumeDuration);
                fadeOutCanvasGroup.alpha = Mathf.Lerp(1, 0, time / timeResumeDuration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            Time.timeScale = 1;
            fadeOutCanvasGroup.alpha = 0;
            stopCanvas.gameObject.SetActive(false);
        }
    }
}