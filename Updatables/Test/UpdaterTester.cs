using UnityEngine;

public class UpdaterTester : MonoBehaviour
{
    public UpdatableTest prefab1;
    public UpdateTest prefab2;
    public bool testUpdatable;
    public int amount;

    private void Awake()
    {
        App.Instance.EnableFps(true);

        for (int i = 0; i < amount; ++i)
        {
            if (testUpdatable)
            {
                UpdatableTest.Create(prefab1, transform);
            }
            else
            {
                UpdateTest.Create(prefab2, transform);
            }
        }
    }

    public static Vector3 RandomMove(Vector3 localPosition)
    {
        return new Vector3(localPosition.x + (Random.value - 0.5f), localPosition.y + (Random.value - 0.5f), localPosition.z);
    }
}
