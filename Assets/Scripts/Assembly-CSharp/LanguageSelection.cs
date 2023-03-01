using UnityEngine;

[RequireComponent(typeof(UIPopupList))]
[AddComponentMenu("NGUI/Interaction/Language Selection")]
public class LanguageSelection : MonoBehaviour
{
	private UIPopupList mList;

	private void Start()
	{
		mList = GetComponent<UIPopupList>();
		UpdateList();
		mList.eventReceiver = base.gameObject;
		mList.functionName = "OnLanguageSelection";
	}

	private void UpdateList()
	{
		if (!(Localization.instance != null))
		{
			return;
		}
		mList.items.Clear();
		TextAsset[] languages = Localization.instance.languages;
		foreach (TextAsset textAsset in languages)
		{
			if (textAsset != null)
			{
				mList.items.Add(textAsset.name);
			}
		}
		mList.selection = Localization.instance.currentLanguage;
	}

	private void OnLanguageSelection(string language)
	{
		if (Localization.instance != null)
		{
			Localization.instance.currentLanguage = language;
		}
	}
}
