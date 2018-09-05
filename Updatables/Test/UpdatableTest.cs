using Camus.Updatables;
using UnityEngine;

public class UpdatableTest : MonoBehaviour, IUpdatable
{
    void IUpdatable.OnUpdate(float duration)
    {
        //Debug.Log("OnUpdate");
        var localPosition = transform.localPosition;
        transform.localPosition = new Vector3(localPosition.x + Random.value, localPosition.y + Random.value, localPosition.z);
    }
}
