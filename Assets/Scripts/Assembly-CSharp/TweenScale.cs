using UnityEngine;

[AddComponentMenu("NGUI/Tween/Scale")]
public class TweenScale : UITweener
{
	public Vector3 from;

	public Vector3 to;

	public bool updateTable;

	private Transform mTrans;

	private UITable mTable;

	public Vector3 scale
	{
		get
		{
			return mTrans.localScale;
		}
		set
		{
			mTrans.localScale = value;
		}
	}

	private void Awake()
	{
		mTrans = base.transform;
	}

	protected override void OnUpdate(float factor)
	{
		mTrans.localScale = from * (1f - factor) + to * factor;
		if (!updateTable)
		{
			return;
		}
		if (mTable == null)
		{
			mTable = NGUITools.FindInParents<UITable>(base.gameObject);
			if (mTable == null)
			{
				updateTable = false;
				return;
			}
		}
		mTable.repositionNow = true;
	}

	public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
	{
		TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration);
		tweenScale.from = tweenScale.scale;
		tweenScale.to = scale;
		return tweenScale;
	}
}
