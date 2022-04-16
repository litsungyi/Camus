using System;
using Camus.Validators;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Camus.UiUtilities
{
    public class ButtonHelper : MonoBehaviour, IButtonProcessingExternalHandler
    {
        [SerializeField, NotNull]
        private Button button;
        private Text uiText;

        private void Awake()
        {
            if (uiText == null)
            {
                uiText = button.GetComponentInChildren<Text>();
            }
        }

        public void SetOnClickedCallback(Action onClicked)
        {
            Assert.IsNotNull(onClicked);

            button.onClick.AddListener(() => onClicked?.Invoke());
        }

        public void SetText(string text)
        {
            if (uiText != null)
            {
                uiText.text = text;
            }
        }

        public void SetColor(Color color)
        {
            button.image.color = color;
        }

        #region IButtonProcessingExternalHandler

        IButtonProcessingHandler IButtonProcessingExternalHandler.Holder
        {
            get;
            set;
        }

        #endregion
    }
}
