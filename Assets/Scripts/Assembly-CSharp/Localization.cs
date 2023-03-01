using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Localization")]
public class Localization : MonoBehaviour
{
	private static Localization mInst;

	public string startingLanguage;

	public TextAsset[] languages;

	private Dictionary<string, string> mDictionary = new Dictionary<string, string>();

	private string mLanguage;

	public static Localization instance
	{
		get
		{
			return mInst;
		}
	}

	public string currentLanguage
	{
		get
		{
			if (string.IsNullOrEmpty(mLanguage))
			{
				currentLanguage = PlayerPrefs.GetString("Language");
				if (string.IsNullOrEmpty(mLanguage))
				{
					currentLanguage = startingLanguage;
					if (string.IsNullOrEmpty(mLanguage) && languages.Length > 0)
					{
						currentLanguage = languages[0].name;
					}
				}
			}
			return mLanguage;
		}
		set
		{
			if (languages == null || !(mLanguage != value))
			{
				return;
			}
			if (string.IsNullOrEmpty(value))
			{
				mDictionary.Clear();
			}
			else
			{
				TextAsset[] array = languages;
				foreach (TextAsset textAsset in array)
				{
					if (textAsset != null && textAsset.name == value)
					{
						Load(textAsset);
						return;
					}
				}
			}
			PlayerPrefs.DeleteKey("Language");
		}
	}

	private void Awake()
	{
		if (mInst == null)
		{
			mInst = this;
		}
	}

	private void OnDestroy()
	{
		if (mInst == this)
		{
			mInst = null;
		}
	}

	private void Load(TextAsset asset)
	{
		mLanguage = asset.name;
		PlayerPrefs.SetString("Language", mLanguage);
		ByteReader byteReader = new ByteReader(asset);
		mDictionary = byteReader.ReadDictionary();
		NGUITools.Broadcast("OnLocalize", this);
	}

	public string Get(string key)
	{
		string value;
		return (!mDictionary.TryGetValue(key, out value)) ? key : value;
	}
}
