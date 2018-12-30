using UnityEngine;

public class UpdateTest : MonoBehaviour
{
    public static void Create(UpdateTest prefab, Transform parent)
    {
        Object.Instantiate<UpdateTest>(prefab, parent);
    }

    private void Update()
    {
        transform.localPosition = UpdaterTester.RandomMove(transform.localPosition);
    }
}
