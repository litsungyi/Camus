using System;

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

	public interface ILacalization
	{
		LocalKey GetLocalKey (string key);
		string GetLocalString (LocalKey localKey);

		Language CurrentLanguage
		{
			get;
			set;
		}
	}
}

