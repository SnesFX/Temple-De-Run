using UnityEngine;

public class DirectSwipeDetector : MonoBehaviour
{
	public TouchInput IManager;

	private bool isPressed;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			switch (touch.phase)
			{
			case TouchPhase.Began:
				IManager.TouchBegan(touch.position);
				break;
			case TouchPhase.Moved:
				IManager.TouchMoved(touch.position);
				break;
			case TouchPhase.Ended:
				IManager.TouchEnded(touch.position);
				break;
			case TouchPhase.Canceled:
				IManager.TouchEnded(touch.position);
				break;
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			IManager.TouchBegan(Input.mousePosition);
			isPressed = true;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			IManager.TouchEnded(Input.mousePosition);
			isPressed = false;
		}
		else if (isPressed)
		{
			IManager.TouchMoved(Input.mousePosition);
		}
	}
}
