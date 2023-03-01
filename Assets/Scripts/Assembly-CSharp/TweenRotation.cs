using UnityEngine;

[AddComponentMenu("NGUI/Tween/Rotation")]
public class TweenRotation : UITweener
{
	public Vector3 from;

	public Vector3 to;

	private Transform mTrans;

	public Quaternion rotation
	{
		get
		{
			return mTrans.localRotation;
		}
		set
		{
			mTrans.localRotation = value;
		}
	}

	private void Awake()
	{
		mTrans = base.transform;
	}

	protected override void OnUpdate(float factor)
	{
		mTrans.localRotation = Quaternion.Slerp(Quaternion.Euler(from), Quaternion.Euler(to), factor);
	}

	public static TweenRotation Begin(GameObject go, float duration, Quaternion rot)
	{
		TweenRotation tweenRotation = UITweener.Begin<TweenRotation>(go, duration);
		tweenRotation.from = tweenRotation.rotation.eulerAngles;
		tweenRotation.to = rot.eulerAngles;
		return tweenRotation;
	}
}
