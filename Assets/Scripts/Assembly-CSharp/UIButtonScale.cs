using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIButtonScale : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);

	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);

	public float duration = 0.2f;

	private Vector3 mScale;

	private bool mInitDone;

	private void OnDisable()
	{
		if (tweenTarget != null)
		{
			TweenScale component = tweenTarget.GetComponent<TweenScale>();
			if (component != null)
			{
				component.scale = mScale;
				component.enabled = false;
			}
		}
	}

	private void Init()
	{
		mInitDone = true;
		if (tweenTarget == null)
		{
			tweenTarget = base.transform;
		}
		mScale = tweenTarget.localScale;
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!mInitDone)
			{
				Init();
			}
			TweenScale.Begin(tweenTarget.gameObject, duration, (!isPressed) ? mScale : Vector3.Scale(mScale, pressed)).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!mInitDone)
			{
				Init();
			}
			TweenScale.Begin(tweenTarget.gameObject, duration, (!isOver) ? mScale : Vector3.Scale(mScale, hover)).method = UITweener.Method.EaseInOut;
		}
	}
}
