#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class TimeScaleResetOnPlay
{
    static TimeScaleResetOnPlay()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                Time.timeScale = 1f;
            }
        };
    }
}
#endif