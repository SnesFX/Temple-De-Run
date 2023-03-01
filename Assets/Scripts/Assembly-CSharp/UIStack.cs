using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Stack")]
public class UIStack : MonoBehaviour
{
	public enum Arrangement
	{
		Horizontal = 0,
		Vertical = 1
	}

	public Arrangement arrangement;

	public float padding;

	public bool repositionNow;

	private void Start()
	{
		Reposition();
	}

	private void Update()
	{
		if (repositionNow)
		{
			repositionNow = false;
			Reposition();
		}
	}

	public void Reposition()
	{
		Transform transform = base.transform;
		float num = 0f;
		float num2 = 0f;
		Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>(transform.childCount);
		foreach (Transform item in transform)
		{
			dictionary.Add(item.name, item);
		}
		List<string> list = new List<string>(dictionary.Keys);
		list.Sort();
		foreach (string item2 in list)
		{
			Transform transform3 = dictionary[item2];
			float num3 = num;
			float num4 = num2;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			UIWidget uIWidget = null;
			UIStackSpacing component = transform3.GetComponent<UIStackSpacing>();
			if (component != null)
			{
				uIWidget = component.SpacingWidget;
				num7 = component.ExtraPadding;
			}
			if (uIWidget == null)
			{
				UISprite componentInChildren = transform3.GetComponentInChildren<UISprite>();
				uIWidget = componentInChildren;
			}
			if (uIWidget != null)
			{
				Vector2 relativeSize = uIWidget.relativeSize;
				relativeSize.x *= uIWidget.transform.localScale.x;
				relativeSize.y *= uIWidget.transform.localScale.y;
				if (arrangement == Arrangement.Horizontal)
				{
					num += relativeSize.x + padding + num7;
					num3 += num7;
				}
				else
				{
					num2 -= relativeSize.y + padding + num7;
					num4 -= num7;
				}
				if (uIWidget.pivot == UIWidget.Pivot.Center)
				{
					num5 += relativeSize.y / 2f;
				}
				num3 += num6;
				num4 -= num5;
			}
			transform3.localPosition = new Vector3(num3, num4, 0f);
		}
	}
}
