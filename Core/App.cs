using System;
using Camus.General;
using Camus.Localization;

// NOTE: Use global partial singleton for Game extends.
[Serializable]
public partial class App : Singleton<App>
{
    private LocalizationManager localizationManager = null;

    public LocalizationManager Localization
    {
        get
        {
            if ( null == localizationManager )
            {
                Create( ref localizationManager, () => { return new LocalizationManager(); } );
            }

            return localizationManager;
        }
    }

#region Create
    private object createLock = new object();

    // NOTE: Use double-lock to keep performance and thread-safe.
    private void Create<T>( ref T createdObject, Func<T> creator )
    {
        lock ( createLock )
        {
            if ( null == createdObject )
            {
                createdObject = creator();
            }
        }
    }
#endregion  // Create
}
