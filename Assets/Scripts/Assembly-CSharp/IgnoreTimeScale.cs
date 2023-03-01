using UnityEngine;

[AddComponentMenu("NGUI/Internal/Ignore TimeScale Behaviour")]
public class IgnoreTimeScale : MonoBehaviour
{
	private float mTime;

	private float mDelta;

	public float realTimeDelta
	{
		get
		{
			return mDelta;
		}
	}

	private void OnEnable()
	{
		mTime = Time.realtimeSinceStartup;
	}

	private void Start()
	{
		mTime = Time.realtimeSinceStartup;
	}

	protected float UpdateRealTimeDelta()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		mDelta = Mathf.Max(0f, realtimeSinceStartup - mTime);
		mTime = realtimeSinceStartup;
		return mDelta;
	}
}
