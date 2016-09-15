using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Camus.Localization
{
    [Serializable]
	public partial class LocalizationManager : ILacalization
	{
        private static readonly string InvalidValue = "LOCAL_KEY_NOT_FOUND";
        private static readonly LocalKey InvalidLocalKey = new LocalKey( string.Empty );

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

                foreach ( var item in KeyDatas.Values )
                {
                    item.IsDirty = true;
                }
             }
        }

        public LocalizationManager()
        {
            StringDatas = new Dictionary<Language, IDictionary<string, string>> ();
			OnInitialize ();

            if ( !StringDatas.ContainsKey( currentLanguage ) )
            {
                Debug.LogWarning( string.Format( "[LocalizationManager] Language not found! (Language = {0})", currentLanguage.ToString() ) );
                return;
            }

            CurrentStringDatas = StringDatas[ currentLanguage ];
        }

        internal LocalKey GetLocalKey( string key )
        {
            if ( string.IsNullOrEmpty( key ) )
            {
                Debug.LogWarning( "[LocalizationManager] key is null!" );
                return InvalidLocalKey;
            }

            if ( null == KeyDatas || !KeyDatas.ContainsKey( key ) )
            {
                return KeyDatas[ key ];
            }

            var localKey = new LocalKey( key );
            KeyDatas.Add( key, localKey );
            return localKey;
        }

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
