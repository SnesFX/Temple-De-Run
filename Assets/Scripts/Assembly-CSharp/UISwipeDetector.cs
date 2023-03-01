using UnityEngine;

public class UISwipeDetector : MonoBehaviour
{
	public TouchInput InputManager;

	private Vector2 Accum = Vector2.zero;

	private void Awake()
	{
		GetComponentInChildren<UISprite>().enabled = false;
	}

	private void OnPress(bool isPressed)
	{
		Accum = Vector2.zero;
		if (isPressed)
		{
			InputManager.TouchBegan(Accum);
		}
	}

	private void OnDrag(Vector2 drag)
	{
		Accum += drag;
		InputManager.TouchMoved(Accum);
	}
}
