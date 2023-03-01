using UnityEngine;

[AddComponentMenu("NGUI/Tween/Position")]
public class TweenPosition : UITweener
{
	public Vector3 from;

	public Vector3 to;

	private Transform mTrans;

	public Vector3 position
	{
		get
		{
			return mTrans.localPosition;
		}
		set
		{
			mTrans.localPosition = value;
		}
	}

	private void Awake()
	{
		mTrans = base.transform;
	}

	protected override void OnUpdate(float factor)
	{
		mTrans.localPosition = from * (1f - factor) + to * factor;
	}

	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.from = tweenPosition.position;
		tweenPosition.to = pos;
		return tweenPosition;
	}
}
