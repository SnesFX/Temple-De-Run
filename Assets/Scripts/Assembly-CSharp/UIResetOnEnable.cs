using UnityEngine;

public class UIResetOnEnable : MonoBehaviour
{
	internal Vector3 StartPosition;

	private void Awake()
	{
		StartPosition = base.transform.localPosition;
	}

	private void OnEnable()
	{
		UITweener[] components = GetComponents<UITweener>();
		UITweener[] array = components;
		foreach (UITweener obj in array)
		{
			Object.Destroy(obj);
		}
		base.transform.localPosition = StartPosition;
	}
}
