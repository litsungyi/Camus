using UnityEngine;
using UnityEngine.UI;

namespace Camus.Localizables
{
    public class LocalStringUpdater : MonoBehaviour
    {
        [SerializeField] private Text Text = null;
        [SerializeField] private string Key = string.Empty;

        private LocalKey localKey;

        private void Start()
        {
            if (Text == null || string.IsNullOrEmpty(Key))
            {
                return;
            }

            //localKey = LocalKey.Create(Key);
            //Text.text = localKey.Value;
        }

        private void Update()
        {
            if (Text == null || string.IsNullOrEmpty(Key))
            {
                return;
            }

            Text.text = localKey.Value;
        }
    }
}
