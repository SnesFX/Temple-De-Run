using System.Collections.Generic;
using UnityEngine;

public class FlurryUIManager : MonoBehaviour
{
	private void OnGUI()
	{
		float num = 5f;
		float left = 5f;
		float num2 = ((Screen.width < 800 && Screen.height < 800) ? 160 : 320);
		float num3 = ((Screen.width < 800 && Screen.height < 800) ? 30 : 70);
		float num4 = num3 + 10f;
		if (GUI.Button(new Rect(left, num, num2, num3), "Start Flurry Session"))
		{
			FlurryAndroid.enableAppCircle("YOUR_INTENT_NAME_FROM_MANIFEST_FILE");
			FlurryAndroid.onStartSession("YOUR_FLURRY_APP_ID");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "End Flurry Session"))
		{
			FlurryAndroid.onEndSession();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Log Timed Event"))
		{
			FlurryAndroid.logEvent("timed", true);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Log Event with Params"))
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("akey1", "value1");
			dictionary.Add("bkey2", "value2");
			dictionary.Add("ckey3", "value3");
			dictionary.Add("dkey4", "value4");
			FlurryAndroid.logEvent("EventWithParams", dictionary);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Create banner"))
		{
			float screenDensity = FlurryAndroid.getScreenDensity();
			int topMargin = (int)((float)Screen.height - 60f * screenDensity);
			FlurryAndroid.showBanner("hook_name", 0, topMargin);
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Get Offer"))
		{
			FlurryOffer offer = FlurryAndroid.getOffer("madeUpHook");
			if (offer != null)
			{
				Debug.Log(offer);
			}
		}
		left = (float)Screen.width - num2 - 5f;
		num = 5f;
		if (GUI.Button(new Rect(left, num, num2, num3), "End Timed Event"))
		{
			FlurryAndroid.endTimedEvent("timed");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Log Page View"))
		{
			FlurryAndroid.onPageView();
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Log Error"))
		{
			FlurryAndroid.onError("666", "bad things happend", "Exception");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Show Catalog"))
		{
			FlurryAndroid.openCatalog("myHookName");
		}
		if (GUI.Button(new Rect(left, num += num4, num2, num3), "Destroy Banner"))
		{
			FlurryAndroid.destroyBanner();
		}
		if (!GUI.Button(new Rect(left, num += num4, num2, num3), "Get All Offers"))
		{
			return;
		}
		List<FlurryOffer> allOffers = FlurryAndroid.getAllOffers("hook-hook");
		foreach (FlurryOffer item in allOffers)
		{
			Debug.Log(item);
		}
	}
}
