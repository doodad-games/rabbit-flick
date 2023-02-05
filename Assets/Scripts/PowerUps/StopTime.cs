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
        [SerializeField] float meshRotationDegreesPerSecond = 40;
        bool _rotatingMesh;

        void Awake()
        {
            Assert.IsNotNull(stopCanvas);
            stopCanvas.SetActive(false);
            _rotatingMesh = true;
        }

        void Update()
        {
            if (_rotatingMesh)
            {
                transform.Rotate(new Vector3(0, meshRotationDegreesPerSecond, 0) * Time.deltaTime);
            }
        }

        public void StartStopTime()
        {
            _rotatingMesh = false;
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