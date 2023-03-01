using System.Collections;
using UnityEngine;

public class GameGUITotem : MonoBehaviour
{
	public GameGUI MainGameInterface;

	public UIFilledSprite ProgressBarSprite;

	public UISprite TotemEmblem1;

	public UISprite TotemEmblem2;

	public UISprite TotemEmblem3;

	public UISprite TotemEmblem4;

	public UISprite FallingTotemEmblem1;

	public UISprite FallingTotemEmblem2;

	public UISprite FallingTotemEmblem3;

	public UISprite FallingTotemEmblem4;

	public GameObject BonusObject;

	public CharacterPlayer GamePlayer;

	private float lastCoinPercent = 2f;

	public int BonusLevel;

	public bool BonusLevelJustChagned;

	public int AccumulatedScore;

	public float TimeSinceBonusLevelChang;

	public bool TestAnimation;

	private float testTime = 0.5f;

	private int bl = 1;

	private void Awake()
	{
		Setup();
	}

	private void Setup()
	{
		Reset();
	}

	private void SetVisibility(bool visible)
	{
	}

	public void ShowAll()
	{
		SetVisibility(true);
	}

	public void HideAll()
	{
		SetVisibility(false);
	}

	private void Update()
	{
		float num = (float)GamePlayer.CoinCountForBonus / (float)GamePlayer.CoinCountForBonusThreshold;
		if (num != lastCoinPercent)
		{
			UpdateProgress(num);
			lastCoinPercent = num;
		}
		if (BonusLevel != GamePlayer.BonusLevel)
		{
			SetBonusLevel(GamePlayer.BonusLevel, true);
		}
		if (!TestAnimation)
		{
			return;
		}
		testTime -= Time.smoothDeltaTime;
		if (testTime < 0f)
		{
			testTime = 3f;
			SetBonusLevel(bl, true);
			bl++;
			if (bl > 5)
			{
				bl = 1;
			}
		}
	}

	public void Reset()
	{
		lastCoinPercent = 2f;
		BonusLevel = 1;
		BonusLevelJustChagned = false;
		AccumulatedScore = 0;
		TotemEmblem1.enabled = false;
		TotemEmblem2.enabled = false;
		TotemEmblem3.enabled = false;
		TotemEmblem4.enabled = false;
		FallingTotemEmblem1.color = new Color(1f, 1f, 1f, 0f);
		FallingTotemEmblem2.color = new Color(1f, 1f, 1f, 0f);
		FallingTotemEmblem3.color = new Color(1f, 1f, 1f, 0f);
		FallingTotemEmblem4.color = new Color(1f, 1f, 1f, 0f);
		UISprite componentInChildren = BonusObject.GetComponentInChildren<UISprite>();
		if (componentInChildren != null)
		{
			componentInChildren.enabled = false;
		}
	}

	public void UpdateProgress(float coinPercent)
	{
		ProgressBarSprite.fillAmount = coinPercent;
	}

	private void StartEmblemFallIfVisible(UISprite emblem)
	{
		emblem.transform.localPosition = new Vector3(0f, 0f, 0f);
		TweenPosition.Begin(emblem.gameObject, 1.5f, new Vector3(Random.Range(-10, 90), -Random.Range(800, 1024), 0f)).method = UITweener.Method.EaseIn;
		emblem.color = new Color(1f, 1f, 1f, 1f);
		TweenColor.Begin(emblem.gameObject, 1.5f, new Color(1f, 1f, 1f, 0f)).method = UITweener.Method.EaseIn;
	}

	public void SetBonusLevel(int level, bool awardBonus)
	{
		if (level <= 0 || level == BonusLevel)
		{
			return;
		}
		bool flag = level > BonusLevel;
		BonusLevel = level;
		BonusLevelJustChagned = true;
		TimeSinceBonusLevelChang = 0f;
		if (BonusLevel == 1)
		{
			StartEmblemFallIfVisible(FallingTotemEmblem1);
			StartEmblemFallIfVisible(FallingTotemEmblem2);
			StartEmblemFallIfVisible(FallingTotemEmblem3);
			StartEmblemFallIfVisible(FallingTotemEmblem4);
		}
		TotemEmblem1.enabled = BonusLevel > 1;
		TotemEmblem2.enabled = BonusLevel > 2;
		TotemEmblem3.enabled = BonusLevel > 3;
		TotemEmblem4.enabled = BonusLevel > 4;
		if (flag && awardBonus)
		{
			int bonusLevel = BonusLevel;
			bonusLevel = Mathf.Min(bonusLevel, 4);
			int num = 0;
			string bonusName = null;
			switch (bonusLevel)
			{
			case 2:
				num = 100;
				bonusName = "bonus100.png";
				break;
			case 3:
				num = 250;
				bonusName = "bonus250.png";
				break;
			case 4:
				num = 500;
				bonusName = "bonus500.png";
				break;
			case 5:
				num = 1000;
				bonusName = "bonus1000.png";
				break;
			}
			if (num != 0)
			{
				StartCoroutine(AnimateBonus(bonusName));
				AccumulatedScore += num;
			}
			AudioManager.Instance.PlayFX(AudioManager.Effects.bonusMeterFull, 0.5f);
		}
	}

	public IEnumerator AnimateBonus(string BonusName)
	{
		BonusObject.transform.localPosition = new Vector3(100f, -80f, 0f);
		BonusObject.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
		TweenScale.Begin(BonusObject, 1f, new Vector3(0.5f, 0.5f, 1f)).method = UITweener.Method.Linear;
		TweenPosition.Begin(BonusObject, 1f, new Vector3(530f, -80f, 0f)).method = UITweener.Method.Linear;
		UISprite s = BonusObject.GetComponentInChildren<UISprite>();
		s.color = new Color(0.75f, 0.75f, 0.75f, 0.75f);
		TweenColor.Begin(s.gameObject, 1f, new Color(0f, 0f, 0f, 0f)).method = UITweener.Method.Linear;
		s.spriteName = BonusName;
		s.enabled = true;
		yield return new WaitForSeconds(1f);
		s.enabled = false;
		for (int i = 0; i < 25; i++)
		{
			GUIParticle star = MainGameInterface.NextFreeParticle("sparkleParticle.png");
			star.transform.parent = base.transform;
			SetupStar(star);
		}
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
		AudioManager.Instance.PlayFX(AudioManager.Effects.scoreBlast);
	}

	private void SetupStar(GUIParticle sprite)
	{
		sprite.transform.localPosition = new Vector3(570f, -60f, 0f);
		sprite.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		Vector3 motion = new Vector3(Random.Range(-200, 100), Random.Range(-400, 100), 0f);
		motion.Normalize();
		motion *= (float)Random.Range(400, 500);
		sprite.Motion = motion;
		sprite.RotationRate = 1024f;
		sprite.LifeSpan = 0.5f * (Random.value * 1.5f + 0.75f);
		sprite.FinalScale = new Vector3(1.5f, 1.5f, 1f);
		sprite.Fade = GUIParticle.FadeType.FadeOut;
		sprite.Gravity = false;
		sprite.Delay = Random.Range(0f, 0.125f);
		sprite.BeginAnimation();
	}
}
