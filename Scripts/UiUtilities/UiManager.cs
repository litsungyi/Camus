using System;
using System.Collections.Generic;
using Camus.Validators;
using UnityEngine;

namespace Camus.UiUtilities
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager Instance
        {
            get;
            private set;
        }

        [NotNull, SerializeField]
        private Transform uiRoot;

        private IDictionary<string, IDisplyable> Controllers
        {
            get;
        } = new Dictionary<string, IDisplyable>();

        #region MonoBehaviour

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("UiManager Instance is not null!!");
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            foreach (var item in Controllers)
            {
                item.Value.Demolish();
            }

            Controllers.Clear();
            Instance = null;
        }

        #endregion

        public T CreateEffect<T>(T prefab) where T : MonoBehaviour
        {
            var instance = Instantiate(prefab);
            instance.gameObject.SetActive(true);
            instance.transform.SetParent(Instance.uiRoot, false);

            return instance;
        }

        public UiBaseController<T> CreateUi<T>(string uiName, UiBaseController<T> prefab) where T : UiBaseView
        {
            if (string.IsNullOrEmpty(uiName))
            {
                Debug.LogError($"Ui name is empty!");
                return null;
            }

            if (Controllers.TryGetValue(uiName, out var target))
            {
                Debug.LogWarning($"Ui {uiName} already created!");
                return target as UiBaseController<T>;
            }

            var instance = Instantiate(prefab);
            instance.gameObject.name = uiName;
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(Instance.uiRoot, false);
            Controllers.Add(uiName, instance);

            return instance;
        }

        public UiBaseController<T> CreateAndShowUi<T>(string uiName, UiBaseController<T> prefab, Action<bool> callback = null) where T : UiBaseView
        {
            var target = CreateUi(uiName, prefab);
            if (target == null)
            {
                callback?.Invoke(false);
                return null;
            }

            target.gameObject.SetActive(true);
            callback?.Invoke(true);

            return target;
        }

        public void ShowUi(string uiName, Action<bool> callback = null)
        {
            if (string.IsNullOrEmpty(uiName))
            {
                Debug.LogError($"Ui name is empty!");
                callback?.Invoke(false);
                return;
            }

            if (!Controllers.TryGetValue(uiName, out var target))
            {
                Debug.LogWarning($"Ui {uiName} not created!");
                callback?.Invoke(false);
                return;
            }

            callback?.Invoke(true);
            return;
        }

        public void HideUi(string uiName, Action<bool> callback = null, bool destroy = false)
        {
            if (string.IsNullOrEmpty(uiName))
            {
                Debug.LogError($"Ui name is empty!");
                callback?.Invoke(false);
                return;
            }

            if (!Controllers.TryGetValue(uiName, out var target))
            {
                Debug.LogWarning($"Ui {uiName} not created!");
                callback?.Invoke(false);
                return;
            }

            callback?.Invoke(true);

            if (destroy)
            {
                Controllers.Remove(uiName);
                target.Demolish();
            }
        }
    }
}
