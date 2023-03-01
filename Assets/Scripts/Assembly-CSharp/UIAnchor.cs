using UnityEngine;

[AddComponentMenu("NGUI/UI/Anchor")]
[ExecuteInEditMode]
public class UIAnchor : MonoBehaviour
{
	public enum Side
	{
		BottomLeft = 0,
		Left = 1,
		TopLeft = 2,
		Top = 3,
		TopRight = 4,
		Right = 5,
		BottomRight = 6,
		Bottom = 7,
		Center = 8
	}

	public Camera uiCamera;

	public Side side = Side.Center;

	public bool halfPixelOffset = true;

	public bool stretchToFill;

	public float depthOffset;

	private Transform mTrans;

	private bool mIsWindows;

	private void ChangeWidgetPivot()
	{
		UIWidget component = GetComponent<UIWidget>();
		if (component != null)
		{
			component.pivot = UIWidget.Pivot.TopLeft;
		}
	}

	private void Start()
	{
		if (stretchToFill)
		{
			ChangeWidgetPivot();
		}
	}

	private void OnEnable()
	{
		mTrans = base.transform;
		mIsWindows = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
		if (uiCamera == null)
		{
			uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
	}

	public void Update()
	{
		if (!(uiCamera != null))
		{
			return;
		}
		if (stretchToFill)
		{
			side = Side.TopLeft;
			if (!Application.isPlaying)
			{
				ChangeWidgetPivot();
			}
		}
		Vector3 position = new Vector3(Screen.width, Screen.height, 0f);
		if (side == Side.Center)
		{
			position.x *= uiCamera.rect.width * 0.5f;
			position.y *= uiCamera.rect.height * 0.5f;
		}
		else
		{
			if (side == Side.Right || side == Side.TopRight || side == Side.BottomRight)
			{
				position.x *= uiCamera.rect.xMax;
			}
			else if (side == Side.Top || side == Side.Center || side == Side.Bottom)
			{
				position.x *= (uiCamera.rect.xMax - uiCamera.rect.xMin) * 0.5f;
			}
			else
			{
				position.x *= uiCamera.rect.xMin;
			}
			if (side == Side.Top || side == Side.TopRight || side == Side.TopLeft)
			{
				position.y *= uiCamera.rect.yMax;
			}
			else if (side == Side.Left || side == Side.Center || side == Side.Right)
			{
				position.y *= (uiCamera.rect.yMax - uiCamera.rect.yMin) * 0.5f;
			}
			else
			{
				position.y *= uiCamera.rect.yMin;
			}
		}
		position.z = (mTrans.TransformPoint(Vector3.forward * depthOffset) - mTrans.TransformPoint(Vector3.zero)).magnitude * Mathf.Sign(depthOffset);
		if (uiCamera.orthographic)
		{
			position.z += (uiCamera.nearClipPlane + uiCamera.farClipPlane) * 0.5f;
			if (halfPixelOffset && mIsWindows)
			{
				position.x -= 0.5f;
				position.y += 0.5f;
			}
		}
		Vector3 vector = uiCamera.ScreenToWorldPoint(position);
		Vector3 position2 = mTrans.position;
		if (vector != position2)
		{
			mTrans.position = vector;
		}
		if (stretchToFill)
		{
			Vector3 localPosition = mTrans.localPosition;
			Vector3 vector2 = new Vector3(Mathf.Abs(localPosition.x) * 2f, Mathf.Abs(localPosition.y) * 2f, 1f);
			if (mTrans.localScale != vector2)
			{
				mTrans.localScale = vector2;
			}
		}
	}
}
