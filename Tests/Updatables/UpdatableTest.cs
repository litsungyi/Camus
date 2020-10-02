using Camus.Updatables;
using UnityEngine;

public class UpdatableTest : MonoBehaviour, IUpdatable
{
    public static void Create(UpdatableTest prefab, Transform parent, int count)
    {
        var instance = Instantiate(prefab, parent);
        instance.name = $"instance #{count}";
        App.Instance.Updater.Register(instance);
    }

    void IUpdatable.OnUpdate(float duration)
    {
        transform.localPosition = UpdaterTester.RandomMove(transform.localPosition);
    }
}
