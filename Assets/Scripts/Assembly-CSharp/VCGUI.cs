using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCGUI : BasePanel
{
	public GameObject SlideInOffset;

	public GameObject BackButton;

	public UILabel CoinsLabel;

	public UILabel MessageLabel;

	public RecordManager RecordManager;

	public PlayerManager PlayerManager;

	public UIStack Stack;

	public VCItem ItemPrefab;

	public GameObject FreeOffersSection;

	private int OnItem = 100;

	private int lastCoins = -1;

	private bool ListAcquired;

	private bool FreeOffersSetionAdded;

	public static VCGUI Instance;

	private void Awake()
	{
		Setup();
	}

	private void Setup()
	{
		Instance = this;
		IAPShim.init();
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
		MessageLabel.enabled = true;
		MessageLabel.text = "Loading...";
		ClearList();
		ShowAll();
		SlideInOffset.transform.localPosition = new Vector3(0f, 1125f, -40f);
		TweenPosition.Begin(SlideInOffset, 0.5f, new Vector3(0f, 0f, -40f)).method = UITweener.Method.EaseIn;
		Vector3 mm = BackButton.transform.localPosition;
		BackButton.GetComponentInChildren<UISprite>().enabled = false;
		BackButton.transform.Translate(0f, -200f, 0f);
		yield return new WaitForSeconds(0.75f);
		BackButton.GetComponentInChildren<UISprite>().enabled = true;
		TweenPosition.Begin(BackButton, 0.25f, mm);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
	}

	private void Update()
	{
		if (!ListAcquired && IAPShim.BillingStatus == IAPShim.BillingAvaialbleStatus.Allowed)
		{
			FillMeuItems();
		}
		int coinCount = RecordManager.GetCoinCount(PlayerManager.GetActivePlayer());
		if (coinCount != lastCoins)
		{
			lastCoins = coinCount;
			CoinsLabel.text = coinCount.ToString("##,0");
		}
	}

	private void ClearList()
	{
		lastCoins = -1;
		ListAcquired = false;
		FreeOffersSetionAdded = false;
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
		OnItem = 100;
	}

	private void FillMeuItems()
	{
		ListAcquired = true;
		IAPShim.GetProducts(this);
	}

	public void RefreshList()
	{
		ClearList();
		ListAcquired = false;
	}

	public void AddVCItem(string icon, string offer, string price, string code)
	{
		MessageLabel.enabled = false;
		VCItem vCItem = (VCItem)Object.Instantiate(ItemPrefab);
		vCItem.transform.parent = Stack.transform;
		vCItem.name = OnItem + ". VC Item";
		vCItem.Setup(this, icon, offer, price, code);
		vCItem.GetComponent<UIDragObject>().target = Stack.transform;
		OnItem++;
	}

	private void AddFreeOfferSection()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(FreeOffersSection);
		gameObject.transform.parent = Stack.transform;
		gameObject.GetComponent<UIDragObject>().target = Stack.transform;
		OnItem = 300;
		FreeOffersSetionAdded = true;
	}

	public void AddFreeOffer(string icon, string offer, string price, string code)
	{
		if (!FreeOffersSetionAdded)
		{
			AddFreeOfferSection();
		}
		VCItem vCItem = (VCItem)Object.Instantiate(ItemPrefab);
		vCItem.transform.parent = Stack.transform;
		vCItem.name = OnItem + ". Free Item";
		vCItem.Setup(this, icon, offer, price, code);
		vCItem.GetComponent<UIDragObject>().target = Stack.transform;
		vCItem.IconLeft();
		OnItem++;
	}

	private void BackButtonClicked()
	{
		ReturnMenu();
	}
}
