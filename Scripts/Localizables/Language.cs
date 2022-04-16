using System;

namespace Camus.Localizables
{
    // Ref: ISO-639 Language Codes https://zh.wikipedia.org/wiki/ISO_639-1%E4%BB%A3%E7%A0%81%E8%A1%A8
    [Serializable]
    public enum Language
    {
        None,
        Zh_Tw, // TraditionChinese, // 繁體中文
        Zh_Cn, // SimplifiedChinese, // 簡體中文
        En, // English, // 英語
        Ja, // Japanese, // 日語
        Pt, // Portuguese, // 葡萄牙語
        Es, // Spanish, // 西班牙語
        Fr, // French, // 法語
        De, // German, // 德語
        Ko, // Korean, // 韓語
        Ru, // Russian, // 俄語
        Ar, // Arabic, // 阿拉伯語
    }
}
