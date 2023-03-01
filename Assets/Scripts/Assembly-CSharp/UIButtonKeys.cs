using UnityEngine;

[RequireComponent(typeof(Collider))]
[AddComponentMenu("NGUI/Interaction/Button Keys")]
public class UIButtonKeys : MonoBehaviour
{
	public bool startsSelected;

	public UIButtonKeys selectOnClick;

	public UIButtonKeys selectOnUp;

	public UIButtonKeys selectOnDown;

	public UIButtonKeys selectOnLeft;

	public UIButtonKeys selectOnRight;

	private void Start()
	{
		if (startsSelected && (UICamera.selectedObject == null || !UICamera.selectedObject.active))
		{
			UICamera.selectedObject = base.gameObject;
		}
	}

	private void OnKey(KeyCode key)
	{
		if (!base.enabled || !base.gameObject.active)
		{
			return;
		}
		switch (key)
		{
		case KeyCode.LeftArrow:
			if (selectOnLeft != null)
			{
				UICamera.selectedObject = selectOnLeft.gameObject;
			}
			break;
		case KeyCode.RightArrow:
			if (selectOnRight != null)
			{
				UICamera.selectedObject = selectOnRight.gameObject;
			}
			break;
		case KeyCode.UpArrow:
			if (selectOnUp != null)
			{
				UICamera.selectedObject = selectOnUp.gameObject;
			}
			break;
		case KeyCode.DownArrow:
			if (selectOnDown != null)
			{
				UICamera.selectedObject = selectOnDown.gameObject;
			}
			break;
		case KeyCode.Tab:
			if (selectOnRight != null)
			{
				UICamera.selectedObject = selectOnRight.gameObject;
			}
			else if (selectOnDown != null)
			{
				UICamera.selectedObject = selectOnDown.gameObject;
			}
			break;
		}
	}

	private void OnClick()
	{
		if (base.enabled && base.gameObject.active && selectOnClick != null)
		{
			UICamera.selectedObject = selectOnClick.gameObject;
		}
	}
}
