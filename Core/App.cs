using Camus.Core;
using Camus.Utilities;

// NOTE: Use global partial singleton for Game extends.
public partial class App : Singleton<App>
{
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
    }
}
