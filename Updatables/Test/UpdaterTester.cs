using UnityEngine;

public class UpdaterTester : MonoBehaviour
{
    public UpdatableTest prefab1;
    public UpdateTest prefab2;
    public bool testUpdatable;
    public int amount;

	void Awake()
    {
        App.Instance.EnableFps(true);

        if (testUpdatable)
        {
            for (int i = 0; i < amount; ++i)
            {
                var instance = Object.Instantiate<UpdatableTest>(prefab1, transform);
                App.Instance.Updater.Register(instance);
            }
        }
        else
        {
            for (int i = 0; i < amount; ++i)
            {
                Object.Instantiate<UpdateTest>(prefab2, transform);
            }
        }
	}
}
