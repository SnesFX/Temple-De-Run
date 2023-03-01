using UnityEngine;

public class PlayOnEnable : MonoBehaviour
{
	private void OnEnable()
	{
		UITweener[] components = GetComponents<UITweener>();
		UITweener[] array = components;
		foreach (UITweener uITweener in array)
		{
			uITweener.Reset();
			uITweener.Play(true);
		}
	}
}
