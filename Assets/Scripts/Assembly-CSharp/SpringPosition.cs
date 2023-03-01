using UnityEngine;

[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : IgnoreTimeScale
{
	public Vector3 target = Vector3.zero;

	public float strength = 10f;

	public bool worldSpace;

	public bool ignoreTimeScale;

	private Transform mTrans;

	private float mThreshold;

	private void Start()
	{
		mTrans = base.transform;
	}

	private void Update()
	{
		float deltaTime = ((!ignoreTimeScale) ? Time.deltaTime : UpdateRealTimeDelta());
		if (worldSpace)
		{
			if (mThreshold == 0f)
			{
				mThreshold = (target - mTrans.position).magnitude * 0.005f;
			}
			mTrans.position = NGUIMath.SpringLerp(mTrans.position, target, strength, deltaTime);
			if (mThreshold >= (target - mTrans.position).magnitude)
			{
				base.enabled = false;
			}
		}
		else
		{
			if (mThreshold == 0f)
			{
				mThreshold = (target - mTrans.localPosition).magnitude * 0.005f;
			}
			mTrans.localPosition = NGUIMath.SpringLerp(mTrans.localPosition, target, strength, deltaTime);
			if (mThreshold >= (target - mTrans.localPosition).magnitude)
			{
				base.enabled = false;
			}
		}
	}

	public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPosition springPosition = go.GetComponent<SpringPosition>();
		if (springPosition == null)
		{
			springPosition = go.AddComponent<SpringPosition>();
		}
		springPosition.target = pos;
		springPosition.strength = strength;
		if (!springPosition.enabled)
		{
			springPosition.mThreshold = 0f;
			springPosition.enabled = true;
		}
		return springPosition;
	}
}
