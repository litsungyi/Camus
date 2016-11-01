using Camus.General;
using Camus.Localization;

// NOTE: Use global partial singleton for Game extends.
using Zenject;

public partial class App : Singleton<App>
{
	public ILocalization Localization
    {
        get;
        set;
    }
}
