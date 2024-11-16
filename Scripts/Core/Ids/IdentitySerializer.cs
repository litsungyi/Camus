using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Camus.Core.Ids
{
    /// <summary>
    /// 對 Identity 作 Jsonlization 的轉換類別
    /// 將繼承 Identity<int> 的類別轉成 "Id" = 0
    /// 將繼承 Identity<string> 的類別轉成 "Id" = "0"
    /// </summary>
    public class IdentitySerializer : JsonConverter
    {
        #region JsonConverter
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Type baseType = objectType.BaseType;
            if (!baseType.IsGenericType || baseType.GetGenericTypeDefinition() != typeof(Identity<>))
            {
                throw new Exception("XxxId must be sealed and inheritance from Identity<>");
            }

            Type idType = baseType.GetGenericArguments()[0];
            if (idType == typeof(string))
            {
                string id = ReadStringId(reader.Value);
                return Activator.CreateInstance(objectType, id);
            }
            else
            {
                MethodInfo methodInfo = typeof(IdentitySerializer).GetMethod("ReadIntegerId", BindingFlags.Static | BindingFlags.NonPublic);
                methodInfo = methodInfo.MakeGenericMethod(new Type[] { idType });
                var parameters = new object[] { reader.Value };
                var id = methodInfo.Invoke(null, parameters);
                return Activator.CreateInstance(objectType, id);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type baseType = value.GetType().BaseType;
            if (!baseType.IsGenericType || baseType.GetGenericTypeDefinition() != typeof(Identity<>))
            {
                throw new Exception("XxxId must be sealed and inheritance from Identity<>");
            }

            Type idType = baseType.GetGenericArguments()[0];
            MethodInfo methodInfo = typeof(IdentitySerializer).GetMethod("GetId", BindingFlags.Static | BindingFlags.NonPublic);
            methodInfo = methodInfo.MakeGenericMethod(new Type[] { idType });
            var parameters = new object[] { value };
            var id = methodInfo.Invoke(null, parameters);
            serializer.Serialize(writer, id);
        }
        #endregion

        #region private
        private static T GetId<T>(Identity<T> identity)
        {
            return identity.Id;
        }

        private static string ReadStringId(object value)
        {
            return (string) value;
        }

        private static TType ReadIntegerId<TType>(object value)
        {
            if (value == null)
            {
                return default(TType);
            }

            return (TType) Convert.ChangeType(value.ToString(), typeof(TType));
        }
        #endregion
    }
}
