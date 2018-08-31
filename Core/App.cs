using Camus.General;
using Camus.Updatables;
using System;

// NOTE: Use global partial singleton for Game extends.
public partial class App : Singleton<App>
{
    private void Update()
    {
        
    }


    private UpdateManager updater;
    public UpdateManager Updater
    {
        get
        {
            if (updater == null)
            {
                updater = gameObject.AddComponent<UpdateManager>();
            }

            return updater;
        }
    }
}
