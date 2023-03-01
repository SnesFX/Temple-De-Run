using System.Collections;
using UnityEngine;

public class FacebookAndroidEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		FacebookAndroidManager.authDidSucceedEvent += authDidSucceedEvent;
		FacebookAndroidManager.authDidFailEvent += authDidFailEvent;
		FacebookAndroidManager.dialogDidSucceedEvent += dialogDidSucceedEvent;
		FacebookAndroidManager.dialogWasCancelledEvent += dialogWasCancelledEvent;
		FacebookAndroidManager.dialogDidFailEvent += dialogDidFailEvent;
		FacebookAndroidManager.dialogWasCancelledEvent += dialogWasCancelledEvent;
		FacebookAndroidManager.graphRequestDidSucceedEvent += graphRequestDidSucceedEvent;
		FacebookAndroidManager.graphRequestDidFailEvent += graphRequestDidFailEvent;
		FacebookAndroidManager.restRequestDidSucceedEvent += restRequestDidSucceedEvent;
		FacebookAndroidManager.restRequestDidFailEvent += restRequestDidFailEvent;
		FacebookAndroidManager.imageUploadDidSucceedEvent += imageUploadDidSucceedEvent;
		FacebookAndroidManager.imageUploadDidFailEvent += imageUploadDidFailEvent;
		FacebookAndroidManager.extendAccessTokenSucceeded += extendAccessTokenSucceeded;
		FacebookAndroidManager.extendAccessTokenFailed += extendAccessTokenFailed;
	}

	private void OnDisable()
	{
		FacebookAndroidManager.authDidSucceedEvent -= authDidSucceedEvent;
		FacebookAndroidManager.authDidFailEvent -= authDidFailEvent;
		FacebookAndroidManager.dialogDidSucceedEvent -= dialogDidSucceedEvent;
		FacebookAndroidManager.dialogWasCancelledEvent -= dialogWasCancelledEvent;
		FacebookAndroidManager.dialogDidFailEvent -= dialogDidFailEvent;
		FacebookAndroidManager.dialogWasCancelledEvent -= dialogWasCancelledEvent;
		FacebookAndroidManager.graphRequestDidSucceedEvent -= graphRequestDidSucceedEvent;
		FacebookAndroidManager.graphRequestDidFailEvent -= graphRequestDidFailEvent;
		FacebookAndroidManager.imageUploadDidSucceedEvent -= imageUploadDidSucceedEvent;
		FacebookAndroidManager.imageUploadDidFailEvent -= imageUploadDidFailEvent;
		FacebookAndroidManager.extendAccessTokenSucceeded -= extendAccessTokenSucceeded;
		FacebookAndroidManager.extendAccessTokenFailed -= extendAccessTokenFailed;
	}

	private void authDidSucceedEvent()
	{
		Debug.Log("authDidSucceedEvent");
	}

	private void authDidFailEvent(string error)
	{
		Debug.Log("authDidFailEvent: " + error);
	}

	private void dialogDidSucceedEvent(string querystring)
	{
		Debug.Log("dialogDidSucceedEvent: " + querystring);
	}

	private void dialogWasCancelledEvent()
	{
		Debug.Log("dialogWasCancelledEvent");
	}

	private void dialogDidFailEvent(string error)
	{
		Debug.Log("dialogDidFailEvent: " + error);
	}

	private void graphRequestDidSucceedEvent(object result)
	{
		Debug.Log("graphRequestDidSucceedEvent");
		ResultLogger.logObject(result);
	}

	private void graphRequestDidFailEvent(string error)
	{
		Debug.Log("graphRequestDidFailEvent: " + error);
	}

	private void restRequestDidSucceedEvent(object result)
	{
		Debug.Log("restRequestDidSucceedEvent");
		ResultLogger.logObject(result);
	}

	private void restRequestDidFailEvent(string error)
	{
		Debug.Log("restRequestDidFailEvent: " + error);
	}

	private void imageUploadDidSucceedEvent(Hashtable result)
	{
		Debug.Log("imageUploadDidSucceedEvent");
		ResultLogger.logObject(result);
	}

	private void imageUploadDidFailEvent(string error)
	{
		Debug.Log("imageUploadDidFailEvent: " + error);
	}

	private void extendAccessTokenSucceeded()
	{
		Debug.Log("extendAccessTokenSucceeded");
	}

	private void extendAccessTokenFailed(string error)
	{
		Debug.Log("extendAccessTokenFailed: " + error);
	}
}
