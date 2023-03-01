using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public enum Effects
	{
		angelWings = 0,
		bonusMeterFull = 1,
		bonusPickup = 2,
		boostLoop = 3,
		buttonClick = 4,
		cashRegister = 5,
		coin = 6,
		cymbalCrash = 7,
		footstepsCliff = 8,
		footstepsPlank = 9,
		footstepsTemple = 10,
		footstepsTurn = 11,
		gruntJump = 12,
		gruntJumpFemale = 13,
		gruntJumpLand = 14,
		gruntJumpLandFemale = 15,
		gruntTrip = 16,
		gruntTripFemale = 17,
		magnet = 18,
		monkeyRoar = 19,
		monkeys = 20,
		recharged = 21,
		scoreBlast = 22,
		scream = 23,
		screamFemale = 24,
		shimmer = 25,
		sizzle = 26,
		slide = 27,
		splash = 28,
		splat = 29,
		splatFemale = 30,
		swish = 31,
		woohoo = 32,
		woohooFemale = 33,
		wooosh = 34,
		MAX = 35
	}

	public AudioSource Music;

	public AudioSource SoundEffects;

	public AudioSource FootSteps;

	public AudioSource Boost;

	public AudioSource Coins;

	public AudioSource Magnet;

	public AudioSource Invincible;

	public AudioClip MainMenuMusic;

	public AudioClip GameMusic;

	public GameController Controller;

	public CharacterPlayer GamePlayer;

	public AudioClip FootStepsTemple;

	public AudioClip FootStepsCliff;

	public AudioClip FootStepsPlank;

	public UISlider SoundVolumeSlider;

	public UISlider MusicVolumeSlider;

	public static AudioManager Instance;

	private float AllowScreamIn;

	private List<AudioClip> StartedThisFrame = new List<AudioClip>();

	private float _MusicVolume = 0.5f;

	private float _SoundVolume = 0.75f;

	private Dictionary<Effects, AudioClip> Sounds = new Dictionary<Effects, AudioClip>();

	private float TimeSinceLastCoin;

	private int CoinSequenceID;

	public float MusicVolume
	{
		get
		{
			return _MusicVolume;
		}
		set
		{
			_MusicVolume = value;
			PlayerPrefs.SetFloat("TR Music Volume", value);
			Music.volume = value;
		}
	}

	public float SoundVolume
	{
		get
		{
			return _SoundVolume;
		}
		set
		{
			_SoundVolume = value;
			PlayerPrefs.SetFloat("TR Sound Volume", value);
			SoundEffects.volume = value;
			FootSteps.volume = value;
			Boost.volume = value;
			Coins.volume = value;
			Magnet.volume = value;
			Invincible.volume = value;
		}
	}

	public void OnSoundVolumeSlider(float v)
	{
		string functionName = SoundVolumeSlider.functionName;
		SoundVolumeSlider.functionName = string.Empty;
		SoundVolume = v;
		SoundVolumeSlider.functionName = functionName;
	}

	public void OnMusicVolumeSlider(float v)
	{
		string functionName = MusicVolumeSlider.functionName;
		MusicVolumeSlider.functionName = string.Empty;
		MusicVolume = v;
		MusicVolumeSlider.functionName = functionName;
	}

	private void Start()
	{
		Instance = this;
		PreloadSounds();
	}

	private void Awake()
	{
		SoundVolume = PlayerPrefs.GetFloat("TR Sound Volume", 0.75f);
		MusicVolume = PlayerPrefs.GetFloat("TR Music Volume", 0.5f);
		SoundVolumeSlider.rawValue = _SoundVolume;
		MusicVolumeSlider.rawValue = _MusicVolume;
	}

	private void PreloadSounds()
	{
		for (int i = 0; i < 35; i++)
		{
			Effects effects = (Effects)i;
			string text = "Sound/" + effects;
			AudioClip audioClip = Resources.Load(text) as AudioClip;
			if (audioClip == null)
			{
				Debug.Log("Could not load sound: " + text);
				Debug.Break();
			}
			Sounds.Add(effects, audioClip);
		}
	}

	public void StartMainMenuMusic()
	{
		Music.Stop();
		Music.clip = MainMenuMusic;
		Music.Play();
	}

	public void StartGameMusic()
	{
		Music.Stop();
		Music.clip = GameMusic;
		Music.Play();
	}

	public void StopMusic()
	{
		Music.Stop();
	}

	public void PlayFX(Effects effect, float volumeScale = 1f)
	{
		if (GamePlayer.ActiveCharacterId == 1 || GamePlayer.ActiveCharacterId == 3)
		{
			if (effect == Effects.gruntTrip)
			{
				effect = Effects.gruntTripFemale;
			}
			if (effect == Effects.gruntJump)
			{
				effect = Effects.gruntJumpFemale;
			}
			if (effect == Effects.gruntJumpLand)
			{
				effect = Effects.gruntJumpLandFemale;
			}
			if (effect == Effects.splat)
			{
				effect = Effects.splatFemale;
			}
			if (effect == Effects.scream)
			{
				effect = Effects.screamFemale;
			}
			if (effect == Effects.woohoo)
			{
				effect = Effects.woohooFemale;
			}
		}
		AudioClip audioClip = Sounds[effect];
		if (!audioClip)
		{
			return;
		}
		if (effect == Effects.scream || effect == Effects.screamFemale)
		{
			if (AllowScreamIn > 0f)
			{
				return;
			}
			AllowScreamIn = audioClip.length;
		}
		if (!StartedThisFrame.Contains(audioClip))
		{
			StartedThisFrame.Add(audioClip);
			float volumeScale2 = volumeScale * SoundVolume;
			SoundEffects.PlayOneShot(audioClip, volumeScale2);
		}
	}

	public void StopFX()
	{
		Debug.Log("StopFX Sounds");
		SoundEffects.Stop();
	}

	public void PlayCoin()
	{
		if (TimeSinceLastCoin > 0.25f)
		{
			CoinSequenceID = 0;
		}
		else
		{
			CoinSequenceID++;
		}
		TimeSinceLastCoin = 0f;
		float pitch = (float)CoinSequenceID / 10f + 0.1f;
		Coins.pitch = pitch;
		Coins.PlayOneShot(Coins.clip);
	}

	private void Update()
	{
		bool flag = Controller.IsPaused || Controller.IsGameOver || GamePlayer.Hold || GamePlayer.IsDead || Controller.IsInCountdown;
		bool flag2 = true;
		if (flag || GamePlayer.IsJumping || GamePlayer.IsSliding || !GamePlayer.IsOverGround)
		{
			flag2 = false;
		}
		if (!flag2)
		{
			FootSteps.Stop();
		}
		else
		{
			AudioClip audioClip = FootStepsTemple;
			float num = 0.15f;
			if (Controller.LastLevel == PathElement.PathLevel.kPathLevelPlank)
			{
				audioClip = FootStepsPlank;
				num = 0.1f;
			}
			else if (Controller.LastLevel == PathElement.PathLevel.kPathLevelCliff)
			{
				audioClip = FootStepsCliff;
				num = 0.3f;
			}
			if (FootSteps.clip != audioClip)
			{
				FootSteps.clip = audioClip;
			}
			FootSteps.volume = num * SoundVolume;
			FootSteps.pitch = ((!GamePlayer.HasBoost) ? 1.25f : 2f);
			if (!FootSteps.isPlaying)
			{
				FootSteps.Play();
			}
		}
		bool flag3 = GamePlayer.HasVacuum && !flag;
		if (Magnet.isPlaying != flag3)
		{
			if (flag3)
			{
				Magnet.Play();
			}
			else
			{
				Magnet.Stop();
			}
		}
		bool flag4 = GamePlayer.HasInvincibility && !flag;
		if (Invincible.isPlaying != flag4)
		{
			if (flag4)
			{
				Invincible.Play();
			}
			else
			{
				Invincible.Stop();
			}
		}
		if (GamePlayer.BoostDistanceLeft < GamePlayer.BoostSlowdownDistance)
		{
			float num2 = 0.3f * (GamePlayer.BoostDistanceLeft / GamePlayer.BoostSlowdownDistance) + 0.1f;
			float alpha = BoostEffect.Instance.GetAlpha();
			Boost.volume = ((!(alpha < 0.2f)) ? num2 : 0f) * SoundVolume;
		}
		else
		{
			Boost.volume = 0.4f * SoundVolume;
		}
		bool flag5 = (GamePlayer.HasBoost || GamePlayer.IsMegaBoost) && !flag;
		if (Boost.isPlaying != flag5)
		{
			if (flag5)
			{
				Debug.Log("BOOST PLAY");
				Boost.Play();
			}
			else
			{
				Debug.Log("BOOST STOP");
				Boost.Stop();
			}
		}
		StartedThisFrame.Clear();
		if (AllowScreamIn > 0f)
		{
			AllowScreamIn -= Time.smoothDeltaTime;
		}
		TimeSinceLastCoin += Time.smoothDeltaTime;
	}
}
