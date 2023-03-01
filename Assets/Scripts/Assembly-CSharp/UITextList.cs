using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
	public enum Style
	{
		Text = 0,
		Chat = 1
	}

	protected class Paragraph
	{
		public string text;

		public string[] lines;
	}

	public Style style;

	public UILabel textLabel;

	public float maxWidth;

	public float maxHeight;

	public int maxEntries = 50;

	public bool supportScrollWheel = true;

	protected char[] mSeparator = new char[1] { '\n' };

	protected List<Paragraph> mParagraphs = new List<Paragraph>();

	protected float mScroll;

	protected bool mSelected;

	protected int mTotalLines;

	public void Clear()
	{
		mParagraphs.Clear();
		UpdateVisibleText();
	}

	public void Add(string text)
	{
		Add(text, true);
	}

	protected void Add(string text, bool updateVisible)
	{
		Paragraph paragraph = null;
		if (mParagraphs.Count < maxEntries)
		{
			paragraph = new Paragraph();
		}
		else
		{
			paragraph = mParagraphs[0];
			mParagraphs.RemoveAt(0);
		}
		paragraph.text = text;
		mParagraphs.Add(paragraph);
		if (textLabel != null && textLabel.font != null)
		{
			paragraph.lines = textLabel.font.WrapText(paragraph.text, maxWidth / textLabel.transform.localScale.y, true, true).Split(mSeparator);
			mTotalLines = 0;
			foreach (Paragraph mParagraph in mParagraphs)
			{
				mTotalLines += mParagraph.lines.Length;
			}
		}
		if (updateVisible)
		{
			UpdateVisibleText();
		}
	}

	private void Awake()
	{
		if (textLabel == null)
		{
			textLabel = GetComponentInChildren<UILabel>();
		}
		if (textLabel != null)
		{
			textLabel.lineWidth = 0;
		}
		Collider collider = base.GetComponent<Collider>();
		if (collider != null)
		{
			if (maxHeight <= 0f)
			{
				maxHeight = collider.bounds.size.y / base.transform.lossyScale.y;
			}
			if (maxWidth <= 0f)
			{
				maxWidth = collider.bounds.size.x / base.transform.lossyScale.x;
			}
		}
	}

	private void OnSelect(bool selected)
	{
		mSelected = selected;
	}

	protected void UpdateVisibleText()
	{
		if (!(textLabel != null))
		{
			return;
		}
		UIFont font = textLabel.font;
		if (!(font != null))
		{
			return;
		}
		int num = 0;
		int num2 = ((!(maxHeight > 0f)) ? 100000 : Mathf.FloorToInt(maxHeight / textLabel.cachedTransform.localScale.y));
		int num3 = Mathf.RoundToInt(mScroll);
		if (num2 + num3 > mTotalLines)
		{
			num3 = Mathf.Max(0, mTotalLines - num2);
			mScroll = num3;
		}
		if (style == Style.Chat)
		{
			num3 = Mathf.Max(0, mTotalLines - num2 - num3);
		}
		string text = string.Empty;
		foreach (Paragraph mParagraph in mParagraphs)
		{
			string[] lines = mParagraph.lines;
			foreach (string text2 in lines)
			{
				if (num3 > 0)
				{
					num3--;
					continue;
				}
				if (text.Length > 0)
				{
					text += "\n";
				}
				text += text2;
				num++;
				if (num >= num2)
				{
					break;
				}
			}
			if (num >= num2)
			{
				break;
			}
		}
		textLabel.text = text;
	}

	private void OnScroll(float val)
	{
		if (mSelected && supportScrollWheel)
		{
			val *= ((style != Style.Chat) ? (-10f) : 10f);
			mScroll = Mathf.Max(0f, mScroll + val);
			UpdateVisibleText();
		}
	}
}
