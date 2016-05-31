using System;
using Camus.General;
using Camus.Localization;
using Zenject;

// NOTE: Use global partial singleton for Game extends.


[Serializable]
public partial class App : Singleton<App>
{
	[PostInject]
    public LocalizationManager Localization
	{
		get;
		private set;
    }
}
