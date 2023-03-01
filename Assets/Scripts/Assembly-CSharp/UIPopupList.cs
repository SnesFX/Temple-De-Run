using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Popup List")]
public class UIPopupList : MonoBehaviour
{
	public enum Position
	{
		Auto = 0,
		Above = 1,
		Below = 2
	}

	private const float animSpeed = 0.15f;

	public UIAtlas atlas;

	public UIFont font;

	public UILabel textLabel;

	public string backgroundSprite;

	public string highlightSprite;

	public Position position;

	public List<string> items = new List<string>();

	public Vector2 padding = new Vector3(4f, 4f);

	public float textScale = 1f;

	public Color textColor = Color.white;

	public Color backgroundColor = Color.white;

	public Color highlightColor = new Color(0.59607846f, 1f, 0.2f, 1f);

	public bool isAnimated = true;

	public bool isLocalized;

	public GameObject eventReceiver;

	public string functionName = "OnSelectionChange";

	[SerializeField]
	private string mSelectedItem;

	private UIPanel mPanel;

	private GameObject mChild;

	private UISprite mHighlight;

	private UILabel mHighlightedLabel;

	private List<UILabel> mLabelList = new List<UILabel>();

	public bool isOpen
	{
		get
		{
			return mChild != null;
		}
	}

