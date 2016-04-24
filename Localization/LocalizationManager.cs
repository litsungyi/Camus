using System;
using System.Collections.Generic;
using UnityEngine;

namespace Camus.Localization
{
    // Ref: ISO-639 Language Codes https://zh.wikipedia.org/wiki/ISO_639-1%E4%BB%A3%E7%A0%81%E8%A1%A8
    public enum Language
    {
        None,
        TraditionChinese, // 繁體中文
        SimplifiedChinese, // 簡體中文
        English, // 英語
        Japanese, // 日語
        Portuguese, // 葡萄牙語
        Spanish, // 西班牙語
        French, // 法語
        German, // 德語
        Korean, // 韓語
        Russian, // 俄語
        Arabic, // 阿拉伯語
    }

    [Serializable]
    public partial class LocalizationManager
    {
        private static readonly string InvalidValue = "LOCAL_KEY_NOT_FOUND";

        private IDictionary<Language, IDictionary<string, string>> StringDatas = new Dictionary<Language, IDictionary<string, string>>();
        private IDictionary<string, string> CurrentStringDatas = new Dictionary<string, string>();
        private Language currentLanguage = Language.None;

        public Language CurrentLanguage
        {
             get
             {
                 return currentLanguage;
             }

             set
             {
                 if ( currentLanguage == value )
                 {
                     return;
                 }

                if ( !StringDatas.ContainsKey( value ) )
                {
                    Debug.LogWarning( string.Format( "[LocalizationManager] Unknown language! (Language = {0})", value.ToString() ) );
                    return;
                }

                CurrentStringDatas = StringDatas[ value ];
                currentLanguage = value;
             }
        }

        public LocalizationManager()
        {
            StringDatas = new Dictionary<Language, IDictionary<string, string>> ();
            Initialize();

            if ( !StringDatas.ContainsKey( currentLanguage ) )
            {
                Debug.LogWarning( string.Format( "[LocalizationManager] Language not found! (Language = {0})", currentLanguage.ToString() ) );
                return;
            }

            CurrentStringDatas = StringDatas[ currentLanguage ];
        }

        /// <summary>
        /// Initialize LocalizationManager, use to initialize LocalizationManager.StringDatas and LocalizationManager.currentLanguage
        /// NOTE: This is a partial method, add a partial class LocalizationManager with this partial nethod implement.
        /// </summary>
        partial void Initialize();

        /// <summary>
        /// Get localization string from a LocalKey,
        /// if localKey is null or localKey cannot found in CurrentStringDatas, return null
        /// otherwise return a localization of localKey (equals to localKey.Value)
        /// NOTE: Don't (and can't) use this method directly, use localKey.Value instead
        /// </summary>
        internal string GetLocalString( LocalKey localKey )
        {
            if ( localKey == null )
            {
                Debug.LogWarning( string.Format( "[LocalizationManager] LocalKey is null! (Language = {0})", currentLanguage.ToString() ) );
                return InvalidValue;
            }

            if ( null == CurrentStringDatas || !CurrentStringDatas.ContainsKey( localKey.Key ) )
            {
                Debug.LogWarning( string.Format( "[LocalizationManager] LocalKey not found! (Language = {0}, LocalKey = {1})", currentLanguage.ToString(), localKey.Key ) );
                return InvalidValue;
            }

            return CurrentStringDatas[ localKey.Key ];
        }
    }
}
