using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Message")]
public class UIButtonMessage : MonoBehaviour
{
	public enum Trigger
	{
		OnClick = 0,
		OnMouseOver = 1,
		OnMouseOut = 2,
		OnPress = 3,
		OnRelease = 4
	}

	public GameObject target;

	public string functionName;

	public Trigger trigger;

	public bool includeChildren;

	private void OnHover(bool isOver)
	{
		if ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut))
		{
			Send();
		}
	}

	private void OnPress(bool isPressed)
	{
		if ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease))
		{
			Send();
		}
	}

	private void OnClick()
	{
		if (trigger == Trigger.OnClick)
		{
			Send();
		}
	}

	private void Send()
	{
		if (!base.enabled || !base.gameObject.active || string.IsNullOrEmpty(functionName))
		{
			return;
		}
		if (target == null)
		{
			target = base.gameObject;
		}
		if (includeChildren)
		{
			Transform[] componentsInChildren = target.GetComponentsInChildren<Transform>();
			Transform[] array = componentsInChildren;
			foreach (Transform transform in array)
			{
				transform.gameObject.SendMessage(functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			target.SendMessage(functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
}
