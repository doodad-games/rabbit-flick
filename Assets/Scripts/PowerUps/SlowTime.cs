using UnityEngine;
using UnityEngine.Assertions;

namespace DefaultNamespace
{
    public class SlowTime : MonoBehaviour
    {
        [SerializeField] float timeSlow = 0.4f;
        [SerializeField] GameObject slowCanvas;

        void Awake()
        {
            Assert.IsNotNull(slowCanvas);
            slowCanvas.SetActive(false);
        }

        public void StartSlowTime()
        {
            slowCanvas.SetActive(true);
            Time.timeScale = timeSlow;
        }

        public void EndSlowTime()
        {
            Time.timeScale = 1;
            slowCanvas.SetActive(false);
        }
    }
}