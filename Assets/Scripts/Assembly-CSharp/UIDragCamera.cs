using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Camera")]
public class UIDragCamera : IgnoreTimeScale
{
	public Camera target;

	public Transform rootForBounds;

	public Vector2 scale = Vector2.one;

	public float scrollWheelFactor;

	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	public float momentumAmount = 35f;

	private Transform mTrans;

	private bool mPressed;

	private Vector3 mMomentum = Vector3.zero;

	private Bounds mBounds;

	private float mScroll;

	private void Start()
	{
		if (target is Transform)
		{
			target = target.GetComponent<Camera>();
		}
		if (target != null)
		{
			mTrans = target.transform;
		}
		else
		{
			base.enabled = false;
		}
	}

	private void OnPress(bool isPressed)
	{
		if (!(rootForBounds != null))
		{
			return;
		}
		mPressed = isPressed;
		if (isPressed)
		{
			mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(rootForBounds);
			mMomentum = Vector3.zero;
			mScroll = 0f;
			SpringPosition component = target.GetComponent<SpringPosition>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
		else if (dragEffect == UIDragObject.DragEffect.MomentumAndSpring)
		{
			ConstrainToBounds(false);
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (target != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			Vector3 vector = Vector3.Scale(delta, -scale);
			mTrans.localPosition += vector;
			mMomentum = Vector3.Lerp(mMomentum, vector * (base.realTimeDelta * momentumAmount), 0.5f);
			if (dragEffect != UIDragObject.DragEffect.MomentumAndSpring && ConstrainToBounds(true))
			{
				mMomentum = Vector3.zero;
				mScroll = 0f;
			}
		}
	}

	private Vector3 CalculateConstrainOffset()
	{
		if (target == null || rootForBounds == null)
		{
			return Vector3.zero;
		}
		Vector3 position = new Vector3(target.rect.xMin * (float)Screen.width, target.rect.yMin * (float)Screen.height, 0f);
		Vector3 position2 = new Vector3(target.rect.xMax * (float)Screen.width, target.rect.yMax * (float)Screen.height, 0f);
		position = target.ScreenToWorldPoint(position);
		position2 = target.ScreenToWorldPoint(position2);
		Vector2 minRect = new Vector2(mBounds.min.x, mBounds.min.y);
		Vector2 maxRect = new Vector2(mBounds.max.x, mBounds.max.y);
		return NGUIMath.ConstrainRect(minRect, maxRect, position, position2);
	}

	public bool ConstrainToBounds(bool immediate)
	{
		if (mTrans != null && rootForBounds != null)
		{
			Vector3 vector = CalculateConstrainOffset();
			if (vector.magnitude > 0f)
			{
				if (immediate)
				{
					mTrans.position -= vector;
				}
				else
				{
					SpringPosition springPosition = SpringPosition.Begin(target.gameObject, mTrans.position - vector, 13f);
					springPosition.ignoreTimeScale = true;
					springPosition.worldSpace = true;
				}
				return true;
			}
		}
		return false;
	}

	private void Update()
	{
		if (target == null)
		{
			return;
		}
		float deltaTime = UpdateRealTimeDelta();
		if (mPressed)
		{
			SpringPosition component = target.GetComponent<SpringPosition>();
			if (component != null)
			{
				component.enabled = false;
			}
			mScroll = 0f;
			return;
		}
		mMomentum += (Vector3)scale * (mScroll * 20f);
		mScroll = NGUIMath.SpringLerp(mScroll, 0f, 20f, deltaTime);
		if (mMomentum.magnitude > 0.01f)
		{
			mTrans.localPosition += NGUIMath.SpringDampen(ref mMomentum, 9f, deltaTime);
			mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(rootForBounds);
			if (!ConstrainToBounds(dragEffect == UIDragObject.DragEffect.None))
			{
				SpringPosition component2 = target.GetComponent<SpringPosition>();
				if (component2 != null)
				{
					component2.enabled = false;
				}
			}
		}
		else
		{
			mScroll = 0f;
		}
	}

	private void OnScroll(float delta)
	{
		if (base.enabled && base.gameObject.active)
		{
			if (Mathf.Sign(mScroll) != Mathf.Sign(delta))
			{
				mScroll = 0f;
			}
			mScroll += delta * scrollWheelFactor;
		}
	}
}
