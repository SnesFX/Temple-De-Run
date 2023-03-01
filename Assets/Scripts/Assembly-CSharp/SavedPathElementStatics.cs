using System;
using UnityEngine;

[Serializable]
public class SavedPathElementStatics
{
	public int sCount;

	public bool sSafetyNetEnabled;

	public bool sAllowBonusItems;

	public bool sAllowVariations;

	public bool sAllowTurns;

	public bool sAllowPathHoles;

	public bool sAllowPathNarrows;

	public bool sAllowFlameTowers;

	public bool sAllowTreeStumble;

	public bool sAllowTreeJump;

	public bool sAllowTreeSlide;

	public bool sAllowTurnAfterObstacles;

	public bool sAllowSidePaths;

	public bool sAllowCoins;

	public bool sAllowDoubleCoins;

	public bool sAllowTripleCoins;

	public bool sAllowCenterCoins;

	public bool sAllowSideCoins;

	public bool sAllowJumpCoins;

	public bool sAllowSlantCoins;

	public int sMinSpacesBetweenBonusItems;

	public float sProbabilityCoinBonus;

	public float sProbabilityInvincibility;

	public float sProbabilityVacuum;

	public float sProbabilityBoost;

	public int sMinSpaceBetweenTurns;

	public int sMaxBackToBackObstacles;

	public int sMinSpaceBetweenObstacles;

	public float sDoubleObstaclePercent;

	public int sMinSpacesBetweenCoinRuns;

	public int sMaxCoinsPerRun;

	public bool sIsFastTurnSection;

	public PathElement.PathLevel sPathLevel;

	public Color sSafteyNetColor = Color.white;

	public int MessageBoardLastDistance;

	public SavedPathElementStatics()
	{
	}

	public SavedPathElementStatics(int version)
	{
		sCount = PathElement.sCount;
		sSafetyNetEnabled = PathElement.sSafetyNetEnabled;
		sAllowBonusItems = PathElement.sAllowBonusItems;
		sAllowVariations = PathElement.sAllowVariations;
		sAllowTurns = PathElement.sAllowTurns;
		sAllowPathHoles = PathElement.sAllowPathHoles;
		sAllowPathNarrows = PathElement.sAllowPathNarrows;
		sAllowFlameTowers = PathElement.sAllowFlameTowers;
		sAllowTreeStumble = PathElement.sAllowTreeStumble;
		sAllowTreeJump = PathElement.sAllowTreeJump;
		sAllowTreeSlide = PathElement.sAllowTreeSlide;
		sAllowTurnAfterObstacles = PathElement.sAllowTurnAfterObstacles;
		sAllowSidePaths = PathElement.sAllowSidePaths;
		sAllowCoins = PathElement.sAllowCoins;
		sAllowDoubleCoins = PathElement.sAllowDoubleCoins;
		sAllowTripleCoins = PathElement.sAllowTripleCoins;
		sAllowCenterCoins = PathElement.sAllowCenterCoins;
		sAllowSideCoins = PathElement.sAllowSideCoins;
		sAllowJumpCoins = PathElement.sAllowJumpCoins;
		sAllowSlantCoins = PathElement.sAllowSlantCoins;
		sMinSpacesBetweenBonusItems = PathElement.sMinSpacesBetweenBonusItems;
		sProbabilityCoinBonus = PathElement.sProbabilityCoinBonus;
		sProbabilityInvincibility = PathElement.sProbabilityInvincibility;
		sProbabilityVacuum = PathElement.sProbabilityVacuum;
		sProbabilityBoost = PathElement.sProbabilityBoost;
		sMinSpaceBetweenTurns = PathElement.sMinSpaceBetweenTurns;
		sMaxBackToBackObstacles = PathElement.sMaxBackToBackObstacles;
		sMinSpaceBetweenObstacles = PathElement.sMinSpaceBetweenObstacles;
		sDoubleObstaclePercent = PathElement.sDoubleObstaclePercent;
		sMinSpacesBetweenCoinRuns = PathElement.sMinSpacesBetweenCoinRuns;
		sMaxCoinsPerRun = PathElement.sMaxCoinsPerRun;
		sIsFastTurnSection = PathElement.sIsFastTurnSection;
		sPathLevel = PathElement.sPathLevel;
		sSafteyNetColor = PathElement.sSafteyNetColor;
		MessageBoardLastDistance = MessageBoard.Instance.MessageBoardLastDistance;
	}

	public void Apply()
	{
		PathElement.sCount = sCount;
		PathElement.sSafetyNetEnabled = sSafetyNetEnabled;
		PathElement.sAllowBonusItems = sAllowBonusItems;
		PathElement.sAllowVariations = sAllowVariations;
		PathElement.sAllowTurns = sAllowTurns;
		PathElement.sAllowPathHoles = sAllowPathHoles;
		PathElement.sAllowPathNarrows = sAllowPathNarrows;
		PathElement.sAllowFlameTowers = sAllowFlameTowers;
		PathElement.sAllowTreeStumble = sAllowTreeStumble;
		PathElement.sAllowTreeJump = sAllowTreeJump;
		PathElement.sAllowTreeSlide = sAllowTreeSlide;
		PathElement.sAllowTurnAfterObstacles = sAllowTurnAfterObstacles;
		PathElement.sAllowSidePaths = sAllowSidePaths;
		PathElement.sAllowCoins = sAllowCoins;
		PathElement.sAllowDoubleCoins = sAllowDoubleCoins;
		PathElement.sAllowTripleCoins = sAllowTripleCoins;
		PathElement.sAllowCenterCoins = sAllowCenterCoins;
		PathElement.sAllowSideCoins = sAllowSideCoins;
		PathElement.sAllowJumpCoins = sAllowJumpCoins;
		PathElement.sAllowSlantCoins = sAllowSlantCoins;
		PathElement.sMinSpacesBetweenBonusItems = sMinSpacesBetweenBonusItems;
		PathElement.sProbabilityCoinBonus = sProbabilityCoinBonus;
		PathElement.sProbabilityInvincibility = sProbabilityInvincibility;
		PathElement.sProbabilityVacuum = sProbabilityVacuum;
		PathElement.sProbabilityBoost = sProbabilityBoost;
		PathElement.sMinSpaceBetweenTurns = sMinSpaceBetweenTurns;
		PathElement.sMaxBackToBackObstacles = sMaxBackToBackObstacles;
		PathElement.sMinSpaceBetweenObstacles = sMinSpaceBetweenObstacles;
		PathElement.sDoubleObstaclePercent = sDoubleObstaclePercent;
		PathElement.sMinSpacesBetweenCoinRuns = sMinSpacesBetweenCoinRuns;
		PathElement.sMaxCoinsPerRun = sMaxCoinsPerRun;
		PathElement.sIsFastTurnSection = sIsFastTurnSection;
		PathElement.sPathLevel = sPathLevel;
		PathElement.sSafteyNetColor = sSafteyNetColor;
		MessageBoard.Instance.MessageBoardLastDistance = MessageBoardLastDistance;
	}
}
