using Camus.Updatables;
using UnityEngine;

public class UpdatableTest : MonoBehaviour, IUpdatable
{
    public static void Create(UpdatableTest prefab, Transform parent)
    {
        var instance = Object.Instantiate<UpdatableTest>(prefab, parent);
        App.Instance.Updater.Register(instance);
    }

    void IUpdatable.OnUpdate(float duration)
    {
        transform.localPosition = UpdaterTester.RandomMove(transform.localPosition);
    }
}
