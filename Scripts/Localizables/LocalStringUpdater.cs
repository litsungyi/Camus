using Camus.EventSystems;
using Camus.Localizables.Events;
using Camus.Validators;
using UnityEngine;
using UnityEngine.UI;

namespace Camus.Localizables
{
    public class LocalStringUpdater : MonoBehaviour
    {
        [NotNull, SerializeField] private Text Text;
        [SerializeField] private string Key;

        private LocalKey localKey;

        private void Awake()
        {
            if (string.IsNullOrEmpty(Key))
            {
                return;
            }

            localKey = new LocalKey(Key);
        }

        private void OnEnable()
        {
            EventDispatcher.Register<LocaleChangedEvent>(OnLocaleChange);
            UpdateText();
        }

        private void OnDisable()
        {
            EventDispatcher.Unregister<LocaleChangedEvent>(OnLocaleChange);
        }

        private void OnLocaleChange(LocaleChangedEvent e)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            if (Text == null || localKey == null)
            {
                return;
            }

            Text.text = localKey.Value;
        }

        public void UpdateKey(LocalKey key)
        {
            localKey = key;
            UpdateText();
        }
    }
}
