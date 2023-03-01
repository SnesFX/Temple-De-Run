using UnityEngine;

[ExecuteInEditMode]
public class OffsetOnWidth : MonoBehaviour
{
	public UILabel Label;

	private void Start()
	{
	}

	private void Update()
	{
		if (!(Label == null) && Label.panel.enabled)
		{
			Transform transform = Label.transform;
			Vector3 localPosition = transform.localPosition;
			Vector3 localScale = transform.localScale;
			Vector2 visibleSize = Label.visibleSize;
			visibleSize.x *= localScale.x;
			visibleSize.y *= localScale.y;
			if (Label.pivot == UIWidget.Pivot.Center)
			{
				visibleSize.x /= 2f;
			}
			if (base.transform.localPosition.x != 0f - visibleSize.x)
			{
				base.transform.localPosition = new Vector3(0f - visibleSize.x, 0f, 0f);
			}
		}
	}
}
