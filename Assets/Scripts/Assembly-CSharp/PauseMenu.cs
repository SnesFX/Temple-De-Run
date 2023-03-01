using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : BasePanel
{
	public GameObject SlideInOffset;

	public GameObject MainMenuButton;

	public GameObject ResumeButton;

	public GameObject NewGameButton;

	public UILabel ScoreLabel;

	public UILabel DistanceLabel;

	public UILabel CoinsLabel;

	public UILabel MultiplierLabel;

	public AlertView AlertView;

	public Transform NextObjectivePlate;

	public GameObject ObjectiveItemPrefab;

	public RecordManager RecordManager;

	public PlayerManager PlayerManager;

	public static PauseMenu Instance;

	private void Awake()
	{
		Instance = this;
		Setup();
	}

	private void Setup()
	{
		HideAll();
	}

	public override void SlideIn(BasePanel returnTo)
	{
		Controller.Pause();
		base.SlideIn(returnTo);
		ShowAll();
		StartCoroutine(SlideInAnimation());
	}

	private IEnumerator SlideInAnimation()
	{
		AudioManager.Instance.StopMusic();
		ShowAll();
		UpdateStats();
		SlideInOffset.transform.localPosition = new Vector3(0f, 1125f, -40f);
		TweenPosition.Begin(SlideInOffset, 0.5f, new Vector3(0f, 0f, -40f)).method = UITweener.Method.EaseIn;
		Vector3 mm = MainMenuButton.transform.localPosition;
		Vector3 rb = ResumeButton.transform.localPosition;
		Vector3 ng = NewGameButton.transform.localPosition;
		ResumeButton.GetComponentInChildren<UISprite>().enabled = false;
		ResumeButton.transform.Translate(0f, -200f, 0f);
		NewGameButton.GetComponentInChildren<UISprite>().enabled = false;
		NewGameButton.transform.Translate(0f, -200f, 0f);
		MainMenuButton.GetComponentInChildren<UISprite>().enabled = false;
		MainMenuButton.transform.Translate(0f, -200f, 0f);
		yield return new WaitForSeconds(0.75f);
		ResumeButton.GetComponentInChildren<UISprite>().enabled = true;
		TweenPosition.Begin(ResumeButton, 0.25f, rb);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
		yield return new WaitForSeconds(0.25f);
		NewGameButton.GetComponentInChildren<UISprite>().enabled = true;
		TweenPosition.Begin(NewGameButton, 0.25f, ng);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
		yield return new WaitForSeconds(0.25f);
		MainMenuButton.GetComponentInChildren<UISprite>().enabled = true;
		TweenPosition.Begin(MainMenuButton, 0.25f, mm);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
	}

	private void UpdateStats()
	{
		ScoreLabel.text = Controller.GamePlayer.Score.ToString("##,0");
		DistanceLabel.text = Controller.DistanceTraveled.ToString("##,0m");
		CoinsLabel.text = Controller.GamePlayer.CoinCountTotal.ToString("##,0");
		MultiplierLabel.text = Controller.GamePlayer.ScoreMultiplier.ToString("##x");
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in NextObjectivePlate)
		{
			list.Add(item.gameObject);
		}
		list.ForEach(delegate(GameObject child)
		{
			Object.Destroy(child);
		});
		RecordManager.cAchievement nextAchievement = RecordManager.GetNextAchievement(PlayerManager.GetActivePlayer());
		if (nextAchievement != null)
		{
			RecordManager.cAchievement achievement = RecordManager.FindAchievement(nextAchievement.achievementId);
			GameObject gameObject = (GameObject)Object.Instantiate(ObjectiveItemPrefab);
			ObjectiveItem component = gameObject.GetComponent<ObjectiveItem>();
			gameObject.transform.parent = NextObjectivePlate;
			gameObject.transform.localPosition = Vector3.zero;
			component.Setup(achievement, false, false);
		}
	}

	private void MainMenuClicked()
	{
		AlertView.ShowAlert(Strings.Txt("ExitGameTitle"), Strings.Txt("ExitGameDescription"), Strings.Txt("ExitGameCancelButton"), Strings.Txt("ExitGameActionButton"), delegate
		{
		}, delegate
		{
			HideAll();
			Controller.ResetTutorial();
			Controller.SnapMainMenu();
		});
	}

	private void ResumeClicked()
	{
		AudioManager.Instance.StartGameMusic();
		HideAll();
		Controller.Unpause();
	}

	private void NewGameClicked()
	{
		AlertView.ShowAlert(Strings.Txt("NewGameTitle"), Strings.Txt("NewGameDescription"), Strings.Txt("NewGameCancelButton"), Strings.Txt("NewGameActionButton"), delegate
		{
		}, delegate
		{
			HideAll();
			Controller.ResetTutorial();
			Controller.RestartGame();
			Controller.GameStart();
		});
	}
}
