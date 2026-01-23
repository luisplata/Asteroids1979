using _Project.Scripts.Bootstrap;
using UnityEngine;

public static class GamePauseController
{
    public static void Pause()
    {
        GameBootstrap.Instance.SetTimeScale(0f);
    }

    public static void Resume()
    {
        GameBootstrap.Instance.SetTimeScale(1f);
    }

    public static bool IsPaused => Time.timeScale == 0f;
}