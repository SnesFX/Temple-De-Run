using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Panel")]
public class UIPanel : MonoBehaviour
{
	public enum DebugInfo
	{
		None = 0,
		Gizmos = 1,
		Geometry = 2
	}

	public bool showInPanelTool = true;

	public bool generateNormals;

	public bool depthPass;

	[SerializeField]
	private DebugInfo mDebugInfo = DebugInfo.Gizmos;

	[SerializeField]
	private UIDrawCall.Clipping mClipping;

	[SerializeField]
	private Vector4 mClipRange = Vector4.zero;

	[SerializeField]
	private Vector2 mClipSoftness = new Vector2(40f, 40f);

	private Dictionary<Transform, UINode> mChildren = new Dictionary<Transform, UINode>();

	private List<UIWidget> mWidgets = new List<UIWidget>();

	private List<Material> mChanged = new List<Material>();

	private List<UIDrawCall> mDrawCalls = new List<UIDrawCall>();

	private BetterList<Vector3> mVerts = new BetterList<Vector3>();

	private BetterList<Vector3> mNorms = new BetterList<Vector3>();

	private BetterList<Vector4> mTans = new BetterList<Vector4>();

	private BetterList<Vector2> mUvs = new BetterList<Vector2>();

	private BetterList<Color> mCols = new BetterList<Color>();

	private Transform mTrans;

	private Camera mCam;

	private int mLayer = -1;

	private bool mDepthChanged;

	private bool mRebuildAll;

	private float mMatrixTime;

	private Matrix4x4 mWorldToLocal = Matrix4x4.identity;

	private static float[] mTemp = new float[4];

	private Vector2 mMin = Vector2.zero;

	private Vector2 mMax = Vector2.zero;

	private List<Transform> mRemoved = new List<Transform>();

	private bool mCheckVisibility;

	private static List<UINode> mHierarchy = new List<UINode>();

	public Transform cachedTransform
	{
		get
		{
			if (mTrans == null)
			{
				mTrans = base.transform;
			}
			return mTrans;
		}
	}

	public DebugInfo debugInfo
	{
		get
		{
			return mDebugInfo;
		}
		set
		{
			if (mDebugInfo == value)
			{
				return;
			}
			mDebugInfo = value;
			List<UIDrawCall> list = drawCalls;
			HideFlags hideFlags = ((mDebugInfo != DebugInfo.Geometry) ? HideFlags.HideAndDontSave : (HideFlags.DontSave | HideFlags.NotEditable));
			foreach (UIDrawCall item in list)
			{
				GameObject gameObject = item.gameObject;
				gameObject.active = false;
				gameObject.hideFlags = hideFlags;
				gameObject.active = true;
			}
		}
	}

	public UIDrawCall.Clipping clipping
	{
		get
		{
			return mClipping;
		}
		set
		{
			if (mClipping != value)
			{
				mCheckVisibility = true;
				mClipping = value;
				UpdateDrawcalls();
			}
		}
	}

	public Vector4 clipRange
	{
		get
		{
			return mClipRange;
		}
		set
		{
			if (mClipRange != value)
			{
				mCheckVisibility = true;
				mClipRange = value;
				UpdateDrawcalls();
			}
		}
	}

	public Vector2 clipSoftness
	{
		get
		{
			return mClipSoftness;
		}
		set
		{
			if (mClipSoftness != value)
			{
				mClipSoftness = value;
				UpdateDrawcalls();
			}
		}
	}

	public List<UIWidget> widgets
	{
		get
		{
			return mWidgets;
		}
	}

	public List<UIDrawCall> drawCalls
	{
		get
		{
			int num = mDrawCalls.Count;
			while (num > 0)
			{
				UIDrawCall uIDrawCall = mDrawCalls[--num];
				if (uIDrawCall == null)
				{
					mDrawCalls.RemoveAt(num);
				}
			}
			return mDrawCalls;
		}
	}

