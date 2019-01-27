using System;
using Camus.Core;
using Camus.Utilities;

// NOTE: Use global partial singleton for Game extends.
public partial class App : Singleton<App>
{
    private enum LogTag
    {
        HIGH,
        LOW,
        MIDDLE,
    }

    private FPS fps;

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void EnableFps(bool enable)
    {
        if (enable && fps == null)
        {
            fps = gameObject.AddComponent<FPS>();
        }
        else if (fps != null)
        {
            fps.gameObject.SetActive(enable);
        }
    }

    public void Awake()
    {
#if UNITY_EDITOR
        EnableFps(true);
#endif

        Logger.Log("App Awake");
        Logger.LogWarning("App Awake LogWarning");
        Logger.LogError("App Awake LogError");
        Logger.Log(LogTag.HIGH, "App Awake");
        Logger.LogWarning(LogTag.LOW, "App Awake LogWarning");
        Logger.LogError(LogTag.MIDDLE, "App Awake LogError");
        try
        {
            throw new Exception("Error", new Exception("Inner"));
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
        }
    }
}
