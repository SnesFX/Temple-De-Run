using UnityEngine;

[ExecuteInEditMode]
public class StoreItem : MonoBehaviour
{
	public enum StoreItemClass
	{
		None = 0,
		PowerUp = 1,
		Utility = 2,
		Character = 3,
		Wallpaper = 4
	}

	private const int kStoreItemLevelMax = 5;

	public RecordManager.StoreItemType Type;

	public StoreItemClass Class;

	public int Price;

	public Transform ProgressBar;

	public UILabel CostLabel;

	public UILabel ButtonLabel;

	public Transform Counter;

	public UILabel CounterLabel;

	public UISprite UpgradeCoin;

	public UILabel DescriptionLabel;

	public UISprite Icon;

	public UISprite Bar1;

	public UISprite Bar2;

	public UISprite Bar3;

	public UISprite Bar4;

	public UISprite Bar5;

	public AlertView AlertView;

	public PlayerManager PlayerManager;

	public RecordManager RecordManager;

	private int LastPrice = -1;

	private StoreItemClass LastClass;

	private RecordManager.StoreItemType LastType = RecordManager.StoreItemType.kStoreItemCount;

	private static string[] IconNameByType = new string[28]
	{
		"storeItemAngelWings", "storeItemHeadStart", "storeItemHeadStartMega", "storeItemAngelWings", "storeItemAngelWings", "storeItemVacuum", "storeItemInvincibility", "storeItemCoinBonus", "storeItemCoinValue", "storeItemBoost",
		"PU6", "PU7", "PU8", "PU9", "PU10", "storeItemGuy", "storeItemGirl", "storeItemBigB", "storeItemChina", "storeItemIndi",
		"storeItemConq", "storeItemNoAds", "storeItemFootball", "storeItemWallpaper", "storeItemWallpaper", "storeItemWallpaper", "storeItemWallpaper", "storeItemWallpaper"
	};

	private static StoreItemClass[] ClassByType = new StoreItemClass[28]
	{
		StoreItemClass.Utility,
		StoreItemClass.Utility,
		StoreItemClass.Utility,
		StoreItemClass.Utility,
		StoreItemClass.Utility,
		StoreItemClass.PowerUp,
		StoreItemClass.PowerUp,
		StoreItemClass.PowerUp,
		StoreItemClass.PowerUp,
		StoreItemClass.PowerUp,
		StoreItemClass.PowerUp,
		StoreItemClass.PowerUp,
		StoreItemClass.PowerUp,
		StoreItemClass.PowerUp,
		StoreItemClass.PowerUp,
		StoreItemClass.Character,
		StoreItemClass.Character,
		StoreItemClass.Character,
		StoreItemClass.Character,
		StoreItemClass.Character,
		StoreItemClass.Character,
		StoreItemClass.None,
		StoreItemClass.Character,
		StoreItemClass.Wallpaper,
		StoreItemClass.Wallpaper,
		StoreItemClass.Wallpaper,
		StoreItemClass.Wallpaper,
		StoreItemClass.Wallpaper
	};

	private void Start()
	{
		Configure();
	}

	private void Update()
	{
		if (LastClass != Class || LastType != Type || LastPrice != Price)
		{
			Configure();
		}
	}

	private void SetVisible(GameObject go, bool visible)
	{
		go.SetActiveRecursively(visible);
	}

	public void Reconfigure()
	{
		LastClass = StoreItemClass.None;
		LastType = RecordManager.StoreItemType.kStoreItemCount;
		LastPrice = -1;
		Configure();
	}