	private UINode GetNode(Transform t)
	{
		UINode value = null;
		if (t != null)
		{
			mChildren.TryGetValue(t, out value);
		}
		return value;
	}

	private bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		UpdateTransformMatrix();
		a = mWorldToLocal.MultiplyPoint3x4(a);
		b = mWorldToLocal.MultiplyPoint3x4(b);
		c = mWorldToLocal.MultiplyPoint3x4(c);
		d = mWorldToLocal.MultiplyPoint3x4(d);
		mTemp[0] = a.x;
		mTemp[1] = b.x;
		mTemp[2] = c.x;
		mTemp[3] = d.x;
		float num = Mathf.Min(mTemp);
		float num2 = Mathf.Max(mTemp);
		mTemp[0] = a.y;
		mTemp[1] = b.y;
		mTemp[2] = c.y;
		mTemp[3] = d.y;
		float num3 = Mathf.Min(mTemp);
		float num4 = Mathf.Max(mTemp);
		if (num2 < mMin.x)
		{
			return false;
		}
		if (num4 < mMin.y)
		{
			return false;
		}
		if (num > mMax.x)
		{
			return false;
		}
		if (num3 > mMax.y)
		{
			return false;
		}
		return true;
	}

	public bool IsVisible(UIWidget w)
	{
		if (!w.enabled || !w.gameObject.active || w.mainTexture == null || w.color.a < 0.001f)
		{
			return false;
		}
		if (mClipping == UIDrawCall.Clipping.None)
		{
			return true;
		}
		Vector2 relativeSize = w.relativeSize;
		Vector2 vector = Vector2.Scale(w.pivotOffset, relativeSize);
		Vector2 vector2 = vector;
		vector.x += relativeSize.x;
		vector.y -= relativeSize.y;
		Transform transform = w.cachedTransform;
		Vector3 a = transform.TransformPoint(vector);
		Vector3 b = transform.TransformPoint(new Vector2(vector.x, vector2.y));
		Vector3 c = transform.TransformPoint(new Vector2(vector2.x, vector.y));
		Vector3 d = transform.TransformPoint(vector2);
		return IsVisible(a, b, c, d);
	}

	public void MarkMaterialAsChanged(Material mat, bool sort)
	{
		if (mat != null)
		{
			if (sort)
			{
				mDepthChanged = true;
			}
			if (!mChanged.Contains(mat))
			{
				mChanged.Add(mat);
			}
		}
	}

	public bool WatchesTransform(Transform t)
	{
		return t == cachedTransform || mChildren.ContainsKey(t);
	}

	private UINode AddTransform(Transform t)
	{
		UINode value = null;
		UINode uINode = null;
		while (t != null && t != cachedTransform)
		{
			if (mChildren.TryGetValue(t, out value))
			{
				if (uINode == null)
				{
					uINode = value;
				}
				break;
			}
			value = new UINode(t);
			if (uINode == null)
			{
				uINode = value;
			}
			mChildren.Add(t, value);
			t = t.parent;
		}
		return uINode;
	}

	private void RemoveTransform(Transform t)
	{
		if (!(t != null))
		{
			return;
		}
		while (mChildren.Remove(t))
		{
			t = t.parent;
			if (t == null || t == mTrans || t.childCount > 1)
			{
				break;
			}
		}
	}

	public void AddWidget(UIWidget w)
	{
		if (!(w != null))
		{
			return;
		}
		UINode uINode = AddTransform(w.cachedTransform);
		if (uINode != null)
		{
			uINode.widget = w;
			if (!mWidgets.Contains(w))
			{
				mWidgets.Add(w);
				if (!mChanged.Contains(w.material))
				{
					mChanged.Add(w.material);
				}
				mDepthChanged = true;
			}
		}
		else
		{
			Debug.LogError("Unable to find an appropriate UIRoot for " + NGUITools.GetHierarchy(w.gameObject) + "\nPlease make sure that there is at least one game object above this widget!", w.gameObject);
		}
	}

	public void RemoveWidget(UIWidget w)
	{
		if (!(w != null))
		{
			return;
		}
		UINode node = GetNode(w.cachedTransform);
		if (node != null)
		{
			if (node.visibleFlag == 1 && !mChanged.Contains(w.material))
			{
				mChanged.Add(w.material);
			}
			RemoveTransform(w.cachedTransform);
		}
		mWidgets.Remove(w);
	}

	private UIDrawCall GetDrawCall(Material mat, bool createIfMissing)
	{
		foreach (UIDrawCall drawCall in drawCalls)
		{
			if (drawCall.material == mat)
			{
				return drawCall;
			}
		}
		UIDrawCall uIDrawCall = null;
		if (createIfMissing)
		{
			GameObject gameObject = new GameObject("_UIDrawCall [" + mat.name + "]");
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			gameObject.layer = base.gameObject.layer;
			uIDrawCall = gameObject.AddComponent<UIDrawCall>();
			uIDrawCall.material = mat;
			mDrawCalls.Add(uIDrawCall);
		}
		return uIDrawCall;
	}

	private void Start()
	{
		mLayer = base.gameObject.layer;
		UICamera uICamera = UICamera.FindCameraForLayer(mLayer);
		mCam = ((!(uICamera != null)) ? NGUITools.FindCameraForLayer(mLayer) : uICamera.cachedCamera);
	}

	private void OnEnable()
	{
		foreach (UIWidget mWidget in mWidgets)
		{
			AddWidget(mWidget);
		}
		mRebuildAll = true;
	}

	private void OnDisable()
	{
		int num = mDrawCalls.Count;
		while (num > 0)
		{
			UIDrawCall uIDrawCall = mDrawCalls[--num];
			if (uIDrawCall != null)
			{
				Object.DestroyImmediate(uIDrawCall.gameObject);
			}
		}
		mDrawCalls.Clear();
		mChanged.Clear();
		mChildren.Clear();
	}

	private int GetChangeFlag(UINode start)
	{
		int num = start.changeFlag;
		if (num == -1)
		{
			Transform parent = start.trans.parent;
			while (true)
			{
				UINode value;
				if (mChildren.TryGetValue(parent, out value))
				{
					num = value.changeFlag;
					parent = parent.parent;
					if (num == -1)
					{
						mHierarchy.Add(value);
						continue;
					}
					break;
				}
				num = 0;
				break;
			}
			foreach (UINode item in mHierarchy)
			{
				item.changeFlag = num;
			}
			mHierarchy.Clear();
		}
		return num;
	}

	private void UpdateTransformMatrix()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (realtimeSinceStartup != 0f && mMatrixTime == realtimeSinceStartup)
		{
			return;
		}
		mMatrixTime = realtimeSinceStartup;
		mWorldToLocal = cachedTransform.worldToLocalMatrix;
		if (mClipping != 0)
		{
			Vector2 vector = new Vector2(mClipRange.z, mClipRange.w);
			if (vector.x == 0f)
			{
				vector.x = ((!(mCam == null)) ? mCam.pixelWidth : ((float)Screen.width));
			}
			if (vector.y == 0f)
			{
				vector.y = ((!(mCam == null)) ? mCam.pixelHeight : ((float)Screen.height));
			}
			vector *= 0.5f;
			mMin.x = mClipRange.x - vector.x;
			mMin.y = mClipRange.y - vector.y;
			mMax.x = mClipRange.x + vector.x;
			mMax.y = mClipRange.y + vector.y;
		}
	}

	private void UpdateTransforms()
	{
		bool flag = mCheckVisibility;
		foreach (KeyValuePair<Transform, UINode> mChild in mChildren)
		{
			UINode value = mChild.Value;
			if (value.trans == null)
			{
				mRemoved.Add(value.trans);
			}
			else if (value.HasChanged())
			{
				value.changeFlag = 1;
				flag = true;
			}
			else
			{
				value.changeFlag = -1;
			}
		}
		foreach (Transform item in mRemoved)
		{
			mChildren.Remove(item);
		}
		mRemoved.Clear();
		if (flag || mRebuildAll)
		{
			foreach (KeyValuePair<Transform, UINode> mChild2 in mChildren)
			{
				UINode value2 = mChild2.Value;
				if (!(value2.widget != null))
				{
					continue;
				}
				if (value2.changeFlag == -1)
				{
					value2.changeFlag = GetChangeFlag(value2);
				}
				int num = ((!mCheckVisibility && value2.changeFlag != 1) ? value2.visibleFlag : (IsVisible(value2.widget) ? 1 : 0));
				if (value2.visibleFlag != num)
				{
					value2.changeFlag = 1;
				}
				if (value2.changeFlag == 1 && (num == 1 || value2.visibleFlag != 0))
				{
					value2.visibleFlag = num;
					Material material = value2.widget.material;
					if (!mChanged.Contains(material))
					{
						mChanged.Add(material);
					}
				}
			}
		}
		mCheckVisibility = false;
	}

	private void UpdateWidgets()
	{
		foreach (KeyValuePair<Transform, UINode> mChild in mChildren)
		{
			UINode value = mChild.Value;
			UIWidget widget = value.widget;
			if (value.visibleFlag == 1 && widget != null && widget.UpdateGeometry(ref mWorldToLocal, value.changeFlag == 1, generateNormals) && !mChanged.Contains(widget.material))
			{
				mChanged.Add(widget.material);
			}
		}
	}

	public void UpdateDrawcalls()
	{
		Vector4 vector = Vector4.zero;
		if (mClipping != 0)
		{
			vector = new Vector4(mClipRange.x, mClipRange.y, mClipRange.z * 0.5f, mClipRange.w * 0.5f);
		}
		if (vector.z == 0f)
		{
			vector.z = (float)Screen.width * 0.5f;
		}
		if (vector.w == 0f)
		{
			vector.w = (float)Screen.height * 0.5f;
		}
		RuntimePlatform platform = Application.platform;
		if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor)
		{
			vector.x -= 0.5f;
			vector.y += 0.5f;
		}
		Transform transform = cachedTransform;
		foreach (UIDrawCall mDrawCall in mDrawCalls)
		{
			mDrawCall.clipping = mClipping;
			mDrawCall.clipRange = vector;
			mDrawCall.clipSoftness = mClipSoftness;
			mDrawCall.depthPass = depthPass;
			Transform transform2 = mDrawCall.transform;
			transform2.position = transform.position;
			transform2.rotation = transform.rotation;
			transform2.localScale = transform.lossyScale;
		}
	}

	private void Fill(Material mat)
	{
		int num = mWidgets.Count;
		while (num > 0)
		{
			if (mWidgets[--num] == null)
			{
				mWidgets.RemoveAt(num);
			}
		}
		foreach (UIWidget mWidget in mWidgets)
		{
			if (mWidget.visibleFlag != 1 || !(mWidget.material == mat))
			{
				continue;
			}
			UINode node = GetNode(mWidget.cachedTransform);
			if (node != null)
			{
				if (generateNormals)
				{
					mWidget.WriteToBuffers(mVerts, mUvs, mCols, mNorms, mTans);
				}
				else
				{
					mWidget.WriteToBuffers(mVerts, mUvs, mCols, null, null);
				}
			}
			else
			{
				Debug.LogError("No transform found for " + NGUITools.GetHierarchy(mWidget.gameObject), this);
			}
		}
		if (mVerts.size > 0)
		{
			UIDrawCall drawCall = GetDrawCall(mat, true);
			drawCall.depthPass = depthPass;
			drawCall.Set(mVerts, (!generateNormals) ? null : mNorms, (!generateNormals) ? null : mTans, mUvs, mCols);
		}
		else
		{
			UIDrawCall drawCall2 = GetDrawCall(mat, false);
			if (drawCall2 != null)
			{
				mDrawCalls.Remove(drawCall2);
				Object.DestroyImmediate(drawCall2.gameObject);
			}
		}
		mVerts.Clear();
		mNorms.Clear();
		mTans.Clear();
		mUvs.Clear();
		mCols.Clear();
	}

	private void LateUpdate()
	{
		UpdateTransformMatrix();
		UpdateTransforms();
		if (mLayer != base.gameObject.layer)
		{
			mLayer = base.gameObject.layer;
			UICamera uICamera = UICamera.FindCameraForLayer(mLayer);
			mCam = ((!(uICamera != null)) ? NGUITools.FindCameraForLayer(mLayer) : uICamera.cachedCamera);
			SetChildLayer(cachedTransform, mLayer);
			foreach (UIDrawCall drawCall in drawCalls)
			{
				drawCall.gameObject.layer = mLayer;
			}
		}
		UpdateWidgets();
		if (mDepthChanged)
		{
			mDepthChanged = false;
			mWidgets.Sort(UIWidget.CompareFunc);
		}
		foreach (Material item in mChanged)
		{
			Fill(item);
		}
		UpdateDrawcalls();
		mChanged.Clear();
		mRebuildAll = false;
	}

	public Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
	{
		float num = clipRange.z * 0.5f;
		float num2 = clipRange.w * 0.5f;
		Vector2 minRect = new Vector2(min.x, min.y);
		Vector2 maxRect = new Vector2(max.x, max.y);
		Vector2 minArea = new Vector2(clipRange.x - num, clipRange.y - num2);
		Vector2 maxArea = new Vector2(clipRange.x + num, clipRange.y + num2);
		if (clipping == UIDrawCall.Clipping.SoftClip)
		{
			minArea.x += clipSoftness.x;
			minArea.y += clipSoftness.y;
			maxArea.x -= clipSoftness.x;
			maxArea.y -= clipSoftness.y;
		}
		return NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
	}

	public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
	{
		Vector3 vector = CalculateConstrainOffset(targetBounds.min, targetBounds.max);
		if (vector.magnitude > 0f)
		{
			if (immediate)
			{
				target.localPosition += vector;
				targetBounds.center += vector;
				SpringPosition component = target.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else
			{
				SpringPosition springPosition = SpringPosition.Begin(target.gameObject, target.localPosition + vector, 13f);
				springPosition.ignoreTimeScale = true;
				springPosition.worldSpace = false;
			}
			return true;
		}
		return false;
	}

	public bool ConstrainTargetToBounds(Transform target, bool immediate)
	{
		Bounds targetBounds = NGUIMath.CalculateRelativeWidgetBounds(cachedTransform, target);
		return ConstrainTargetToBounds(target, ref targetBounds, immediate);
	}

	private static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			if (child.GetComponent<UIPanel>() == null)
			{
				child.gameObject.layer = layer;
				SetChildLayer(child, layer);
			}
		}
	}

	public static UIPanel Find(Transform trans, bool createIfMissing)
	{
		Transform transform = trans;
		UIPanel uIPanel = null;
		while (uIPanel == null && trans != null)
		{
			uIPanel = trans.GetComponent<UIPanel>();
			if (uIPanel != null || trans.parent == null)
			{
				break;
			}
			trans = trans.parent;
		}
		if (createIfMissing && uIPanel == null && trans != transform)
		{
			uIPanel = trans.gameObject.AddComponent<UIPanel>();
			SetChildLayer(uIPanel.cachedTransform, uIPanel.gameObject.layer);
		}
		return uIPanel;
	}

	public static UIPanel Find(Transform trans)
	{
		return Find(trans, true);
	}
}
