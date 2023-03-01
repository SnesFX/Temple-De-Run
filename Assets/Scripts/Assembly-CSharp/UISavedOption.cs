using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
	public string keyName;

	private string key
	{
		get
		{
			return (!string.IsNullOrEmpty(keyName)) ? keyName : ("NGUI State: " + base.name);
		}
	}

	private void OnEnable()
	{
		string @string = PlayerPrefs.GetString(key);
		if (!string.IsNullOrEmpty(@string))
		{
			UICheckbox[] componentsInChildren = GetComponentsInChildren<UICheckbox>();
			UICheckbox[] array = componentsInChildren;
			foreach (UICheckbox uICheckbox in array)
			{
				UIEventListener uIEventListener = UIEventListener.Add(uICheckbox.gameObject);
				uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(Save));
				uICheckbox.isChecked = uICheckbox.name == @string;
				UIEventListener uIEventListener2 = UIEventListener.Add(uICheckbox.gameObject);
				uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(Save));
			}
		}
	}

	private void Save(GameObject go)
	{
		UICheckbox[] componentsInChildren = GetComponentsInChildren<UICheckbox>();
		UICheckbox[] array = componentsInChildren;
		foreach (UICheckbox uICheckbox in array)
		{
			if (uICheckbox.isChecked)
			{
				PlayerPrefs.SetString(key, uICheckbox.name);
				break;
			}
		}
	}

	private void OnDestroy()
	{
		Save(null);
	}
}
