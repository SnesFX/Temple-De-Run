using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IABAndroidManager : MonoBehaviour
{
	[method: MethodImpl(32)]
	public static event Action<bool> billingSupportedEvent;

	[method: MethodImpl(32)]
	public static event Action<string> purchaseSucceededEvent;

	[method: MethodImpl(32)]
	public static event Action<string, string> purchaseSignatureVerifiedEvent;

	[method: MethodImpl(32)]
	public static event Action<string> purchaseCancelledEvent;

	[method: MethodImpl(32)]
	public static event Action<string> purchaseRefundedEvent;

	[method: MethodImpl(32)]
	public static event Action<string> purchaseFailedEvent;

	[method: MethodImpl(32)]
	public static event Action transactionsRestoredEvent;

	[method: MethodImpl(32)]
	public static event Action<string> transactionRestoreFailedEvent;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void billingSupported(string isSupported)
	{
		if (IABAndroidManager.billingSupportedEvent != null)
		{
			IABAndroidManager.billingSupportedEvent(isSupported == "1");
		}
	}

	public void purchaseSignatureVerified(string data)
	{
		if (IABAndroidManager.purchaseSignatureVerifiedEvent != null)
		{
			string[] array = data.Split(new string[1] { "~~~" }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 2)
			{
				IABAndroidManager.purchaseSignatureVerifiedEvent(array[0], array[1]);
			}
		}
	}

	public void purchaseSucceeded(string productId)
	{
		if (IABAndroidManager.purchaseSucceededEvent != null)
		{
			IABAndroidManager.purchaseSucceededEvent(productId);
		}
	}

	public void purchaseCancelled(string productId)
	{
		if (IABAndroidManager.purchaseCancelledEvent != null)
		{
			IABAndroidManager.purchaseCancelledEvent(productId);
		}
	}

	public void purchaseRefunded(string productId)
	{
		if (IABAndroidManager.purchaseRefundedEvent != null)
		{
			IABAndroidManager.purchaseRefundedEvent(productId);
		}
	}

	public void purchaseFailed(string productId)
	{
		if (IABAndroidManager.purchaseFailedEvent != null)
		{
			IABAndroidManager.purchaseFailedEvent(productId);
		}
	}

	public void transactionsRestored(string empty)
	{
		if (IABAndroidManager.transactionsRestoredEvent != null)
		{
			IABAndroidManager.transactionsRestoredEvent();
		}
	}

	public void transactionRestoreFailed(string error)
	{
		if (IABAndroidManager.transactionRestoreFailedEvent != null)
		{
			IABAndroidManager.transactionRestoreFailedEvent(error);
		}
	}
}
