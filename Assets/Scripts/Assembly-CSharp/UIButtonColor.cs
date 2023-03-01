using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Color")]
public class UIButtonColor : MonoBehaviour
{
	public GameObject tweenTarget;

	public Color hover = new Color(0.6f, 1f, 0.2f, 1f);

	public Color pressed = Color.grey;

	public float duration = 0.2f;

	private Color mColor;

	private bool mInitDone;

	private void OnDisable()
	{
		if (tweenTarget != null)
		{
			TweenColor component = tweenTarget.GetComponent<TweenColor>();
			if (component != null)
			{
				component.color = mColor;
				component.enabled = false;
			}
		}
	}

	private void Init()
	{
		mInitDone = true;
		if (tweenTarget == null)
		{
			tweenTarget = base.gameObject;
		}
		UIWidget component = tweenTarget.GetComponent<UIWidget>();
		if (component != null)
		{
			mColor = component.color;
			return;
		}
		Renderer renderer = tweenTarget.GetComponent<Renderer>();
		if (renderer != null)
		{
			mColor = renderer.material.color;
			return;
		}
		Light light = tweenTarget.GetComponent<Light>();
		if (light != null)
		{
			mColor = light.color;
			return;
		}
		Debug.LogWarning(NGUITools.GetHierarchy(base.gameObject) + " has nothing for UIButtonColor to color", this);
		base.enabled = false;
	}

	private void OnPress(bool isPressed)
	{
		if (!mInitDone)
		{
			Init();
		}
		if (base.enabled)
		{
			TweenColor.Begin(tweenTarget, duration, (!isPressed) ? mColor : pressed);
		}
	}

	private void OnHover(bool isOver)
	{
		if (!mInitDone)
		{
			Init();
		}
		if (base.enabled)
		{
			TweenColor.Begin(tweenTarget, duration, (!isOver) ? mColor : hover);
		}
	}
}
