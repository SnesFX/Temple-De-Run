using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Sound FX")]
public class UIPlayFX : MonoBehaviour
{
	public enum Trigger
	{
		OnClick = 0,
		OnMouseOver = 1,
		OnMouseOut = 2,
		OnPress = 3,
		OnRelease = 4,
		OnEnable = 5
	}

	public AudioManager.Effects effect;

	public Trigger trigger;

	public float volume = 1f;

	private void OnHover(bool isOver)
	{
		if (base.enabled && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
		{
			AudioManager.Instance.PlayFX(effect, volume);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
		{
			AudioManager.Instance.PlayFX(effect, volume);
		}
	}

	private void OnClick()
	{
		if (base.enabled && trigger == Trigger.OnClick)
		{
			AudioManager.Instance.PlayFX(effect, volume);
		}
	}

	private void OnEnable()
	{
		if (trigger == Trigger.OnEnable && AudioManager.Instance != null)
		{
			AudioManager.Instance.PlayFX(effect, volume);
		}
	}
}
