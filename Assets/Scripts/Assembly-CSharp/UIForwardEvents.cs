using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Forward Events")]
public class UIForwardEvents : MonoBehaviour
{
	public GameObject target;

	public bool onHover;

	public bool onPress;

	public bool onClick;

	public bool onSelect;

	public bool onDrag;

	public bool onDrop;

	public bool onInput;

	public bool onSubmit;

	private void OnHover(bool isOver)
	{
		if (onHover && target != null)
		{
			target.SendMessage("OnHover", isOver, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnPress(bool pressed)
	{
		if (onPress && target != null)
		{
			target.SendMessage("OnPress", pressed, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnClick()
	{
		if (onClick && target != null)
		{
			target.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnSelect(bool selected)
	{
		if (onSelect && target != null)
		{
			target.SendMessage("OnSelect", selected, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (onDrag && target != null)
		{
			target.SendMessage("OnDrag", delta, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnDrop(GameObject go)
	{
		if (onDrop && target != null)
		{
			target.SendMessage("OnDrop", go, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnInput(string text)
	{
		if (onInput && target != null)
		{
			target.SendMessage("OnInput", text, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void OnSubmit()
	{
		if (onSubmit && target != null)
		{
			target.SendMessage("OnSubmit", SendMessageOptions.DontRequireReceiver);
		}
	}
}
