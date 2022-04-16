using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Camus.Localizables
{
    [Serializable]
    public sealed class LocalizationManager
    {
        private static readonly string InvalidValue = "LOCAL_KEY_NOT_FOUND";
        private static readonly LocalKey InvalidLocalKey = new LocalKey(string.Empty);

        private bool Initialized
        {
            get;
            set;
        }

        private IDictionary<Language, IDictionary<string, string>> StringDatas = new Dictionary<Language, IDictionary<string, string>>();
        private IDictionary<string, string> CurrentStringDatas = new Dictionary<string, string>();
        private IDictionary<string, LocalKey> KeyDatas = new Dictionary<string, LocalKey>();
        private Language currentLanguage = Language.None;

        public IList<Language> AvailableLanguages
        {
            get
            {
                return StringDatas.Keys.ToList();
            }
        }

        public Language CurrentLanguage
        {
            get
            {
                return currentLanguage;
            }

            set
            {
                if (currentLanguage == value)
                {
                    return;
                }

                if (!StringDatas.ContainsKey(value))
                {
                    Debug.LogWarning($"[{nameof(LocalizationManager)}] Unknown language! (Language = {value.ToString()}");
                    return;
                }

                CurrentStringDatas = StringDatas[value];
                currentLanguage = value;

                foreach (var item in KeyDatas.Values)
                {
                    item.IsDirty = true;
                }
            }
        }

        public void Initialize(Language language, IList<LocalData> datas)
        {
            if (Initialized)
            {
                Debug.LogWarning($"[{nameof(LocalizationManager)}] Already initialized!");
                return;
            }

            currentLanguage = language;
            foreach (var data in datas)
            {
                foreach (var value in data.Values)
                {
                    if (!StringDatas.ContainsKey(value.Key))
                    {
                        StringDatas.Add(value.Key, new Dictionary<string, string>());
                    }

                    StringDatas[value.Key].Add(data.Key, value.Value);
                }
            }

            if (!StringDatas.ContainsKey(currentLanguage))
            {
                Debug.LogWarning($"[{nameof(LocalizationManager)}] Language not found! (Language = {currentLanguage.ToString()})");
                Initialized = true;
                return;
            }

            CurrentStringDatas = StringDatas[currentLanguage];
            Initialized = true;
        }

        public LocalizationManager()
        {
            StringDatas = new Dictionary<Language, IDictionary<string, string>>();
        }

        public LocalKey GetLocalKey(string key)
        {
            if (!Initialized)
            {
                Debug.LogWarning($"[{nameof(LocalizationManager)}] Not initialized!");
                return InvalidLocalKey;
            }

            if (string.IsNullOrEmpty(key) || null == KeyDatas)
            {
                Debug.LogWarning($"[{nameof(LocalizationManager)}] Key is null!");
                return InvalidLocalKey;
            }

            if (KeyDatas.ContainsKey(key))
            {
                return KeyDatas[key];
            }

            var localKey = new LocalKey(key);
            KeyDatas.Add(key, localKey);
            return localKey;
        }

        /// <summary>
        /// Get localization string from a LocalKey,
        /// if localKey is null or localKey cannot found in CurrentStringDatas, return null
        /// otherwise return a localization of localKey (equals to localKey.Value)
        /// NOTE: Don't (and can't) use this method directly, use localKey.Value instead
        /// </summary>
        internal string GetLocalString(LocalKey localKey)
        {
            if (!Initialized)
            {
                Debug.LogWarning($"[{nameof(LocalizationManager)}] Not initialized!");
                return $"{InvalidValue}: <Not initialized>";
            }

            if (localKey == null)
            {
                Debug.LogWarning($"[{nameof(LocalizationManager)}] LocalKey is null! (Language = {currentLanguage.ToString()})");
                return $"{InvalidValue}: <null>";
            }

            if (CurrentStringDatas == null|| !CurrentStringDatas.ContainsKey(localKey.Key))
            {
                Debug.LogWarning($"[{nameof(LocalizationManager)}] LocalKey not found! (Language = {currentLanguage.ToString()}, LocalKey = {localKey.Key})");
                return $"{InvalidValue}: {localKey.Key}";
            }

            return CurrentStringDatas[localKey.Key];
        }
    }
}
