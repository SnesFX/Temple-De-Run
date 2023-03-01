using UnityEngine;

[RequireComponent(typeof(UISlider))]
public class UIPersistSlider : MonoBehaviour
{
	public string Key;

	public float DefaultValue;

	private void Awake()
	{
		if (!string.IsNullOrEmpty(Key))
		{
			GetComponent<UISlider>().sliderValue = PlayerPrefs.GetFloat(Key, DefaultValue);
		}
	}

	private void OnSliderChange()
	{
		if (!string.IsNullOrEmpty(Key))
		{
			PlayerPrefs.SetFloat(Key, GetComponent<UISlider>().sliderValue);
		}
	}
}
