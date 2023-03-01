using System;

[Serializable]
public class SavedGameController
{
	public SavedPathElement PathRoot;

	public PathElement.PathDirection PathRootOrigin;

	public string LastTurnPathPiece;

	public int TutorialID;

	public string NearestElement;

	public string LastNearestElement;

	public PathElement.PathLevel LastLevel;

	public float TimeSinceGameStart;

	public float TimeSinceLastPause;

	public float TimeSinceGameEnd;

	public bool IsPaused;

	public bool IsGameStarted;

	public bool IsGameOver;

	public bool IsGameOverFinished;

	public bool IsResurrecting;

	public float TimeSinceResurrectStart;

	public int ResurrectionCount;

	public bool UsedHeadStart;

	public float MaxDistanceWithoutCoins;

	public bool IsNewHighScore;

	public int HighScore;

	public bool IsTutorialMode;

	public bool ShowTutorialGuides;

	public bool IsIntroScene;

	public float TutorialTextExtraDuration;

	public float TimeSinceTutorialSectionEnded;

	public bool AutoRestart;

	public float EndGameDelayTime;

	public float DistanceRemainder;

	public float DistanceTraveled;

	public float DistanceToChangeAtTemple;

	public float DistanceToChangeAtOther;

	public float DistanceTraveledSinceLastLevelChange;

	public float DistanceToTurnSection;

	public float DistanceToTurnSectionEnd;

	public float DistanceTraveledSinceLastTurnSection;

	public float DistanceToChangeDoubleCoins;

	public float DistanceToChangeTripleCoins;

	public float AverageScorePerBlock;

	public int LastBlockScore;

	public int CoinMagnetMultiplier;

	public int CoinBonusValue;

	public float VacuumDuration;

	public float InvincibilityDuration;

	public float BoostDistance;

	public bool DidStumble;

	public bool UsedPowers;

	public float HeadStartStartDistance;

	public float HeadStartEndDistance;

	public float HeadStartBoostDistance;

	public float HeadStartMegaBoostDistance;

	public bool HandelingEndGame;

	public SavedGameController()
	{
	}

	public SavedGameController(GameController gc)
	{
		PathRoot = new SavedPathElement(gc.PathRoot);
		PathRootOrigin = gc.PathRootOrigin;
		LastTurnPathPiece = ((!(gc.LastTurnPathPiece != null)) ? string.Empty : gc.LastTurnPathPiece.name);
		TutorialID = gc.TutorialID;
		NearestElement = ((!(gc.NearestElement != null)) ? string.Empty : gc.NearestElement.name);
		LastNearestElement = ((!(gc.LastNearestElement != null)) ? string.Empty : gc.LastNearestElement.name);
		TimeSinceGameStart = gc.TimeSinceGameStart;
		TimeSinceLastPause = gc.TimeSinceLastPause;
		TimeSinceGameEnd = gc.TimeSinceGameEnd;
		IsPaused = gc.IsPaused;
		IsGameStarted = gc.IsGameStarted;
		IsGameOver = gc.IsGameOver;
		IsGameOverFinished = gc.IsGameOverFinished;
		IsResurrecting = gc.IsResurrecting;
		TimeSinceResurrectStart = gc.TimeSinceResurrectStart;
		ResurrectionCount = gc.ResurrectionCount;
		UsedHeadStart = gc.UsedHeadStart;
		MaxDistanceWithoutCoins = gc.MaxDistanceWithoutCoins;
		IsNewHighScore = gc.IsNewHighScore;
		HighScore = gc.HighScore;
		IsTutorialMode = gc.IsTutorialMode;
		ShowTutorialGuides = gc.ShowTutorialGuides;
		IsIntroScene = gc.IsIntroScene;
		TutorialTextExtraDuration = gc.TutorialTextExtraDuration;
		TimeSinceTutorialSectionEnded = gc.TimeSinceTutorialSectionEnded;
		AutoRestart = gc.AutoRestart;
		EndGameDelayTime = gc.EndGameDelayTime;
		DistanceRemainder = gc.DistanceRemainder;
		DistanceTraveled = gc.DistanceTraveled;
		DistanceToChangeAtTemple = gc.DistanceToChangeAtTemple;
		DistanceToChangeAtOther = gc.DistanceToChangeAtOther;
		DistanceTraveledSinceLastLevelChange = gc.DistanceTraveledSinceLastLevelChange;
		DistanceToTurnSection = gc.DistanceToTurnSection;
		DistanceToTurnSectionEnd = gc.DistanceToTurnSectionEnd;
		DistanceTraveledSinceLastTurnSection = gc.DistanceTraveledSinceLastTurnSection;
		DistanceToChangeDoubleCoins = gc.DistanceToChangeDoubleCoins;
		DistanceToChangeTripleCoins = gc.DistanceToChangeTripleCoins;
		AverageScorePerBlock = gc.AverageScorePerBlock;
		LastBlockScore = gc.LastBlockScore;
		CoinMagnetMultiplier = gc.CoinMagnetMultiplier;
		CoinBonusValue = gc.CoinBonusValue;
		VacuumDuration = gc.VacuumDuration;
		InvincibilityDuration = gc.InvincibilityDuration;
		BoostDistance = gc.BoostDistance;
		DidStumble = gc.DidStumble;
		UsedPowers = gc.UsedPowers;
		HeadStartStartDistance = gc.HeadStartStartDistance;
		HeadStartEndDistance = gc.HeadStartEndDistance;
		HeadStartBoostDistance = gc.HeadStartBoostDistance;
		HeadStartMegaBoostDistance = gc.HeadStartMegaBoostDistance;
		HandelingEndGame = gc.HandelingEndGame;
	}

