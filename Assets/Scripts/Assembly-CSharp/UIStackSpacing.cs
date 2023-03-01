using UnityEngine;

[ExecuteInEditMode]
public class UIStackSpacing : MonoBehaviour
{
	public UIWidget SpacingWidget;

	public float ExtraPadding;

	private float OldExtraPadding;

	private void RepositionNow(GameObject thing)
	{
		UIStack component = thing.GetComponent<UIStack>();
		if (component != null)
		{
			component.repositionNow = true;
		}
		else if (base.transform.parent != null)
		{
			RepositionNow(base.transform.parent.gameObject);
		}
	}

	private void Update()
	{
		if (OldExtraPadding != ExtraPadding)
		{
			OldExtraPadding = ExtraPadding;
			RepositionNow(base.gameObject);
		}
	}
}
