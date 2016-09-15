using System;
using Camus.General;
using Camus.Localization;

// NOTE: Use global partial singleton for Game extends.
using Zenject;

[Serializable]
public partial class App : Singleton<App>
{
	[Inject]
	public ILacalization Localization
	{
		get;
		private set;
    }
}
