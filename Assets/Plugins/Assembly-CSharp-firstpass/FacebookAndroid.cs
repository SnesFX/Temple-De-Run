using System;
using System.Collections.Generic;
using UnityEngine;

public class FacebookAndroid
{
	private static AndroidJavaObject _facebookPlugin;

	static FacebookAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.FacebookPlugin"))
		{
			_facebookPlugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	public static bool init(string appId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return _facebookPlugin.Call<bool>("init", new object[1] { appId });
	}

	public static void showLoginDialog()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			showLoginDialog(new string[0]);
		}
	}

	public static void showLoginDialog(string[] permissions)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			IntPtr methodID = AndroidJNI.GetMethodID(_facebookPlugin.GetRawClass(), "showLoginDialog", "([Ljava/lang/String;)V");
			AndroidJNI.CallVoidMethod(_facebookPlugin.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(new object[1] { permissions }));
		}
	}

	public static void showLoginDialogWithCommaDelimitedPermissions(string permissionsString)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_facebookPlugin.Call("showLoginDialogWithCommaDelimitedPermissions", permissionsString);
		}
	}

	public static bool isSessionValid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return _facebookPlugin.Call<bool>("isSessionValid", new object[0]);
	}

	public static string getSessionToken()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return string.Empty;
		}
		return _facebookPlugin.Call<string>("getSessionToken", new object[0]);
	}

	public static void extendAccessToken()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_facebookPlugin.Call("extendAccessToken");
		}
	}

	public static void logout()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_facebookPlugin.Call("logout");
		}
	}

	public static void showPostMessageDialog()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_facebookPlugin.Call("showPostMessageDialog");
		}
	}

	public static void showPostMessageDialogWithOptions(string link, string linkName, string linkToImage, string caption)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.os.Bundle"))
		{
			IntPtr methodID = AndroidJNI.GetMethodID(androidJavaObject.GetRawClass(), "putString", "(Ljava/lang/String;Ljava/lang/String;)V");
			object[] array = new object[2];
			if (link != null && link != string.Empty)
			{
				array[0] = new AndroidJavaObject("java.lang.String", "link");
				array[1] = new AndroidJavaObject("java.lang.String", link);
				AndroidJNI.CallVoidMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
			}
			if (linkName != null && linkName != string.Empty)
			{
				array[0] = new AndroidJavaObject("java.lang.String", "name");
				array[1] = new AndroidJavaObject("java.lang.String", linkName);
				AndroidJNI.CallVoidMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
			}
			if (linkToImage != null && linkToImage != string.Empty)
			{
				array[0] = new AndroidJavaObject("java.lang.String", "picture");
				array[1] = new AndroidJavaObject("java.lang.String", linkToImage);
				AndroidJNI.CallVoidMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
			}
			if (caption != null && caption != string.Empty)
			{
				array[0] = new AndroidJavaObject("java.lang.String", "caption");
				array[1] = new AndroidJavaObject("java.lang.String", caption);
				AndroidJNI.CallVoidMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
			}
			_facebookPlugin.Call("showPostMessageDialogWithOptions", androidJavaObject);
		}
	}

	public static void showDialog(string dialogType, Dictionary<string, string> parameters)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.os.Bundle"))
		{
			IntPtr methodID = AndroidJNI.GetMethodID(androidJavaObject.GetRawClass(), "putString", "(Ljava/lang/String;Ljava/lang/String;)V");
			object[] array = new object[2];
			if (parameters != null)
			{
				foreach (KeyValuePair<string, string> parameter in parameters)
				{
					array[0] = new AndroidJavaObject("java.lang.String", parameter.Key);
					array[1] = new AndroidJavaObject("java.lang.String", parameter.Value);
					AndroidJNI.CallVoidMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
				}
			}
			_facebookPlugin.Call("showDialog", dialogType, androidJavaObject);
		}
	}

	public static void postImage(byte[] imageData, string caption)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			IntPtr methodID = AndroidJNI.GetMethodID(_facebookPlugin.GetRawClass(), "postImage", "([BLjava/lang/String;)V");
			AndroidJNI.CallVoidMethod(_facebookPlugin.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(new object[2] { imageData, caption }));
		}
	}

	public static void postImageToAlbum(byte[] imageData, string caption, string albumId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			IntPtr methodID = AndroidJNI.GetMethodID(_facebookPlugin.GetRawClass(), "postImageToAlbum", "([BLjava/lang/String;Ljava/lang/String;)V");
			AndroidJNI.CallVoidMethod(_facebookPlugin.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(new object[3] { imageData, caption, albumId }));
		}
	}

	public static void graphRequest(string graphPath, string httpMethod, Dictionary<string, string> parameters)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.os.Bundle"))
		{
			IntPtr methodID = AndroidJNI.GetMethodID(androidJavaObject.GetRawClass(), "putString", "(Ljava/lang/String;Ljava/lang/String;)V");
			object[] array = new object[2];
			if (parameters != null)
			{
				foreach (KeyValuePair<string, string> parameter in parameters)
				{
					array[0] = new AndroidJavaObject("java.lang.String", parameter.Key);
					array[1] = new AndroidJavaObject("java.lang.String", parameter.Value);
					AndroidJNI.CallVoidMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
				}
			}
			_facebookPlugin.Call("graphRequest", graphPath, httpMethod, androidJavaObject);
		}
	}

	public static void restRequest(string restMethod, string httpMethod, Dictionary<string, string> parameters)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (parameters == null)
		{
			parameters = new Dictionary<string, string>();
		}
		parameters.Add("method", restMethod);
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.os.Bundle"))
		{
			IntPtr methodID = AndroidJNI.GetMethodID(androidJavaObject.GetRawClass(), "putString", "(Ljava/lang/String;Ljava/lang/String;)V");
			object[] array = new object[2];
			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				array[0] = new AndroidJavaObject("java.lang.String", parameter.Key);
				array[1] = new AndroidJavaObject("java.lang.String", parameter.Value);
				AndroidJNI.CallVoidMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
			}
			_facebookPlugin.Call("restRequest", httpMethod, androidJavaObject);
		}
	}
}
