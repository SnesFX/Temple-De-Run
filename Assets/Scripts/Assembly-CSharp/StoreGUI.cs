using System.Collections;
using UnityEngine;

public class StoreGUI : BasePanel
{
	public GameObject SlideInOffset;

	public GameObject GetMoreCoinsButton;

	public GameObject BackButton;

	public UILabel CoinLabel;

	public EndGameUI EndGameInterface;

	public VCGUI CoinsGUI;

	public Transform Scroller;

	public RecordManager RecordManager;

	public PlayerManager PlayerManager;

	public static StoreGUI Instance;

	private void Awake()
	{
		Panel = GetComponent<UIPanel>();
		Setup();
	}

	private void Setup()
	{
		Instance = this;
		HideAll();
	}

	public override void ShowAll()
	{
		base.ShowAll();
		Debug.Log("Attempting Reconfigure");
		Scroller.BroadcastMessage("Reconfigure", SendMessageOptions.RequireReceiver);
		AdjustCoins();
	}

	public override void SlideIn(BasePanel returnTo)
	{
		base.SlideIn(returnTo);
		ShowAll();
		StartCoroutine(SlideInAnimation());
	}

	private IEnumerator SlideInAnimation()
	{
		SlideInOffset.transform.localPosition = new Vector3(0f, 1125f, -40f);
		TweenPosition.Begin(SlideInOffset, 0.5f, new Vector3(0f, 0f, -40f)).method = UITweener.Method.EaseIn;
		Vector3 mm = GetMoreCoinsButton.transform.localPosition;
		Vector3 ra = BackButton.transform.localPosition;
		GetMoreCoinsButton.GetComponentInChildren<UISprite>().enabled = false;
		GetMoreCoinsButton.transform.Translate(0f, -200f, 0f);
		BackButton.GetComponentInChildren<UISprite>().enabled = false;
		BackButton.transform.Translate(0f, -200f, 0f);
		yield return new WaitForSeconds(0.75f);
		GetMoreCoinsButton.GetComponentInChildren<UISprite>().enabled = true;
		TweenPosition.Begin(GetMoreCoinsButton, 0.25f, mm);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
		yield return new WaitForSeconds(0.25f);
		BackButton.GetComponentInChildren<UISprite>().enabled = true;
		TweenPosition.Begin(BackButton, 0.25f, ra);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
	}

	private void AdjustCoins()
	{
		int coinCount = RecordManager.GetCoinCount(PlayerManager.GetActivePlayer());
		CoinLabel.text = coinCount.ToString("##,0");
	}

	private void BackClicked()
	{
		ReturnMenu();
	}

	private void GetMoreCoinsClicked()
	{
		HideAll();
		CoinsGUI.SlideIn(this);
	}
}
