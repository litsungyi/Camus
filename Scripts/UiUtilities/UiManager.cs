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

        private IDictionary<string, UiBaseController> Controllers
        {
            get;
        } = new Dictionary<string, UiBaseController>();

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
                Destroy(item.Value.gameObject);
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

        public T CreateUi<T>(string uiName, T prefab) where T : UiBaseController
        {
            if (string.IsNullOrEmpty(uiName))
            {
                Debug.LogError($"Ui name is empty!");
                return null;
            }

            if (Controllers.TryGetValue(uiName, out var target))
            {
                Debug.LogWarning($"Ui {uiName} already created!");
                return target as T;
            }

            var instance = Instantiate(prefab);
            instance.gameObject.name = uiName;
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(Instance.uiRoot, false);
            Controllers.Add(uiName, instance);

            return instance;
        }

        public T CreateAndShowUi<T>(string uiName, T prefab, Action<bool> callback = null) where T : UiBaseController
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

            target.gameObject.SetActive(true);
            callback?.Invoke(true);

            return;
        }

        public void HideUi(string uiName, Action<bool> callback = null)
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

            target.gameObject.SetActive(false);

            callback?.Invoke(true);
        }

        public void DestroyUi(string uiName, Action<bool> callback = null)
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

            target.gameObject.SetActive(false);
            Controllers.Remove(uiName);
            Destroy(target.gameObject);

            callback?.Invoke(true);
        }
    }
}
