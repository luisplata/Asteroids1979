using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Bootstrap
{
    public class TimeScaleGuard : MonoBehaviour
    {
        bool _intentionalPause;

        void Awake()
        {
            // DontDestroyOnLoad(gameObject);
            EnsureTimeScale();
            StartCoroutine(EnsureNextFrames());
        }

        void Update()
        {
            if (Mathf.Approximately(Time.timeScale, 0f) && !_intentionalPause)
                Time.timeScale = 1f;
        }

        public void Pause()
        {
            _intentionalPause = true;
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            _intentionalPause = false;
            Time.timeScale = 1f;
        }

        void EnsureTimeScale()
        {
            if (Mathf.Approximately(Time.timeScale, 0f))
                Time.timeScale = 1f;
        }

        IEnumerator EnsureNextFrames()
        {
            yield return null;
            EnsureTimeScale();
        }
    }
}