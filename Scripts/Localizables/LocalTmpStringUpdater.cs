using Camus.EventSystems;
using Camus.Localizables.Events;
using Camus.Validators;
using UnityEngine;
using TMPro;

namespace Camus.Localizables
{
    /// <summary>
    /// Same as <see cref="LocalStringUpdater"/>, for a TextMeshPro text.
    /// </summary>
    public class LocalTmpStringUpdater : MonoBehaviour
    {
        [NotNull, SerializeField] private TMP_Text Text;
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
            if (localKey != null)
            {
                localKey.IsDirty = true;
            }

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
