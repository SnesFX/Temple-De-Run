using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Camera")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class UICamera : MonoBehaviour
{
	public enum ClickNotification
	{
		None = 0,
		Always = 1,
		BasedOnDelta = 2
	}

	public class MouseOrTouch
	{
		public Vector3 pos;

		public Vector2 delta;

		public Vector2 totalDelta;

		public Camera pressedCam;

		public GameObject current;

		public GameObject hover;

		public GameObject pressed;

		public ClickNotification clickNotification = ClickNotification.Always;
	}

	private class Highlighted
	{
		public GameObject go;

		public int counter;
	}

	public bool useMouse = true;

	public bool useTouch = true;

	public bool useKeyboard = true;

	public bool useController = true;

	public LayerMask eventReceiverMask = -1;

	public float tooltipDelay = 1f;

	public string scrollAxisName = "Mouse ScrollWheel";

	public static Vector3 lastTouchPosition = Vector3.zero;

	public static RaycastHit lastHit;

	public static Camera currentCamera = null;

	public static int currentTouchID = -1;

	public static MouseOrTouch currentTouch = null;

	public static GameObject fallThrough;

	private static List<UICamera> mList = new List<UICamera>();

	private static List<Highlighted> mHighlighted = new List<Highlighted>();

	private static GameObject mSel = null;

	private static MouseOrTouch mMouse = new MouseOrTouch();

	private static MouseOrTouch mJoystick = new MouseOrTouch();

	private static float mNextEvent = 0f;

	private Dictionary<int, MouseOrTouch> mTouches = new Dictionary<int, MouseOrTouch>();

	private GameObject mTooltip;

	private Camera mCam;

	private LayerMask mLayerMask;

	private float mTooltipTime;

	private bool mIsEditor;

	[Obsolete("Use UICamera.currentCamera instead")]
	public static Camera lastCamera
	{
		get
		{
			return currentCamera;
		}
	}

	[Obsolete("Use UICamera.currentTouchID instead")]
	public static int lastTouchID
	{
		get
		{
			return currentTouchID;
		}
	}

	private bool handlesEvents
	{
		get
		{
			return eventHandler == this;
		}
	}

	public Camera cachedCamera
	{
		get
		{
			if (mCam == null)
			{
				mCam = base.GetComponent<Camera>();
			}
			return mCam;
		}
	}

	public static GameObject hoveredObject
	{
		get
		{
			return mMouse.current;
		}
	}

	public static GameObject selectedObject
	{
		get
		{
			return mSel;
		}
		set
		{
			if (!(mSel != value))
			{
				return;
			}
			if (mSel != null)
			{
				UICamera uICamera = FindCameraForLayer(mSel.layer);
				if (uICamera != null)
				{
					currentCamera = uICamera.mCam;
					mSel.SendMessage("OnSelect", false, SendMessageOptions.DontRequireReceiver);
					Highlight(mSel, false);
				}
			}
			mSel = value;
			if (mSel != null)
			{
				UICamera uICamera2 = FindCameraForLayer(mSel.layer);
				if (uICamera2 != null)
				{
					currentCamera = uICamera2.mCam;
					Highlight(mSel, true);
					mSel.SendMessage("OnSelect", true, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	public static Camera mainCamera
	{
		get
		{
			UICamera uICamera = eventHandler;
			return (!(uICamera != null)) ? null : uICamera.cachedCamera;
		}
	}

	public static UICamera eventHandler
	{
		get
		{
			foreach (UICamera m in mList)
			{
				if (m == null || !m.enabled || !m.gameObject.active)
				{
					continue;
				}
				return m;
			}
			return null;
		}
	}

	private void OnApplicationQuit()
	{
		mHighlighted.Clear();
	}

	private static int CompareFunc(UICamera a, UICamera b)
	{
		if (a.cachedCamera.depth < b.cachedCamera.depth)
		{
			return 1;
		}
		if (a.cachedCamera.depth > b.cachedCamera.depth)
		{
			return -1;
		}
		return 0;
	}

	private static bool Raycast(Vector3 inPos, ref RaycastHit hit)
	{
		foreach (UICamera m in mList)
		{
			if (!m.enabled || !m.gameObject.active)
			{
				continue;
			}
			currentCamera = m.cachedCamera;
			Vector3 vector = currentCamera.ScreenToViewportPoint(inPos);
			if (!(vector.x < 0f) && !(vector.x > 1f) && !(vector.y < 0f) && !(vector.y > 1f))
			{
				Ray ray = currentCamera.ScreenPointToRay(inPos);
				int layerMask = currentCamera.cullingMask & (int)m.eventReceiverMask;
				if (Physics.Raycast(ray, out hit, currentCamera.farClipPlane - currentCamera.nearClipPlane, layerMask))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static UICamera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		foreach (UICamera m in mList)
		{
			Camera camera = m.cachedCamera;
			if (camera != null && (camera.cullingMask & num) != 0)
			{
				return m;
			}
		}
		return null;
	}

	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		if (Input.GetKeyDown(up0) || Input.GetKeyDown(up1))
		{
			return 1;
		}
		if (Input.GetKeyDown(down0) || Input.GetKeyDown(down1))
		{
			return -1;
		}
		return 0;
	}

	private static int GetDirection(string axis)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (mNextEvent < realtimeSinceStartup)
		{
			float axis2 = Input.GetAxis(axis);
			if (axis2 > 0.75f)
			{
				mNextEvent = realtimeSinceStartup + 0.25f;
				return 1;
			}
			if (axis2 < -0.75f)
			{
				mNextEvent = realtimeSinceStartup + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	private static void Highlight(GameObject go, bool highlighted)
	{
		if (!(go != null))
		{
			return;
		}
		int num = mHighlighted.Count;
		while (num > 0)
		{
			Highlighted highlighted2 = mHighlighted[--num];
			if (highlighted2 == null || highlighted2.go == null)
			{
				mHighlighted.RemoveAt(num);
			}
			else if (highlighted2.go == go)
			{
				if (highlighted)
				{
					highlighted2.counter++;
				}
				else if (--highlighted2.counter < 1)
				{
					mHighlighted.Remove(highlighted2);
					go.SendMessage("OnHover", false, SendMessageOptions.DontRequireReceiver);
				}
				return;
			}
		}
		if (highlighted)
		{
			Highlighted highlighted3 = new Highlighted();
			highlighted3.go = go;
			highlighted3.counter = 1;
			mHighlighted.Add(highlighted3);
			go.SendMessage("OnHover", true, SendMessageOptions.DontRequireReceiver);
		}
	}

	private MouseOrTouch GetTouch(int id)
	{
		MouseOrTouch value;
		if (!mTouches.TryGetValue(id, out value))
		{
			value = new MouseOrTouch();
			mTouches.Add(id, value);
		}
		return value;
	}

	private void RemoveTouch(int id)
	{
		mTouches.Remove(id);
	}

	private void Awake()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			useMouse = false;
			useTouch = true;
			useKeyboard = false;
			useController = false;
		}
		else if (Application.platform == RuntimePlatform.PS3 || Application.platform == RuntimePlatform.XBOX360)
		{
			useMouse = false;
			useTouch = false;
			useKeyboard = false;
			useController = true;
		}
		else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
		{
			mIsEditor = true;
		}
		mMouse.pos = Input.mousePosition;
		lastTouchPosition = mMouse.pos;
		mList.Add(this);
		mList.Sort(CompareFunc);
		if ((int)eventReceiverMask == -1)
		{
			eventReceiverMask = base.GetComponent<Camera>().cullingMask;
		}
	}

	private void OnDestroy()
	{
		mList.Remove(this);
	}

	private void FixedUpdate()
	{
		if (useMouse && Application.isPlaying && handlesEvents)
		{
			mMouse.current = ((!Raycast(Input.mousePosition, ref lastHit)) ? fallThrough : lastHit.collider.gameObject);
		}
	}

	private void Update()
	{
		if (!Application.isPlaying || !handlesEvents)
		{
			return;
		}
		if (useMouse || (useTouch && mIsEditor))
		{
			ProcessMouse();
		}
		if (useTouch)
		{
			ProcessTouches();
		}
		if (useKeyboard && mSel != null && Input.GetKeyDown(KeyCode.Escape))
		{
			selectedObject = null;
		}
		if (mSel != null)
		{
			string text = Input.inputString;
			if (useKeyboard && Input.GetKeyDown(KeyCode.Delete))
			{
				text += "\b";
			}
			if (text.Length > 0)
			{
				if (mTooltip != null)
				{
					ShowTooltip(false);
				}
				mSel.SendMessage("OnInput", text, SendMessageOptions.DontRequireReceiver);
			}
			ProcessOthers();
		}
		if (useMouse && mMouse.hover != null)
		{
			float axis = Input.GetAxis(scrollAxisName);
			if (axis != 0f)
			{
				mMouse.hover.SendMessage("OnScroll", axis, SendMessageOptions.DontRequireReceiver);
			}
			if (mTooltipTime != 0f && mTooltipTime < Time.realtimeSinceStartup)
			{
				mTooltip = mMouse.hover;
				ShowTooltip(true);
			}
		}
	}

	private void ProcessMouse()
	{
		currentTouch = mMouse;
		lastTouchPosition = Input.mousePosition;
		currentTouch.delta = lastTouchPosition - currentTouch.pos;
		for (int i = 0; i < 3; i++)
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(i);
			bool mouseButtonUp = Input.GetMouseButtonUp(i);
			currentTouchID = -1 - i;
			if (mouseButtonDown || mouseButtonUp || Time.timeScale == 0f)
			{
				currentTouch.current = ((!Raycast(lastTouchPosition, ref lastHit)) ? fallThrough : lastHit.collider.gameObject);
			}
			if (mouseButtonDown)
			{
				currentTouch.pressedCam = currentCamera;
			}
			else if (currentTouch.pressed != null)
			{
				currentCamera = currentTouch.pressedCam;
			}
			if (i == 0 && currentTouch.pos != lastTouchPosition)
			{
				if (mTooltipTime != 0f)
				{
					mTooltipTime = Time.realtimeSinceStartup + tooltipDelay;
				}
				else if (mTooltip != null)
				{
					ShowTooltip(false);
				}
				currentTouch.pos = lastTouchPosition;
			}
			ProcessTouch(mouseButtonDown, mouseButtonUp);
		}
		currentTouch = null;
	}

	private void ProcessTouches()
	{
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			currentTouchID = touch.fingerId;
			currentTouch = GetTouch(currentTouchID);
			bool flag = touch.phase == TouchPhase.Began;
			bool flag2 = touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended;
			lastTouchPosition = touch.position;
			currentTouch.pos = lastTouchPosition;
			currentTouch.delta = touch.deltaPosition;
			if (flag || flag2)
			{
				currentTouch.current = ((!Raycast(touch.position, ref lastHit)) ? fallThrough : lastHit.collider.gameObject);
			}
			if (flag)
			{
				currentTouch.pressedCam = currentCamera;
			}
			else if (currentTouch.pressed != null)
			{
				currentCamera = currentTouch.pressedCam;
			}
			ProcessTouch(flag, flag2);
			if (flag2)
			{
				RemoveTouch(currentTouchID);
			}
			currentTouch = null;
		}
	}

	private void ProcessOthers()
	{
		currentTouchID = -100;
		currentTouch = mJoystick;
		bool flag = useKeyboard && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space));
		bool flag2 = useController && Input.GetKeyDown(KeyCode.JoystickButton0);
		bool flag3 = useKeyboard && (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Space));
		bool flag4 = useController && Input.GetKeyUp(KeyCode.JoystickButton0);
		bool flag5 = flag || flag2;
		bool flag6 = flag3 || flag4;
		if (flag5 || flag6)
		{
			currentTouch.hover = mSel;
			currentTouch.current = mSel;
			ProcessTouch(flag5, flag6);
		}
		int num = 0;
		int num2 = 0;
		if (useKeyboard)
		{
			num += GetDirection(KeyCode.W, KeyCode.UpArrow, KeyCode.S, KeyCode.DownArrow);
			num2 += GetDirection(KeyCode.D, KeyCode.RightArrow, KeyCode.A, KeyCode.LeftArrow);
		}
		if (useController)
		{
			num += GetDirection("Vertical");
			num2 += GetDirection("Horizontal");
		}
		if (num != 0)
		{
			mSel.SendMessage("OnKey", (num <= 0) ? KeyCode.DownArrow : KeyCode.UpArrow, SendMessageOptions.DontRequireReceiver);
		}
		if (num2 != 0)
		{
			mSel.SendMessage("OnKey", (num2 <= 0) ? KeyCode.LeftArrow : KeyCode.RightArrow, SendMessageOptions.DontRequireReceiver);
		}
		if (useKeyboard && Input.GetKeyDown(KeyCode.Tab))
		{
			mSel.SendMessage("OnKey", KeyCode.Tab, SendMessageOptions.DontRequireReceiver);
		}
		if (useController && Input.GetKeyUp(KeyCode.JoystickButton1))
		{
			mSel.SendMessage("OnKey", KeyCode.Escape, SendMessageOptions.DontRequireReceiver);
		}
		currentTouch = null;
	}

	private void ProcessTouch(bool pressed, bool unpressed)
	{
		if (currentTouch.pressed == null && currentTouch.hover != currentTouch.current && currentTouch.hover != null)
		{
			if (mTooltip != null)
			{
				ShowTooltip(false);
			}
			Highlight(currentTouch.hover, false);
		}
		if (currentTouch.pressed != null && currentTouch.delta.magnitude != 0f)
		{
			if (mTooltip != null)
			{
				ShowTooltip(false);
			}
			currentTouch.totalDelta += currentTouch.delta;
			bool flag = currentTouch.clickNotification == ClickNotification.None;
			currentTouch.pressed.SendMessage("OnDrag", currentTouch.delta, SendMessageOptions.DontRequireReceiver);
			if (flag)
			{
				currentTouch.clickNotification = ClickNotification.None;
			}
			else if (currentTouch.clickNotification == ClickNotification.BasedOnDelta)
			{
				float num = ((currentTouch != mMouse) ? ((float)Screen.height * 0.1f) : 10f);
				if (currentTouch.totalDelta.magnitude > num)
				{
					currentTouch.clickNotification = ClickNotification.None;
				}
			}
		}
		if (pressed)
		{
			if (mTooltip != null)
			{
				ShowTooltip(false);
			}
			currentTouch.pressed = currentTouch.current;
			currentTouch.clickNotification = ClickNotification.Always;
			currentTouch.totalDelta = Vector2.zero;
			if (currentTouch.pressed != null)
			{
				currentTouch.pressed.SendMessage("OnPress", true, SendMessageOptions.DontRequireReceiver);
			}
			if (currentTouch.pressed != mSel)
			{
				if (mTooltip != null)
				{
					ShowTooltip(false);
				}
				selectedObject = null;
			}
		}
		if (unpressed)
		{
			if (mTooltip != null)
			{
				ShowTooltip(false);
			}
			if (currentTouch.pressed != null)
			{
				currentTouch.pressed.SendMessage("OnPress", false, SendMessageOptions.DontRequireReceiver);
				if (currentTouch.pressed == currentTouch.hover)
				{
					currentTouch.pressed.SendMessage("OnHover", true, SendMessageOptions.DontRequireReceiver);
				}
				if (currentTouch.pressed == currentTouch.current)
				{
					if (currentTouch.pressed != mSel)
					{
						mSel = currentTouch.pressed;
						currentTouch.pressed.SendMessage("OnSelect", true, SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						mSel = currentTouch.pressed;
					}
					if (currentTouch.clickNotification == ClickNotification.Always)
					{
						currentTouch.pressed.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
					}
				}
				else
				{
					if (currentTouch.current != null)
					{
						currentTouch.current.SendMessage("OnDrop", currentTouch.pressed, SendMessageOptions.DontRequireReceiver);
					}
					Highlight(currentTouch.pressed, false);
				}
			}
			currentTouch.pressed = null;
		}
		if (useMouse && currentTouch.pressed == null && currentTouch.hover != currentTouch.current)
		{
			mTooltipTime = Time.realtimeSinceStartup + tooltipDelay;
			currentTouch.hover = currentTouch.current;
			Highlight(currentTouch.hover, true);
		}
	}

	private void ShowTooltip(bool val)
	{
		mTooltipTime = 0f;
		if (mTooltip != null)
		{
			mTooltip.SendMessage("OnTooltip", val, SendMessageOptions.DontRequireReceiver);
		}
		if (!val)
		{
			mTooltip = null;
		}
	}
}
