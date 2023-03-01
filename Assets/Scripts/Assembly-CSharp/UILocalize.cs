using UnityEngine;

[AddComponentMenu("NGUI/UI/Localize")]
[RequireComponent(typeof(UIWidget))]
public class UILocalize : MonoBehaviour
{
	public string key;

	private string mLanguage;

	private void OnLocalize(Localization loc)
	{
		if (mLanguage != loc.currentLanguage)
		{
			UIWidget component = GetComponent<UIWidget>();
			UILabel uILabel = component as UILabel;
			UISprite uISprite = component as UISprite;
			if (string.IsNullOrEmpty(mLanguage) && string.IsNullOrEmpty(key) && uILabel != null)
			{
				key = uILabel.text;
			}
			string text = ((!string.IsNullOrEmpty(key)) ? loc.Get(key) : loc.Get(component.name));
			if (uILabel != null)
			{
				uILabel.text = text;
			}
			else if (uISprite != null)
			{
				uISprite.spriteName = text;
			}
			mLanguage = loc.currentLanguage;
		}
	}

	private void OnEnable()
	{
		if (Localization.instance != null)
		{
			OnLocalize(Localization.instance);
		}
	}
}
