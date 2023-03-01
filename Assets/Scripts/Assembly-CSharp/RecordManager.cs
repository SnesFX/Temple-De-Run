using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
	public enum StoreItemType
	{
		kStoreItemAngelWings = 0,
		kStoreItemHeadStart = 1,
		kStoreItemHeadStartMega = 2,
		kStoreItemConsumable4 = 3,
		kStoreItemConsumable5 = 4,
		kStoreItemVacuum = 5,
		kStoreItemInvincibility = 6,
		kStoreItemCoinBonus = 7,
		kStoreItemCoinValue = 8,
		kStoreItemBoost = 9,
		kStoreItemPowerup6 = 10,
		kStoreItemPowerup7 = 11,
		kStoreItemPowerup8 = 12,
		kStoreItemPowerup9 = 13,
		kStoreItemPowerup10 = 14,
		kStoreItemPlayerGuy = 15,
		kStoreItemPlayerGirl = 16,
		kStoreItemPlayerBigB = 17,
		kStoreItemPlayerChina = 18,
		kStoreItemPlayerIndi = 19,
		kStoreItemPlayerConquistador = 20,
		kStoreItemDisableAds = 21,
		kStoreItemPlayerFootball = 22,
		kStoreItemWallpaperA = 23,
		kStoreItemWallpaperB = 24,
		kStoreItemWallpaperC = 25,
		kStoreItemWallpaperD = 26,
		kStoreItemWallpaperE = 27,
		kStoreItemCount = 28
	}

	public enum RecordFlagType
	{
		kRecordFlagViewedStore = 0,
		kRecordFlagViewedStoreNag = 1,
		kRecordFlagViewedCoinStore = 2,
		kRecordFlagViewedCoinStoreNag = 3,
		kRecordFlagPurchasedCoins = 4,
		kRecordFlagScrolledDownInStore = 5,
		kRecordFlagVideoAdOfferAvailable = 10,
		kRecordFlagVideoAdPercentFlurry = 11,
		kRecordFlagVideoAdReward = 12,
		kRecordFlagVideoAdWatched = 13,
		kRecordFlagVideoAdFlurryAPIDisabled = 14,
		kRecordFlagVideoAdAdColonyAPIDisabled = 15,
		kRecordFlagFullScreenAdOfferAvailable = 20,
		kRecordFlagFullScreenAdPercentFaad = 21,
		kRecordFlagFullScreenAdReward = 22,
		kRecordFlagFullScreenAdEnabled = 23,
		kRecordFlagFullScreenAdDisableCost = 24,
		kRecordFlagFullScreenAdWatched = 25,
		kRecordFlagFullScreenAdFAADAPIDisabled = 26,
		kRecordFlagFullScreenAdFlurryAPIDisabled = 27,
		kRecordFlagFullScreenAdIncentivizedPercentage = 28,
		kRecordFlagFullScreenAdIncentivizedReward = 29,
		kRecordFlagFullScreenAdFrequency = 33,
		kRecordFlagFacebookOfferAvailable = 30,
		kRecordFlagFacebookOfferReward = 31,
		kRecordFlagFacebookOfferUsed = 32,
		kRecordFlagTwitterOfferAvailable = 34,
		kRecordFlagTwitterOfferUsed = 35,
		kRecordFlagCount = 36
	}

	[Serializable]
	public class cPlayerRecord
	{
		public int playerId;

		public int bestScore;

		public int bestCoinScore;

		public int bestDistanceScore;

		public int lifetimePlays;

		public int lifetimeCoins;

		public int lifetimeDistance;

		public int coinCount;

		public int scoreMultiplier;

		public int activePlayerCharacter;

		public int[] storeItemLevel;

		public int[] flags;

		public bool gameCenterNeedsUpdate;

		public cPlayerRecord(int newPlayerId)
		{
			storeItemLevel = new int[28];
			flags = new int[36];
			SetDefaults();
			playerId = newPlayerId;
		}

		public void SetDefaults()
		{
			flags[10] = 1;
			flags[12] = 250;
			flags[11] = 100;
			flags[20] = 1;
			flags[22] = 5000;
			flags[24] = 2500;
			flags[21] = 50;
			flags[30] = 1;
			flags[34] = 1;
			flags[31] = 250;
			flags[28] = 0;
			flags[33] = 100;
			bestScore = (bestCoinScore = (bestDistanceScore = (lifetimePlays = (lifetimeCoins = (lifetimeDistance = (coinCount = 0))))));
			scoreMultiplier = 10;
			activePlayerCharacter = 0;
			gameCenterNeedsUpdate = true;
		}
	}

	[Serializable]
	public class cAchievement
	{
		public string achievementId;

		public string name;

		public string preDescription;

		public string postDescription;
	}

	[Serializable]
	public class cAchievementRecord
	{
		public int playerId;

		public string achievementId;

		public float percentComplete;

		public bool updated;
	}

	[Serializable]
	public class cGameCenterScore
	{
		public string playerId;

		public string playerName;

		public int value;

		public string formattedValue;

		public string category;

		public int rank;

		public bool shown;
	}

	public PlayerManager PlayerManager;

	public string OldSaveDataPath;

	public string SaveDataPath;

	public List<cPlayerRecord> PlayerRecords = new List<cPlayerRecord>();

	public List<cAchievementRecord> AchievementRecords = new List<cAchievementRecord>();

	public List<cAchievement> TRAchievements = new List<cAchievement>();

	public List<cGameCenterScore> FriendScores = new List<cGameCenterScore>();

	public bool IsBestScoreRecord;

	public bool IsBestCoinScoreRecord;

	public bool IsBestDistanceScoreRecord;

	public static RecordManager Instance;

	private void Start()
	{
		Instance = this;
		AddCoins(PlayerManager.GetActivePlayer(), 0);
		IsBestCoinScoreRecord = false;
		IsBestCoinScoreRecord = false;
		IsBestDistanceScoreRecord = false;
		SetupAchievements();
		DeserializeRecordManager();
	}

	private string GetFileName()
	{
		return Application.persistentDataPath + "/" + SaveDataPath;
	}

	private void SerializeRecordManager()
	{
		int num = 1;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, num);
			binaryFormatter.Serialize(memoryStream, PlayerRecords);
			binaryFormatter.Serialize(memoryStream, AchievementRecords);
			binaryFormatter.Serialize(memoryStream, FriendScores);
			Debug.Log("Stream Length: " + memoryStream.Length);
			string fileName = GetFileName();
			Debug.Log("RECORD File: " + fileName);
			Warble warble = new Warble(82572);
			byte[] data = memoryStream.GetBuffer();
			byte[] array = warble.BlabidyJibber(ref data);
			using (FileStream fileStream = File.Create(fileName))
			{
				fileStream.Write(array, 0, array.Length);
				fileStream.Close();
			}
			Debug.Log("Records Saved");
		}
	}

	private void DeseralizeRecord(byte[] originalData)
	{
		using (MemoryStream serializationStream = new MemoryStream(originalData))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			try
			{
				int num = (int)binaryFormatter.Deserialize(serializationStream);
				PlayerRecords = binaryFormatter.Deserialize(serializationStream) as List<cPlayerRecord>;
				AchievementRecords = binaryFormatter.Deserialize(serializationStream) as List<cAchievementRecord>;
				FriendScores = binaryFormatter.Deserialize(serializationStream) as List<cGameCenterScore>;
			}
			catch (Exception ex)
			{
				Debug.Log("Decode Exception: " + ex);
			}
		}
	}

	private void DeserializeRecordManager()
	{
		string fileName = GetFileName();
		if (!File.Exists(fileName))
		{
			return;
		}
		Debug.Log("Reading: " + fileName);
		using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
		{
			byte[] data = new byte[fileStream.Length];
			int num = (int)fileStream.Length;
			int num2 = 0;
			while (num > 0)
			{
				int num3 = fileStream.Read(data, num2, num);
				if (num3 == 0)
				{
					break;
				}
				num2 += num3;
				num -= num3;
			}
			Warble warble = new Warble(82572);
			byte[] array = warble.BlopJabber(ref data);
			if (array == null)
			{
				Debug.Log("DECRYPT FAIL");
			}
			else
			{
				DeseralizeRecord(array);
			}
		}
	}

	public void RemoveAllProgress()
	{
		string fileName = GetFileName();
		if (!File.Exists(fileName))
		{
			File.Delete(fileName);
		}
		Debug.Log("PROGRESS REMOVED");
	}

	private void AddAchievement(string aId, string aName, string aPreDescription, string aPostDescription = null)
	{
		if (aPostDescription == null)
		{
			aPostDescription = aPreDescription;
		}
		cAchievement cAchievement = new cAchievement();
		cAchievement.achievementId = aId;
		cAchievement.name = aName;
		cAchievement.preDescription = aPreDescription;
		cAchievement.postDescription = aPostDescription;
		cAchievement item = cAchievement;
		TRAchievements.Add(item);
	}

	private void SetupAchievements()
	{
		AddAchievement("distance500", "Novice Runner", "Run 500 Meters", "Ran 500 meters");
		AddAchievement("coins100", "Pocket Change", "Collect 100 coins", "Collected 100 coins");
		AddAchievement("score25000", "Adventurer", "Score 25,000 points", "Scored 25,000 points");
		AddAchievement("distance1000", "Sprinter", "Run 1,000 meters", "Ran 1,000 meters");
		AddAchievement("distance500NoCoins", "Miser Run", "500m collecting no coins");
		AddAchievement("coins250", "Piggy Bank", "Collect 250 coins");
		AddAchievement("score50000", "Treasure Hunter", "Score 50,000 points", "Scored 50,000 points");
		AddAchievement("totemFull", "Mega Bonus", "Fill the bonus meter 4x", "Filled the bonus meter 4x");
		AddAchievement("distance2500", "Athlete", "Run 2,500 meters", "Ran 2,500 meters");
		AddAchievement("coins500", "Lump Sum", "Collect 500 coins", "Collectd 500 coins");
		AddAchievement("resurrection", "Resurrection", "Ressurect after dying", "Resurrected after dying");
		AddAchievement("upgrade1", "Basic Powers", "All Level 1 Powerups");
		AddAchievement("score100000", "High Roller", "Score 100,000 points", "Scored 100,000 points");
		AddAchievement("coins750", "Payday", "Collect 750 coins", "Collected 750 coins");
		AddAchievement("headstart", "Head Start", "Use a Head Start", "Used a Head Start");
		AddAchievement("notrip2500", "Steady Feet", "Ran 2,500m without tripping", "Ran 2,500m without tripping");
		AddAchievement("distance1000NoCoins", "Allergic to Gold", "1,000m collecting no coins");
		AddAchievement("distance5000", "5K Runner", "Run 5,000 meters", "Ran 5,000 meters");
		AddAchievement("notrip5000", "No.Trip.Runner", "Run 5,000m without tripping", "Ran 5,000m without tripping");
		AddAchievement("score250000", "1/4 Million Club", "Score 250,000 points", "Scored 250,000 points");
		AddAchievement("resurrection2", "Double Resurrection", "Resurrect wtice in one run", "Resurrected twice in one run");
		AddAchievement("coins1000", "Money Bags", "Collect 1,000 coins", "Collected 1,000 coins");
		AddAchievement("score500000", "1/2 Million Club", "Score 500,000 points", "Scored 500,000 points");
		AddAchievement("upgrade5", "Super Powers", "All Level 5 Powerups");
		AddAchievement("character2", "Dynamic Duo", "Unlock Two Characters", "Unlocked Two Characters");
		AddAchievement("score1000000", "Million Club", "Score 1,000,000 points", "Scored 1,000,000 points");
		AddAchievement("coins2500", "Money Bin", "Collect 2,500 coins", "Collectd 2,500 coins");
		AddAchievement("character4", "Fantastic Four", "Unlock Four Characters", "Unlocked Four Characters");
		AddAchievement("wallpaper3", "Interior Decorator", "Unlock 3 wallpapers", "Unlocked 3 wallpapers");
		AddAchievement("character6", "Sexy Six", "Unlock 6 characters", "Unlocked 6 characters");
		AddAchievement("distance10000", "10k Runner", "Run 10,000 meters", "Ran 10,000 meters");
		AddAchievement("coins5000", "Fort Knox", "Collect 5,000 coins", "Collected 5,000 coins");
		AddAchievement("score2500000", "2.5 Million Club", "Score 2,500,000 points", "Scored 2,500,000 points");
		AddAchievement("score5000000", "5 Million Club", "Score 5,000,000 points", "Scored 5,000,000 points");
		AddAchievement("score1000000nopowers", "The Spartan", "1 million without powerups");
		AddAchievement("score10000000", "10 Million Club", "Score 10,000,000 points", "Scored 10,000,000 points");
	}

	public cAchievement FindAchievement(string achievementId)
	{
		foreach (cAchievement tRAchievement in TRAchievements)
		{
			if (tRAchievement.achievementId == achievementId)
			{
				return tRAchievement;
			}
		}
		return null;
	}

	public cAchievementRecord FindAchievementRecord(int playerId, string achievementId)
	{
		foreach (cAchievementRecord achievementRecord in AchievementRecords)
		{
			if (achievementRecord.playerId == playerId && achievementRecord.achievementId == achievementId)
			{
				return achievementRecord;
			}
		}
		return null;
	}

	public float GetProgressForAchievement(int playerId, string achievementID)
	{
		cAchievementRecord cAchievementRecord = FindAchievementRecord(playerId, achievementID);
		if (cAchievementRecord != null)
		{
			return cAchievementRecord.percentComplete;
		}
		return 0f;
	}

	public void UpdateAchievementRecord(int playerId, string achievementId, float percentComplete)
	{
		bool updated = true;
		cAchievementRecord cAchievementRecord = FindAchievementRecord(playerId, achievementId);
		cPlayerRecord cPlayerRecord = FindOrCreatePlayerRecord(playerId);
		cPlayerRecord.gameCenterNeedsUpdate = true;
		if (cAchievementRecord == null)
		{
			cAchievementRecord = new cAchievementRecord();
			cAchievementRecord.playerId = playerId;
			cAchievementRecord.achievementId = achievementId;
			cAchievementRecord.percentComplete = 0f;
			AchievementRecords.Add(cAchievementRecord);
		}
		if (cAchievementRecord.percentComplete < 100f && cAchievementRecord.percentComplete != percentComplete)
		{
			cAchievementRecord.percentComplete = percentComplete;
		}
		cAchievementRecord.updated = updated;
	}

	public void MarkAchievementAsReported(int playerId, string achievementId)
	{
		cAchievementRecord cAchievementRecord = FindAchievementRecord(playerId, achievementId);
		if (cAchievementRecord != null)
		{
			cAchievementRecord.updated = true;
		}
	}

	public cAchievement GetNextAchievement(int playerId)
	{
		foreach (cAchievement tRAchievement in TRAchievements)
		{
			if (GetProgressForAchievement(playerId, tRAchievement.achievementId) < 100f)
			{
				return tRAchievement;
			}
		}
		return null;
	}

	private cPlayerRecord FindPlayerRecord(int playerID)
	{
		foreach (cPlayerRecord playerRecord in PlayerRecords)
		{
			if (playerRecord.playerId == playerID)
			{
				return playerRecord;
			}
		}
		return null;
	}

	public int GetPlayerLevelForUpgradeType(int playerId, StoreItemType itemType)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		if (cPlayerRecord != null)
		{
			switch (itemType)
			{
			case StoreItemType.kStoreItemPlayerGuy:
				return 1;
			case StoreItemType.kStoreItemAngelWings:
			case StoreItemType.kStoreItemHeadStart:
			case StoreItemType.kStoreItemHeadStartMega:
			case StoreItemType.kStoreItemConsumable4:
			case StoreItemType.kStoreItemConsumable5:
			case StoreItemType.kStoreItemVacuum:
			case StoreItemType.kStoreItemInvincibility:
			case StoreItemType.kStoreItemCoinBonus:
			case StoreItemType.kStoreItemCoinValue:
			case StoreItemType.kStoreItemBoost:
			case StoreItemType.kStoreItemPowerup6:
			case StoreItemType.kStoreItemPowerup7:
			case StoreItemType.kStoreItemPowerup8:
			case StoreItemType.kStoreItemPowerup9:
			case StoreItemType.kStoreItemPowerup10:
			case StoreItemType.kStoreItemPlayerGirl:
			case StoreItemType.kStoreItemPlayerBigB:
			case StoreItemType.kStoreItemPlayerChina:
			case StoreItemType.kStoreItemPlayerIndi:
			case StoreItemType.kStoreItemPlayerConquistador:
			case StoreItemType.kStoreItemDisableAds:
			case StoreItemType.kStoreItemPlayerFootball:
			case StoreItemType.kStoreItemWallpaperA:
			case StoreItemType.kStoreItemWallpaperB:
			case StoreItemType.kStoreItemWallpaperC:
			case StoreItemType.kStoreItemWallpaperD:
			case StoreItemType.kStoreItemWallpaperE:
				return cPlayerRecord.storeItemLevel[(int)itemType];
			}
		}
		return 0;
	}

	public int GetRecordFlagValue(int playerId, RecordFlagType flagType)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		if (cPlayerRecord == null || flagType < RecordFlagType.kRecordFlagViewedStore || flagType >= RecordFlagType.kRecordFlagCount)
		{
			return 0;
		}
		return cPlayerRecord.flags[(int)flagType];
	}

	public void SetRecordFlagValue(int playerId, RecordFlagType flagType, int value)
	{
		cPlayerRecord cPlayerRecord = FindOrCreatePlayerRecord(playerId);
		cPlayerRecord.flags[(int)flagType] = value;
	}

	public bool HasPlayerUpgradeAnything(int playerId)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		if (cPlayerRecord != null)
		{
			for (int i = 0; i < 28; i++)
			{
				if (cPlayerRecord.storeItemLevel[i] > 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void SetPlayerLevelForUpgradeType(int playerId, StoreItemType itemType, int level)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		if (itemType < StoreItemType.kStoreItemAngelWings || itemType >= StoreItemType.kStoreItemCount)
		{
			Debug.LogError("Attempting to set store item that doesn't exist: " + (int)itemType);
		}
		else
		{
			cPlayerRecord.storeItemLevel[(int)itemType] = level;
		}
	}

	public void AdjustPlayerLevelForUpgradeType(int playerId, StoreItemType itemType, int adjustBy)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		if (itemType < StoreItemType.kStoreItemAngelWings || itemType >= StoreItemType.kStoreItemCount)
		{
			Debug.LogError("Attempting to adjust store item that doesn't exist: " + (int)itemType);
		}
		else
		{
			cPlayerRecord.storeItemLevel[(int)itemType] += adjustBy;
		}
	}

	public cPlayerRecord FindOrCreatePlayerRecord(int playerId)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		if (cPlayerRecord == null)
		{
			cPlayerRecord = new cPlayerRecord(playerId);
			PlayerRecords.Add(cPlayerRecord);
		}
		return cPlayerRecord;
	}

	public int GetFullLevel(StoreItemType itemType)
	{
		switch (itemType)
		{
		case StoreItemType.kStoreItemAngelWings:
			return int.MaxValue;
		case StoreItemType.kStoreItemHeadStart:
			return int.MaxValue;
		case StoreItemType.kStoreItemHeadStartMega:
			return int.MaxValue;
		case StoreItemType.kStoreItemConsumable4:
			return int.MaxValue;
		case StoreItemType.kStoreItemConsumable5:
			return int.MaxValue;
		case StoreItemType.kStoreItemVacuum:
		case StoreItemType.kStoreItemInvincibility:
		case StoreItemType.kStoreItemCoinBonus:
		case StoreItemType.kStoreItemCoinValue:
		case StoreItemType.kStoreItemBoost:
		case StoreItemType.kStoreItemPowerup6:
		case StoreItemType.kStoreItemPowerup7:
		case StoreItemType.kStoreItemPowerup8:
		case StoreItemType.kStoreItemPowerup9:
		case StoreItemType.kStoreItemPowerup10:
			return 5;
		case StoreItemType.kStoreItemPlayerGirl:
			return 1;
		case StoreItemType.kStoreItemPlayerBigB:
			return 1;
		case StoreItemType.kStoreItemPlayerChina:
			return 1;
		case StoreItemType.kStoreItemPlayerIndi:
			return 1;
		case StoreItemType.kStoreItemPlayerConquistador:
			return 1;
		case StoreItemType.kStoreItemPlayerFootball:
			return 1;
		case StoreItemType.kStoreItemDisableAds:
			return 1;
		case StoreItemType.kStoreItemWallpaperA:
			return 1;
		case StoreItemType.kStoreItemWallpaperB:
			return 1;
		case StoreItemType.kStoreItemWallpaperC:
			return 1;
		case StoreItemType.kStoreItemWallpaperD:
			return 1;
		case StoreItemType.kStoreItemWallpaperE:
			return 1;
		default:
			return 0;
		}
	}

	public int GetPriceForUpgradeType(StoreItemType itemType, int currentLevel)
	{
		switch (itemType)
		{
		case StoreItemType.kStoreItemAngelWings:
			return 500;
		case StoreItemType.kStoreItemHeadStart:
			return 2500;
		case StoreItemType.kStoreItemHeadStartMega:
			return 10000;
		case StoreItemType.kStoreItemConsumable4:
			return 500;
		case StoreItemType.kStoreItemConsumable5:
			return 500;
		case StoreItemType.kStoreItemVacuum:
		case StoreItemType.kStoreItemInvincibility:
		case StoreItemType.kStoreItemCoinBonus:
		case StoreItemType.kStoreItemCoinValue:
		case StoreItemType.kStoreItemBoost:
		case StoreItemType.kStoreItemPowerup6:
		case StoreItemType.kStoreItemPowerup7:
		case StoreItemType.kStoreItemPowerup8:
		case StoreItemType.kStoreItemPowerup9:
		case StoreItemType.kStoreItemPowerup10:
			switch (currentLevel)
			{
			case 0:
				return 250;
			case 1:
				return 1000;
			case 2:
				return 2500;
			case 3:
				return 5000;
			case 4:
				return 7500;
			}
			break;
		case StoreItemType.kStoreItemPlayerGirl:
			return 10000;
		case StoreItemType.kStoreItemPlayerBigB:
			return 10000;
		case StoreItemType.kStoreItemPlayerChina:
			return 25000;
		case StoreItemType.kStoreItemPlayerIndi:
			return 25000;
		case StoreItemType.kStoreItemPlayerConquistador:
			return 25000;
		case StoreItemType.kStoreItemPlayerFootball:
			return 25000;
		case StoreItemType.kStoreItemDisableAds:
			return GetRecordFlagValue(PlayerManager.GetActivePlayer(), RecordFlagType.kRecordFlagFullScreenAdDisableCost);
		case StoreItemType.kStoreItemWallpaperA:
			return 5000;
		case StoreItemType.kStoreItemWallpaperB:
			return 5000;
		case StoreItemType.kStoreItemWallpaperC:
			return 5000;
		case StoreItemType.kStoreItemWallpaperD:
			return 5000;
		case StoreItemType.kStoreItemWallpaperE:
			return 5000;
		}
		return 0;
	}

	public int GetAffordableUpgradeTypeCount(int playerId)
	{
		int num = 0;
		int coinCount = GetCoinCount(playerId);
		for (int i = 0; i < 28; i++)
		{
			StoreItemType storeItemType = (StoreItemType)i;
			if (storeItemType == StoreItemType.kStoreItemWallpaperA || storeItemType == StoreItemType.kStoreItemWallpaperB || storeItemType == StoreItemType.kStoreItemWallpaperC || storeItemType == StoreItemType.kStoreItemWallpaperD || storeItemType == StoreItemType.kStoreItemWallpaperE || storeItemType == StoreItemType.kStoreItemDisableAds || storeItemType == StoreItemType.kStoreItemPowerup6 || storeItemType == StoreItemType.kStoreItemPowerup7 || storeItemType == StoreItemType.kStoreItemPowerup8 || storeItemType == StoreItemType.kStoreItemPowerup9 || storeItemType == StoreItemType.kStoreItemPowerup10 || storeItemType == StoreItemType.kStoreItemConsumable4 || storeItemType == StoreItemType.kStoreItemConsumable5 || storeItemType == StoreItemType.kStoreItemPlayerGuy)
			{
				continue;
			}
			int playerLevelForUpgradeType = GetPlayerLevelForUpgradeType(playerId, storeItemType);
			int fullLevel = GetFullLevel(storeItemType);
			if (playerLevelForUpgradeType < fullLevel)
			{
				int priceForUpgradeType = GetPriceForUpgradeType(storeItemType, playerLevelForUpgradeType);
				if (priceForUpgradeType <= coinCount)
				{
					num++;
				}
			}
		}
		return num;
	}

	public int GetCoinCount(int playerId)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		return (cPlayerRecord != null) ? cPlayerRecord.coinCount : 0;
	}

	public void SetCoinCount(int playerId, int coins)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		cPlayerRecord.coinCount = coins;
	}

	public void AddCoins(int playerId, int coins)
	{
		cPlayerRecord cPlayerRecord = FindOrCreatePlayerRecord(playerId);
		int coinCount = cPlayerRecord.coinCount;
		int lifetimeCoins = cPlayerRecord.lifetimeCoins;
		cPlayerRecord.coinCount += coins;
		cPlayerRecord.lifetimeCoins = coinCount + coins;
	}

	public void UpdateRecordsForSession(int playerId, int score, int coins, int distance)
	{
		IsBestScoreRecord = false;
		IsBestCoinScoreRecord = false;
		IsBestDistanceScoreRecord = false;
		cPlayerRecord cPlayerRecord = FindOrCreatePlayerRecord(playerId);
		cPlayerRecord.lifetimePlays++;
		cPlayerRecord.lifetimeCoins += coins;
		cPlayerRecord.lifetimeDistance += distance;
		if (score > cPlayerRecord.bestScore)
		{
			cPlayerRecord.bestScore = score;
			IsBestScoreRecord = true;
		}
		if (coins > cPlayerRecord.bestCoinScore)
		{
			cPlayerRecord.bestCoinScore = coins;
			IsBestCoinScoreRecord = true;
		}
		if (distance > cPlayerRecord.bestDistanceScore)
		{
			cPlayerRecord.bestDistanceScore = distance;
			IsBestDistanceScoreRecord = true;
		}
		cPlayerRecord.coinCount += coins;
		cPlayerRecord.gameCenterNeedsUpdate = true;
	}

	public cPlayerRecord GetPlayerRecord(int playerId)
	{
		return FindOrCreatePlayerRecord(playerId);
	}

	public int GetActiveCharacter(int playerId)
	{
		cPlayerRecord cPlayerRecord = FindOrCreatePlayerRecord(playerId);
		return cPlayerRecord.activePlayerCharacter;
	}

	public void SetActiveCharacter(int playerId, int characterId)
	{
		cPlayerRecord cPlayerRecord = FindOrCreatePlayerRecord(playerId);
		cPlayerRecord.activePlayerCharacter = characterId;
	}

	public int CharcterIDFromType(StoreItemType itemType)
	{
		switch (itemType)
		{
		case StoreItemType.kStoreItemPlayerGuy:
			return 0;
		case StoreItemType.kStoreItemPlayerGirl:
			return 1;
		case StoreItemType.kStoreItemPlayerBigB:
			return 2;
		case StoreItemType.kStoreItemPlayerChina:
			return 3;
		case StoreItemType.kStoreItemPlayerIndi:
			return 4;
		case StoreItemType.kStoreItemPlayerConquistador:
			return 5;
		case StoreItemType.kStoreItemPlayerFootball:
			return 6;
		default:
			Debug.LogError("Invalid character type: " + itemType);
			return 0;
		}
	}

	public int GetScoreMulitplier(int playerid)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerid);
		if (cPlayerRecord != null)
		{
			return cPlayerRecord.scoreMultiplier;
		}
		return 10;
	}

	public void SetScoreMultiplier(int playerId, int multiplier)
	{
		cPlayerRecord cPlayerRecord = FindOrCreatePlayerRecord(playerId);
		cPlayerRecord.scoreMultiplier = multiplier;
	}

	public int GetBestScore(int playerId)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		if (cPlayerRecord != null)
		{
			return cPlayerRecord.bestScore;
		}
		return 0;
	}

	public int GetBesCointScore(int playerId)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		if (cPlayerRecord != null)
		{
			return cPlayerRecord.bestCoinScore;
		}
		return 0;
	}

	public int GetBestDistanceScore(int playerId)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		if (cPlayerRecord != null)
		{
			return cPlayerRecord.bestDistanceScore;
		}
		return 0;
	}

	public int GetLifetimePlays(int playerId)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		if (cPlayerRecord != null)
		{
			return cPlayerRecord.lifetimePlays;
		}
		return 0;
	}

	public int GetLifetimeCoins(int playerId)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		if (cPlayerRecord != null)
		{
			return cPlayerRecord.lifetimeCoins;
		}
		return 0;
	}

	public int GetLifetimeDistance(int playerId)
	{
		cPlayerRecord cPlayerRecord = FindPlayerRecord(playerId);
		if (cPlayerRecord != null)
		{
			return cPlayerRecord.lifetimeDistance;
		}
		return 0;
	}

	public void SaveRecords()
	{
		SerializeRecordManager();
	}
}
