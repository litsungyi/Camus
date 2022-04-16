using System;
using UnityEngine;

namespace Camus.Localizables
{
    // Localization Key
    [Serializable]
    public sealed class LocalKey
    {
        internal LocalKey(string key)
        {
            localKey = key;
            IsDirty = true;
        }

        //public static LocalKey Create(string key)
        //{
        //    return App.Instance.Localization.GetLocalKey(key);
        //}

        [SerializeField]
        private string localKey;

        public string Key => localKey;

        public string Value
        {
            get
            {
                if (IsDirty)
                {
                    CatchedValue = App.Instance.LocalizationManager.GetLocalString(this);
                    IsDirty = false;
                }

                return CatchedValue;
            }
        }

        public bool IsDirty
        {
            get;
            internal set;
        }

        private string CatchedValue
        {
            get;
            set;
        }

        #region Equals
        public override string ToString()
        {
            return Key;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LocalKey);
        }

        public bool Equals(LocalKey obj)
        {
            return obj != null && obj.GetHashCode() == this.GetHashCode();
        }
        #endregion // Equals

        //public static implicit operator string(LocalKey localKey)
        //{
        //    return App.Instance.Localization.GetLocalString(localKey);
        //}
    }
}