	public string selection
	{
		get
		{
			return mSelectedItem;
		}
		set
		{
			if (mSelectedItem != value)
			{
				mSelectedItem = value;
				if (textLabel != null)
				{
					textLabel.text = ((!isLocalized || !(Localization.instance != null)) ? value : Localization.instance.Get(value));
				}
				if (eventReceiver != null && !string.IsNullOrEmpty(functionName))
				{
					eventReceiver.SendMessage(functionName, mSelectedItem, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	private bool handleEvents
	{
		get
		{
			UIButtonKeys component = GetComponent<UIButtonKeys>();
			return component == null || !component.enabled;
		}
		set
		{
			UIButtonKeys component = GetComponent<UIButtonKeys>();
			if (component != null)
			{
				component.enabled = !value;
			}
		}
	}

	private void Start()
	{
		if (string.IsNullOrEmpty(mSelectedItem))
		{
			if (items.Count > 0)
			{
				selection = items[0];
			}
		}
		else
		{
			string text = mSelectedItem;
			mSelectedItem = null;
			selection = text;
		}
	}

	private void OnLocalize(Localization loc)
	{
		if (isLocalized && textLabel != null)
		{
			textLabel.text = loc.Get(mSelectedItem);
		}
	}

	private void Highlight(UILabel lbl, bool instant)
	{
		if (mHighlight != null)
		{
			mHighlightedLabel = lbl;
			Vector3 vector = lbl.cachedTransform.localPosition + new Vector3(0f - padding.x, padding.y, 0f);
			if (instant || !isAnimated)
			{
				mHighlight.cachedTransform.localPosition = vector;
			}
			else
			{
				TweenPosition.Begin(mHighlight.gameObject, 0.1f, vector).method = UITweener.Method.EaseOut;
			}
		}
	}

	private void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			UILabel component = go.GetComponent<UILabel>();
			Highlight(component, false);
		}
	}

	private void Select(UILabel lbl, bool instant)
	{
		Highlight(lbl, instant);
		UIEventListener component = lbl.gameObject.GetComponent<UIEventListener>();
		selection = component.parameter as string;
		UIButtonSound[] components = GetComponents<UIButtonSound>();
		UIButtonSound[] array = components;
		foreach (UIButtonSound uIButtonSound in array)
		{
			if (uIButtonSound.trigger == UIButtonSound.Trigger.OnClick)
			{
				NGUITools.PlaySound(uIButtonSound.audioClip, uIButtonSound.volume);
			}
		}
	}

	private void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			Select(go.GetComponent<UILabel>(), true);
		}
	}

	private void OnKey(KeyCode key)
	{
		if (!base.enabled || !base.gameObject.active || !handleEvents)
		{
			return;
		}
		int num = mLabelList.IndexOf(mHighlightedLabel);
		switch (key)
		{
		case KeyCode.UpArrow:
			if (num > 0)
			{
				Select(mLabelList[--num], false);
			}
			break;
		case KeyCode.DownArrow:
			if (num + 1 < mLabelList.Count)
			{
				Select(mLabelList[++num], false);
			}
			break;
		case KeyCode.Escape:
			OnSelect(false);
			break;
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (isSelected || !(mChild != null))
		{
			return;
		}
		mLabelList.Clear();
		handleEvents = false;
		if (isAnimated)
		{
			UIWidget[] componentsInChildren = mChild.GetComponentsInChildren<UIWidget>();
			UIWidget[] array = componentsInChildren;
			foreach (UIWidget uIWidget in array)
			{
				Color color = uIWidget.color;
				color.a = 0f;
				TweenColor.Begin(uIWidget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
			}
			Collider[] componentsInChildren2 = mChild.GetComponentsInChildren<Collider>();
			Collider[] array2 = componentsInChildren2;
			foreach (Collider collider in array2)
			{
				collider.enabled = false;
			}
			UpdateManager.AddDestroy(mChild, 0.15f);
		}
		else
		{
			Object.Destroy(mChild);
		}
		mChild = null;
	}

	private void AnimateColor(UIWidget widget)
	{
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
	}

	private void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 localPosition = widget.cachedTransform.localPosition;
		Vector3 localPosition2 = ((!placeAbove) ? new Vector3(localPosition.x, 0f, localPosition.z) : new Vector3(localPosition.x, bottom, localPosition.z));
		widget.cachedTransform.localPosition = localPosition2;
		GameObject go = widget.gameObject;
		TweenPosition.Begin(go, 0.15f, localPosition).method = UITweener.Method.EaseOut;
	}

	private void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject go = widget.gameObject;
		Transform cachedTransform = widget.cachedTransform;
		float num = (float)font.size * textScale + padding.y * 2f;
		Vector3 localScale = cachedTransform.localScale;
		cachedTransform.localScale = new Vector3(localScale.x, num, localScale.z);
		TweenScale.Begin(go, 0.15f, localScale).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 localPosition = cachedTransform.localPosition;
			cachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y - localScale.y + num, localPosition.z);
			TweenPosition.Begin(go, 0.15f, localPosition).method = UITweener.Method.EaseOut;
		}
	}

	private void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		AnimateColor(widget);
		AnimatePosition(widget, placeAbove, bottom);
	}

	private void OnClick()
	{
		if (mChild == null && atlas != null && font != null && items.Count > 1)
		{
			mLabelList.Clear();
			handleEvents = true;
			if (mPanel == null)
			{
				mPanel = UIPanel.Find(base.transform, true);
			}
			Transform transform = base.transform;
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform.parent, transform);
			mChild = new GameObject("Drop-down List");
			mChild.layer = base.gameObject.layer;
			Transform transform2 = mChild.transform;
			transform2.parent = transform.parent;
			transform2.localPosition = bounds.min;
			transform2.localRotation = Quaternion.identity;
			transform2.localScale = Vector3.one;
			UISprite uISprite = NGUITools.AddSprite(mChild, atlas, backgroundSprite);
			uISprite.pivot = UIWidget.Pivot.TopLeft;
			uISprite.depth = NGUITools.CalculateNextDepth(mPanel.gameObject);
			uISprite.color = backgroundColor;
			mHighlight = NGUITools.AddSprite(mChild, atlas, highlightSprite);
			mHighlight.pivot = UIWidget.Pivot.TopLeft;
			mHighlight.color = highlightColor;
			float num = (float)font.size * textScale;
			float a = 0f;
			float num2 = 0f - padding.y;
			List<UILabel> list = new List<UILabel>();
			foreach (string item in items)
			{
				UILabel uILabel = NGUITools.AddWidget<UILabel>(mChild);
				uILabel.pivot = UIWidget.Pivot.TopLeft;
				uILabel.font = font;
				uILabel.text = ((!isLocalized || !(Localization.instance != null)) ? item : Localization.instance.Get(item));
				uILabel.color = textColor;
				uILabel.cachedTransform.localPosition = new Vector3(padding.x, num2, 0f);
				uILabel.MakePixelPerfect();
				if (textScale != 1f)
				{
					Vector3 localScale = uILabel.cachedTransform.localScale;
					uILabel.cachedTransform.localScale = localScale * textScale;
				}
				list.Add(uILabel);
				num2 -= num;
				a = Mathf.Max(a, uILabel.relativeSize.x * num);
				UIEventListener uIEventListener = UIEventListener.Add(uILabel.gameObject);
				uIEventListener.onHover = OnItemHover;
				uIEventListener.onPress = OnItemPress;
				uIEventListener.parameter = item;
				if (mSelectedItem == item)
				{
					Highlight(uILabel, true);
				}
				mLabelList.Add(uILabel);
			}
			a = Mathf.Max(a, bounds.size.x - padding.x * 2f);
			foreach (UILabel item2 in list)
			{
				BoxCollider boxCollider = NGUITools.AddWidgetCollider(item2.gameObject);
				boxCollider.center = new Vector3(a * 0.5f / num, -0.5f, boxCollider.center.z);
				boxCollider.size = new Vector3(a / num, 1f, 1f);
			}
			a += padding.x * 2f;
			num2 -= padding.y;
			uISprite.cachedTransform.localScale = new Vector3(a, 0f - num2, 1f);
			mHighlight.cachedTransform.localScale = new Vector3(a, num + padding.y * 2f, 1f);
			bool flag = position == Position.Above;
			if (position == Position.Auto)
			{
				UICamera uICamera = UICamera.FindCameraForLayer(base.gameObject.layer);
				if (uICamera != null)
				{
					flag = uICamera.cachedCamera.WorldToViewportPoint(transform.position).y < 0.5f;
				}
			}
			if (isAnimated)
			{
				float bottom = num2 + num;
				Animate(mHighlight, flag, bottom);
				foreach (UILabel item3 in list)
				{
					Animate(item3, flag, bottom);
				}
				AnimateColor(uISprite);
				AnimateScale(uISprite, flag, bottom);
			}
			if (flag)
			{
				transform2.localPosition = new Vector3(bounds.min.x, bounds.max.y - num2, bounds.min.z);
			}
		}
		else
		{
			OnSelect(false);
		}
	}
}
