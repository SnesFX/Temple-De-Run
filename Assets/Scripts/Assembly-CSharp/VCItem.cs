using UnityEngine;

public class VCItem : MonoBehaviour
{
	public UISprite IconSprite;

	public UILabel OfferLabel;

	public UILabel PriceLabel;

	private VCGUI GUI;

	private string Code;

	public void Setup(VCGUI gui, string icon, string offer, string price, string code)
	{
		IconSprite.spriteName = icon;
		OfferLabel.text = offer;
		PriceLabel.text = price;
		Code = code;
		GUI = gui;
	}

	private void OnClick()
	{
		IAPShim.purchase(GUI, Code);
	}

	public void IconLeft()
	{
		IconSprite.transform.parent.Translate(-50f, 0f, 0f);
	}
}
