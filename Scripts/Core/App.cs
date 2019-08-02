using System.Collections.Generic;
using Camus;
using Camus.Core;
using Camus.Localizables;
using Camus.Scenes;
using Camus.Utilities;
using Newtonsoft.Json;
using UnityEngine;

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

    public SceneController SceneController
    {
        get;
        private set;
    }

    public LocalizationManager LocalizationManager
    {
        get;
        private set;
    }

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

    public void Initialize()
    {
        var text = Resources.Load<TextAsset>("Localization").text;
        var datas = JsonConvert.DeserializeObject<IList<LocalData>>(text);
        LocalizationManager = new LocalizationManager();
        LocalizationManager.Initialize(Language.TraditionChinese, datas);
    }
}
