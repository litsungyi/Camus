using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Camus.Localizables
{
    [Serializable]
    public class LocalData
    {
        public string Key;
        [JsonConverter(typeof(LanguageDictionarySerializer))]
        public IDictionary<Language, string> Values;
    }
}