	public void Configure()
	{
		UpdateClass();
		if (Class != LastClass)
		{
			DescriptionLabel.lineWidth = 320;
			switch (Class)
			{
			case StoreItemClass.PowerUp:
				SetVisible(ProgressBar.gameObject, true);
				SetVisible(Counter.gameObject, false);
				break;
			case StoreItemClass.Utility:
				SetVisible(ProgressBar.gameObject, false);
				SetVisible(Counter.gameObject, true);
				DescriptionLabel.lineWidth = 280;
				break;
			case StoreItemClass.Character:
				SetVisible(ProgressBar.gameObject, false);
				SetVisible(Counter.gameObject, false);
				break;
			case StoreItemClass.Wallpaper:
				SetVisible(ProgressBar.gameObject, false);
				SetVisible(Counter.gameObject, false);
				break;
			}
		}
		if (Type != LastType)
		{
			SetIcon();
		}
		UpdatePrice();
		if (Price != LastPrice)
		{
			CostLabel.text = Price.ToString();
		}
		UpdateDescription();
		UpdateBar();
		LastClass = Class;
		LastType = Type;
		LastPrice = Price;
	}

	private void SetIcon()
	{
		if (Type != RecordManager.StoreItemType.kStoreItemCount)
		{
			string spriteName = IconNameByType[(int)Type] + "@2x.png";
			Icon.spriteName = spriteName;
			Icon.MakePixelPerfect();
		}
	}

	private void UpdateClass()
	{
		Class = ClassByType[(int)Type];
	}

