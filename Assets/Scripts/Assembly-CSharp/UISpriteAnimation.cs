using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/UI/Sprite Animation")]
[ExecuteInEditMode]
public class UISpriteAnimation : MonoBehaviour
{
	[SerializeField]
	private int mFPS = 30;

	[SerializeField]
	private string mPrefix = string.Empty;

	private UISprite mSprite;

	private float mDelta;

	private int mIndex;

	private List<string> mSpriteNames = new List<string>();

	public int framesPerSecond
	{
		get
		{
			return mFPS;
		}
		set
		{
			mFPS = value;
		}
	}

	public string namePrefix
	{
		get
		{
			return mPrefix;
		}
		set
		{
			if (mPrefix != value)
			{
				mPrefix = value;
				RebuildSpriteList();
			}
		}
	}

	private void Start()
	{
		RebuildSpriteList();
	}

	private void Update()
	{
		if (mSpriteNames.Count <= 1 || !Application.isPlaying)
		{
			return;
		}
		mDelta += Time.deltaTime;
		float num = ((!((float)mFPS > 0f)) ? 0f : (1f / (float)mFPS));
		if (num < mDelta)
		{
			mDelta = ((!(num > 0f)) ? 0f : (mDelta - num));
			if (++mIndex >= mSpriteNames.Count)
			{
				mIndex = 0;
			}
			mSprite.spriteName = mSpriteNames[mIndex];
			mSprite.MakePixelPerfect();
		}
	}

	private void RebuildSpriteList()
	{
		if (mSprite == null)
		{
			mSprite = GetComponent<UISprite>();
		}
		mSpriteNames.Clear();
		if (!(mSprite != null) || !(mSprite.atlas != null))
		{
			return;
		}
		List<UIAtlas.Sprite> spriteList = mSprite.atlas.spriteList;
		foreach (UIAtlas.Sprite item in spriteList)
		{
			if (string.IsNullOrEmpty(mPrefix) || item.name.StartsWith(mPrefix))
			{
				mSpriteNames.Add(item.name);
			}
		}
		mSpriteNames.Sort();
	}
}
