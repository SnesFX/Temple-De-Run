using UnityEngine;

public class HeadStartButton : MonoBehaviour
{
	public HeadStartButton OtherButton;

	public bool MegaHeadStart;

	public CharacterPlayer GamePlayer;

	public GameController Controller;

	public RecordManager RecordManager;

	public PlayerManager PlayerManager;

	private RecordManager.StoreItemType Type;

	private UISprite Sprite;

	private bool TweenStarted;

	private float TimeTillDisable;

	private int CurrentLevel;

	private void Update()
	{
		if (!TweenStarted)
		{
			if (Controller.IsIntroScene)
			{
				return;
			}
			TweenStarted = true;
			GetComponentInChildren<TweenColor>().Reset();
			GetComponentInChildren<TweenColor>().Play(true);
			TimeTillDisable = 5f;
		}
		TimeTillDisable -= Time.smoothDeltaTime;
		if (TimeTillDisable <= 0f)
		{
			base.gameObject.SetActiveRecursively(false);
		}
	}

	private void OnClick()
	{
		if (TweenStarted)
		{
			if (MegaHeadStart)
			{
				Controller.UseHeadStartMega();
			}
			else
			{
				Controller.UseHeadStart();
			}
			base.gameObject.SetActiveRecursively(false);
			OtherButton.gameObject.SetActiveRecursively(false);
		}
	}

	private void OnEnable()
	{
		Type = ((!MegaHeadStart) ? RecordManager.StoreItemType.kStoreItemHeadStart : RecordManager.StoreItemType.kStoreItemHeadStartMega);
		Sprite = GetComponentInChildren<UISprite>();
		Sprite.color = Color.white;
		GetComponentInChildren<TweenColor>().enabled = false;
		int playerLevelForUpgradeType = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), RecordManager.StoreItemType.kStoreItemHeadStart);
		int playerLevelForUpgradeType2 = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), RecordManager.StoreItemType.kStoreItemHeadStartMega);
		CurrentLevel = ((Type != RecordManager.StoreItemType.kStoreItemHeadStartMega) ? playerLevelForUpgradeType : playerLevelForUpgradeType2);
		if (CurrentLevel == 0)
		{
			base.gameObject.SetActiveRecursively(false);
			return;
		}
		base.gameObject.SetActiveRecursively(true);
		int num = ((playerLevelForUpgradeType > 0 && playerLevelForUpgradeType2 > 0) ? 127 : 0);
		base.transform.localPosition = new Vector3((Type != RecordManager.StoreItemType.kStoreItemHeadStart) ? num : (-num), 0f, 0f);
		Sprite.color = new Color(0f, 0f, 0f, 0f);
		TweenStarted = false;
	}
}
