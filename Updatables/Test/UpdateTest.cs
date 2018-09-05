using UnityEngine;

public class UpdateTest : MonoBehaviour
{
    private void Update()
    {
        var localPosition = transform.localPosition;
        transform.localPosition = new Vector3(localPosition.x + (Random.value - 0.5f), localPosition.y + (Random.value - 0.5f), localPosition.z);
    }
}
