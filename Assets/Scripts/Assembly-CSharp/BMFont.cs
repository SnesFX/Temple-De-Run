using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BMFont
{
	[SerializeField]
	private BMGlyph[] mGlyphs;

	[SerializeField]
	private int mSize;

	[SerializeField]
	private int mBase;

	[SerializeField]
	private int mWidth;

	[SerializeField]
	private int mHeight;

	[SerializeField]
	private string mSpriteName;

	[SerializeField]
	private List<BMGlyph> mSaved = new List<BMGlyph>();

	private Dictionary<int, BMGlyph> mDict = new Dictionary<int, BMGlyph>();

	public bool isValid
	{
		get
		{
			return mSaved.Count > 0 || LegacyCheck();
		}
	}

	public int charSize
	{
		get
		{
			return mSize;
		}
		set
		{
			mSize = value;
		}
	}

	public int baseOffset
	{
		get
		{
			return mBase;
		}
		set
		{
			mBase = value;
		}
	}

	public int texWidth
	{
		get
		{
			return mWidth;
		}
		set
		{
			mWidth = value;
		}
	}

	public int texHeight
	{
		get
		{
			return mHeight;
		}
		set
		{
			mHeight = value;
		}
	}

	public int glyphCount
	{
		get
		{
			return isValid ? mSaved.Count : 0;
		}
	}

	public string spriteName
	{
		get
		{
			return mSpriteName;
		}
		set
		{
			mSpriteName = value;
		}
	}

	public bool LegacyCheck()
	{
		if (mGlyphs != null && mGlyphs.Length > 0)
		{
			int num = 0;
			BMGlyph[] array = mGlyphs;
			foreach (BMGlyph bMGlyph in array)
			{
				if (bMGlyph != null)
				{
					bMGlyph.index = num;
					mSaved.Add(bMGlyph);
					mDict.Add(num, bMGlyph);
				}
				num++;
			}
			mGlyphs = null;
			return true;
		}
		return false;
	}

	private int GetArraySize(int index)
	{
		if (index < 256)
		{
			return 256;
		}
		if (index < 65536)
		{
			return 65536;
		}
		if (index < 262144)
		{
			return 262144;
		}
		return 0;
	}

	public BMGlyph GetGlyph(int index, bool createIfMissing)
	{
		BMGlyph value = null;
		if (mDict.Count == 0)
		{
			if (mSaved.Count == 0)
			{
				LegacyCheck();
			}
			else
			{
				foreach (BMGlyph item in mSaved)
				{
					mDict.Add(item.index, item);
				}
			}
		}
		if (!mDict.TryGetValue(index, out value) && createIfMissing)
		{
			value = new BMGlyph();
			value.index = index;
			mSaved.Add(value);
			mDict.Add(index, value);
		}
		return value;
	}

	public BMGlyph GetGlyph(int index)
	{
		return GetGlyph(index, false);
	}

	public void Clear()
	{
		mGlyphs = null;
		mDict.Clear();
		mSaved.Clear();
	}

	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		if (!isValid)
		{
			return;
		}
		foreach (BMGlyph item in mSaved)
		{
			if (item != null)
			{
				item.Trim(xMin, yMin, xMax, yMax);
			}
		}
	}
}
