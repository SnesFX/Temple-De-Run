using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Sound")]
public class UIButtonSound : MonoBehaviour
{
	public enum Trigger
	{
		OnClick = 0,
		OnMouseOver = 1,
		OnMouseOut = 2,
		OnPress = 3,
		OnRelease = 4
	}

	public AudioClip audioClip;

	public Trigger trigger;

	public float volume = 1f;

	private void OnHover(bool isOver)
	{
		if (base.enabled && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
		{
			NGUITools.PlaySound(audioClip, volume);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
		{
			NGUITools.PlaySound(audioClip, volume);
		}
	}

	private void OnClick()
	{
		if (base.enabled && trigger == Trigger.OnClick)
		{
			NGUITools.PlaySound(audioClip, volume);
		}
	}
}
