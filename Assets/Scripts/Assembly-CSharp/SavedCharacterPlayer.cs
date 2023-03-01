using System;
using UnityEngine;

[Serializable]
public class SavedCharacterPlayer : SavedCommonPlayer
{
	public bool IsDead;

	public float TimeSinceDeath;

	public CharacterPlayer.DeathTypes DeathType;

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

	public float BoostSlowdownDistance;

	public float TimeSinceLastPowerup;

	public int ActiveCharacterId;

	public float BaseAnimationFPS;

	public float PlayerXOffset;

	public bool Hold;

	public SavedCharacterPlayer()
	{
	}

	public SavedCharacterPlayer(CharacterPlayer p)
		: base(p)
	{
		IsDead = p.IsDead;
		TimeSinceDeath = p.TimeSinceDeath;
		DeathType = p.DeathType;
		StartRunVelocity = p.StartRunVelocity;
		DeathRunVelocity = p.DeathRunVelocity;
		MaxRunVelocity = p.MaxRunVelocity;
		IsFalling = p.IsFalling;
		TimeSinceFallStart = p.TimeSinceFallStart;
		IsJumping = p.IsJumping;
		JumpAfterDelay = p.JumpAfterDelay;
		JumpDelay = p.JumpDelay;
		IsSliding = p.IsSliding;
		TimeSinceSlideStart = p.TimeSinceSlideStart;
		SlideDuration = p.SlideDuration;
		IsStumbling = p.IsStumbling;
		StumbleAfterDelay = p.StumbleAfterDelay;
		StumbleDelay = p.StumbleDelay;
		TimeSinceStumbleStart = p.TimeSinceStumbleStart;
		StumbleDuration = p.StumbleDuration;
		StumbleKillTimer = p.StumbleKillTimer;
		StumbleKillVelocityPercent = p.StumbleKillVelocityPercent;
		IsOnGround = p.IsOnGround;
		IsOverGround = p.IsOverGround;
		IsGroundHeightChangeHigher = p.IsGroundHeightChangeHigher;
		GroundHeight = p.GroundHeight;
		Score = p.Score;
		ScoreMultiplier = p.ScoreMultiplier;
		BonusLevel = p.BonusLevel;
		BonusLevelMax = p.BonusLevelMax;
		CoinCountTotal = p.CoinCountTotal;
		CoinCountForBonus = p.CoinCountForBonus;
		CoinCountForBonusThreshold = p.CoinCountForBonusThreshold;
		HasInvincibility = p.HasInvincibility;
		InvincibilityTimeLeft = p.InvincibilityTimeLeft;
		AngelWingsCount = p.AngelWingsCount;
		HasAngelWings = p.HasAngelWings;
		AngelWingsTimeLeft = p.AngelWingsTimeLeft;
		AngelWingsRechargeTimeLeft = p.AngelWingsRechargeTimeLeft;
		HasVacuum = p.HasVacuum;
		VacuumTimeLeft = p.VacuumTimeLeft;
		HasBoost = p.HasBoost;
		IsMegaBoost = p.IsMegaBoost;
		BoostDistanceLeft = p.BoostDistanceLeft;
		VelocityBeforeBoost = p.VelocityBeforeBoost;
		BoostSlowdownDistance = p.BoostSlowdownDistance;
		TimeSinceLastPowerup = p.TimeSinceLastPowerup;
		ActiveCharacterId = p.ActiveCharacterId;
		BaseAnimationFPS = p.BaseAnimationFPS;
		PlayerXOffset = p.PlayerXOffset;
		Hold = p.Hold;
	}

	public void Apply(CharacterPlayer p)
	{
		Apply((CommonPlayer)p);
		p.IsDead = IsDead;
		p.TimeSinceDeath = TimeSinceDeath;
		p.DeathType = DeathType;
		p.StartRunVelocity = StartRunVelocity;
		p.DeathRunVelocity = DeathRunVelocity;
		p.MaxRunVelocity = MaxRunVelocity;
		p.IsFalling = IsFalling;
		p.TimeSinceFallStart = TimeSinceFallStart;
		p.IsJumping = IsJumping;
		p.JumpAfterDelay = JumpAfterDelay;
		p.JumpDelay = JumpDelay;
		p.IsSliding = IsSliding;
		p.TimeSinceSlideStart = TimeSinceSlideStart;
		p.SlideDuration = SlideDuration;
		p.IsStumbling = IsStumbling;
		p.StumbleAfterDelay = StumbleAfterDelay;
		p.StumbleDelay = StumbleDelay;
		p.TimeSinceStumbleStart = TimeSinceStumbleStart;
		p.StumbleDuration = StumbleDuration;
		p.StumbleKillTimer = StumbleKillTimer;
		p.StumbleKillVelocityPercent = StumbleKillVelocityPercent;
		p.IsOnGround = IsOnGround;
		p.IsOverGround = IsOverGround;
		p.IsGroundHeightChangeHigher = IsGroundHeightChangeHigher;
		p.GroundHeight = GroundHeight;
		p.Score = Score;
		p.ScoreMultiplier = ScoreMultiplier;
		p.BonusLevel = BonusLevel;
		p.BonusLevelMax = BonusLevelMax;
		p.CoinCountTotal = CoinCountTotal;
		p.CoinCountForBonus = CoinCountForBonus;
		p.CoinCountForBonusThreshold = CoinCountForBonusThreshold;
		p.HasInvincibility = HasInvincibility;
		p.InvincibilityTimeLeft = InvincibilityTimeLeft;
		p.AngelWingsCount = AngelWingsCount;
		p.HasAngelWings = HasAngelWings;
		p.AngelWingsTimeLeft = AngelWingsTimeLeft;
		p.AngelWingsRechargeTimeLeft = AngelWingsRechargeTimeLeft;
		p.HasVacuum = HasVacuum;
		p.VacuumTimeLeft = VacuumTimeLeft;
		p.HasBoost = HasBoost;
		p.IsMegaBoost = IsMegaBoost;
		p.BoostDistanceLeft = BoostDistanceLeft;
		p.VelocityBeforeBoost = VelocityBeforeBoost;
		p.BoostSlowdownDistance = BoostSlowdownDistance;
		p.TimeSinceLastPowerup = TimeSinceLastPowerup;
		p.ActiveCharacterId = ActiveCharacterId;
		p.BaseAnimationFPS = BaseAnimationFPS;
		p.PlayerXOffset = PlayerXOffset;
		p.Hold = Hold;
		p.SetupCharacter();
		p.AnimateObject.Stop();
		p.CharacterModel.transform.localEulerAngles = Vector3.zero;
		if (p.IsSliding)
		{
			p.PlayAnimation("Slide Loop");
		}
		else
		{
			p.PlayAnimation("Run");
		}
		if (HasInvincibility)
		{
			p.SetInvincibilityTexture();
		}
	}
}
