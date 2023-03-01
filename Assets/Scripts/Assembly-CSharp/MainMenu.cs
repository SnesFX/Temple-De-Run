using UnityEngine;

public class MainMenu : BasePanel
{
	public static MainMenu Instance;

	public override void Start()
	{
		Instance = this;
		base.Start();
		HideAll();
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public override void SlideIn(BasePanel returnTo)
	{
		base.SlideIn(returnTo);
		FadeIn(0f, 0.5f);
	}

	public override void ShowAll()
	{
		base.ShowAll();
		EtceteraAndroid.askForReview(10, 48, 24, Strings.Txt("ReviewRequestTitle"), Strings.Txt("ReviewRequestBody"), "com.imangi.templerun");
	}

	public void OnPlayClicked()
	{
		HideAll();
		Controller.GameStart();
	}
}
