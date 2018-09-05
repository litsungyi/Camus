using Camus.Updatables;
using UnityEngine;

public class UpdatableTest : MonoBehaviour, IUpdatable
{
    void IUpdatable.OnUpdate(float duration)
    {
        var localPosition = transform.localPosition;
        transform.localPosition = new Vector3(localPosition.x + (Random.value - 0.5f), localPosition.y + (Random.value - 0.5f), localPosition.z);
    }
}
