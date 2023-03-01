using System;
using UnityEngine;

public class AlertView : BasePanel
{
	public GameObject LeftButton;

	public GameObject CenterButton;

	public GameObject RightButton;

	public UILabel LeftButtonText;

	public UILabel CenterButtonText;

	public UILabel RightButtonText;

	public UILabel TitleText;

	public UILabel DialogText;

	private Action LeftButtonAction;

	private Action RightButtonAction;

	private Action CenterButtonAction;

	public override void Start()
	{
		base.Start();
		HideAll();
	}

	public void ShowAlert(string title, string description, string leftButton, string rightButton, Action leftButtonAction = null, Action rightButtonAction = null)
	{
		ShowAll();
		CenterButton.SetActiveRecursively(false);
		TitleText.text = title;
		DialogText.text = description;
		LeftButtonText.text = leftButton;
		RightButtonText.text = rightButton;
		RightButtonAction = rightButtonAction;
		LeftButtonAction = leftButtonAction;
	}

	public void ShowAlert(string title, string description, string button, Action centerButtonAction = null)
	{
		ShowAll();
		RightButton.SetActiveRecursively(false);
		LeftButton.SetActiveRecursively(false);
		TitleText.text = title;
		DialogText.text = description;
		CenterButtonText.text = button;
		CenterButtonAction = centerButtonAction;
	}

	private void OnClickLeft()
	{
		HideAll();
		if (LeftButtonAction != null)
		{
			LeftButtonAction();
		}
	}

	private void OnClickRight()
	{
		HideAll();
		if (RightButtonAction != null)
		{
			RightButtonAction();
		}
	}

	private void OnClickCenter()
	{
		HideAll();
		if (CenterButtonAction != null)
		{
			CenterButtonAction();
		}
	}
}
