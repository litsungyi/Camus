using Camus.Updatables;
using Camus.Utilities;
using UnityEngine;

public partial class App
{
    [ReadOnly, SerializeField] private UpdateManagerHost updaterHost;

    private UpdateManager internalUpdater;

    internal UpdateManager InternalUpdater
    {
        get
        {
            if (updaterHost == null)
            {
                updaterHost = gameObject.AddComponent<UpdateManagerHost>();
            }

            if (internalUpdater == null)
            {
                internalUpdater = new UpdateManager();
                updaterHost.Add(UpdateManagerHost.Priority.VeryHigh, internalUpdater);
            }

            return internalUpdater;
        }
    }

    private UpdateManager updater;

    public UpdateManager Updater
    {
        get
        {
            if (updaterHost == null)
            {
                updaterHost = gameObject.AddComponent<UpdateManagerHost>();
            }

            if (updater == null)
            {
                updater = new UpdateManager();
                updaterHost.Add(UpdateManagerHost.Priority.Middle, updater);
            }

            return updater;
        }
    }
}
