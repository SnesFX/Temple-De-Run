using System.Collections;
using UnityEngine;

public class OptionsGUI : BasePanel
{
	public GameObject SlideInOffset;

	public GameObject BackButton;

	private void Awake()
	{
		base.Start();
		HideAll();
	}

	public override void SlideIn(BasePanel returnTo)
	{
		base.SlideIn(returnTo);
		ShowAll();
		StartCoroutine(SlideInAnimation());
	}

	private IEnumerator SlideInAnimation()
	{
		ShowAll();
		SlideInOffset.transform.localPosition = new Vector3(0f, 1125f, -40f);
		TweenPosition.Begin(SlideInOffset, 0.5f, new Vector3(0f, 0f, -40f)).method = UITweener.Method.EaseIn;
		Vector3 mm = BackButton.transform.localPosition;
		BackButton.GetComponentInChildren<UISprite>().enabled = false;
		BackButton.transform.Translate(0f, -200f, 0f);
		yield return new WaitForSeconds(0.75f);
		BackButton.GetComponentInChildren<UISprite>().enabled = true;
		TweenPosition.Begin(BackButton, 0.25f, mm);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
	}

	private void BackButtonClicked()
	{
		ReturnMenu();
	}
}
