using UnityEngine;

public class UIPersistCheckBox : MonoBehaviour
{
	public string Key;

	public bool DefaultValue;

	public bool Opposite;

	private void Awake()
	{
		if (!string.IsNullOrEmpty(Key))
		{
			int defaultValue = (DefaultValue ? 1 : 0);
			bool flag = PlayerPrefs.GetInt(Key, defaultValue) == 1;
			if (Opposite)
			{
				flag = !flag;
			}
			GetComponent<UICheckbox>().startsChecked = flag;
		}
	}

	private void OnActivate(bool check)
	{
		if (!Opposite && !string.IsNullOrEmpty(Key))
		{
			int value = (check ? 1 : 0);
			PlayerPrefs.SetInt(Key, value);
		}
	}
}
