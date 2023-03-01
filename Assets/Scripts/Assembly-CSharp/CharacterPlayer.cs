using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayer : CommonPlayer
{
	public enum DeathTypes
	{
		kDeathTypeUnknown = 0,
		kDeathTypeFall = 1,
		kDeathTypeTree = 2,
		kDeathTypeSlide = 3,
		kDeathTypeLedge = 4,
		kDeathTypeBurnt = 5,
		kDeathTypeEaten = 6,
		kDeathTypeTangle = 7,
		kDeathTypeSceneryTree = 8,
		kDeathTypeSceneryRock = 9
	}

	public const float kPlayerBoostSlowdownDistance = 50f;

	public Texture InvincibilityTexture;

	public Animation AnimateObject;

	public GameObject CharacterModel;

	public Renderer RenderObject;

	public Transform ShadowSpot;

	public UISprite ShadowSPrite;

	public UISprite DustSprite;

	public bool IsDead;

	public float TimeSinceDeath;

	public DeathTypes DeathType;

	public float StartRunVelocity;

	public float DeathRunVelocity;

	public float MaxRunVelocity;

	public bool IsFalling;

	public float TimeSinceFallStart;

	public bool IsJumping;

	public bool JumpAfterDelay;

	public float JumpDelay;

	public bool IsSliding;

	public float TimeSinceSlideStart;

	public float SlideDuration;

	public bool IsStumbling;

	public bool StumbleAfterDelay;

	public float StumbleDelay;

	public float TimeSinceStumbleStart;

	public float StumbleDuration;

	public float StumbleKillTimer;

	public float StumbleKillVelocityPercent;

	public bool IsOnGround;

	public bool IsOverGround;

	public bool IsGroundHeightChangeHigher;

	public float GroundHeight;

	public int Score;

	public int ScoreMultiplier;

	public int BonusLevel;

	public int BonusLevelMax;

	public int CoinCountTotal;

	public int CoinCountForBonus;

	public int CoinCountForBonusThreshold;

	public bool HasInvincibility;

	public float InvincibilityTimeLeft;

	public int AngelWingsCount;

	public bool HasAngelWings;

	public float AngelWingsTimeLeft;

	public float AngelWingsRechargeTimeLeft;

	public bool HasVacuum;

	public float VacuumTimeLeft;

	public bool HasBoost;

	public bool IsMegaBoost;

	public float BoostDistanceLeft;

	public float VelocityBeforeBoost;

	public float BoostSlowdownDistance = 50f;

	public float TimeSinceLastPowerup;

	public int ActiveCharacterId = 1;

	public float BaseAnimationFPS;

	public float PlayerXOffset;

	public GameGUI GameInterface;

	public EndGameUI GameOverInterface;

	public UISprite PickupEffect;

	public bool Hold;

	public AudioManager Audio;

	public List<GameObject> Characters = new List<GameObject>();

	public List<Texture2D> Textures = new List<Texture2D>();

	public static CharacterPlayer Instance;

	public Color debugColor = new Color(1f, 1f, 1f, 1f);

	public bool ColorDebugging;

	private void Start()
	{
		BaseAnimationFPS = 24f;
		StartRunVelocity = 100f;
		DeathRunVelocity = 0f;
		MaxRunVelocity = 210f;
		StumbleKillTimer = 7f;
		StumbleKillVelocityPercent = 0.65f;
		BonusLevel = 1;
		CoinCountTotal = 0;
		CoinCountForBonus = 0;
		CoinCountForBonusThreshold = 100;
		HasVacuum = false;
		IsGroundHeightChangeHigher = false;
		HasAngelWings = false;
		HasBoost = false;
		TimeSinceLastPowerup = 0f;
		HasInvincibility = false;
		Instance = this;
		Reset();
	}

	public void SetAlpha(float a)
	{
		if (ColorDebugging)
		{
			RenderObject.material.color = new Color(debugColor.r, debugColor.g, debugColor.b, a);
		}
		else
		{
			RenderObject.material.color = new Color(1f, 1f, 1f, a);
		}
	}

	public float GetAlpha()
	{
		return RenderObject.material.color.a;
	}

	public override void Update()
	{
		if (IsDead || Hold || GameController.SharedInstance.IsResurrecting || GameController.SharedInstance.IsInCountdown || GameController.SharedInstance.IsPaused)
		{
			return;
		}
		if (StumbleAfterDelay)
		{
			StumbleDelay -= Time.smoothDeltaTime;
			if (StumbleDelay <= 0f)
			{
				Stumble(0f);
			}
		}
		if (JumpAfterDelay)
		{
			JumpDelay -= Time.smoothDeltaTime;
			if (JumpDelay <= 0f)
			{
				Jump(0f);
			}
		}
		float num = 1f;
		if (HasInvincibility)
		{
			num = 0.5f;
			if (!HasBoost || (HasBoost && BoostDistanceLeft < 50f))
			{
				InvincibilityTimeLeft -= Time.smoothDeltaTime;
				num = GetAlpha();
				if (InvincibilityTimeLeft <= 2.5f)
				{
					num -= Time.smoothDeltaTime * 2f;
					if (num <= 0f)
					{
						num = 0.5f;
					}
				}
				else
				{
					num = 0.5f;
				}
			}
			Vector2 mainTextureOffset = RenderObject.material.mainTextureOffset;
			mainTextureOffset.x += 2f * Time.smoothDeltaTime;
			mainTextureOffset.y += 2f * Time.smoothDeltaTime;
			RenderObject.material.mainTextureOffset = mainTextureOffset;
			if (InvincibilityTimeLeft <= 0f)
			{
				Debug.Log("Invincibility Over");
				HasInvincibility = false;
				ApplyPlayerTexture();
				if (HasBoost)
				{
					HasBoost = false;
					IsMegaBoost = false;
				}
			}
		}
		SetAlpha(num);
		if (HasAngelWings)
		{
			AngelWingsTimeLeft -= Time.smoothDeltaTime;
			if (AngelWingsTimeLeft <= 0f)
			{
				HasAngelWings = false;
			}
		}
		if (AngelWingsRechargeTimeLeft > 0f)
		{
			AngelWingsRechargeTimeLeft -= Time.smoothDeltaTime;
			if (AngelWingsRechargeTimeLeft <= 0f)
			{
				AngelWingsRechargeTimeLeft = 0f;
				if (AngelWingsCount > 0)
				{
					Audio.PlayFX(AudioManager.Effects.recharged);
				}
			}
		}
		if (HasVacuum)
		{
			VacuumTimeLeft -= Time.smoothDeltaTime;
			if (VacuumTimeLeft <= 0f)
			{
				HasVacuum = false;
			}
		}
		TimeSinceLastPowerup += Time.smoothDeltaTime;
		ApplyForce(Vector3.down * 250f);
		base.Update();
		bool isOnGround = IsOnGround;
		float num2 = base.transform.position.y;
		if (IsOverGround)
		{
			if ((!IsFalling && num2 <= GroundHeight) || (IsFalling && num2 <= GroundHeight && num2 >= GroundHeight - 6f))
			{
				if (IsJumping)
				{
					Audio.PlayFX(AudioManager.Effects.gruntJumpLand);
					DustSprite.enabled = true;
					IsJumping = false;
				}
				if (IsFalling && num2 < GroundHeight - 2f && !HasInvincibility)
				{
					Stumble(0f);
				}
				if (IsGroundHeightChangeHigher && num2 >= GroundHeight - 6f && num2 < GroundHeight - 2f && !HasInvincibility)
				{
					Stumble(0f);
				}
				base.transform.position = new Vector3(base.transform.position.x, GroundHeight, base.transform.position.z);
				num2 = GroundHeight;
				Velocity.y = 0f;
				IsOnGround = true;
				IsGroundHeightChangeHigher = false;
			}
			else
			{
				IsOnGround = false;
			}
		}
		else
		{
			IsOnGround = false;
		}
		ShadowSPrite.enabled = IsOverGround;
		if (IsOverGround)
		{
			ShadowSpot.position = new Vector3(ShadowSpot.position.x, GroundHeight, ShadowSpot.position.z);
		}
		if (num2 < GroundHeight)
		{
			TimeSinceFallStart += Time.smoothDeltaTime;
			if (!IsFalling)
			{
				TimeSinceFallStart = 0f;
				IsFalling = true;
			}
			if (TimeSinceFallStart > 0.25f)
			{
				Audio.PlayFX(AudioManager.Effects.scream);
			}
		}
		else
		{
			if (IsFalling)
			{
			}
			IsFalling = false;
		}
		if (IsSliding)
		{
			TimeSinceSlideStart += Time.smoothDeltaTime;
			DustSprite.enabled = true;
			if (TimeSinceSlideStart > SlideDuration)
			{
				IsSliding = false;
				Audio.PlayFX(AudioManager.Effects.slide);
				PlayAnimation("Run");
			}
		}
		if (IsOnGround && !isOnGround && !IsStumbling && !IsSliding)
		{
			PlayAnimation("Run");
		}
		if (IsStumbling)
		{
			TimeSinceStumbleStart += Time.smoothDeltaTime;
			if (TimeSinceStumbleStart > StumbleDuration)
			{
				IsStumbling = false;
			}
		}
		StumbleKillTimer += Time.smoothDeltaTime;
		if (num2 < -35f)
		{
			Kill(DeathTypes.kDeathTypeFall);
		}
		AdjustAnimationSpeed();
	}

	public void SetGroundHeight(float groundHeight)
	{
		if (groundHeight > GroundHeight)
		{
			IsGroundHeightChangeHigher = true;
		}
		GroundHeight = groundHeight;
	}

	public float GetRunVelocity()
	{
		return new Vector2(Velocity.x, Velocity.z).magnitude;
	}

	public float GetMaxRunVelocity()
	{
		if (HasBoost)
		{
			return (!IsMegaBoost) ? (MaxRunVelocity * 2f) : (MaxRunVelocity * 2.5f);
		}
		return MaxRunVelocity;
	}

	public override void Reset()
	{
		Debug.Log("CharacterPlayer Reset");
		SetupCharacter();
		Hold = true;
		PlayerXOffset = 0f;
		IsDead = false;
		TimeSinceDeath = 0f;
		DeathType = DeathTypes.kDeathTypeUnknown;
		IsFalling = false;
		IsJumping = false;
		JumpAfterDelay = false;
		IsSliding = false;
		TimeSinceSlideStart = 0f;
		IsOnGround = false;
		IsOverGround = false;
		IsStumbling = false;
		StumbleAfterDelay = false;
		Score = 0;
		ScoreMultiplier = RecordManager.Instance.GetScoreMulitplier(PlayerManager.Instance.GetActivePlayer());
		BonusLevel = 1;
		CoinCountForBonus = 0;
		CoinCountTotal = 0;
		IsGroundHeightChangeHigher = false;
		GroundHeight = 0f;
		HasInvincibility = false;
		InvincibilityTimeLeft = 0f;
		AngelWingsCount = 0;
		HasAngelWings = false;
		AngelWingsTimeLeft = 0f;
		AngelWingsRechargeTimeLeft = 0f;
		HasVacuum = false;
		VacuumTimeLeft = 0f;
		HasBoost = false;
		BoostDistanceLeft = 0f;
		VelocityBeforeBoost = 0f;
		TimeSinceLastPowerup = 0f;
		StumbleKillTimer = 7f;
		DeathRunVelocity = 0f;
		Velocity = base.transform.forward * StartRunVelocity;
		ApplyPlayerTexture();
		GameInterface.TotemUI.SetBonusLevel(1, false);
		PreviousLocation = base.transform.position;
	}

	public Vector2 GetPreviousPosition2D()
	{
		return new Vector2(PreviousLocation.x, PreviousLocation.z);
	}

	public void Stumble(float delay = 0f)
	{
		Debug.Log("STUMBLE called.  Delay: " + delay + "  kill timer: " + StumbleKillTimer);
		if (delay > 0f)
		{
			StumbleAfterDelay = true;
			StumbleDelay = delay;
			return;
		}
		StumbleAfterDelay = false;
		if (StumbleKillTimer < 10f)
		{
			Kill(DeathTypes.kDeathTypeEaten);
			Audio.PlayFX(AudioManager.Effects.monkeyRoar, 0.8f);
			return;
		}
		Velocity *= 0.85f;
		IsStumbling = true;
		TimeSinceStumbleStart = 0f;
		StumbleDuration = GetRunVelocity() * 0.005f;
		BonusLevel = 1;
		CoinCountForBonus = 0;
		if (StumbleDuration > 0.5f)
		{
			StumbleDuration = 0.5f;
		}
		StumbleKillTimer = 0f;
		Audio.PlayFX(AudioManager.Effects.gruntTrip);
		Audio.PlayFX(AudioManager.Effects.monkeys);
		Debug.Log("Stumble Animation");
		PlayAnimation("Stumble");
		PlayAnimation("Run", true);
	}

	public bool Jump(float delay = 0f)
	{
		if (IsJumping)
		{
			Debug.Log("CANT JUMP: Already Jumping");
		}
		if (IsStumbling)
		{
			Debug.Log("CANT JUMP: Stumbling");
		}
		if (IsFalling)
		{
			Debug.Log("CANT JUMP: Falling");
		}
		if (!IsJumping && !IsStumbling && !IsFalling)
		{
			if (delay > 0f)
			{
				JumpAfterDelay = true;
				JumpDelay = delay;
				return true;
			}
			float num = GetRunVelocity();
			if (num < 80f)
			{
				num = 80f;
			}
			else if (num > 100f)
			{
				num = 100f;
			}
			Audio.PlayFX(AudioManager.Effects.gruntJump);
			JumpAfterDelay = false;
			IsJumping = true;
			IsSliding = false;
			Velocity.y = num;
			PlayAnimation("Jump");
			return true;
		}
		return false;
	}

	public bool Slide()
	{
		if (!IsSliding && !IsJumping && !IsFalling)
		{
			IsSliding = true;
			TimeSinceSlideStart = 0f;
			SlideDuration = GetRunVelocity() * 0.007f;
			if (SlideDuration > 0.75f)
			{
				SlideDuration = 0.75f;
			}
			Audio.PlayFX(AudioManager.Effects.slide);
			PlayAnimation("Slide Start");
			PlayAnimation("Slide Loop", true);
			return true;
		}
		return false;
	}

	public void Kill(DeathTypes deathType)
	{
		Debug.Log("Player Killed: " + deathType);
		IsDead = true;
		DeathType = deathType;
		DeathRunVelocity = GetRunVelocity();
		Velocity = new Vector3(0f, 0f, 0f);
		Audio.StopFX();
		switch (deathType)
		{
		case DeathTypes.kDeathTypeBurnt:
			Audio.PlayFX(AudioManager.Effects.sizzle);
			break;
		case DeathTypes.kDeathTypeFall:
			Audio.PlayFX(AudioManager.Effects.splash);
			break;
		default:
			Audio.PlayFX(AudioManager.Effects.splat);
			break;
		case DeathTypes.kDeathTypeEaten:
			break;
		}
		SetVisibility(false);
	}

	public void TurnLeft()
	{
		base.transform.Rotate(0f, -90f, 0f);
		Vector3 velocity = default(Vector3);
		velocity.x = 0f - Velocity.z;
		velocity.y = Velocity.y;
		velocity.z = Velocity.x;
		Velocity = velocity;
		ResetForce();
		if (!IsJumping)
		{
			Audio.PlayFX(AudioManager.Effects.footstepsTurn);
		}
	}

	public void TurnRight()
	{
		base.transform.Rotate(0f, 90f, 0f);
		Vector3 velocity = default(Vector3);
		velocity.x = Velocity.z;
		velocity.y = Velocity.y;
		velocity.z = 0f - Velocity.x;
		Velocity = velocity;
		ResetForce();
		if (!IsJumping)
		{
			Audio.PlayFX(AudioManager.Effects.footstepsTurn);
		}
	}

	public void AddScore(int score)
	{
		if (score > 0)
		{
			Score += score * ScoreMultiplier;
		}
	}

	public void AddCoins(int count)
	{
		if (count <= 0)
		{
			return;
		}
		CoinCountTotal += count;
		CoinCountForBonus += count;
		if (CoinCountForBonus >= CoinCountForBonusThreshold)
		{
			AddScore(500);
			BonusLevel++;
			if (BonusLevel > BonusLevelMax)
			{
				BonusLevelMax = BonusLevel;
			}
			CoinCountForBonus -= CoinCountForBonusThreshold;
		}
	}

	public void SetInvincibilityTexture()
	{
		RenderObject.material.mainTexture = InvincibilityTexture;
		SetAlpha(0.5f);
	}

	public void StartInvcibility(float duration)
	{
		Debug.Log("PICKUP: INVINCIBILITY");
		HasInvincibility = true;
		InvincibilityTimeLeft = duration;
		TimeSinceLastPowerup = 0f;
		SetInvincibilityTexture();
		if (!HasBoost)
		{
			Audio.PlayFX(AudioManager.Effects.shimmer);
		}
	}

	public void StartAngelWings(float duration)
	{
		Debug.Log("PICKUP: ANGEL WINGS");
		AngelWingsCount--;
		HasAngelWings = true;
		AngelWingsTimeLeft = duration;
		AngelWingsRechargeTimeLeft = 45f;
	}

	public void StartVacuum(float duration)
	{
		Debug.Log("PICKUP: MAGNET");
		HasVacuum = true;
		VacuumTimeLeft = duration;
		TimeSinceLastPowerup = 0f;
	}

	public void StartCoinBonus(int value)
	{
		Debug.Log("PICKUP: COIN BONUS");
		AddCoins(value);
		TimeSinceLastPowerup = 0f;
	}

	public void StartBoost(float distance, bool isMega)
	{
		Debug.Log("PICKUP: BOOST of " + distance);
		bool hasBoost = HasBoost;
		HasBoost = true;
		IsMegaBoost = isMega;
		if (!hasBoost)
		{
			VelocityBeforeBoost = ((!IsMegaBoost) ? GetRunVelocity() : MaxRunVelocity);
			StartInvcibility(4f);
		}
		BoostDistanceLeft = distance + 50f;
		TimeSinceLastPowerup = 0f;
		StumbleKillTimer += 10f;
	}

	public void StopAllAnimation()
	{
		AnimateObject.Stop();
	}

	public void Resurrect()
	{
		Debug.Log("TODO Ressurect!", this);
		PlayAnimation("Run");
		SetY(GroundHeight);
		SetAlpha(1f);
		ApplyPlayerTexture();
		IsDead = false;
		TimeSinceDeath = 0f;
		DeathType = DeathTypes.kDeathTypeUnknown;
		IsFalling = false;
		IsJumping = false;
		JumpAfterDelay = false;
		IsSliding = false;
		TimeSinceSlideStart = 0f;
		IsOnGround = true;
		IsStumbling = false;
		StumbleAfterDelay = false;
		IsGroundHeightChangeHigher = false;
		HasInvincibility = false;
		InvincibilityTimeLeft = 0f;
		HasAngelWings = false;
		HasVacuum = false;
		VacuumTimeLeft = 0f;
		HasBoost = false;
		BoostDistanceLeft = 0f;
		VelocityBeforeBoost = 0f;
		StumbleKillTimer = 10f;
		BonusLevel = 1;
		CoinCountForBonus = 0;
	}

	public override Bounds GetWorldSpaceBounds()
	{
		Vector3 position = base.transform.position;
		Vector3 min = new Vector3(position.x - 1.7469001f, position.y - 0.16485f, position.z - 1.7469001f);
		Vector3 max = new Vector3(position.x + 1.7469001f, position.y + 9.58665f, position.z + 1.7469001f);
		Bounds result = default(Bounds);
		result.SetMinMax(min, max);
		return result;
	}

	public void ApplyPlayerTexture()
	{
		RenderObject.material.mainTexture = Textures[ActiveCharacterId];
		RenderObject.material.mainTextureOffset = new Vector2(0f, 0f);
		SetAlpha(1f);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Bounds worldSpaceBounds = GetWorldSpaceBounds();
		Gizmos.DrawWireCube(worldSpaceBounds.center, worldSpaceBounds.size);
	}

	public override void SetVisibility(bool visible)
	{
		RenderObject.enabled = visible;
	}

	public void SetupCharacter()
	{
		ActiveCharacterId = RecordManager.Instance.GetActiveCharacter(PlayerManager.Instance.GetActivePlayer());
		if (CharacterModel != null)
		{
			Object.Destroy(CharacterModel);
			CharacterModel = null;
			AnimateObject = null;
			RenderObject = null;
		}
		if (ActiveCharacterId > Characters.Count)
		{
			Debug.LogError("Problem, character ID was out of range!");
			ActiveCharacterId = 0;
		}
		GameObject gameObject = Characters[ActiveCharacterId];
		if (gameObject != null)
		{
			GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.localPosition = new Vector3(0f, 1f, 0f);
			CharacterModel = gameObject2;
			AnimateObject = gameObject2.GetComponentInChildren<Animation>();
			RenderObject = gameObject2.GetComponentInChildren<SkinnedMeshRenderer>();
		}
	}

	public void PlayAnimation(string anim, bool queued = false)
	{
		if (queued)
		{
			AnimateObject.PlayQueued(anim);
		}
		else
		{
			AnimateObject.Play(anim);
		}
	}

	public void AdjustAnimationSpeed()
	{
		float num = Velocity.magnitude / StartRunVelocity;
		foreach (AnimationState item in AnimateObject.GetComponent<Animation>())
		{
			item.speed = 0.65f * num;
		}
	}

	public void PauseAnimation()
	{
		foreach (AnimationState item in AnimateObject.GetComponent<Animation>())
		{
			item.speed = 0f;
		}
	}

	public void UnpauseAnimation()
	{
		AdjustAnimationSpeed();
	}
}
