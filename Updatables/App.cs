using Camus.Updatables;

public partial class App
{
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