	public void Apply(GameController gc)
	{
		gc.DeleteAnyExistingPath();
		LastTurnPathPiece = null;
		gc.PathRoot = PathRoot.Apply(this);
		gc.PathRootOrigin = PathRootOrigin;
		gc.TutorialID = TutorialID;
		gc.TimeSinceGameStart = TimeSinceGameStart;
		gc.TimeSinceLastPause = TimeSinceLastPause;
		gc.TimeSinceGameEnd = TimeSinceGameEnd;
		gc.IsPaused = IsPaused;
		gc.IsGameStarted = IsGameStarted;
		gc.IsGameOver = IsGameOver;
		gc.IsGameOverFinished = IsGameOverFinished;
		gc.IsResurrecting = IsResurrecting;
		gc.TimeSinceResurrectStart = TimeSinceResurrectStart;
		gc.ResurrectionCount = ResurrectionCount;
		gc.UsedHeadStart = UsedHeadStart;
		gc.MaxDistanceWithoutCoins = MaxDistanceWithoutCoins;
		gc.IsNewHighScore = IsNewHighScore;
		gc.HighScore = HighScore;
		gc.IsTutorialMode = IsTutorialMode;
		gc.ShowTutorialGuides = ShowTutorialGuides;
		gc.IsIntroScene = IsIntroScene;
		gc.TutorialTextExtraDuration = TutorialTextExtraDuration;
		gc.TimeSinceTutorialSectionEnded = TimeSinceTutorialSectionEnded;
		gc.AutoRestart = AutoRestart;
		gc.EndGameDelayTime = EndGameDelayTime;
		gc.DistanceRemainder = DistanceRemainder;
		gc.DistanceTraveled = DistanceTraveled;
		gc.DistanceToChangeAtTemple = DistanceToChangeAtTemple;
		gc.DistanceToChangeAtOther = DistanceToChangeAtOther;
		gc.DistanceTraveledSinceLastLevelChange = DistanceTraveledSinceLastLevelChange;
		gc.DistanceToTurnSection = DistanceToTurnSection;
		gc.DistanceToTurnSectionEnd = DistanceToTurnSectionEnd;
		gc.DistanceTraveledSinceLastTurnSection = DistanceTraveledSinceLastTurnSection;
		gc.DistanceToChangeDoubleCoins = DistanceToChangeDoubleCoins;
		gc.DistanceToChangeTripleCoins = DistanceToChangeTripleCoins;
		gc.AverageScorePerBlock = AverageScorePerBlock;
		gc.LastBlockScore = LastBlockScore;
		gc.CoinMagnetMultiplier = CoinMagnetMultiplier;
		gc.CoinBonusValue = CoinBonusValue;
		gc.VacuumDuration = VacuumDuration;
		gc.InvincibilityDuration = InvincibilityDuration;
		gc.BoostDistance = BoostDistance;
		gc.DidStumble = DidStumble;
		gc.UsedPowers = UsedPowers;
		gc.HeadStartStartDistance = HeadStartStartDistance;
		gc.HeadStartEndDistance = HeadStartEndDistance;
		gc.HeadStartBoostDistance = HeadStartBoostDistance;
		gc.HeadStartMegaBoostDistance = HeadStartMegaBoostDistance;
		gc.HandelingEndGame = HandelingEndGame;
		gc.SkipFrame = true;
	}

	public void ProcessPathElement(PathElement element)
	{
		if (element.name == LastTurnPathPiece)
		{
			GameController.SharedInstance.LastTurnPathPiece = element;
		}
		if (element.name == NearestElement)
		{
			GameController.SharedInstance.NearestElement = element;
		}
		if (element.name == LastNearestElement)
		{
			GameController.SharedInstance.LastNearestElement = element;
		}
	}
}
