﻿using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace DefaultNamespace
{
    public class SlowTime : MonoBehaviour
    {
        [SerializeField] float timeSlowMultiplier = 0.4f;
        [SerializeField] float timeResumeDuration = 0.8f;
        [SerializeField] GameObject slowCanvas;

        void Awake()
        {
            Assert.IsNotNull(slowCanvas);
            slowCanvas.SetActive(false);
        }

        public void StartSlowTime()
        {
            slowCanvas.SetActive(true);
            Time.timeScale = timeSlowMultiplier;
        }

        public void EndSlowTime()
        {
            StartCoroutine(SlowlyResume());
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
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            Time.timeScale = 1;
            fadeOutCanvasGroup.alpha = 0;
            slowCanvas.gameObject.SetActive(false);
        }
    }
}