	private void UpdatePrice()
	{
		int playerLevelForUpgradeType = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), Type);
		if (Class == StoreItemClass.Utility)
		{
			ButtonLabel.text = Strings.Txt("StoreItemSingleUse");
		}
		else if (Class == StoreItemClass.Character || Class == StoreItemClass.Wallpaper)
		{
			ButtonLabel.text = Strings.Txt("StoreItemUnlock");
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemDisableAds)
		{
			ButtonLabel.text = Strings.Txt("StoreItemDisableAds");
		}
		else
		{
			ButtonLabel.text = Strings.Txt("StoreItemUpgrade");
		}
		if ((Class == StoreItemClass.PowerUp && playerLevelForUpgradeType < 5) || Class == StoreItemClass.Utility || (Class == StoreItemClass.Character && playerLevelForUpgradeType < 1) || (Class == StoreItemClass.Wallpaper && playerLevelForUpgradeType < 1) || (Type == RecordManager.StoreItemType.kStoreItemDisableAds && RecordManager.GetRecordFlagValue(PlayerManager.GetActivePlayer(), RecordManager.RecordFlagType.kRecordFlagFullScreenAdEnabled) == 1))
		{
			RecordManager.StoreItemType type = Type;
			int priceForUpgradeType = RecordManager.GetPriceForUpgradeType(type, playerLevelForUpgradeType);
			Price = priceForUpgradeType;
			CostLabel.enabled = true;
			UpgradeCoin.enabled = true;
			if (Class == StoreItemClass.Wallpaper)
			{
				CostLabel.enabled = false;
				UpgradeCoin.enabled = false;
				ButtonLabel.text = "Coming Soon!";
			}
			return;
		}
		CostLabel.enabled = false;
		if (Class == StoreItemClass.Character)
		{
			bool flag = false;
			int activeCharacter = RecordManager.GetActiveCharacter(PlayerManager.GetActivePlayer());
			if ((activeCharacter == 0 && Type == RecordManager.StoreItemType.kStoreItemPlayerGuy) || (activeCharacter == 1 && Type == RecordManager.StoreItemType.kStoreItemPlayerGirl) || (activeCharacter == 2 && Type == RecordManager.StoreItemType.kStoreItemPlayerBigB) || (activeCharacter == 3 && Type == RecordManager.StoreItemType.kStoreItemPlayerChina) || (activeCharacter == 4 && Type == RecordManager.StoreItemType.kStoreItemPlayerIndi) || (activeCharacter == 5 && Type == RecordManager.StoreItemType.kStoreItemPlayerConquistador) || (activeCharacter == 6 && Type == RecordManager.StoreItemType.kStoreItemPlayerFootball))
			{
				flag = true;
			}
			if (flag)
			{
				ButtonLabel.text = Strings.Txt("StoreItemCurrentlyActive");
			}
			else
			{
				ButtonLabel.text = Strings.Txt("StoreItemActivate");
			}
		}
		else if (Class == StoreItemClass.Wallpaper)
		{
			ButtonLabel.text = Strings.Txt("StoreItemDownload");
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemDisableAds)
		{
			ButtonLabel.text = Strings.Txt("StoreItemAdsDisabled");
		}
		else
		{
			ButtonLabel.text = Strings.Txt("StoreItemFullyUpgraded");
		}
		UpgradeCoin.enabled = false;
	}

	private void UpdateDescription()
	{
		int playerLevelForUpgradeType = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), Type);
		string text = Strings.Txt("StoreItemDescriptionUnknown");
		if (Type == RecordManager.StoreItemType.kStoreItemCoinBonus)
		{
			switch (playerLevelForUpgradeType)
			{
			case 0:
				text = Strings.Txt("StoreItemCoinBonusLevel0");
				break;
			case 1:
				text = Strings.Txt("StoreItemCoinBonusLevel1");
				break;
			case 2:
				text = Strings.Txt("StoreItemCoinBonusLevel2");
				break;
			case 3:
				text = Strings.Txt("StoreItemCoinBonusLevel3");
				break;
			case 4:
				text = Strings.Txt("StoreItemCoinBonusLevel4");
				break;
			case 5:
				text = Strings.Txt("StoreItemCoinBonusLevel5");
				break;
			}
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemVacuum)
		{
			switch (playerLevelForUpgradeType)
			{
			case 0:
				text = Strings.Txt("StoreItemMagnetLevel0");
				break;
			case 1:
				text = Strings.Txt("StoreItemMagnetLevel1");
				break;
			case 2:
				text = Strings.Txt("StoreItemMagnetLevel2");
				break;
			case 3:
				text = Strings.Txt("StoreItemMagnetLevel3");
				break;
			case 4:
				text = Strings.Txt("StoreItemMagnetLevel4");
				break;
			case 5:
				text = Strings.Txt("StoreItemMagnetLevel5");
				break;
			}
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemInvincibility)
		{
			switch (playerLevelForUpgradeType)
			{
			case 0:
				text = Strings.Txt("StoreItemInvincibilityLevel0");
				break;
			case 1:
				text = Strings.Txt("StoreItemInvincibilityLevel1");
				break;
			case 2:
				text = Strings.Txt("StoreItemInvincibilityLevel2");
				break;
			case 3:
				text = Strings.Txt("StoreItemInvincibilityLevel3");
				break;
			case 4:
				text = Strings.Txt("StoreItemInvincibilityLevel4");
				break;
			case 5:
				text = Strings.Txt("StoreItemInvincibilityLevel5");
				break;
			}
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemCoinValue)
		{
			switch (playerLevelForUpgradeType)
			{
			case 0:
				text = Strings.Txt("StoreItemCoinValueLevel0");
				break;
			case 1:
				text = Strings.Txt("StoreItemCoinValueLevel1");
				break;
			case 2:
				text = Strings.Txt("StoreItemCoinValueLevel2");
				break;
			case 3:
				text = Strings.Txt("StoreItemCoinValueLevel3");
				break;
			case 4:
				text = Strings.Txt("StoreItemCoinValueLevel4");
				break;
			case 5:
				text = Strings.Txt("StoreItemCoinValueLevel5");
				break;
			}
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemBoost)
		{
			switch (playerLevelForUpgradeType)
			{
			case 0:
				text = Strings.Txt("StoreItemBoostLevel0");
				break;
			case 1:
				text = Strings.Txt("StoreItemBoostLevel1");
				break;
			case 2:
				text = Strings.Txt("StoreItemBoostLevel2");
				break;
			case 3:
				text = Strings.Txt("StoreItemBoostLevel3");
				break;
			case 4:
				text = Strings.Txt("StoreItemBoostLevel4");
				break;
			case 5:
				text = Strings.Txt("StoreItemBoostLevel5");
				break;
			}
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemAngelWings)
		{
			text = Strings.Txt("StoreItemAngelWingsDescription");
			CounterLabel.text = playerLevelForUpgradeType.ToString();
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemHeadStart)
		{
			text = Strings.Txt("StoreItemHeadStartDescription");
			CounterLabel.text = playerLevelForUpgradeType.ToString();
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemHeadStartMega)
		{
			text = Strings.Txt("StoreItemHeadStartMegaDescription");
			CounterLabel.text = playerLevelForUpgradeType.ToString();
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemPlayerGuy)
		{
			text = ((playerLevelForUpgradeType != 0) ? Strings.Txt("StoreItemPlayerGuyLevel1") : Strings.Txt("StoreItemPlayerGuyLevel0"));
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemPlayerGirl)
		{
			text = ((playerLevelForUpgradeType != 0) ? Strings.Txt("StoreItemPlayerGirlLevel1") : Strings.Txt("StoreItemPlayerGirlLevel0"));
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemPlayerBigB)
		{
			text = ((playerLevelForUpgradeType != 0) ? Strings.Txt("StoreItemPlayerBigBLevel1") : Strings.Txt("StoreItemPlayerBigBLevel0"));
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemPlayerChina)
		{
			text = ((playerLevelForUpgradeType != 0) ? Strings.Txt("StoreItemPlayerChinaLevel1") : Strings.Txt("StoreItemPlayerChinaLevel0"));
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemPlayerIndi)
		{
			text = ((playerLevelForUpgradeType != 0) ? Strings.Txt("StoreItemPlayerIndiLevel1") : Strings.Txt("StoreItemPlayerIndiLevel0"));
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemPlayerConquistador)
		{
			text = ((playerLevelForUpgradeType != 0) ? Strings.Txt("StoreItemPlayerConquistadorLevel1") : Strings.Txt("StoreItemPlayerConquistadorLevel0"));
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemPlayerFootball)
		{
			text = ((playerLevelForUpgradeType != 0) ? Strings.Txt("StoreItemPlayerFootballLevel1") : Strings.Txt("StoreItemPlayerFootballLevel0"));
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemWallpaperA)
		{
			text = ((playerLevelForUpgradeType != 0) ? Strings.Txt("StoreItemWallpaperALevel1") : Strings.Txt("StoreItemWallpaperALevel0"));
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemWallpaperB)
		{
			text = ((playerLevelForUpgradeType != 0) ? Strings.Txt("StoreItemWallpaperBLevel1") : Strings.Txt("StoreItemWallpaperBLevel0"));
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemWallpaperC)
		{
			text = ((playerLevelForUpgradeType != 0) ? Strings.Txt("StoreItemWallpaperCLevel1") : Strings.Txt("StoreItemWallpaperCLevel0"));
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemWallpaperD)
		{
			text = ((playerLevelForUpgradeType != 0) ? Strings.Txt("StoreItemWallpaperDLevel1") : Strings.Txt("StoreItemWallpaperDLevel0"));
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemWallpaperE)
		{
			text = ((playerLevelForUpgradeType != 0) ? Strings.Txt("StoreItemWallpaperELevel1") : Strings.Txt("StoreItemWallpaperELevel0"));
		}
		else if (Type == RecordManager.StoreItemType.kStoreItemDisableAds)
		{
			text = ((RecordManager.GetRecordFlagValue(PlayerManager.GetActivePlayer(), RecordManager.RecordFlagType.kRecordFlagFullScreenAdEnabled) != 1) ? Strings.Txt("StoreItemDisableAdsLevel1") : Strings.Txt("StoreItemDisableAdsLevel0"));
		}
		DescriptionLabel.text = text;
	}

	private void UpdateBar()
	{
		if (Class == StoreItemClass.PowerUp)
		{
			int playerLevelForUpgradeType = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), Type);
			Bar1.enabled = playerLevelForUpgradeType > 0;
			Bar2.enabled = playerLevelForUpgradeType > 1;
			Bar3.enabled = playerLevelForUpgradeType > 2;
			Bar4.enabled = playerLevelForUpgradeType > 3;
			Bar5.enabled = playerLevelForUpgradeType > 4;
		}
	}

	private void OnClick()
	{
		if (Class == StoreItemClass.Wallpaper)
		{
			AlertView.ShowAlert("Wall Paper", "Our artists are busy on great wall paper.  Coming soon!", Strings.Txt("Ok"));
			return;
		}
		int playerLevelForUpgradeType = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), Type);
		if ((Class == StoreItemClass.PowerUp && playerLevelForUpgradeType < 5) || Class == StoreItemClass.Utility || Class == StoreItemClass.Character || Class == StoreItemClass.Wallpaper || (Type == RecordManager.StoreItemType.kStoreItemDisableAds && RecordManager.GetRecordFlagValue(PlayerManager.GetActivePlayer(), RecordManager.RecordFlagType.kRecordFlagFullScreenAdEnabled) == 1))
		{
			if (Class == StoreItemClass.Character && playerLevelForUpgradeType >= 1)
			{
				ActivateCharacter(Type);
			}
			else if (Class != StoreItemClass.Wallpaper || playerLevelForUpgradeType < 1)
			{
				BuyItem();
			}
		}
	}

	private void ActivateCharacter(RecordManager.StoreItemType type)
	{
		CharacterPlayer.Instance.ActiveCharacterId = RecordManager.Instance.CharcterIDFromType(type);
		RecordManager.SetActiveCharacter(PlayerManager.GetActivePlayer(), CharacterPlayer.Instance.ActiveCharacterId);
		base.transform.parent.BroadcastMessage("Reconfigure");
		RecordManager.SaveRecords();
	}

	private void BuyItem()
	{
		int coinCount = RecordManager.GetCoinCount(PlayerManager.GetActivePlayer());
		if (Price <= coinCount)
		{
			coinCount -= Price;
			RecordManager.SetCoinCount(PlayerManager.GetActivePlayer(), coinCount);
			if (Type == RecordManager.StoreItemType.kStoreItemDisableAds)
			{
			}
			int playerLevelForUpgradeType = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), Type);
			RecordManager.SetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), Type, playerLevelForUpgradeType + 1);
			SendMessageUpwards("AdjustCoins");
			RecordManager.SaveRecords();
			Reconfigure();
			AudioManager.Instance.PlayFX(AudioManager.Effects.cashRegister);
			if (Type == RecordManager.StoreItemType.kStoreItemAngelWings && playerLevelForUpgradeType == 0)
			{
				AlertView.ShowAlert(Strings.Txt("StoreItemResurrectionWingsPopupTitle"), Strings.Txt("StoreItemResurrectionWingsPopupText"), Strings.Txt("Ok"));
			}
			if (Type == RecordManager.StoreItemType.kStoreItemHeadStart && playerLevelForUpgradeType == 0)
			{
				AlertView.ShowAlert(Strings.Txt("StoreItemHeadStartPopupTitle"), Strings.Txt("StoreItemHeadStartPopupText"), Strings.Txt("Ok"));
			}
			if (Type == RecordManager.StoreItemType.kStoreItemHeadStartMega && playerLevelForUpgradeType == 0)
			{
				AlertView.ShowAlert(Strings.Txt("StoreItemHeadStartMegaPopupTitle"), Strings.Txt("StoreItemHeadStartMegaPopupText"), Strings.Txt("Ok"));
			}
			if (Class == StoreItemClass.Character)
			{
				ActivateCharacter(Type);
			}
		}
		else
		{
			AlertView.ShowAlert(Strings.Txt("StoreItemNotEnoughCoinsTitle"), Strings.Txt("StoreItemNotEnoughCoinsText"), Strings.Txt("No"), Strings.Txt("Yes"), null, delegate
			{
				StoreGUI.Instance.HideAll();
				VCGUI.Instance.SlideIn(StoreGUI.Instance);
			});
		}
	}
}
