using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Panel Contents")]
public class UIDragPanelContents : IgnoreTimeScale
{
	public enum DragEffect
	{
		None = 0,
		Momentum = 1,
		MomentumAndSpring = 2
	}

	public UIPanel panel;

	public Vector3 scale = Vector3.one;

	public float scrollWheelFactor;

	public bool restrictWithinPanel;

	public DragEffect dragEffect = DragEffect.MomentumAndSpring;

	public float momentumAmount = 35f;

	private Plane mPlane;

	private Vector3 mLastPos;

	private bool mPressed;

	private Vector3 mMomentum = Vector3.zero;

	private float mScroll;

	private Bounds mBounds;

	private bool mCalculatedBounds;

	private Bounds bounds
	{
		get
		{
			if (!mCalculatedBounds)
			{
				mCalculatedBounds = true;
				Transform cachedTransform = panel.cachedTransform;
				mBounds = NGUIMath.CalculateRelativeWidgetBounds(cachedTransform, cachedTransform);
			}
			return mBounds;
		}
	}

	private void OnPress(bool pressed)
	{
		if (base.enabled && base.gameObject.active && panel != null)
		{
			mCalculatedBounds = false;
			mPressed = pressed;
			if (pressed)
			{
				mMomentum = Vector3.zero;
				mScroll = 0f;
				DisableSpring();
				mLastPos = UICamera.lastHit.point;
				Transform transform = UICamera.currentCamera.transform;
				mPlane = new Plane(((!(panel != null)) ? transform.rotation : panel.cachedTransform.rotation) * Vector3.back, mLastPos);
			}
			else if (restrictWithinPanel && panel.clipping != 0 && dragEffect == DragEffect.MomentumAndSpring)
			{
				RestrictWithinBounds();
			}
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (!base.enabled || !base.gameObject.active || !(panel != null))
		{
			return;
		}
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
		Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
		float enter = 0f;
		if (mPlane.Raycast(ray, out enter))
		{
			Vector3 point = ray.GetPoint(enter);
			Vector3 vector = point - mLastPos;
			mLastPos = point;
			if (vector.x != 0f || vector.y != 0f)
			{
				Transform cachedTransform = panel.cachedTransform;
				vector = cachedTransform.InverseTransformDirection(vector);
				vector.Scale(scale);
				vector = cachedTransform.TransformDirection(vector);
			}
			mMomentum = Vector3.Lerp(mMomentum, vector * (base.realTimeDelta * momentumAmount), 0.5f);
			MoveAbsolute(vector);
			if (restrictWithinPanel && panel.clipping != 0 && dragEffect != DragEffect.MomentumAndSpring)
			{
				RestrictWithinBounds();
			}
		}
	}

	private void RestrictWithinBounds()
	{
		Vector3 vector = panel.CalculateConstrainOffset(bounds.min, bounds.max);
		if (vector.magnitude > 0.001f)
		{
			if (dragEffect == DragEffect.MomentumAndSpring)
			{
				SpringPanel.Begin(panel.gameObject, panel.cachedTransform.localPosition + vector, 13f);
				return;
			}
			MoveRelative(vector);
			mMomentum = Vector3.zero;
			mScroll = 0f;
		}
		else
		{
			DisableSpring();
		}
	}

	private void DisableSpring()
	{
		SpringPanel component = panel.GetComponent<SpringPanel>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	private void MoveRelative(Vector3 relative)
	{
		panel.cachedTransform.localPosition += relative;
		Vector4 clipRange = panel.clipRange;
		clipRange.x -= relative.x;
		clipRange.y -= relative.y;
		panel.clipRange = clipRange;
	}

	private void MoveAbsolute(Vector3 absolute)
	{
		Transform cachedTransform = panel.cachedTransform;
		Vector3 vector = cachedTransform.InverseTransformPoint(absolute);
		Vector3 vector2 = cachedTransform.InverseTransformPoint(Vector3.zero);
		MoveRelative(vector - vector2);
	}

	private void LateUpdate()
	{
		float deltaTime = UpdateRealTimeDelta();
		if (panel == null || mPressed)
		{
			return;
		}
		mMomentum += scale * ((0f - mScroll) * 0.05f);
		if (mMomentum.magnitude > 0.0001f)
		{
			mScroll = NGUIMath.SpringLerp(mScroll, 0f, 20f, deltaTime);
			MoveAbsolute(NGUIMath.SpringDampen(ref mMomentum, 9f, deltaTime));
			if (restrictWithinPanel && panel.clipping != 0)
			{
				RestrictWithinBounds();
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
