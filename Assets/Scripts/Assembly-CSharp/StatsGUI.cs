using System.Collections;
using UnityEngine;

public class StatsGUI : BasePanel
{
	public GameObject SlideInOffset;

	public GameObject MainMenuButton;

	public RecordManager RecordManager;

	public PlayerManager PlayerManager;

	public UILabel Multiplier;

	public UILabel Coins;

	public UILabel HighestScore;

	public UILabel LongestRun;

	public UILabel MostCoins;

	public UILabel TotalCoins;

	public UILabel TotalDistance;

	public UILabel TotalGames;

	private void Awake()
	{
		Setup();
	}

	private void Setup()
	{
		HideAll();
	}

	public override void SlideIn(BasePanel returnTo)
	{
		base.SlideIn(returnTo);
		RecordManager.cPlayerRecord cPlayerRecord = RecordManager.FindOrCreatePlayerRecord(PlayerManager.GetActivePlayer());
		Multiplier.text = cPlayerRecord.scoreMultiplier.ToString("##,0x");
		Coins.text = cPlayerRecord.coinCount.ToString("##,0");
		HighestScore.text = cPlayerRecord.bestScore.ToString("##,0");
		LongestRun.text = cPlayerRecord.bestDistanceScore.ToString("##,0m");
		MostCoins.text = cPlayerRecord.bestCoinScore.ToString("##,0");
		TotalCoins.text = cPlayerRecord.lifetimeCoins.ToString("##,0");
		TotalDistance.text = cPlayerRecord.lifetimeDistance.ToString("##,0m");
		TotalGames.text = cPlayerRecord.lifetimePlays.ToString("##,0");
		ShowAll();
		StartCoroutine(SlideInAnimation());
	}

	private IEnumerator SlideInAnimation()
	{
		ShowAll();
		SlideInOffset.transform.localPosition = new Vector3(0f, 1125f, -40f);
		TweenPosition.Begin(SlideInOffset, 0.5f, new Vector3(0f, 0f, -40f)).method = UITweener.Method.EaseIn;
		Vector3 mm = MainMenuButton.transform.localPosition;
		MainMenuButton.GetComponentInChildren<UISprite>().enabled = false;
		MainMenuButton.transform.Translate(0f, -200f, 0f);
		yield return new WaitForSeconds(0.75f);
		MainMenuButton.GetComponentInChildren<UISprite>().enabled = true;
		TweenPosition.Begin(MainMenuButton, 0.25f, mm);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
	}

	private void BackClicked()
	{
		ReturnMenu();
	}
}
