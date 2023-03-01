using System.Collections;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
	public GameController Controller;

	public UIPanel Panel;

	private BasePanel ReturnToPanel;

	public virtual void Start()
	{
		Panel = GetComponent<UIPanel>();
	}

	public virtual void ShowAll()
	{
		Debug.Log("PANEL SHOW ALL: " + base.name);
		base.gameObject.SetActiveRecursively(true);
	}

	public virtual void HideAll()
	{
		Debug.Log("PANEL HIDE ALL: " + base.name);
		base.gameObject.SetActiveRecursively(false);
	}

	public void ShowPanel()
	{
		Panel.enabled = true;
	}

	public void HidePanel()
	{
		Panel.enabled = false;
	}

	public void FadeIn(float delay = 0f, float duration = 1f)
	{
		HidePanel();
		ShowAll();
		StartCoroutine(FadeInAfterDelay(new Color(0f, 0f, 0f, 0f), new Color(1f, 1f, 1f, 1f), delay, duration));
	}

	public void FadeOut(float delay = 0f, float duration = 1f)
	{
		StartCoroutine(FadeInAfterDelay(new Color(1f, 1f, 1f, 1f), new Color(0f, 0f, 0f, 0f), delay, duration));
	}

	private IEnumerator FadeInAfterDelay(Color startColor, Color endColor, float delay, float duration)
	{
		yield return new WaitForSeconds(delay);
		ShowPanel();
		UISprite[] ss = GetComponentsInChildren<UISprite>();
		UISprite[] array = ss;
		foreach (UISprite s in array)
		{
			s.color = startColor;
			TweenColor.Begin(s.gameObject, duration, endColor);
		}
	}

	public virtual void Show()
	{
		SlideIn();
	}

	public void SlideInDontClearReturn()
	{
		SlideIn(ReturnToPanel);
	}

	public void SlideIn()
	{
		SlideIn(null);
	}

	public virtual void SlideIn(BasePanel returnTo)
	{
		ReturnToPanel = returnTo;
	}

	public void ReturnMenu()
	{
		HideAll();
		if (ReturnToPanel != null)
		{
			ReturnToPanel.SlideInDontClearReturn();
		}
		else
		{
			MainMenu.Instance.Show();
		}
	}
}
