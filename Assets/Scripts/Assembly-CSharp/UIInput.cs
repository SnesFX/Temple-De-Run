using UnityEngine;

[AddComponentMenu("NGUI/UI/Input (Basic)")]
public class UIInput : MonoBehaviour
{
	public static UIInput current;

	public UILabel label;

	public int maxChars;

	public string caratChar = "|";

	public Color activeColor = Color.white;

	public GameObject eventReceiver;

	public string functionName = "OnSubmit";

	private string mText = string.Empty;

	private string mDefaultText = string.Empty;

	private Color mDefaultColor = Color.white;

	private TouchScreenKeyboard mKeyboard;

	public string text
	{
		get
		{
			if (selected)
			{
				return mText;
			}
			return (!(label != null)) ? string.Empty : label.text;
		}
		set
		{
			mText = value;
			if (label != null)
			{
				label.supportEncoding = false;
				label.text = ((!selected) ? value : (value + caratChar));
				label.showLastPasswordChar = selected;
			}
		}
	}

	public bool selected
	{
		get
		{
			return UICamera.selectedObject == base.gameObject;
		}
		set
		{
			if (!value && UICamera.selectedObject == base.gameObject)
			{
				UICamera.selectedObject = null;
			}
			else if (value)
			{
				UICamera.selectedObject = base.gameObject;
			}
		}
	}

	protected void Init()
	{
		if (label == null)
		{
			label = GetComponentInChildren<UILabel>();
		}
		if (label != null)
		{
			mDefaultText = label.text;
			mDefaultColor = label.color;
			label.supportEncoding = false;
		}
	}

	private void Start()
	{
		Init();
	}

	private void OnSelect(bool isSelected)
	{
		if (!(label != null) || !base.enabled || !base.gameObject.active)
		{
			return;
		}
		if (isSelected)
		{
			mText = ((!(label.text == mDefaultText)) ? label.text : string.Empty);
			label.color = activeColor;
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
				mKeyboard = TouchScreenKeyboard.Open(mText);
				return;
			}
			Input.imeCompositionMode = IMECompositionMode.On;
			Transform cachedTransform = label.cachedTransform;
			Vector3 position = label.pivotOffset;
			position.y += label.relativeSize.y;
			position = cachedTransform.TransformPoint(position);
			Input.compositionCursorPos = UICamera.currentCamera.WorldToScreenPoint(position);
			UpdateLabel();
		}
		else if (mKeyboard != null)
		{
			mKeyboard.active = false;
		}
		else
		{
			if (string.IsNullOrEmpty(mText))
			{
				label.text = mDefaultText;
				label.color = mDefaultColor;
			}
			else
			{
				label.text = mText;
			}
			label.showLastPasswordChar = false;
			Input.imeCompositionMode = IMECompositionMode.Off;
		}
	}

	private void Update()
	{
		if (mKeyboard == null)
		{
			return;
		}
		string text = mKeyboard.text;
		if (mText != text)
		{
			mText = text;
			UpdateLabel();
		}
		if (mKeyboard.done)
		{
			mKeyboard = null;
			current = this;
			if (eventReceiver == null)
			{
				eventReceiver = base.gameObject;
			}
			eventReceiver.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
			current = null;
			selected = false;
		}
	}

	private void OnInput(string input)
	{
		if (!selected || !base.enabled || !base.gameObject.active || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return;
		}
		foreach (char c in input)
		{
			switch (c)
			{
			case '\b':
				if (mText.Length > 0)
				{
					mText = mText.Substring(0, mText.Length - 1);
				}
				break;
			case '\n':
			case '\r':
				current = this;
				if (eventReceiver == null)
				{
					eventReceiver = base.gameObject;
				}
				eventReceiver.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
				current = null;
				selected = false;
				return;
			default:
				mText += c;
				break;
			}
		}
		UpdateLabel();
	}

	private void UpdateLabel()
	{
		if (maxChars > 0 && mText.Length > maxChars)
		{
			mText = mText.Substring(0, maxChars);
		}
		if (label.font != null)
		{
			string text = ((!selected) ? mText : (mText + Input.compositionString + caratChar));
			text = label.font.WrapText(text, (float)label.lineWidth / label.cachedTransform.localScale.x, true, false);
			if (!label.multiLine)
			{
				string[] array = text.Split('\n');
				text = ((array.Length <= 0) ? string.Empty : array[array.Length - 1]);
			}
			label.text = text;
			label.showLastPasswordChar = selected;
		}
	}
}
