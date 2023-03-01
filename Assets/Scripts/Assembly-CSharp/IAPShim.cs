using System.Collections.Generic;
using UnityEngine;

public class IAPShim
{
	public enum BillingAvaialbleStatus
	{
		Checking = 0,
		NotAllowed = 1,
		Allowed = 2
	}

	private static List<string> ProductIDs;

	private static Dictionary<string, int> ProductCoinRewards;

	private static Dictionary<string, string> ProductPrices;

	private static Dictionary<string, string> ProductIcons;

	private static string code_fblikeus;

	private static string code_followus;

	private static BillingAvaialbleStatus _BillingStatus;

	private static string GiveCoinsOn;

	private static string GiveCoinsProductID;

	private static VCGUI CallBackGUI;

	public static BillingAvaialbleStatus BillingStatus
	{
		get
		{
			return _BillingStatus;
		}
	}

	static IAPShim()
	{
		ProductIDs = new List<string>();
		ProductCoinRewards = new Dictionary<string, int>();
		ProductPrices = new Dictionary<string, string>();
		ProductIcons = new Dictionary<string, string>();
		code_fblikeus = "http://facebook.com/templerun";
		code_followus = "http://twitter.com/#!/templerun";
		GiveCoinsOn = string.Empty;
		GiveCoinsProductID = string.Empty;
		CallBackGUI = null;
		code_fblikeus = Strings.Txt("FacebookLikeURL");
		code_followus = Strings.Txt("FollowUsURL");
		ProductIDs.Add("com.imangi.templerun.iap.coinpack.a");
		ProductIDs.Add("com.imangi.templerun.iap.coinpack.b");
		ProductIDs.Add("com.imangi.templerun.iap.coinpack.c");
		ProductIDs.Add("com.imangi.templerun.iap.coinpack.d");
		ProductCoinRewards.Add("com.imangi.templerun.iap.coinpack.a", 2500);
		ProductCoinRewards.Add("com.imangi.templerun.iap.coinpack.b", 25000);
		ProductCoinRewards.Add("com.imangi.templerun.iap.coinpack.c", 75000);
		ProductCoinRewards.Add("com.imangi.templerun.iap.coinpack.d", 200000);
		ProductCoinRewards.Add(code_fblikeus, 250);
		ProductCoinRewards.Add(code_followus, 250);
		ProductIcons.Add("com.imangi.templerun.iap.coinpack.a", "coinStoreSizeA@2x.png");
		ProductIcons.Add("com.imangi.templerun.iap.coinpack.b", "coinStoreSizeC@2x.png");
		ProductIcons.Add("com.imangi.templerun.iap.coinpack.c", "coinStoreSizeD@2x.png");
		ProductIcons.Add("com.imangi.templerun.iap.coinpack.d", "coinStoreSizeF@2x.png");
		foreach (string productID in ProductIDs)
		{
			ProductPrices.Add(productID, "Buy Now!");
		}
	}

	public static void init()
	{
		Debug.Log("IAP Init");
		_BillingStatus = BillingAvaialbleStatus.Checking;
		IABAndroidManager.billingSupportedEvent += billingSupportedEvent;
		IABAndroidManager.purchaseSucceededEvent += purchaseSucceededEvent;
		IABAndroidManager.purchaseCancelledEvent += purchaseCancelledEvent;
		IABAndroidManager.purchaseFailedEvent += purchaseFailedEvent;
		IABAndroid.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAn1u2YeH2E6+usI47Me1ZpAfoEFmv4FJ4aAfz3iNVNlRAOInvVA1LNVVw4bU2UAvdGpJgMABtxmyeAUgsoKkRkGm6yCM4ZDpDbAlYucTeigVKQD+WUuYoU9eJm6FCnp0/piORYJ6M0m0SB560Xvc2XZilCODRXXN7dUmsTKPrQUc7wmevA/bppf0nqcFd0oQ2gRaviqJ9ec3sHl7DJi7RM5vdlVgrKq3f5BIfTrfydEn8pK9ACVMQApYgv6Uo1ph1utYl8yKTl6GYENdIncD3ENaon1rNytCiUg71Ync41fqKaMGTW/B+qVzux4AYDENnjoZoUtnx0f7AVnfk8rKpZwIDAQAB");
		IABAndroid.startCheckBillingAvailableRequest();
	}

