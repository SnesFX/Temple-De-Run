using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameUI : BasePanel
{
	public class Illustration
	{
		public string Image;

		public List<string> Captions;
	}

	public Dictionary<CharacterPlayer.DeathTypes, Dictionary<int, List<Illustration>>> DeathDetails;

	public TextAsset DeathImageFile;

	public GameObject SlideInOffset;

	public GameObject MainMenuButton;

	public GameObject RunAgainButton;

	public UILabel ScoreLabel;

	public UILabel DistanceLabel;

	public UILabel CoinsLabel;

	public UILabel MultiplierLabel;

	public GameObject StoreButton;

	public GameObject FreeCoinsButton;

	public GameObject TweetButton;

	public GameObject StatsButton;

	public StoreGUI StoreInterface;

	public VCGUI CoinsInterface;

	public UITexture DeathIllustration;

	public UILabel DeathCaption;

	private string[] SheetName = new string[7] { "DeathGuy", "DeathGirl", "DeathBigB", "DeathChina", "DeathIndi", "DeathConq", "DeathFootball" };

	private void Awake()
	{
		Setup();
		ParseDeathDetails();
	}

	private void Setup()
	{
		HideAll();
		DeathIllustration.material = new Material(DeathIllustration.material);
	}

	public override void SlideIn(BasePanel returnTo)
	{
		base.SlideIn(returnTo);
		ShowAll();
		StartCoroutine(SlideInAnimation());
	}

	private IEnumerator SlideInAnimation()
	{
		ShowAll();
		UpdateStats();
		SlideInOffset.transform.localPosition = new Vector3(0f, 1125f, -40f);
		TweenPosition.Begin(SlideInOffset, 0.5f, new Vector3(0f, 0f, -40f)).method = UITweener.Method.EaseIn;
		Vector3 mm = MainMenuButton.transform.localPosition;
		Vector3 ra = RunAgainButton.transform.localPosition;
		Vector3 sb = StoreButton.transform.localPosition;
		Vector3 fb = FreeCoinsButton.transform.localPosition;
		Vector3 gb = StatsButton.transform.localPosition;
		Vector3 tb = TweetButton.transform.localPosition;
		MainMenuButton.GetComponentInChildren<UISprite>().enabled = false;
		MainMenuButton.transform.Translate(0f, -200f, 0f);
		RunAgainButton.GetComponentInChildren<UISprite>().enabled = false;
		RunAgainButton.transform.Translate(0f, -200f, 0f);
		StoreButton.transform.Translate(0f, 600f, 0f);
		FreeCoinsButton.transform.Translate(0f, 600f, 0f);
		StatsButton.transform.Translate(0f, 600f, 0f);
		TweetButton.transform.Translate(0f, 600f, 0f);
		yield return new WaitForSeconds(0.75f);
		RunAgainButton.GetComponentInChildren<UISprite>().enabled = true;
		TweenPosition.Begin(RunAgainButton, 0.25f, ra);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
		yield return new WaitForSeconds(0.25f);
		MainMenuButton.GetComponentInChildren<UISprite>().enabled = true;
		TweenPosition.Begin(MainMenuButton, 0.25f, mm);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
		yield return new WaitForSeconds(0.25f);
		TweenPosition.Begin(FreeCoinsButton, 0.25f, fb);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
		yield return new WaitForSeconds(0.25f);
		TweenPosition.Begin(StoreButton, 0.25f, sb);
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
	}

	private void UpdateStats()
	{
		ScoreLabel.text = Controller.GamePlayer.Score.ToString("##,0");
		DistanceLabel.text = Controller.DistanceTraveled.ToString("##,0m");
		CoinsLabel.text = Controller.GamePlayer.CoinCountTotal.ToString("##,0");
		MultiplierLabel.text = Controller.GamePlayer.ScoreMultiplier.ToString("##x");
		AssignDeathDetails();
	}

	private void RunAgainClicked()
	{
		HideAll();
		Controller.RestartGame();
		Controller.GameStart();
	}

	private void MainMenuClicked()
	{
		HideAll();
		Controller.SnapMainMenu();
	}

	private void StoreClick()
	{
		HideAll();
		StoreInterface.SlideIn(this);
	}

	private void FreeCoinsClicked()
	{
		HideAll();
		CoinsInterface.SlideIn(this);
	}

	public override void HideAll()
	{
		base.HideAll();
		Resources.UnloadUnusedAssets();
	}

	private void AssignDeathDetails()
	{
		Dictionary<int, List<Illustration>> dictionary = null;
		CharacterPlayer.DeathTypes deathType = GameController.SharedInstance.GamePlayer.DeathType;
		dictionary = ((!DeathDetails.ContainsKey(deathType)) ? DeathDetails[CharacterPlayer.DeathTypes.kDeathTypeUnknown] : DeathDetails[deathType]);
		if (dictionary == null)
		{
			Debug.LogError("Could not find proper death type details: " + deathType);
			return;
		}
		int activeCharacterId = GameController.SharedInstance.GamePlayer.ActiveCharacterId;
		List<Illustration> list = null;
		list = ((!dictionary.ContainsKey(activeCharacterId)) ? dictionary[0] : dictionary[activeCharacterId]);
		if (list == null)
		{
			Debug.LogError(string.Concat("Could not find proper death type details for character: ", deathType, "  cid: ", activeCharacterId));
			return;
		}
		Debug.Log(list);
		int num = UnityEngine.Random.Range(0, list.Count);
		Debug.Log(num + " count: " + list.Count);
		Illustration illustration = list[num];
		string key = illustration.Captions[UnityEngine.Random.Range(0, illustration.Captions.Count)];
		DeathCaption.text = Strings.Txt(key);
		string text = "Deaths/" + illustration.Image.Replace(".png", string.Empty);
		Texture2D texture2D = Resources.Load(text, typeof(Texture2D)) as Texture2D;
		Debug.Log("Death: " + text + " T: " + texture2D, texture2D);
		DeathIllustration.material.mainTexture = texture2D;
		DeathIllustration.MakePixelPerfect();
	}

	private void ParseDeathDetails()
	{
		DeathDetails = new Dictionary<CharacterPlayer.DeathTypes, Dictionary<int, List<Illustration>>>();
		string[] array = DeathImageFile.text.Split('\n');
		Debug.Log("Lines: " + array.Length);
		int num = 1;
		Dictionary<int, List<Illustration>> dictionary = null;
		List<Illustration> list = null;
		Illustration illustration = null;
		string[] array2 = array;
		foreach (string text in array2)
		{
			string text2 = text.Trim();
			string[] array3 = text2.Split(' ');
			if (array3.Length <= 1)
			{
				continue;
			}
			switch (array3[0])
			{
			case "deathtype":
			{
				CharacterPlayer.DeathTypes deathTypes = TypeFromName(array3[1]);
				if (DeathDetails.ContainsKey(deathTypes))
				{
					Debug.LogError(num + ": repeated death type: " + deathTypes);
					return;
				}
				dictionary = new Dictionary<int, List<Illustration>>();
				DeathDetails.Add(deathTypes, dictionary);
				break;
			}
			case "character":
			{
				int num4 = int.Parse(array3[1]);
				if (dictionary.ContainsKey(num4))
				{
					Debug.LogError(num + ": repeated character: " + num4);
					return;
				}
				list = new List<Illustration>();
				dictionary.Add(num4, list);
				break;
			}
			case "image":
				illustration = new Illustration();
				illustration.Image = array3[1];
				illustration.Captions = new List<string>();
				list.Add(illustration);
				break;
			case "caption":
				if (array3.Length == 2)
				{
					illustration.Captions.Add(array3[1]);
				}
				else if (array3.Length == 3)
				{
					string[] array4 = array3[2].Split(new string[1] { ".." }, StringSplitOptions.None);
					if (array4.Length != 2)
					{
						Debug.LogError(num + ": error could not parse: " + text);
						return;
					}
					int num2 = int.Parse(array4[0]);
					int num3 = int.Parse(array4[1]);
					if (num2 >= num3 || num2 == 0 || num3 == 0)
					{
						Debug.LogError(num + ": error could not parse: " + text);
						return;
					}
					for (int j = num2; j <= num3; j++)
					{
						illustration.Captions.Add(array3[1] + j);
					}
				}
				break;
			default:
				Debug.LogError(num + ": Unknown element: " + array3[0]);
				Debug.Break();
				break;
			}
			num++;
		}
	}

	private CharacterPlayer.DeathTypes TypeFromName(string name)
	{
		switch (name)
		{
		case "kDeathTypeUnknown":
			return CharacterPlayer.DeathTypes.kDeathTypeUnknown;
		case "kDeathTypeFall":
			return CharacterPlayer.DeathTypes.kDeathTypeFall;
		case "kDeathTypeTree":
			return CharacterPlayer.DeathTypes.kDeathTypeTree;
		case "kDeathTypeSlide":
			return CharacterPlayer.DeathTypes.kDeathTypeSlide;
		case "kDeathTypeLedge":
			return CharacterPlayer.DeathTypes.kDeathTypeLedge;
		case "kDeathTypeBurnt":
			return CharacterPlayer.DeathTypes.kDeathTypeBurnt;
		case "kDeathTypeEaten":
			return CharacterPlayer.DeathTypes.kDeathTypeEaten;
		case "kDeathTypeTangle":
			return CharacterPlayer.DeathTypes.kDeathTypeTangle;
		case "kDeathTypeSceneryTree":
			return CharacterPlayer.DeathTypes.kDeathTypeSceneryTree;
		case "kDeathTypeSceneryRock":
			return CharacterPlayer.DeathTypes.kDeathTypeSceneryRock;
		default:
			return CharacterPlayer.DeathTypes.kDeathTypeUnknown;
		}
	}
}
