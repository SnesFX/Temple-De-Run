using AnimationOrTween;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Tween")]
public class UIButtonTween : MonoBehaviour
{
	public GameObject tweenTarget;

	public int tweenGroup;

	public Trigger trigger;

	public Direction playDirection = Direction.Forward;

	public bool resetOnPlay;

	public EnableCondition ifDisabledOnPlay;

	public DisableCondition disableWhenFinished;

	public bool includeChildren;

	private UITweener[] mTweens;

	private void Start()
	{
		if (tweenTarget == null)
		{
			tweenTarget = base.gameObject;
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled && (trigger == Trigger.OnHover || (trigger == Trigger.OnHoverTrue && isOver) || (trigger == Trigger.OnHoverFalse && !isOver)))
		{
			Play(isOver);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && (trigger == Trigger.OnPress || (trigger == Trigger.OnPressTrue && isPressed) || (trigger == Trigger.OnPressFalse && !isPressed)))
		{
			Play(isPressed);
		}
	}

	private void OnClick()
	{
		if (base.enabled && trigger == Trigger.OnClick)
		{
			Play(true);
		}
	}

	private void Update()
	{
		if (disableWhenFinished == DisableCondition.DoNotDisable || playDirection != (Direction)disableWhenFinished || mTweens == null)
		{
			return;
		}
		bool flag = true;
		UITweener[] array = mTweens;
		foreach (UITweener uITweener in array)
		{
			if (uITweener.enabled)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			NGUITools.SetActive(tweenTarget, false);
			mTweens = null;
		}
	}

	private void Play(bool forward)
	{
		GameObject gameObject = ((!(tweenTarget == null)) ? tweenTarget : base.gameObject);
		if (!gameObject.active)
		{
			if (ifDisabledOnPlay != EnableCondition.EnableThenPlay)
			{
				return;
			}
			NGUITools.SetActive(gameObject, true);
		}
		mTweens = ((!includeChildren) ? gameObject.GetComponents<UITweener>() : gameObject.GetComponentsInChildren<UITweener>());
		if (mTweens.Length == 0)
		{
			if (disableWhenFinished != 0)
			{
				NGUITools.SetActive(tweenTarget, false);
			}
			return;
		}
		bool flag = false;
		if (playDirection == Direction.Reverse)
		{
			forward = !forward;
		}
		UITweener[] array = mTweens;
		foreach (UITweener uITweener in array)
		{
			if (uITweener.tweenGroup == tweenGroup)
			{
				if (!flag && !gameObject.active)
				{
					flag = true;
					NGUITools.SetActive(gameObject, true);
				}
				if (playDirection == Direction.Toggle)
				{
					uITweener.Toggle();
				}
				else
				{
					uITweener.Play(forward);
				}
				if (resetOnPlay)
				{
					uITweener.Reset();
				}
			}
		}
	}
}