	public static void GetProducts(VCGUI callBack)
	{
		string text = "Buy Now!";
		foreach (string productID in ProductIDs)
		{
			callBack.AddVCItem(ProductIcons[productID], ProductCoinRewards[productID].ToString("##,0"), ProductPrices[productID], productID);
		}
		if (RecordManager.Instance.GetRecordFlagValue(PlayerManager.Instance.GetActivePlayer(), RecordManager.RecordFlagType.kRecordFlagFacebookOfferAvailable) == 1 && RecordManager.Instance.GetRecordFlagValue(PlayerManager.Instance.GetActivePlayer(), RecordManager.RecordFlagType.kRecordFlagFacebookOfferUsed) == 0)
		{
			callBack.AddFreeOffer("coinStoreIconFacebook@2x.png", MapCoinsToProductID(code_fblikeus).ToString(), "Like Temple Run!", code_fblikeus);
		}
		if (RecordManager.Instance.GetRecordFlagValue(PlayerManager.Instance.GetActivePlayer(), RecordManager.RecordFlagType.kRecordFlagTwitterOfferAvailable) == 1 && RecordManager.Instance.GetRecordFlagValue(PlayerManager.Instance.GetActivePlayer(), RecordManager.RecordFlagType.kRecordFlagTwitterOfferUsed) == 0)
		{
			callBack.AddFreeOffer("coinStoreIconTwitter@2x", MapCoinsToProductID(code_followus).ToString(), "Follow on Twitter!", code_followus);
		}
	}

	public static int MapCoinsToProductID(string productId)
	{
		if (ProductCoinRewards.ContainsKey(productId))
		{
			return ProductCoinRewards[productId];
		}
		return 0;
	}

	public static void AlertOK(string result)
	{
		EtceteraAndroidManager.alertButtonClickedEvent -= AlertOK;
		if (result == GiveCoinsOn)
		{
			if (GiveCoinsProductID == code_fblikeus)
			{
				RecordManager.Instance.SetRecordFlagValue(PlayerManager.Instance.GetActivePlayer(), RecordManager.RecordFlagType.kRecordFlagFacebookOfferUsed, 1);
			}
			if (GiveCoinsProductID == code_followus)
			{
				RecordManager.Instance.SetRecordFlagValue(PlayerManager.Instance.GetActivePlayer(), RecordManager.RecordFlagType.kRecordFlagTwitterOfferUsed, 1);
			}
			EtceteraAndroid.showWebView("Temple Run", GiveCoinsProductID);
			if (CallBackGUI != null)
			{
				CallBackGUI.RefreshList();
			}
			purchaseSucceededEvent(GiveCoinsProductID);
		}
	}

	public static void purchase(VCGUI callBack, string productId)
	{
		Debug.Log("IAP Product Attempt: " + productId);
		if (productId == code_fblikeus)
		{
			GiveCoinsOn = Strings.Txt("StoreFacebookLikeAlertYes");
			GiveCoinsProductID = productId;
			CallBackGUI = callBack;
			EtceteraAndroidManager.alertButtonClickedEvent += AlertOK;
			EtceteraAndroid.showAlert(Strings.Txt("StoreFacebookLikeAlertTitle"), string.Format(Strings.Txt("StoreFacebookLikeAlertText"), MapCoinsToProductID(productId)), Strings.Txt("StoreFacebookLikeAlertYes"), Strings.Txt("StoreFacebookLikeAlertNo"));
		}
		else if (productId == code_followus)
		{
			GiveCoinsOn = Strings.Txt("StoreTwitterFollowAlertYes");
			GiveCoinsProductID = productId;
			CallBackGUI = callBack;
			EtceteraAndroidManager.alertButtonClickedEvent += AlertOK;
			EtceteraAndroid.showAlert(Strings.Txt("StoreTwitterFollowAlertTitle"), string.Format(Strings.Txt("StoreTwitterFollowAlertText"), MapCoinsToProductID(productId)), Strings.Txt("StoreTwitterFollowAlertYes"), Strings.Txt("StoreTwitterFollowAlertNo"));
		}
		else
		{
			IABAndroid.purchaseProduct(productId);
		}
	}

	private static void billingSupportedEvent(bool isSupported)
	{
		Debug.Log("MYIAP billingSupportedEvent: " + isSupported);
		_BillingStatus = ((!isSupported) ? BillingAvaialbleStatus.NotAllowed : BillingAvaialbleStatus.Allowed);
	}

	private static void purchaseSucceededEvent(string productId)
	{
		Debug.Log("purchaseSucceededEvent: " + productId);
		int num = MapCoinsToProductID(productId);
		if (num > 0)
		{
			EtceteraAndroid.showToast(string.Format("Added {0} coins!", num), true);
			AudioManager.Instance.PlayFX(AudioManager.Effects.cashRegister);
			RecordManager.Instance.AddCoins(PlayerManager.Instance.GetActivePlayer(), num);
			RecordManager.Instance.SaveRecords();
		}
	}

	private static void purchaseFailedEvent(string productId)
	{
		Debug.Log("purchaseFailedEvent: " + productId);
		EtceteraAndroid.showAlert("Coin Purchase", "Coin Purchase failed.  Check internet connection and try again.", "Ok");
	}

	private static void purchaseCancelledEvent(string productId)
	{
		EtceteraAndroid.showToast("Purchase Canceled", false);
		Debug.Log("purchaseCancelledEvent: " + productId);
	}
}
