using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Camus.Core.Ids
{
    /// <summary>
    /// 用來作為識別碼的類別
    /// 目前只支援 int, uint, long, ulong, string
    /// </summary>
    /// <typeparam name="T">實際的識別碼型別</typeparam>
    [Serializable]
    [JsonConverter(typeof(IdentitySerializer))]
    public abstract class Identity<T>
    {
        [SerializeField] private T id;

        /// <summary>
        /// 建構識別碼
        /// </summary>
        /// <param name="id">唯一識別碼</param>
        public Identity(T id)
        {
            this.id = id;
        }

        /// <summary>
        /// 唯一識別碼
        /// </summary>
        public T Id
        {
            get
            {
                return id;
            }
        }

        #region Equal
        public static bool operator ==(Identity<T> lhs, Identity<T> rhs)
        {
            if (object.ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object) lhs == null) || ((object) rhs == null))
            {
                return false;
            }

            if (lhs.Id == null)
            {
                return false;
            }

            // Return true if the fields match:
            return lhs.Id.Equals(rhs.Id);
        }

        public static bool operator !=(Identity<T> lhs, Identity<T> rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Identity<T> that = obj as Identity<T>;
            if ((object) that == null)
            {
                return false;
            }

            if (this.Id == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.Id.Equals(that.Id);
        }

        public bool Equals(Identity<T> that)
        {
            if ((object) that == null)
            {
                return false;
            }

            if (this.Id == null)
            {
                return false;
            }

            return this.Id.Equals(that.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return this.Id.ToString();
        }
        #endregion
    }
}
