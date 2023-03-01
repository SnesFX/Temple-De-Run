using System;
using System.Collections.Generic;
using UnityEngine;

public class FlurryAndroid
{
	private static AndroidJavaClass _flurryAgent;

	private static AndroidJavaObject _plugin;

	static FlurryAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		_flurryAgent = new AndroidJavaClass("com.flurry.android.FlurryAgent");
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.FlurryPlugin"))
		{
			_plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	public static void onStartSession(string apiKey)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("onStartSession", apiKey);
		}
	}

	public static void onEndSession()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("onEndSession");
		}
	}

	public static void setContinueSessionMillis(long milliseconds)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_flurryAgent.CallStatic("setContinueSessionMillis", milliseconds);
		}
	}

	public static void logEvent(string eventName)
	{
		logEvent(eventName, false);
	}

	public static void logEvent(string eventName, bool isTimed)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (isTimed)
			{
				_plugin.Call("logTimedEvent", eventName);
			}
			else
			{
				_plugin.Call("logEvent", eventName);
			}
		}
	}

	public static void logEvent(string eventName, Dictionary<string, string> parameters)
	{
		logEvent(eventName, parameters, false);
	}

	public static void logEvent(string eventName, Dictionary<string, string> parameters, bool isTimed)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap"))
		{
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			object[] array = new object[2];
			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", parameter.Key))
				{
					using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", parameter.Value))
					{
						array[0] = androidJavaObject2;
						array[1] = androidJavaObject3;
						AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
					}
				}
			}
			if (isTimed)
			{
				_plugin.Call("logTimedEventWithParams", eventName, androidJavaObject);
			}
			else
			{
				_plugin.Call("logEventWithParams", eventName, androidJavaObject);
			}
		}
	}

	public static void endTimedEvent(string eventName)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("endTimedEvent", eventName);
		}
	}

	public static void onPageView()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_flurryAgent.CallStatic("onPageView");
		}
	}

	public static void onError(string errorId, string message, string errorClass)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_flurryAgent.CallStatic("onError", errorId, message, errorClass);
		}
	}

	public static void setUserID(string userId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_flurryAgent.CallStatic("setUserID", userId);
		}
	}

	public static void setAge(int age)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_flurryAgent.CallStatic("setAge", age);
		}
	}

	public static void setLogEnabled(bool enable)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_flurryAgent.CallStatic("setLogEnabled", enable);
		}
	}

	public static void enableAppCircle(string intentName)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("enableAppCircle", intentName);
		}
	}

	public static float getScreenDensity()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return 1f;
		}
		return _plugin.Call<float>("getScreenDensity", new object[0]);
	}

	public static void showBanner(string hookname, int leftMargin, int topMargin)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("showBanner", hookname, leftMargin, topMargin);
		}
	}

	public static void destroyBanner()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("destroyBanner");
		}
	}

	public static void addUserCookie(string key, string value)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("addUserCookie", key, value);
		}
	}

	public static void clearUserCookies()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("clearUserCookies");
		}
	}

	public static void openCatalog(string hookname)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("openCatalog", hookname);
		}
	}

	public static void launchCatalogOnBannerClicked(bool shouldLaunch)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("launchCatalogOnBannerClicked", shouldLaunch);
		}
	}

	public static void setDefaultNoAdsMessage(string msg)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("setDefaultNoAdsMessage", msg);
		}
	}

	public static FlurryOffer getOffer(string hookname)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return null;
		}
		string json = _plugin.Call<string>("getOffer", new object[1] { hookname });
		List<FlurryOffer> list = FlurryOffer.fromJSON(json);
		if (list.Count > 0)
		{
			return list[0];
		}
		return null;
	}

	public static List<FlurryOffer> getAllOffers(string hookname)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return null;
		}
		string json = _plugin.Call<string>("getAllOffers", new object[1] { hookname });
		return FlurryOffer.fromJSON(json);
	}

	public static void acceptOffer(long offerId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("acceptOffer", offerId);
		}
	}

	public static void removeOffer(long offerId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("removeOffer", offerId);
		}
	}
}
