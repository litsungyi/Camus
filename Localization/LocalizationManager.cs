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
        public GameObject StringSettings = null;

        private IDictionary<Language, IDictionary<LocalKey, string>> StringDatas = new Dictionary<Language, IDictionary<LocalKey, string>> ();
        private IDictionary<LocalKey, string> CurrentStringDatas = new Dictionary<LocalKey, string>();
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
            StringDatas = new Dictionary<Language, IDictionary<LocalKey, string>> ();
            Initialize();

            if ( !StringDatas.ContainsKey( currentLanguage ) )
            {
                Debug.LogWarning( string.Format( "[LocalizationManager] Language not found! (Language = {0})", currentLanguage.ToString() ) );
                return;
            }

            CurrentStringDatas = StringDatas[ currentLanguage ];
        }

        partial void Initialize();

        internal string GetLocalString( LocalKey localKey )
        {
            if ( localKey == null )
            {
                Debug.LogWarning( string.Format( "[LocalizationManager] LocalKey is null! (Language = {0})", currentLanguage.ToString() ) );
                return InvalidValue;
            }

            if ( null == CurrentStringDatas || !CurrentStringDatas.ContainsKey( localKey ) )
            {
                Debug.LogWarning( string.Format( "[LocalizationManager] LocalKey not found! (Language = {0}, LocalKey = {1})", currentLanguage.ToString(), localKey.Key ) );
                return InvalidValue;
            }

            return CurrentStringDatas[ localKey ];
        }
    }
}
