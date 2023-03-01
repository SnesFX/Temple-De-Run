using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesGUI : BasePanel
{
	public GameObject SlideInOffset;

	public GameObject MainMenuButton;

	public RecordManager RecordManager;

	public PlayerManager PlayerManager;

	public UIStack Stack;

	public UIPanel ScrollingPanel;

	public GameObject ItemPrefab;

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
		ShowAll();
		StartCoroutine(SlideInAnimation());
	}

	private IEnumerator SlideInAnimation()
	{
		ShowAll();
		FillObjectives();
		SlideInOffset.transform.localPosition = new Vector3(0f, 1125f, -40f);
		TweenPosition.Begin(SlideInOffset, 0.5f, new Vector3(0f, 0f, -40f)).method = UITweener.Method.EaseIn;
		Vector3 mm = MainMenuButton.transform.localPosition;
		MainMenuButton.GetComponentInChildren<UISprite>().enabled = false;
		MainMenuButton.transform.Translate(0f, -200f, 0f);
		yield return new WaitForSeconds(0.25f);
		MainMenuButton.GetComponentInChildren<UISprite>().enabled = true;
		TweenPosition.Begin(MainMenuButton, 0.25f, mm);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
	}

	private void FillObjectives()
	{
		int activePlayer = PlayerManager.GetActivePlayer();
		List<RecordManager.cAchievement> tRAchievements = RecordManager.TRAchievements;
		List<Transform> list = new List<Transform>();
		foreach (Transform item in Stack.transform)
		{
			list.Add(item);
		}
		Stack.transform.DetachChildren();
		foreach (Transform item2 in list)
		{
			Object.Destroy(item2.gameObject);
		}
		int num = 100;
		foreach (RecordManager.cAchievement item3 in tRAchievements)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(ItemPrefab);
			ObjectiveItem component = gameObject.GetComponent<ObjectiveItem>();
			gameObject.transform.parent = Stack.transform;
			gameObject.name = num + ". " + item3.name;
			component.Setup(item3, RecordManager.GetProgressForAchievement(activePlayer, item3.achievementId) >= 100f);
			component.DragPanel.panel = ScrollingPanel;
			num++;
		}
		Stack.Reposition();
	}

	private void MainMenuClicked()
	{
		ReturnMenu();
	}
}
