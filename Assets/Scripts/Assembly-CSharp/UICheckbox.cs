using AnimationOrTween;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Checkbox")]
public class UICheckbox : MonoBehaviour
{
	public static UICheckbox current;

	public UISprite checkSprite;

	public Animation checkAnimation;

	public GameObject eventReceiver;

	public string functionName = "OnActivate";

	public bool startsChecked = true;

	public bool option;

	public bool optionCanBeNone;

	private bool mChecked = true;

	private bool mStarted;

	private Transform mTrans;

	public bool isChecked
	{
		get
		{
			return mChecked;
		}
		set
		{
			if (!option || value || optionCanBeNone || !mStarted)
			{
				Set(value);
			}
		}
	}

	private void Start()
	{
		mTrans = base.transform;
		if (eventReceiver == null)
		{
			eventReceiver = base.gameObject;
		}
		mChecked = !startsChecked;
		mStarted = true;
		Set(startsChecked);
	}

	private void OnClick()
	{
		if (base.enabled)
		{
			isChecked = !isChecked;
		}
	}

	private void Set(bool state)
	{
		if (!mStarted)
		{
			startsChecked = state;
		}
		else
		{
			if (mChecked == state)
			{
				return;
			}
			if (option && state)
			{
				UICheckbox[] componentsInChildren = mTrans.parent.GetComponentsInChildren<UICheckbox>();
				UICheckbox[] array = componentsInChildren;
				foreach (UICheckbox uICheckbox in array)
				{
					if (uICheckbox != this)
					{
						uICheckbox.Set(false);
					}
				}
			}
			mChecked = state;
			if (checkSprite != null)
			{
				Color color = checkSprite.color;
				color.a = ((!mChecked) ? 0f : 1f);
				TweenColor.Begin(checkSprite.gameObject, 0.2f, color);
			}
			if (eventReceiver != null && !string.IsNullOrEmpty(functionName))
			{
				current = this;
				eventReceiver.SendMessage(functionName, mChecked, SendMessageOptions.DontRequireReceiver);
			}
			if (checkAnimation != null)
			{
				ActiveAnimation.Play(checkAnimation, state ? Direction.Forward : Direction.Reverse);
			}
		}
	}
}
