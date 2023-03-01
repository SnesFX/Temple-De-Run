using System.Collections.Generic;
using UnityEngine;

public class GameGUI : BasePanel
{
	public GameGUITotem TotemUI;

	public CharacterPlayer GamePlayer;

	public UILabel ScoreLabel;

	public UILabel CoinsLabel;

	private int LastScore;

	private int LastCoins;

	private int MessageBoardLastDistance;

	private List<GUIParticle> ParticleSpriteList;

	public static GameGUI Instance;

	public Transform GUIParticleRoot;

	public GUIParticle ParticlePrefab;

	private void Awake()
	{
		Instance = this;
		SetScore(0);
		SetCoins(0);
		SetupParticleSprites(25);
	}

	public void PauseClicked()
	{
		if (!GameController.SharedInstance.IsPaused)
		{
			PauseMenu.Instance.Show();
		}
	}

	private void Update()
	{
		int score = GamePlayer.Score;
		if (score != LastScore)
		{
			LastScore = score;
			SetScore(score);
		}
		int coinCountTotal = GamePlayer.CoinCountTotal;
		if (coinCountTotal != LastCoins)
		{
			LastCoins = coinCountTotal;
			SetCoins(coinCountTotal);
		}
	}

	public void SetScore(int score)
	{
		ScoreLabel.text = score.ToString();
	}

	public void SetCoins(int coins)
	{
		CoinsLabel.text = coins.ToString();
	}

	public void Reset()
	{
		LastScore = 99999;
		LastCoins = 99999;
		MessageBoardLastDistance = 0;
		TotemUI.Reset();
	}

	private GUIParticle NewParticleSprite()
	{
		GUIParticle gUIParticle = (GUIParticle)Object.Instantiate(ParticlePrefab);
		gUIParticle.transform.parent = GUIParticleRoot;
		gUIParticle.transform.localPosition = Vector3.zero;
		gUIParticle.Setup();
		return gUIParticle;
	}

	private void SetupParticleSprites(int total)
	{
		ParticleSpriteList = new List<GUIParticle>(total);
		for (int i = 0; i < total; i++)
		{
			ParticleSpriteList.Add(NewParticleSprite());
		}
	}

	public GUIParticle NextFreeParticle(string name = null)
	{
		GUIParticle gUIParticle = null;
		foreach (GUIParticle particleSprite in ParticleSpriteList)
		{
			if (!particleSprite.enabled)
			{
				gUIParticle = particleSprite;
				break;
			}
		}
		if (gUIParticle == null)
		{
			gUIParticle = NewParticleSprite();
			ParticleSpriteList.Add(gUIParticle);
		}
		if (name != null)
		{
			gUIParticle.Sprite.spriteName = name;
			gUIParticle.Sprite.MakePixelPerfect();
		}
		gUIParticle.Reset();
		return gUIParticle;
	}

	private string nameForCoin(int coinValue)
	{
		switch (coinValue)
		{
		case 1:
			return "coin.png";
		case 2:
			return "coinRed.png";
		case 3:
			return "coinBlue.png";
		default:
			return "coin.png";
		}
	}

	public void SpawnSpriteParticleForCoin(int coinValue)
	{
		GUIParticle gUIParticle = NextFreeParticle(nameForCoin(coinValue));
		gUIParticle.LifeSpan = 0.6f;
		float x = Random.Range(-20, 21);
		float num = 75f + (float)Random.Range(-20, 21);
		gUIParticle.transform.parent = base.transform;
		gUIParticle.transform.localPosition = new Vector3(x, 0f - num, 0f);
		gUIParticle.Motion = new Vector3(-700f, 700f, 0f);
		gUIParticle.RotationRate = 1000f;
		gUIParticle.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
		gUIParticle.FinalScale = new Vector3(0.6f, 0.6f);
		float num2 = 0.5f;
		gUIParticle.Sprite.color = new Color(num2, num2, num2, num2);
		gUIParticle.Fade = GUIParticle.FadeType.FadeIn;
		gUIParticle.Gravity = true;
		gUIParticle.Delay = 0f;
		gUIParticle.BeginAnimation();
	}

	public void SpawnMegaCoinParticle(int coinCount)
	{
		for (int i = 0; i < coinCount; i++)
		{
			int coinValue = Random.Range(0, 4);
			GUIParticle gUIParticle = NextFreeParticle(nameForCoin(coinValue));
			gUIParticle.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
			float x = 0f;
			float num = -15f;
			gUIParticle.transform.parent = base.transform;
			gUIParticle.transform.localPosition = new Vector3(x, 0f - num, 0f);
			Vector3 motion = new Vector3(Random.Range(-700, 700), Random.Range(-700, 700)).normalized * 700f;
			gUIParticle.Motion = motion;
			gUIParticle.RotationRate = 1000f;
			gUIParticle.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
			gUIParticle.FinalScale = new Vector3(0.7f, 0.7f, 1f);
			float lifeSpan = Random.Range(0f, 0.5f) + 0.5f;
			gUIParticle.LifeSpan = lifeSpan;
			gUIParticle.Fade = GUIParticle.FadeType.FadeOut;
			gUIParticle.Gravity = false;
			gUIParticle.Delay = Random.Range(0f, 0.125f);
			gUIParticle.BeginAnimation();
		}
	}
}
