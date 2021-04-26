using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Camus.Localizables
{
    public class LanguageDictionarySerializer : JsonConverter
    {
        public static IDictionary<TKey, TValue> ReadAsDictionary<TKey, TValue>(JsonReader reader, JsonSerializer serializer)
        {
            var list = (IList<KeyValuePair<TKey, TValue>>) serializer.Deserialize<IList<KeyValuePair<TKey, TValue>>>(reader);
            return ConvertToDictionary(list);
        }

        public static IList<KeyValuePair<TKey, TValue>> ConvertToList<TKey, TValue>(IDictionary<TKey, TValue> values)
        {
            var list = new List<KeyValuePair<TKey, TValue>>();
            foreach (var item in values)
            {
                list.Add(new KeyValuePair<TKey, TValue>(item.Key, item.Value));
            }

            return list;
        }

        #region JsonConverter
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!objectType.IsGenericType)
            {
                throw new Exception("Object must implemented IDictionary<Language, TValue>");
            }

            var genericArguments = objectType.GetGenericArguments();
            if (genericArguments.Length != 2)
            {
                throw new Exception("Object must implemented IDictionary<Language, TValue>");
            }

            var keyType = genericArguments[0];
            var valueType = genericArguments[1];
            var interfaceType = typeof(IDictionary<,>).MakeGenericType(new[] { keyType, valueType });
            if (!interfaceType.IsAssignableFrom(objectType))
            {
                throw new Exception("Object must implemented IDictionary<Identity<TKey>, TValue>");
            }

            MethodInfo methodInfo = typeof(LanguageDictionarySerializer).GetMethod(nameof(ReadAsDictionary), BindingFlags.Static | BindingFlags.Public);
            methodInfo = methodInfo.MakeGenericMethod(new Type[] { keyType, valueType });
            var parameters = new object[] { reader, serializer };
            return methodInfo.Invoke(null, parameters);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var enumerable = value.GetType().GetInterfaces().Where(i => i.Name == "IEnumerable`1").FirstOrDefault();
            if (enumerable == null || enumerable.GetGenericArguments()[0].Name != "KeyValuePair`2")
            {
                throw new Exception("Object must implemented IEnumerable<Language, TValue>");
            }

            var keyValueType = enumerable.GetGenericArguments()[0];
            var keyType = keyValueType.GetGenericArguments()[0];
            var valueType = keyValueType.GetGenericArguments()[1];
            MethodInfo methodInfo = typeof(LanguageDictionarySerializer).GetMethod("ConvertToList", BindingFlags.Static | BindingFlags.Public);
            methodInfo = methodInfo.MakeGenericMethod(new Type[] { keyType, valueType });
            var parameters = new object[] { value };
            var list = methodInfo.Invoke(null, parameters);

            serializer.Serialize(writer, list);
        }
        #endregion

        private static IDictionary<TKey, TValue> ConvertToDictionary<TKey, TValue>(IList<KeyValuePair<TKey, TValue>> values)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in values)
            {
                dictionary.Add(item.Key, item.Value);
            }

            return dictionary;
        }
    }
}
