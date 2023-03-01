using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Slider")]
[ExecuteInEditMode]
public class UISlider : IgnoreTimeScale
{
	public enum Direction
	{
		Horizontal = 0,
		Vertical = 1
	}

	public static UISlider current;

	public Transform foreground;

	public Transform thumb;

	public Direction direction;

	public float rawValue = 1f;

	public Vector2 fullSize = Vector2.zero;

	public GameObject eventReceiver;

	public string functionName = "OnSliderChange";

	public int numberOfSteps;

	private float mStepValue = 1f;

	private BoxCollider mCol;

	private Transform mTrans;

	private Transform mForeTrans;

	private UIWidget mWidget;

	private UIFilledSprite mSprite;

	public float sliderValue
	{
		get
		{
			return mStepValue;
		}
		set
		{
			Set(value);
		}
	}

	private void Awake()
	{
		mTrans = base.transform;
		mCol = base.GetComponent<Collider>() as BoxCollider;
	}

	private void Start()
	{
		if (foreground != null)
		{
			mWidget = foreground.GetComponent<UIWidget>();
			mSprite = ((!(mWidget != null)) ? null : (mWidget as UIFilledSprite));
			mForeTrans = foreground.transform;
			if (fullSize == Vector2.zero)
			{
				fullSize = foreground.localScale;
			}
		}
		else if (mCol != null)
		{
			if (fullSize == Vector2.zero)
			{
				fullSize = mCol.size;
			}
		}
		else
		{
			Debug.LogWarning("UISlider expected to find a foreground object or a box collider to work with", this);
		}
		if (Application.isPlaying && thumb != null && thumb.GetComponent<Collider>() != null)
		{
			UIEventListener uIEventListener = UIEventListener.Add(thumb.gameObject);
			uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressThumb));
			uIEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener.onDrag, new UIEventListener.VectorDelegate(OnDragThumb));
		}
		Set(rawValue);
	}

	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			UpdateDrag();
		}
	}

	private void OnDrag(Vector2 delta)
	{
		UpdateDrag();
	}

	private void OnPressThumb(GameObject go, bool pressed)
	{
		if (pressed)
		{
			UpdateDrag();
		}
	}

	private void OnDragThumb(GameObject go, Vector2 delta)
	{
		UpdateDrag();
	}

	private void OnKey(KeyCode key)
	{
		float num = ((!((float)numberOfSteps > 1f)) ? 0.125f : (1f / (float)(numberOfSteps - 1)));
		if (direction == Direction.Horizontal)
		{
			switch (key)
			{
			case KeyCode.LeftArrow:
				Set(rawValue - num);
				break;
			case KeyCode.RightArrow:
				Set(rawValue + num);
				break;
			}
		}
		else
		{
			switch (key)
			{
			case KeyCode.DownArrow:
				Set(rawValue - num);
				break;
			case KeyCode.UpArrow:
				Set(rawValue + num);
				break;
			}
		}
	}

	private void UpdateDrag()
	{
		if (!(mCol == null) && !(UICamera.currentCamera == null) && UICamera.currentTouch != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float enter;
			if (new Plane(mTrans.rotation * Vector3.back, mTrans.position).Raycast(ray, out enter))
			{
				Vector3 vector = mTrans.localPosition + mCol.center - mCol.extents;
				Vector3 vector2 = mTrans.localPosition - vector;
				Vector3 vector3 = mTrans.InverseTransformPoint(ray.GetPoint(enter));
				Vector3 vector4 = vector3 + vector2;
				Set((direction != 0) ? (vector4.y / mCol.size.y) : (vector4.x / mCol.size.x));
			}
		}
	}

	private void Set(float input)
	{
		float num = Mathf.Clamp01(input);
		if (num < 0.001f)
		{
			num = 0f;
		}
		rawValue = num;
		if (numberOfSteps > 1)
		{
			num = Mathf.Round(num * (float)(numberOfSteps - 1)) / (float)(numberOfSteps - 1);
		}
		if (mStepValue == num)
		{
			return;
		}
		mStepValue = num;
		Vector3 localScale = fullSize;
		if (direction == Direction.Horizontal)
		{
			localScale.x *= mStepValue;
		}
		else
		{
			localScale.y *= mStepValue;
		}
		if (mSprite != null)
		{
			mSprite.fillAmount = mStepValue;
		}
		else if (mForeTrans != null)
		{
			mForeTrans.localScale = localScale;
			if (mWidget != null)
			{
				mWidget.MarkAsChanged();
			}
		}
		if (thumb != null)
		{
			Vector3 localPosition = thumb.localPosition;
			if (mSprite != null)
			{
				switch (mSprite.fillDirection)
				{
				case UIFilledSprite.FillDirection.TowardRight:
					localPosition.x = localScale.x;
					break;
				case UIFilledSprite.FillDirection.TowardTop:
					localPosition.y = localScale.y;
					break;
				case UIFilledSprite.FillDirection.TowardLeft:
					localPosition.x = fullSize.x - localScale.x;
					break;
				case UIFilledSprite.FillDirection.TowardBottom:
					localPosition.y = fullSize.y - localScale.y;
					break;
				}
			}
			else if (direction == Direction.Horizontal)
			{
				localPosition.x = localScale.x;
			}
			else
			{
				localPosition.y = localScale.y;
			}
			thumb.localPosition = localPosition;
		}
		if (eventReceiver != null && !string.IsNullOrEmpty(functionName) && Application.isPlaying)
		{
			current = this;
			eventReceiver.SendMessage(functionName, mStepValue, SendMessageOptions.DontRequireReceiver);
			current = null;
		}
	}
}
