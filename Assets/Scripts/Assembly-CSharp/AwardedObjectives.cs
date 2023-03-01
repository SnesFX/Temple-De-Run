using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardedObjectives : BasePanel
{
	public GameObject SlideInOffset;

	public GameObject NextButton;

	public RecordManager RecordManager;

	public PlayerManager PlayerManager;

	public CharacterPlayer GamePlayer;

	public UIStack Stack;

	public GameObject ItemPrefab;

	public UILabel MultiplierLabel;

	public Transform NextObjectivePlate;

	public EndGameUI EndGameInterface;

	public Transform MultUp;

	public UISprite MultUpSprite;

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
		if (Controller.AwardedAchievements.Count == 0)
		{
			NextClicked();
			return;
		}
		ShowAll();
		StartCoroutine(SlideInAnimation());
	}

	private IEnumerator SlideInAnimation()
	{
		int startSM = RecordManager.GetScoreMulitplier(PlayerManager.GetActivePlayer());
		MultUpSprite.color = new Color(0f, 0f, 0f, 0f);
		UITweener[] tweens = MultUp.GetComponentsInChildren<UITweener>();
		UITweener[] array = tweens;
		foreach (UITweener t in array)
		{
			t.enabled = false;
		}
		ShowAll();
		FillObjectives();
		MultiplierLabel.text = startSM + "x";
		SlideInOffset.transform.localPosition = new Vector3(0f, 1125f, -40f);
		TweenPosition.Begin(SlideInOffset, 0.5f, new Vector3(0f, 0f, -40f)).method = UITweener.Method.EaseIn;
		Vector3 mm = NextButton.transform.localPosition;
		NextButton.GetComponentInChildren<UISprite>().enabled = false;
		NextButton.transform.Translate(0f, -200f, 0f);
		yield return new WaitForSeconds(0.75f);
		NextButton.GetComponentInChildren<UISprite>().enabled = true;
		TweenPosition.Begin(NextButton, 0.25f, mm);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
		int newMultiplier = startSM + Controller.AwardedAchievements.Count;
		RecordManager.SetScoreMultiplier(PlayerManager.GetActivePlayer(), newMultiplier);
		RecordManager.SaveRecords();
		GamePlayer.ScoreMultiplier = newMultiplier;
		yield return new WaitForSeconds(1f);
		for (int i = startSM + 1; i <= newMultiplier; i++)
		{
			UITweener[] array2 = tweens;
			foreach (UITweener t2 in array2)
			{
				t2.enabled = true;
			}
			MultUp.BroadcastMessage("Reset");
			MultUp.BroadcastMessage("Play", true);
			yield return new WaitForSeconds(0.5f);
			AudioManager.Instance.PlayFX(AudioManager.Effects.bonusMeterFull, 0.5f);
			MultiplierLabel.text = i + "x";
		}
	}

	private void FillObjectives()
	{
		int activePlayer = PlayerManager.GetActivePlayer();
		List<Transform> list = new List<Transform>();
		foreach (Transform item3 in Stack.transform)
		{
			list.Add(item3);
		}
		foreach (Transform item4 in NextObjectivePlate.transform)
		{
			list.Add(item4);
		}
		NextObjectivePlate.DetachChildren();
		Stack.transform.DetachChildren();
		foreach (Transform item5 in list)
		{
			Object.Destroy(item5.gameObject);
		}
		int num = 100;
		foreach (string awardedAchievement in Controller.AwardedAchievements)
		{
			RecordManager.cAchievement cAchievement = RecordManager.FindAchievement(awardedAchievement);
			if (cAchievement == null)
			{
				Debug.LogError("RecordManager.FindAchievement(" + awardedAchievement + ") failed.");
				continue;
			}
			GameObject gameObject = (GameObject)Object.Instantiate(ItemPrefab);
			ObjectiveItem component = gameObject.GetComponent<ObjectiveItem>();
			gameObject.transform.parent = Stack.transform;
			gameObject.name = num + ". " + cAchievement.name;
			component.Setup(cAchievement, true);
			num++;
		}
		Stack.Reposition();
		RecordManager.cAchievement nextAchievement = RecordManager.GetNextAchievement(PlayerManager.GetActivePlayer());
		if (nextAchievement != null)
		{
			RecordManager.cAchievement achievement = RecordManager.FindAchievement(nextAchievement.achievementId);
			GameObject gameObject2 = (GameObject)Object.Instantiate(ItemPrefab);
			ObjectiveItem component2 = gameObject2.GetComponent<ObjectiveItem>();
			gameObject2.transform.parent = NextObjectivePlate;
			gameObject2.transform.localPosition = Vector3.zero;
			component2.Setup(achievement, false, false);
		}
	}

	private void NextClicked()
	{
		HideAll();
		EndGameInterface.SlideIn();
	}
}
