using Camus.General;
using Camus.Localization;
using Camus.Updates;

// NOTE: Use global partial singleton for Game extends.
public partial class App : Singleton<App>
{
	public ILocalization Localization
    {
        get;
        private set;
    }

    private UpdateManager updater;
    public UpdateManager Updater
    {
        get
        {
            if(updater == null)
            {
                updater = gameObject.AddComponent<UpdateManager>();
            }

            return updater;
        }
    }
}
