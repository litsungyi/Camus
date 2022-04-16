using UnityEngine;

public class UpdateTest : MonoBehaviour
{
    public static void Create(UpdateTest prefab, Transform parent, int count)
    {
        var instance = Instantiate(prefab, parent);
        instance.name = $"instance #{count}";
    }

    private void Update()
    {
        transform.localPosition = UpdaterTester.RandomMove(transform.localPosition);
    }
}
