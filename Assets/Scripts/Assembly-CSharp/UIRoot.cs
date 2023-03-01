using UnityEngine;

[AddComponentMenu("NGUI/UI/Root")]
[ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{
	public bool automatic = true;

	public int manualHeight = 800;

	private Transform mTrans;

	private void Start()
	{
		mTrans = base.transform;
		UIOrthoCamera componentInChildren = GetComponentInChildren<UIOrthoCamera>();
		if (componentInChildren != null)
		{
			Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
			Camera component = componentInChildren.gameObject.GetComponent<Camera>();
			componentInChildren.enabled = false;
			if (component != null)
			{
				component.orthographicSize = 1f;
			}
		}
	}

	private void Update()
	{
		manualHeight = Mathf.Max(2, (!automatic) ? manualHeight : Screen.height);
		float num = 2f / (float)manualHeight;
		Vector3 localScale = mTrans.localScale;
		if (!Mathf.Approximately(localScale.x, num) || !Mathf.Approximately(localScale.y, num) || !Mathf.Approximately(localScale.z, num))
		{
			mTrans.localScale = new Vector3(num, num, num);
		}
	}
}
