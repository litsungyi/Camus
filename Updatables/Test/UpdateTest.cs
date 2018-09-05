using UnityEngine;

public class UpdateTest : MonoBehaviour
{
    private void Update()
    {
        //Debug.Log("Update");
        var localPosition = transform.localPosition;
        transform.localPosition = new Vector3(localPosition.x + Random.value, localPosition.y + Random.value, localPosition.z);
    }
}
