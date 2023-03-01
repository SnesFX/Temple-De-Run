using UnityEngine;

public class PathElement : DynamicElement
{
	public enum PathType
	{
		kPathNone = 0,
		kPathStraightVert = 1,
		kPathStraightHorz = 2,
		kPathTurnA = 3,
		kPathTurnB = 4,
		kPathTurnC = 5,
		kPathTurnD = 6,
		kPathTLeft = 7,
		kPathTRight = 8,
		kPathTUp = 9,
		kPathTDown = 10,
		kPathStraightHoleVert = 11,
		kPathStraightHoleLongAVert = 12,
		kPathStraightHoleLongBVert = 13,
		kPathStraightHoleHorz = 14,
		kPathStraightHoleLongAHorz = 15,
		kPathStraightHoleLongBHorz = 16,
		kPathStraightNarrowSmallVert = 17,
		kPathStraightNarrowSmallHorz = 18,
		kPathStraightNarrowLongLeftVert = 19,
		kPathStraightNarrowLongLeftAVert = 20,
		kPathStraightNarrowLongLeftBVert = 21,
		kPathStraightNarrowLongRightVert = 22,
		kPathStraightNarrowLongRightAVert = 23,
		kPathStraightNarrowLongRightBVert = 24,
		kPathStraightNarrowLongUpHorz = 25,
		kPathStraightNarrowLongUpAHorz = 26,
		kPathStraightNarrowLongUpBHorz = 27,
		kPathStraightNarrowLongDownHorz = 28,
		kPathStraightNarrowLongDownAHorz = 29,
		kPathStraightNarrowLongDownBHorz = 30,
		kPathStraightFlameTowerHighHorz = 31,
		kPathStraightFlameTowerHighVert = 32,
		kPathSingleLevelStepDownCapAHorz = 33,
		kPathSingleLevelStepDownCapBHorz = 34,
		kPathSingleLevelStepDownCapAVert = 35,
		kPathSingleLevelStepDownCapBVert = 36,
		kPathTemple = 37,
		kPathMax = 38
	}

	public enum PathLevel
	{
		kPathLevelNone = 0,
		kPathLevelPlank = 1,
		kPathLevelTemple = 2,
		kPathLevelCliff = 3,
		kPathlevelMax = 4
	}

	public enum PathDirection
	{
		kPathDirectionNone = 0,
		kPathDirectionNorth = 1,
		kPathDirectionEast = 2,
		kPathDirectionSouth = 3,
		kPathDirectionWest = 4
	}

	public enum ObstacleType
	{
		kObstacleNone = 0,
		kObstacleHole = 1,
		kObstacleNarrow = 2,
		kObstacleTreeStumble = 3,
		kObstacleTreeJump = 4,
		kObstacleTreeSlide = 5,
		kObstacleFlameTower = 6,
		kObstacleLevelChange = 7
	}

	public enum CoinRunLocation
	{
		kCoinRunNone = 0,
		kCoinRunCenter = 1,
		kCoinRunLeft = 2,
		kCoinRunRight = 3
	}

	public enum RenderState
	{
		NotSet = 0,
		Enabled = 1,
		Disabled = 2
	}

	public const int GridX = 60;

	public const int GridZ = 60;

	public PathType Type;

	public PathLevel Level;

	public ObstacleType Obstacles;

	public bool HasObstacles;

	public bool HasCoins;

	public bool HasBonusItems;

	public bool SkippedBonusItems;

	public bool IsTutorialObstacle;

	public bool IsTutorialEnd;

	public CoinRunLocation CoinLocationStart;

	public CoinRunLocation CoinLocationEnd;

	public bool IsCoinRunInAir;

	public PathElement PathNorth;

	public PathElement PathEast;

	public PathElement PathWest;

	public PathElement PathSouth;

	public int TileIndex;

	public int SpacesSinceLastBonusItem;

	public int SpacesSinceLastTurn;

	public int SpacesSinceLastObstacle;

	public int BackToBackObstacleCount;

	public int SpacesSinceLastCoinRun;

	public int CoinRunCount;

	public GameObject SafteyNet;

	public PathType OriginalPick;

	public PathType FinalPick;

	public Rect Rectangle;

	public static int sCount = 0;

	public static bool sSafetyNetEnabled = false;

	public static bool sAllowBonusItems = false;

	public static bool sAllowVariations = true;

	public static bool sAllowTurns = false;

	public static bool sAllowPathHoles = false;

	public static bool sAllowPathNarrows = false;

	public static bool sAllowFlameTowers = false;

	public static bool sAllowTreeStumble = false;

	public static bool sAllowTreeJump = false;

	public static bool sAllowTreeSlide = false;

	public static bool sAllowTurnAfterObstacles = false;

	public static bool sAllowSidePaths = false;

	public static bool sAllowCoins = false;

	public static bool sAllowDoubleCoins = false;

	public static bool sAllowTripleCoins = false;

	public static bool sAllowCenterCoins = false;

	public static bool sAllowSideCoins = false;

	public static bool sAllowJumpCoins = false;

	public static bool sAllowSlantCoins = false;

	public static int sMinSpacesBetweenBonusItems = 50;

	public static float sProbabilityCoinBonus;

	public static float sProbabilityInvincibility;

	public static float sProbabilityVacuum;

	public static float sProbabilityBoost;

	public static int sMinSpaceBetweenTurns = 6;

	public static int sMaxBackToBackObstacles = 2;

	public static int sMinSpaceBetweenObstacles = 6;

	public static float sDoubleObstaclePercent = 0f;

	public static int sMinSpacesBetweenCoinRuns = 6;

	public static int sMaxCoinsPerRun = 5;

	public static PathLevel sPathLevel = PathLevel.kPathLevelTemple;

	public static bool sIsFastTurnSection = false;

	public static Color sSafteyNetColor = Color.white;

	private static Transform PathElementPrefab;

	public RenderState LastRenderState;

	public UISprite MySafteyNetSprite;

	private Renderer[] MyRenderers;

	private bool IsInView;

	private static Rect TurnRect = default(Rect);

	private static Rect TutorialRect = default(Rect);

	public static void DumpStatics()
	{
		string text = string.Format("SafteynetEnabled: {0}\nAllowBonusItems: {1}\nAllowVariations: {2}\nAllowTurns: {3}\nAllowHoles: {4}\n", sSafetyNetEnabled, sAllowBonusItems, sAllowVariations, sAllowTreeJump, sAllowPathHoles);
		text += string.Format("ProbabilityCoinBonus: {0}\nProbabilityInvincibility: {1}\nProbabilityVacuum: {2}\nProbabilityBoost: {3}\n", sProbabilityCoinBonus, sProbabilityInvincibility, sProbabilityVacuum, sProbabilityBoost);
		Debug.Log(text);
		Debug.Break();
	}

	public static PathElement InstantiatePathElement(PathType type, PathLevel level = PathLevel.kPathLevelTemple)
	{
		return Instantiate(type, level).GetComponent<PathElement>();
	}

	public static GameObject Instantiate(PathType type, PathLevel level = PathLevel.kPathLevelTemple)
	{
		if (PathElementPrefab == null)
		{
			PathElementPrefab = ((GameObject)Resources.Load("Prefabs/Temple/Path/PathElement")).transform;
		}
		SpawnPool spawnPool = PoolManager.Pools["PathElements"];
		GameObject gameObject = spawnPool.Spawn(PathElementPrefab).gameObject;
		PathElement component = gameObject.GetComponent<PathElement>();
		component.Pool = spawnPool;
		component.SetPathType(type, level);
		component.TileIndex = ++sCount;
		string text = "PathElement: " + component.TileIndex;
		gameObject.name = text;
		return gameObject;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = ((!IsInView) ? new Color(1f, 0f, 0f, 1f) : new Color(1f, 1f, 1f, 1f));
		Gizmos.DrawWireCube(new Vector3(Rectangle.center.x, 0f, Rectangle.center.y), new Vector3(Rectangle.width, 0f, Rectangle.height));
	}

	private void SetPathType(PathType type, PathLevel level)
	{
		Type = type;
		Level = level;
		SetupTerrain();
	}

	public void SetupTerrain()
	{
		if (Type == PathType.kPathNone || Level == PathLevel.kPathLevelNone)
		{
			return;
		}
		ClearTerrainPieces();
		string prefabNameForType = GetPrefabNameForType(Type, Level);
		if (prefabNameForType == string.Empty)
		{
			return;
		}
		GameObject gameObject = SetDataModelReference(prefabNameForType, "Terrain");
		if (gameObject == null)
		{
			Debug.LogError("Unable to instantiate prefab named: [" + prefabNameForType + "]", this);
			return;
		}
		if (primaryRenderer == null)
		{
			Debug.LogError("PrimaryRenderer null on: " + base.name, this);
		}
		if (IsPathTypeObstacle(Type))
		{
			HasObstacles = true;
			IsTutorialObstacle = true;
			if (IsPathTypeHole(Type))
			{
				Obstacles = ObstacleType.kObstacleHole;
			}
			else if (IsPathTypeNarrow(Type))
			{
				Obstacles = ObstacleType.kObstacleNarrow;
			}
			else if (Type == PathType.kPathStraightFlameTowerHighHorz || Type == PathType.kPathStraightFlameTowerHighVert)
			{
				Obstacles = ObstacleType.kObstacleFlameTower;
			}
			else if (IsPathTypeLevelChange(Type))
			{
				Obstacles = ObstacleType.kObstacleLevelChange;
			}
		}
		if (IsTurn())
		{
			IsTutorialObstacle = true;
		}
	}

	protected static bool HalfTheTime()
	{
		return Random.Range(0, 2) == 0;
	}

	public void AddObstacle(ObstacleType type)
	{
		if (HasObstacles || type == ObstacleType.kObstacleNone || type == ObstacleType.kObstacleHole || type == ObstacleType.kObstacleNarrow || type == ObstacleType.kObstacleLevelChange || type == ObstacleType.kObstacleFlameTower || (Type != PathType.kPathStraightVert && Type != PathType.kPathStraightHorz))
		{
			return;
		}
		switch (type)
		{
		case ObstacleType.kObstacleTreeStumble:
			if (Type == PathType.kPathStraightVert)
			{
				SetDataModelReference((!HalfTheTime()) ? "Walls/templeStraightTreeTripRightVert" : "Walls/templeStraightTreeTripLeftVert");
			}
			else
			{
				SetDataModelReference((!HalfTheTime()) ? "Walls/templeStraightTreeTripDownHorz" : "Walls/templeStraightTreeTripUpHorz");
			}
			Obstacles = ObstacleType.kObstacleTreeStumble;
			break;
		case ObstacleType.kObstacleTreeJump:
			if (Type == PathType.kPathStraightVert)
			{
				SetDataModelReference((!HalfTheTime()) ? "Walls/templeStraightTreeJumpRightVert" : "Walls/templeStraightTreeJumpLeftVert");
			}
			else
			{
				SetDataModelReference((!HalfTheTime()) ? "Walls/templeStraightTreeJumpDownHorz" : "Walls/templeStraightTreeJumpUpHorz");
			}
			Obstacles = ObstacleType.kObstacleTreeJump;
			break;
		case ObstacleType.kObstacleTreeSlide:
			if (Type == PathType.kPathStraightVert)
			{
				SetDataModelReference((Level != PathLevel.kPathLevelTemple) ? "Walls/cliffStraightTreeVert" : "Walls/templeStraightTreeSlideVert");
			}
			else
			{
				SetDataModelReference((Level != PathLevel.kPathLevelTemple) ? "Walls/cliffStraightTreeHorz" : "Walls/templeStraightTreeSlideHorz");
			}
			Obstacles = ObstacleType.kObstacleTreeSlide;
			break;
		}
		HasObstacles = true;
		IsTutorialObstacle = true;
	}

	private void ClearTerrainPieces()
	{
		RemoveAllSubObjects();
		ClearAllChildren();
	}

	public bool IsTurn()
	{
		return IsPathTypeTurn(Type);
	}

	public bool IsStraight()
	{
		return IsStraightVert() || IsStraightHorz();
	}

	public bool IsStraightVert()
	{
		return IsPathTypeStraightVert(Type);
	}

	public bool IsStraightHorz()
	{
		return IsPathTypeStraightHorz(Type);
	}

	public bool CanTurnLeft(PathDirection origin)
	{
		if (!IsTurn())
		{
			return false;
		}
		switch (origin)
		{
		case PathDirection.kPathDirectionNorth:
			if (Type == PathType.kPathTRight || Type == PathType.kPathTUp || Type == PathType.kPathTurnD)
			{
				return true;
			}
			break;
		case PathDirection.kPathDirectionEast:
			if (Type == PathType.kPathTDown || Type == PathType.kPathTRight || Type == PathType.kPathTurnB)
			{
				return true;
			}
			break;
		case PathDirection.kPathDirectionSouth:
			if (Type == PathType.kPathTDown || Type == PathType.kPathTLeft || Type == PathType.kPathTurnA)
			{
				return true;
			}
			break;
		case PathDirection.kPathDirectionWest:
			if (Type == PathType.kPathTLeft || Type == PathType.kPathTUp || Type == PathType.kPathTurnC)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public bool CanTurnRight(PathDirection origin)
	{
		if (!IsTurn())
		{
			return false;
		}
		switch (origin)
		{
		case PathDirection.kPathDirectionNorth:
			if (Type == PathType.kPathTLeft || Type == PathType.kPathTUp || Type == PathType.kPathTurnC)
			{
				return true;
			}
			break;
		case PathDirection.kPathDirectionEast:
			if (Type == PathType.kPathTRight || Type == PathType.kPathTUp || Type == PathType.kPathTurnD)
			{
				return true;
			}
			break;
		case PathDirection.kPathDirectionSouth:
			if (Type == PathType.kPathTDown || Type == PathType.kPathTRight || Type == PathType.kPathTurnB)
			{
				return true;
			}
			break;
		case PathDirection.kPathDirectionWest:
			if (Type == PathType.kPathTDown || Type == PathType.kPathTLeft || Type == PathType.kPathTurnA)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public bool DoesSidePathHaveObstacles()
	{
		return HasObstacle(PathNorth) || HasObstacle(PathEast) || HasObstacle(PathWest) || HasObstacle(PathSouth);
	}

	public static bool HasObstacle(PathElement node)
	{
		return node != null && node.HasObstacles;
	}

	public static bool IsPathTypeTurn(PathType type)
	{
		return type == PathType.kPathTurnA || type == PathType.kPathTurnB || type == PathType.kPathTurnC || type == PathType.kPathTurnD || type == PathType.kPathTUp || type == PathType.kPathTDown || type == PathType.kPathTLeft || type == PathType.kPathTRight;
	}

	public static bool ISsPathTypeStraight(PathType type)
	{
		return IsPathTypeStraightVert(type) || IsPathTypeStraightHorz(type);
	}

	public static bool IsPathTypeStraightVert(PathType type)
	{
		return type == PathType.kPathStraightVert || type == PathType.kPathStraightHoleVert || type == PathType.kPathStraightHoleLongAVert || type == PathType.kPathStraightHoleLongBVert || type == PathType.kPathStraightNarrowSmallVert || type == PathType.kPathStraightNarrowLongLeftVert || type == PathType.kPathStraightNarrowLongLeftAVert || type == PathType.kPathStraightNarrowLongLeftBVert || type == PathType.kPathStraightNarrowLongRightVert || type == PathType.kPathStraightNarrowLongRightAVert || type == PathType.kPathStraightNarrowLongRightBVert || type == PathType.kPathStraightFlameTowerHighVert || type == PathType.kPathSingleLevelStepDownCapAVert || type == PathType.kPathSingleLevelStepDownCapBVert || type == PathType.kPathTemple;
	}

	public static bool IsPathTypeStraightHorz(PathType type)
	{
		return type == PathType.kPathStraightHorz || type == PathType.kPathStraightHoleHorz || type == PathType.kPathStraightHoleLongAHorz || type == PathType.kPathStraightHoleLongBHorz || type == PathType.kPathStraightNarrowSmallHorz || type == PathType.kPathStraightNarrowLongUpHorz || type == PathType.kPathStraightNarrowLongUpAHorz || type == PathType.kPathStraightNarrowLongUpBHorz || type == PathType.kPathStraightNarrowLongDownHorz || type == PathType.kPathStraightNarrowLongDownAHorz || type == PathType.kPathStraightNarrowLongDownBHorz || type == PathType.kPathStraightFlameTowerHighHorz || type == PathType.kPathSingleLevelStepDownCapAHorz || type == PathType.kPathSingleLevelStepDownCapBHorz;
	}

	public static bool IsPathTypeObstacle(PathType type)
	{
		return type == PathType.kPathStraightHoleVert || type == PathType.kPathStraightHoleLongAVert || type == PathType.kPathStraightHoleLongBVert || type == PathType.kPathStraightHoleHorz || type == PathType.kPathStraightHoleLongAHorz || type == PathType.kPathStraightHoleLongBHorz || type == PathType.kPathStraightNarrowSmallVert || type == PathType.kPathStraightNarrowSmallHorz || type == PathType.kPathStraightNarrowLongLeftVert || type == PathType.kPathStraightNarrowLongLeftAVert || type == PathType.kPathStraightNarrowLongLeftBVert || type == PathType.kPathStraightNarrowLongRightVert || type == PathType.kPathStraightNarrowLongRightAVert || type == PathType.kPathStraightNarrowLongRightBVert || type == PathType.kPathStraightNarrowLongUpHorz || type == PathType.kPathStraightNarrowLongUpAHorz || type == PathType.kPathStraightNarrowLongUpBHorz || type == PathType.kPathStraightNarrowLongDownHorz || type == PathType.kPathStraightNarrowLongDownAHorz || type == PathType.kPathStraightNarrowLongDownBHorz || type == PathType.kPathStraightFlameTowerHighHorz || type == PathType.kPathStraightFlameTowerHighVert || type == PathType.kPathSingleLevelStepDownCapAHorz || type == PathType.kPathSingleLevelStepDownCapBHorz || type == PathType.kPathSingleLevelStepDownCapAVert || type == PathType.kPathSingleLevelStepDownCapBVert;
	}

	public static bool IsPathTypeHole(PathType type)
	{
		return type == PathType.kPathStraightHoleHorz || type == PathType.kPathStraightHoleLongAHorz || type == PathType.kPathStraightHoleLongBHorz || type == PathType.kPathStraightHoleVert || type == PathType.kPathStraightHoleLongAVert || type == PathType.kPathStraightHoleLongBVert;
	}

	public static bool IsPathTypeNarrow(PathType type)
	{
		return type == PathType.kPathStraightNarrowSmallHorz || type == PathType.kPathStraightNarrowSmallVert || type == PathType.kPathStraightNarrowLongLeftVert || type == PathType.kPathStraightNarrowLongLeftAVert || type == PathType.kPathStraightNarrowLongLeftBVert || type == PathType.kPathStraightNarrowLongRightVert || type == PathType.kPathStraightNarrowLongRightAVert || type == PathType.kPathStraightNarrowLongRightBVert || type == PathType.kPathStraightNarrowLongUpHorz || type == PathType.kPathStraightNarrowLongUpAHorz || type == PathType.kPathStraightNarrowLongUpBHorz || type == PathType.kPathStraightNarrowLongDownHorz || type == PathType.kPathStraightNarrowLongDownAHorz || type == PathType.kPathStraightNarrowLongDownBHorz;
	}

	public static bool IsPathTypeLevelChange(PathType type)
	{
		return type == PathType.kPathSingleLevelStepDownCapAHorz || type == PathType.kPathSingleLevelStepDownCapBHorz || type == PathType.kPathSingleLevelStepDownCapAVert || type == PathType.kPathSingleLevelStepDownCapBVert;
	}

	public static bool IsPathTypeStepDown(PathType type)
	{
		return type.IsAnyOf(PathType.kPathSingleLevelStepDownCapAHorz, PathType.kPathSingleLevelStepDownCapBHorz, PathType.kPathSingleLevelStepDownCapAVert, PathType.kPathSingleLevelStepDownCapBVert);
	}

	public static void WarmResources()
	{
		Debug.Log("WARMING START");
		int num = 0;
		for (int i = 0; i < 38; i++)
		{
			PathType type = (PathType)i;
			for (int j = 0; j < 4; j++)
			{
				string prefabNameForType = GetPrefabNameForType(type, (PathLevel)j);
				if (prefabNameForType != null && prefabNameForType != string.Empty)
				{
					num++;
					string path = string.Format("Prefabs/Temple/{0}_prefab", prefabNameForType);
					Resources.Load(path, typeof(GameObject));
				}
			}
		}
		Debug.Log("WARMING DONE: " + num);
	}

	private static string GetPrefabNameForType(PathType type, PathLevel level)
	{
		switch (level)
		{
		case PathLevel.kPathLevelPlank:
			switch (type)
			{
			case PathType.kPathStraightVert:
				return "Walls/plankStraightVert";
			case PathType.kPathStraightHorz:
				return "Walls/plankStraightHorz";
			case PathType.kPathTurnA:
				return "Walls/plankTurnA";
			case PathType.kPathTurnB:
				return "Walls/plankTurnB";
			case PathType.kPathTurnC:
				return "Walls/plankTurnC";
			case PathType.kPathTurnD:
				return "Walls/plankTurnD";
			case PathType.kPathTLeft:
				return "Walls/plankTLeft";
			case PathType.kPathTRight:
				return "Walls/plankTRight";
			case PathType.kPathTUp:
				return "Walls/plankTUp";
			case PathType.kPathTDown:
				return "Walls/plankTDown";
			case PathType.kPathStraightHoleVert:
				return "Walls/plankStraightHoleVert";
			case PathType.kPathStraightHoleLongAVert:
				return "Walls/plankStraightHoleVert";
			case PathType.kPathStraightHoleLongBVert:
				return "Walls/plankStraightHoleVert";
			case PathType.kPathStraightHoleHorz:
				return "Walls/plankStraightHoleHorz";
			case PathType.kPathStraightHoleLongAHorz:
				return "Walls/plankStraightHoleHorz";
			case PathType.kPathStraightHoleLongBHorz:
				return "Walls/plankStraightHoleHorz";
			case PathType.kPathStraightFlameTowerHighHorz:
				return "Walls/plankStraightTowerHorz";
			case PathType.kPathStraightFlameTowerHighVert:
				return "Walls/plankStraightTowerVert";
			}
			break;
		case PathLevel.kPathLevelTemple:
			switch (type)
			{
			case PathType.kPathStraightVert:
			{
				if (Random.Range(0, 20) == 0 && sAllowVariations)
				{
					return "Walls/templeStraightTowerVert";
				}
				int num2 = Random.Range(0, 10);
				if (num2 <= 5 || !sAllowVariations)
				{
					return "Walls/templeStraightVert";
				}
				switch (num2)
				{
				case 6:
					return "Walls/templeStraightVert2";
				case 7:
					return "Walls/templeStraightVert3";
				case 8:
					return "Walls/templeStraightVert4";
				case 9:
					return "Walls/templeStraightVert5";
				}
				break;
			}
			case PathType.kPathStraightHorz:
			{
				if (Random.Range(0, 20) == 0 && sAllowVariations)
				{
					return "Walls/templeStraightTowerHorz";
				}
				int num = Random.Range(0, 10);
				if (num <= 5 || !sAllowVariations)
				{
					return "Walls/templeStraightHorz";
				}
				switch (num)
				{
				case 6:
					return "Walls/templeStraightHorz2";
				case 7:
					return "Walls/templeStraightHorz3";
				case 8:
					return "Walls/templeStraightHorz4";
				case 9:
					return "Walls/templeStraightHorz5";
				}
				break;
			}
			case PathType.kPathTurnA:
				return "Walls/templeTurnA";
			case PathType.kPathTurnB:
				return "Walls/templeTurnB";
			case PathType.kPathTurnC:
				return "Walls/templeTurnC";
			case PathType.kPathTurnD:
				return "Walls/templeTurnD";
			case PathType.kPathTLeft:
				return "Walls/templeTLeft";
			case PathType.kPathTRight:
				return "Walls/templeTRight";
			case PathType.kPathTUp:
				return "Walls/templeTUp";
			case PathType.kPathTDown:
				return "Walls/templeTDown";
			case PathType.kPathStraightHoleVert:
				return "Walls/templeStraightHoleVert";
			case PathType.kPathStraightHoleLongAVert:
				return "Walls/templeStraightHoleLongAVert";
			case PathType.kPathStraightHoleLongBVert:
				return "Walls/templeStraightHoleLongBVert";
			case PathType.kPathStraightHoleHorz:
				return "Walls/templeStraightHoleHorz";
			case PathType.kPathStraightHoleLongAHorz:
				return "Walls/templeStraightHoleLongAHorz";
			case PathType.kPathStraightHoleLongBHorz:
				return "Walls/templeStraightHoleLongBHorz";
			case PathType.kPathStraightNarrowSmallVert:
				if (HalfTheTime())
				{
					return "Walls/templeStraightNarrowSmallRightVert";
				}
				return "Walls/templeStraightNarrowSmallLeftVert";
			case PathType.kPathStraightNarrowSmallHorz:
				return "Walls/templeStraightNarrowSmallDownHorz";
			case PathType.kPathStraightNarrowLongLeftVert:
				return "Walls/templeStraightNarrowLongLeftVert";
			case PathType.kPathStraightNarrowLongLeftAVert:
				return "Walls/templeStraightNarrowLongLeftAVert";
			case PathType.kPathStraightNarrowLongLeftBVert:
				return "Walls/templeStraightNarrowLongLeftBVert";
			case PathType.kPathStraightNarrowLongRightVert:
				return "Walls/templeStraightNarrowLongRightVert";
			case PathType.kPathStraightNarrowLongRightAVert:
				return "Walls/templeStraightNarrowLongRightAVert";
			case PathType.kPathStraightNarrowLongRightBVert:
				return "Walls/templeStraightNarrowLongRightBVert";
			case PathType.kPathStraightNarrowLongUpHorz:
				return "Walls/templeStraightNarrowLongUpHorz";
			case PathType.kPathStraightNarrowLongUpAHorz:
				return "Walls/templeStraightNarrowLongUpAHorz";
			case PathType.kPathStraightNarrowLongUpBHorz:
				return "Walls/templeStraightNarrowLongUpBHorz";
			case PathType.kPathStraightNarrowLongDownHorz:
				return "Walls/templeStraightNarrowLongDownHorz";
			case PathType.kPathStraightNarrowLongDownAHorz:
				return "Walls/templeStraightNarrowLongDownAHorz";
			case PathType.kPathStraightNarrowLongDownBHorz:
				return "Walls/templeStraightNarrowLongDownBHorz";
			case PathType.kPathStraightFlameTowerHighHorz:
				return "Walls/templeStraightFlameTowerHighHorz";
			case PathType.kPathStraightFlameTowerHighVert:
				return "Walls/templeStraightFlameTowerHighVert";
			case PathType.kPathSingleLevelStepDownCapAHorz:
				return "Walls/templeToPlankCapAHorz";
			case PathType.kPathSingleLevelStepDownCapBHorz:
				return "Walls/templeToPlankCapBHorz";
			case PathType.kPathSingleLevelStepDownCapAVert:
				return "Walls/templeToPlankCapAVert";
			case PathType.kPathSingleLevelStepDownCapBVert:
				return "Walls/templeToPlankCapBVert";
			case PathType.kPathTemple:
				return "Temple/temple";
			}
			break;
		case PathLevel.kPathLevelCliff:
			switch (type)
			{
			case PathType.kPathStraightVert:
				return "Walls/cliffStraightVert";
			case PathType.kPathStraightHorz:
				return "Walls/cliffStraightHorz";
			case PathType.kPathTurnA:
				return "Walls/cliffTurnA";
			case PathType.kPathTurnB:
				return "Walls/cliffTurnB";
			case PathType.kPathTurnC:
				return "Walls/cliffTurnC";
			case PathType.kPathTurnD:
				return "Walls/cliffTurnD";
			case PathType.kPathTLeft:
				return "Walls/cliffTLeft";
			case PathType.kPathTRight:
				return "Walls/cliffTRight";
			case PathType.kPathTUp:
				return "Walls/cliffTUp";
			case PathType.kPathTDown:
				return "Walls/cliffTDown";
			case PathType.kPathStraightHoleVert:
				return string.Empty;
			case PathType.kPathStraightHoleLongAVert:
				return "Walls/cliffToTempleCapAVert";
			case PathType.kPathStraightHoleLongBVert:
				return "Walls/cliffToTempleCapBVert";
			case PathType.kPathStraightHoleHorz:
				return string.Empty;
			case PathType.kPathStraightHoleLongAHorz:
				return "Walls/cliffToTempleCapAHorz";
			case PathType.kPathStraightHoleLongBHorz:
				return "Walls/cliffToTempleCapBHorz";
			case PathType.kPathSingleLevelStepDownCapAHorz:
				return "Walls/cliffToTempleCapAHorz";
			case PathType.kPathSingleLevelStepDownCapBHorz:
				return "Walls/cliffToTempleCapBHorz";
			case PathType.kPathSingleLevelStepDownCapAVert:
				return "Walls/cliffToTempleCapAVert";
			case PathType.kPathSingleLevelStepDownCapBVert:
				return "Walls/cliffToTempleCapBVert";
			}
			break;
		}
		return string.Empty;
	}

	public float GetPathHeight()
	{
		switch (Level)
		{
		case PathLevel.kPathLevelPlank:
			return -15f;
		case PathLevel.kPathLevelCliff:
			return 15f;
		default:
			return 0f;
		}
	}

	public PathElement GetPath(PathDirection direction)
	{
		switch (direction)
		{
		case PathDirection.kPathDirectionNorth:
			return PathNorth;
		case PathDirection.kPathDirectionEast:
			return PathEast;
		case PathDirection.kPathDirectionSouth:
			return PathSouth;
		case PathDirection.kPathDirectionWest:
			return PathWest;
		default:
			return null;
		}
	}

	public PathElement GetPathForward(PathDirection origin)
	{
		switch (origin)
		{
		case PathDirection.kPathDirectionNorth:
			return PathSouth;
		case PathDirection.kPathDirectionEast:
			return PathWest;
		case PathDirection.kPathDirectionSouth:
			return PathNorth;
		case PathDirection.kPathDirectionWest:
			return PathEast;
		default:
			return null;
		}
	}

	public PathElement GetPathBackward(PathDirection origin)
	{
		return GetPath(origin);
	}

	public PathElement GetPathLeft(PathDirection origin)
	{
		if (!IsTurn())
		{
			return null;
		}
		switch (origin)
		{
		case PathDirection.kPathDirectionNorth:
			if (Type == PathType.kPathTRight || Type == PathType.kPathTUp || Type == PathType.kPathTurnD)
			{
				return GetPath(PathDirection.kPathDirectionEast);
			}
			break;
		case PathDirection.kPathDirectionEast:
			if (Type == PathType.kPathTDown || Type == PathType.kPathTRight || Type == PathType.kPathTurnB)
			{
				return GetPath(PathDirection.kPathDirectionSouth);
			}
			break;
		case PathDirection.kPathDirectionSouth:
			if (Type == PathType.kPathTDown || Type == PathType.kPathTLeft || Type == PathType.kPathTurnA)
			{
				return GetPath(PathDirection.kPathDirectionWest);
			}
			break;
		case PathDirection.kPathDirectionWest:
			if (Type == PathType.kPathTLeft || Type == PathType.kPathTUp || Type == PathType.kPathTurnC)
			{
				return GetPath(PathDirection.kPathDirectionNorth);
			}
			break;
		}
		return null;
	}

	public PathElement GetPathRight(PathDirection origin)
	{
		if (!IsTurn())
		{
			return null;
		}
		switch (origin)
		{
		case PathDirection.kPathDirectionNorth:
			if (Type == PathType.kPathTLeft || Type == PathType.kPathTUp || Type == PathType.kPathTurnC)
			{
				return GetPath(PathDirection.kPathDirectionWest);
			}
			break;
		case PathDirection.kPathDirectionEast:
			if (Type == PathType.kPathTRight || Type == PathType.kPathTUp || Type == PathType.kPathTurnD)
			{
				return GetPath(PathDirection.kPathDirectionNorth);
			}
			break;
		case PathDirection.kPathDirectionSouth:
			if (Type == PathType.kPathTDown || Type == PathType.kPathTRight || Type == PathType.kPathTurnB)
			{
				return GetPath(PathDirection.kPathDirectionEast);
			}
			break;
		case PathDirection.kPathDirectionWest:
			if (Type == PathType.kPathTDown || Type == PathType.kPathTLeft || Type == PathType.kPathTurnA)
			{
				return GetPath(PathDirection.kPathDirectionSouth);
			}
			break;
		}
		return null;
	}

	public static PathDirection GetOpposite(PathDirection direction)
	{
		switch (direction)
		{
		case PathDirection.kPathDirectionNorth:
			return PathDirection.kPathDirectionSouth;
		case PathDirection.kPathDirectionEast:
			return PathDirection.kPathDirectionWest;
		case PathDirection.kPathDirectionWest:
			return PathDirection.kPathDirectionEast;
		case PathDirection.kPathDirectionSouth:
			return PathDirection.kPathDirectionNorth;
		default:
			return PathDirection.kPathDirectionNone;
		}
	}

	private bool DoesPieceTypeFitOnSide(PathType type, PathDirection direction, bool checkPieceTypes = true)
	{
		if (GetPath(direction) != null)
		{
			return false;
		}
		bool result = false;
		if (CanPathExtendOnSide(direction))
		{
			result = !checkPieceTypes || WillPieceTypeConnectOnSide(type, direction);
		}
		return result;
	}

	private bool CanPathExtendOnSide2(PathDirection direction)
	{
		bool flag = CanPathExtendOnSide2(direction);
		Debug.Log(string.Concat(Type, " ", (!flag) ? "cannot" : "can", " extend ", direction));
		return flag;
	}

	private bool CanPathExtendOnSide(PathDirection direction)
	{
		if (IsStraightVert())
		{
			if (direction == PathDirection.kPathDirectionNorth || direction == PathDirection.kPathDirectionSouth)
			{
				return true;
			}
		}
		else if (IsStraightHorz())
		{
			if (direction == PathDirection.kPathDirectionEast || direction == PathDirection.kPathDirectionWest)
			{
				return true;
			}
		}
		else if (Type == PathType.kPathTurnA)
		{
			if (direction == PathDirection.kPathDirectionWest || direction == PathDirection.kPathDirectionSouth)
			{
				return true;
			}
		}
		else if (Type == PathType.kPathTurnB)
		{
			if (direction == PathDirection.kPathDirectionEast || direction == PathDirection.kPathDirectionSouth)
			{
				return true;
			}
		}
		else if (Type == PathType.kPathTurnC)
		{
			if (direction == PathDirection.kPathDirectionWest || direction == PathDirection.kPathDirectionNorth)
			{
				return true;
			}
		}
		else if (Type == PathType.kPathTurnD)
		{
			if (direction == PathDirection.kPathDirectionEast || direction == PathDirection.kPathDirectionNorth)
			{
				return true;
			}
		}
		else if (Type == PathType.kPathTRight)
		{
			if (direction == PathDirection.kPathDirectionNorth || direction == PathDirection.kPathDirectionEast || direction == PathDirection.kPathDirectionSouth)
			{
				return true;
			}
		}
		else if (Type == PathType.kPathTLeft)
		{
			if (direction == PathDirection.kPathDirectionNorth || direction == PathDirection.kPathDirectionSouth || direction == PathDirection.kPathDirectionWest)
			{
				return true;
			}
		}
		else if (Type == PathType.kPathTUp)
		{
			if (direction == PathDirection.kPathDirectionNorth || direction == PathDirection.kPathDirectionEast || direction == PathDirection.kPathDirectionWest)
			{
				return true;
			}
		}
		else if (Type == PathType.kPathTDown && (direction == PathDirection.kPathDirectionEast || direction == PathDirection.kPathDirectionSouth || direction == PathDirection.kPathDirectionWest))
		{
			return true;
		}
		return false;
	}

	private bool WillPieceTypeConnectOnSide(PathType type, PathDirection direction)
	{
		switch (direction)
		{
		case PathDirection.kPathDirectionNorth:
			if (Type == PathType.kPathStraightNarrowLongLeftAVert || Type == PathType.kPathStraightNarrowLongLeftVert)
			{
				return type == PathType.kPathStraightNarrowLongLeftVert || type == PathType.kPathStraightNarrowLongLeftBVert;
			}
			if (Type == PathType.kPathStraightNarrowLongLeftBVert)
			{
				return type == PathType.kPathStraightVert;
			}
			if (Type == PathType.kPathStraightNarrowLongRightAVert || Type == PathType.kPathStraightNarrowLongRightVert)
			{
				return type == PathType.kPathStraightNarrowLongRightVert || type == PathType.kPathStraightNarrowLongRightBVert;
			}
			if (Type == PathType.kPathStraightNarrowLongRightBVert)
			{
				return type == PathType.kPathStraightVert;
			}
			if (Type == PathType.kPathStraightNarrowSmallVert)
			{
				return type == PathType.kPathStraightVert;
			}
			if (Type == PathType.kPathStraightHoleLongAVert)
			{
				return type == PathType.kPathStraightVert || type == PathType.kPathStraightHoleLongBVert;
			}
			return type.IsAnyOf(PathType.kPathStraightVert, PathType.kPathTurnA, PathType.kPathTurnB, PathType.kPathTRight, PathType.kPathTLeft, PathType.kPathTDown, PathType.kPathStraightHoleVert, PathType.kPathStraightHoleLongAVert, PathType.kPathStraightNarrowSmallVert, PathType.kPathStraightNarrowLongLeftAVert, PathType.kPathStraightNarrowLongRightAVert, PathType.kPathStraightFlameTowerHighVert, PathType.kPathSingleLevelStepDownCapAVert, PathType.kPathSingleLevelStepDownCapBVert, PathType.kPathTemple);
		case PathDirection.kPathDirectionEast:
			if (Type == PathType.kPathStraightNarrowLongUpAHorz || Type == PathType.kPathStraightNarrowLongUpHorz)
			{
				return type == PathType.kPathStraightNarrowLongUpHorz || type == PathType.kPathStraightNarrowLongUpBHorz;
			}
			if (Type == PathType.kPathStraightNarrowLongUpBHorz)
			{
				return type == PathType.kPathStraightHorz;
			}
			if (Type == PathType.kPathStraightNarrowLongDownAHorz || Type == PathType.kPathStraightNarrowLongDownHorz)
			{
				return type == PathType.kPathStraightNarrowLongDownHorz || type == PathType.kPathStraightNarrowLongDownBHorz;
			}
			if (Type == PathType.kPathStraightNarrowLongDownBHorz)
			{
				return type == PathType.kPathStraightHorz;
			}
			if (Type == PathType.kPathStraightHoleLongAHorz)
			{
				return type == PathType.kPathStraightHorz || type == PathType.kPathStraightHoleLongBHorz;
			}
			if (Type == PathType.kPathStraightNarrowSmallHorz)
			{
				return type == PathType.kPathStraightHorz;
			}
			return type.IsAnyOf(PathType.kPathStraightHorz, PathType.kPathTurnA, PathType.kPathTurnC, PathType.kPathTLeft, PathType.kPathTUp, PathType.kPathTDown, PathType.kPathStraightHoleHorz, PathType.kPathStraightHoleLongAHorz, PathType.kPathStraightNarrowSmallHorz, PathType.kPathStraightNarrowLongUpAHorz, PathType.kPathStraightNarrowLongDownAHorz, PathType.kPathStraightFlameTowerHighHorz, PathType.kPathSingleLevelStepDownCapAHorz, PathType.kPathSingleLevelStepDownCapBHorz);
		case PathDirection.kPathDirectionSouth:
			if (Type == PathType.kPathStraightNarrowLongLeftBVert || Type == PathType.kPathStraightNarrowLongLeftVert)
			{
				return type == PathType.kPathStraightNarrowLongLeftVert || type == PathType.kPathStraightNarrowLongLeftAVert;
			}
			if (Type == PathType.kPathStraightNarrowLongLeftAVert)
			{
				return type == PathType.kPathStraightVert;
			}
			if (Type == PathType.kPathStraightNarrowLongRightBVert || Type == PathType.kPathStraightNarrowLongRightVert)
			{
				return type == PathType.kPathStraightNarrowLongRightVert || type == PathType.kPathStraightNarrowLongRightAVert;
			}
			if (Type == PathType.kPathStraightNarrowLongRightAVert)
			{
				return type == PathType.kPathStraightVert;
			}
			if (Type == PathType.kPathStraightNarrowSmallVert)
			{
				return type == PathType.kPathStraightVert;
			}
			if (Type == PathType.kPathStraightHoleLongBVert)
			{
				return type == PathType.kPathStraightVert || type == PathType.kPathStraightHoleLongAVert;
			}
			return type.IsAnyOf(PathType.kPathStraightVert, PathType.kPathTurnC, PathType.kPathTurnD, PathType.kPathTRight, PathType.kPathTLeft, PathType.kPathTUp, PathType.kPathStraightHoleVert, PathType.kPathStraightHoleLongBVert, PathType.kPathStraightNarrowSmallVert, PathType.kPathStraightNarrowLongLeftBVert, PathType.kPathStraightNarrowLongRightBVert, PathType.kPathStraightFlameTowerHighVert, PathType.kPathSingleLevelStepDownCapAVert, PathType.kPathSingleLevelStepDownCapBVert, PathType.kPathTemple);
		case PathDirection.kPathDirectionWest:
			if (Type == PathType.kPathStraightNarrowLongUpBHorz || Type == PathType.kPathStraightNarrowLongUpHorz)
			{
				return type == PathType.kPathStraightNarrowLongUpHorz || type == PathType.kPathStraightNarrowLongUpAHorz;
			}
			if (Type == PathType.kPathStraightNarrowLongUpAHorz)
			{
				return type == PathType.kPathStraightHorz;
			}
			if (Type == PathType.kPathStraightNarrowLongDownBHorz || Type == PathType.kPathStraightNarrowLongDownHorz)
			{
				return type == PathType.kPathStraightNarrowLongDownHorz || type == PathType.kPathStraightNarrowLongDownAHorz;
			}
			if (Type == PathType.kPathStraightNarrowLongDownAHorz)
			{
				return type == PathType.kPathStraightHorz;
			}
			if (Type == PathType.kPathStraightHoleLongBHorz)
			{
				return type == PathType.kPathStraightHorz || type == PathType.kPathStraightHoleLongAHorz;
			}
			if (Type == PathType.kPathStraightNarrowSmallHorz)
			{
				return type == PathType.kPathStraightHorz;
			}
			return type.IsAnyOf(PathType.kPathStraightHorz, PathType.kPathTurnB, PathType.kPathTurnD, PathType.kPathTRight, PathType.kPathTUp, PathType.kPathTDown, PathType.kPathStraightHoleHorz, PathType.kPathStraightHoleLongBHorz, PathType.kPathStraightNarrowSmallHorz, PathType.kPathStraightNarrowLongUpBHorz, PathType.kPathStraightNarrowLongDownBHorz, PathType.kPathStraightFlameTowerHighHorz, PathType.kPathSingleLevelStepDownCapAHorz, PathType.kPathSingleLevelStepDownCapBHorz);
		default:
			return false;
		}
	}

	private bool ACertainTypeCheck(PathType currentType, PathType parentType)
	{
		return ((parentType == PathType.kPathStraightNarrowLongLeftVert || parentType == PathType.kPathStraightNarrowLongLeftAVert || parentType == PathType.kPathStraightNarrowLongLeftBVert) && (currentType == PathType.kPathStraightNarrowLongLeftVert || currentType == PathType.kPathStraightNarrowLongLeftAVert || currentType == PathType.kPathStraightNarrowLongLeftBVert)) || ((parentType == PathType.kPathStraightNarrowLongRightVert || parentType == PathType.kPathStraightNarrowLongRightAVert || parentType == PathType.kPathStraightNarrowLongRightBVert) && (currentType == PathType.kPathStraightNarrowLongRightVert || currentType == PathType.kPathStraightNarrowLongRightAVert || currentType == PathType.kPathStraightNarrowLongRightBVert)) || ((parentType == PathType.kPathStraightNarrowLongUpHorz || parentType == PathType.kPathStraightNarrowLongUpAHorz || parentType == PathType.kPathStraightNarrowLongUpBHorz) && (currentType == PathType.kPathStraightNarrowLongUpHorz || currentType == PathType.kPathStraightNarrowLongUpAHorz || currentType == PathType.kPathStraightNarrowLongUpBHorz)) || ((parentType == PathType.kPathStraightNarrowLongDownHorz || parentType == PathType.kPathStraightNarrowLongDownAHorz || parentType == PathType.kPathStraightNarrowLongDownBHorz) && (currentType == PathType.kPathStraightNarrowLongDownHorz || currentType == PathType.kPathStraightNarrowLongDownAHorz || currentType == PathType.kPathStraightNarrowLongDownBHorz));
	}

	private PathElement GetRandomPathElementForSide(PathDirection direction)
	{
		bool flag = sAllowTurns && SpacesSinceLastTurn >= sMinSpaceBetweenTurns;
		bool flag2 = false;
		bool flag3 = true;
		bool flag4 = false;
		bool flag5 = true;
		if (SpacesSinceLastTurn == 1)
		{
			flag3 = false;
		}
		if (BackToBackObstacleCount >= sMaxBackToBackObstacles)
		{
			flag3 = false;
		}
		if (SpacesSinceLastObstacle < sMinSpaceBetweenObstacles && (float)Random.Range(0, 1000) >= 1000f * sDoubleObstaclePercent)
		{
			flag3 = false;
		}
		PathType pathType = PathType.kPathNone;
		PathLevel pathLevel = PathLevel.kPathLevelNone;
		PathLevel level = sPathLevel;
		pathType = Type;
		if (pathType.IsAnyOf(PathType.kPathTurnA, PathType.kPathTurnB, PathType.kPathTurnC, PathType.kPathTurnD))
		{
			flag3 = false;
		}
		else if (pathType.IsAnyOf(PathType.kPathTUp, PathType.kPathTDown, PathType.kPathTLeft, PathType.kPathTRight))
		{
			if (DoesSidePathHaveObstacles())
			{
				flag3 = false;
			}
			else
			{
				flag4 = true;
			}
		}
		if (HasObstacles)
		{
			if (!sAllowTurnAfterObstacles)
			{
				flag = false;
			}
			flag5 = false;
			if (Obstacles == ObstacleType.kObstacleTreeSlide || Obstacles == ObstacleType.kObstacleFlameTower)
			{
				flag3 = false;
			}
		}
		pathLevel = Level;
		if (pathLevel != sPathLevel)
		{
			flag = false;
			flag3 = false;
			flag5 = false;
			flag4 = false;
			if ((pathType == PathType.kPathStraightVert || pathType == PathType.kPathStraightHorz) && !HasObstacles && SpacesSinceLastTurn > 2 && SpacesSinceLastObstacle > 2)
			{
				flag2 = true;
				if (pathLevel == PathLevel.kPathLevelTemple && sPathLevel == PathLevel.kPathLevelCliff)
				{
					switch (direction)
					{
					case PathDirection.kPathDirectionEast:
						SetPathType(PathType.kPathSingleLevelStepDownCapAHorz, pathLevel);
						break;
					case PathDirection.kPathDirectionWest:
						SetPathType(PathType.kPathSingleLevelStepDownCapBHorz, pathLevel);
						break;
					case PathDirection.kPathDirectionNorth:
						SetPathType(PathType.kPathSingleLevelStepDownCapAVert, pathLevel);
						break;
					case PathDirection.kPathDirectionSouth:
						SetPathType(PathType.kPathSingleLevelStepDownCapBVert, pathLevel);
						break;
					}
					RemoveAllSubObjects();
				}
				if ((pathLevel == PathLevel.kPathLevelCliff && (sPathLevel == PathLevel.kPathLevelTemple || sPathLevel == PathLevel.kPathLevelPlank)) || (pathLevel == PathLevel.kPathLevelTemple && sPathLevel == PathLevel.kPathLevelPlank))
				{
					switch (direction)
					{
					case PathDirection.kPathDirectionEast:
						SetPathType(PathType.kPathSingleLevelStepDownCapAHorz, pathLevel);
						break;
					case PathDirection.kPathDirectionWest:
						SetPathType(PathType.kPathSingleLevelStepDownCapBHorz, pathLevel);
						break;
					case PathDirection.kPathDirectionNorth:
						SetPathType(PathType.kPathSingleLevelStepDownCapAVert, pathLevel);
						break;
					case PathDirection.kPathDirectionSouth:
						SetPathType(PathType.kPathSingleLevelStepDownCapBVert, pathLevel);
						break;
					}
					RemoveAllSubObjects();
				}
			}
			else
			{
				level = Level;
			}
		}
		if (Obstacles == ObstacleType.kObstacleLevelChange || flag2)
		{
			flag = false;
			flag5 = false;
			flag3 = false;
			flag4 = false;
		}
		if (sIsFastTurnSection)
		{
			flag5 = false;
			flag3 = false;
			flag4 = false;
		}
		int num = 0;
		PathType[] array = new PathType[38];
		for (int i = 1; i < 38; i++)
		{
			PathType pathType2 = (PathType)i;
			bool flag6 = DoesPieceTypeFitOnSide(pathType2, direction);
			if (!flag && IsPathTypeTurn(pathType2))
			{
				flag6 = false;
			}
			if (!sAllowSidePaths)
			{
				if (Type == PathType.kPathStraightVert && (pathType2 == PathType.kPathTLeft || pathType2 == PathType.kPathTRight))
				{
					flag6 = false;
				}
				else if (Type == PathType.kPathStraightHorz && (pathType2 == PathType.kPathTUp || pathType2 == PathType.kPathTDown))
				{
					flag6 = false;
				}
			}
			if (!flag3)
			{
				if (!ACertainTypeCheck(pathType2, pathType) && IsPathTypeObstacle(pathType2))
				{
					flag6 = false;
				}
			}
			else
			{
				switch (pathType2)
				{
				case PathType.kPathStraightHoleVert:
				case PathType.kPathStraightHoleHorz:
					if (!sAllowPathHoles || HalfTheTime())
					{
						flag6 = false;
					}
					break;
				case PathType.kPathStraightFlameTowerHighHorz:
				case PathType.kPathStraightFlameTowerHighVert:
					if (!sAllowFlameTowers || level == PathLevel.kPathLevelCliff || !flag5 || flag4 || HalfTheTime())
					{
						flag6 = false;
					}
					break;
				default:
					if (IsPathTypeNarrow(pathType2) && (!sAllowPathNarrows || level != PathLevel.kPathLevelTemple || SpacesSinceLastTurn < 2 || flag4 || HalfTheTime()) && !ACertainTypeCheck(pathType2, pathType))
					{
						flag6 = false;
					}
					break;
				}
			}
			if (i >= 12 && i <= 13)
			{
				flag6 = false;
			}
			if (i >= 15 && i <= 16)
			{
				flag6 = false;
			}
			if (i >= 33 && i <= 36)
			{
				flag6 = false;
			}
			if (i == 37)
			{
				flag6 = false;
			}
			if (flag6)
			{
				array[num++] = pathType2;
			}
		}
		if (num == 0)
		{
			for (int j = 1; j < 38; j++)
			{
				PathType pathType3 = (PathType)j;
				bool flag7 = DoesPieceTypeFitOnSide(pathType3, direction);
				Debug.Log(string.Concat(Type, " >> ", pathType3, " : ", direction, " : ", flag7));
			}
			Debug.LogError("No piece fits.  This is bad!", this);
			return null;
		}
		int num2 = Random.Range(0, num);
		PathType pathType4 = array[num2];
		PathType originalPick = pathType4;
		ObstacleType obstacleType = ObstacleType.kObstacleNone;
		if (pathType4 == PathType.kPathStraightHoleHorz || pathType4 == PathType.kPathStraightHoleVert)
		{
			obstacleType = ObstacleType.kObstacleHole;
		}
		else if (IsPathTypeNarrow(pathType4))
		{
			obstacleType = ObstacleType.kObstacleNarrow;
		}
		if (flag3 && (sAllowTreeStumble || sAllowTreeJump || sAllowTreeSlide) && !flag4 && (pathType4 == PathType.kPathStraightHorz || pathType4 == PathType.kPathStraightVert) && HalfTheTime())
		{
			int num3 = 0;
			int[] array2 = new int[4];
			if (sAllowTreeStumble && level == PathLevel.kPathLevelTemple)
			{
				array2[num3++] = 0;
			}
			if (sAllowTreeJump && level == PathLevel.kPathLevelTemple)
			{
				array2[num3++] = 1;
			}
			if (sAllowTreeSlide && flag5 && level != PathLevel.kPathLevelPlank)
			{
				array2[num3++] = 2;
			}
			if (num3 > 0)
			{
				switch (array2[Random.Range(0, num3)])
				{
				case 0:
					obstacleType = ObstacleType.kObstacleTreeStumble;
					break;
				case 1:
					obstacleType = ObstacleType.kObstacleTreeJump;
					break;
				case 2:
					obstacleType = ObstacleType.kObstacleTreeSlide;
					break;
				}
			}
		}
		if (Obstacles == ObstacleType.kObstacleHole && level == PathLevel.kPathLevelCliff)
		{
			switch (pathType)
			{
			case PathType.kPathStraightHoleHorz:
				pathType4 = PathType.kPathStraightHoleHorz;
				break;
			case PathType.kPathStraightHoleVert:
				pathType4 = PathType.kPathStraightHoleVert;
				break;
			}
		}
		if (Obstacles == ObstacleType.kObstacleHole && (pathType == PathType.kPathStraightHoleHorz || pathType == PathType.kPathStraightHoleVert) && (pathType4 == PathType.kPathStraightHoleHorz || pathType4 == PathType.kPathStraightHoleVert))
		{
			switch (pathType4)
			{
			case PathType.kPathStraightHoleHorz:
				if (direction == PathDirection.kPathDirectionEast)
				{
					SetPathType(PathType.kPathStraightHoleLongAHorz, level);
					pathType4 = PathType.kPathStraightHoleLongBHorz;
				}
				else
				{
					SetPathType(PathType.kPathStraightHoleLongBHorz, level);
					pathType4 = PathType.kPathStraightHoleLongAHorz;
				}
				break;
			case PathType.kPathStraightHoleVert:
				if (direction == PathDirection.kPathDirectionNorth)
				{
					SetPathType(PathType.kPathStraightHoleLongAVert, level);
					pathType4 = PathType.kPathStraightHoleLongBVert;
				}
				else
				{
					SetPathType(PathType.kPathStraightHoleLongBVert, level);
					pathType4 = PathType.kPathStraightHoleLongAVert;
				}
				break;
			}
		}
		if (flag2)
		{
			if (pathLevel == PathLevel.kPathLevelCliff && level == PathLevel.kPathLevelTemple)
			{
				switch (direction)
				{
				case PathDirection.kPathDirectionEast:
					pathType4 = PathType.kPathSingleLevelStepDownCapBHorz;
					break;
				case PathDirection.kPathDirectionWest:
					pathType4 = PathType.kPathSingleLevelStepDownCapAHorz;
					break;
				case PathDirection.kPathDirectionNorth:
					pathType4 = PathType.kPathSingleLevelStepDownCapBVert;
					break;
				case PathDirection.kPathDirectionSouth:
					pathType4 = PathType.kPathSingleLevelStepDownCapAVert;
					break;
				}
			}
			if ((pathLevel == PathLevel.kPathLevelTemple && level == PathLevel.kPathLevelCliff) || (pathLevel == PathLevel.kPathLevelPlank && level == PathLevel.kPathLevelTemple))
			{
				switch (direction)
				{
				case PathDirection.kPathDirectionEast:
					pathType4 = PathType.kPathSingleLevelStepDownCapBHorz;
					break;
				case PathDirection.kPathDirectionWest:
					pathType4 = PathType.kPathSingleLevelStepDownCapAHorz;
					break;
				case PathDirection.kPathDirectionNorth:
					pathType4 = PathType.kPathSingleLevelStepDownCapBVert;
					break;
				case PathDirection.kPathDirectionSouth:
					pathType4 = PathType.kPathSingleLevelStepDownCapAVert;
					break;
				}
			}
		}
		GameObject gameObject = Instantiate(pathType4, level);
		PathElement component = gameObject.GetComponent<PathElement>();
		component.OriginalPick = originalPick;
		component.FinalPick = pathType4;
		if (obstacleType == ObstacleType.kObstacleHole || obstacleType == ObstacleType.kObstacleNarrow)
		{
			component.Obstacles = obstacleType;
		}
		else
		{
			component.AddObstacle(obstacleType);
		}
		if (!flag4)
		{
			component.AddCoins(direction, this);
			component.AddBonusItems(direction, this);
		}
		AddSafteyNet(component, GetOpposite(direction));
		component.UpdatePathDescription(this);
		component.UpdateCachedComponents();
		return component;
	}

	public GameObject InstantiateSafteynet()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(GameController.SharedInstance.SafteyNetPrefab);
		gameObject.transform.parent = base.transform;
		return gameObject;
	}

	public void AddSafteyNet(PathElement newPath, PathDirection origin)
	{
		if (SafteyNet != null)
		{
			Object.Destroy(SafteyNet);
			SafteyNet = null;
			UpdateCachedComponents();
		}
		if (!Obstacles.IsAnyOf(ObstacleType.kObstacleHole, ObstacleType.kObstacleNarrow, ObstacleType.kObstacleLevelChange) && (newPath.Obstacles != ObstacleType.kObstacleLevelChange || Level == PathLevel.kPathLevelCliff))
		{
			return;
		}
		float y = GetPathHeight() - 1f;
		GameObject gameObject = InstantiateSafteynet();
		if (IsStraightHorz())
		{
			gameObject.transform.Rotate(0f, 0f, 90f);
		}
		if (GetPathHeight() < newPath.GetPathHeight())
		{
			y = GetPathHeight() + 6.5f;
			if (origin == PathDirection.kPathDirectionWest || origin == PathDirection.kPathDirectionNorth)
			{
				gameObject.transform.Rotate(14.4f, 0f, 0f);
			}
			else
			{
				gameObject.transform.Rotate(-14.4f, 0f, 0f);
			}
		}
		else if (GetPathHeight() > newPath.GetPathHeight())
		{
			y = GetPathHeight() - 8.5f;
			if (origin == PathDirection.kPathDirectionWest || origin == PathDirection.kPathDirectionNorth)
			{
				gameObject.transform.Rotate(-14.4f, 0f, 0f);
			}
			else
			{
				gameObject.transform.Rotate(14.4f, 0f, 0f);
			}
		}
		gameObject.transform.localPosition = new Vector3(0f, y, 0f);
		SafteyNet = gameObject;
		MySafteyNetSprite = gameObject.GetComponentInChildren<UISprite>();
		UpdateCachedComponents();
	}

	public void AddTutorialPath(int tutorialId)
	{
		PathElement pathElement = this;
		PathElement pathElement2 = PathNorth;
		while (pathElement2 != null)
		{
			if (pathElement2.Type == PathType.kPathStraightVert)
			{
				pathElement = pathElement2;
				pathElement2 = pathElement2.GetPath(PathDirection.kPathDirectionNorth);
				continue;
			}
			Debug.LogError("ERROR: This is not a straight path, can't extend");
			return;
		}
		switch (tutorialId)
		{
		case 0:
		{
			Debug.Log("ADD TUTORIAL: Turns");
			for (int j = 0; j < 6; j++)
			{
				pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
				pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
				pathElement = pathElement2;
			}
			pathElement2 = InstantiatePathElement(PathType.kPathTurnB);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			for (int k = 0; k < 6; k++)
			{
				pathElement2 = InstantiatePathElement(PathType.kPathStraightHorz);
				pathElement.SetPath(pathElement2, PathDirection.kPathDirectionEast);
				pathElement = pathElement2;
			}
			pathElement2 = InstantiatePathElement(PathType.kPathTurnA);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionEast);
			pathElement = pathElement2;
			for (int l = 0; l < 4; l++)
			{
				pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
				pathElement.SetPath(pathElement2, PathDirection.kPathDirectionSouth);
				pathElement = pathElement2;
			}
			pathElement2 = InstantiatePathElement(PathType.kPathTurnD);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionSouth);
			pathElement = pathElement2;
			for (int m = 0; m < 2; m++)
			{
				pathElement2 = InstantiatePathElement(PathType.kPathStraightHorz);
				pathElement.SetPath(pathElement2, PathDirection.kPathDirectionEast);
				pathElement = pathElement2;
			}
			pathElement2 = InstantiatePathElement(PathType.kPathTurnC);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionEast);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement.IsTutorialEnd = true;
			break;
		}
		case 1:
		{
			Debug.Log("ADD TUTORIAL: Jumps");
			pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
			pathElement2.AddObstacle(ObstacleType.kObstacleTreeStumble);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			for (int num5 = 0; num5 < 4; num5++)
			{
				pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
				pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
				pathElement = pathElement2;
			}
			pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
			pathElement2.AddObstacle(ObstacleType.kObstacleTreeJump);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			for (int num6 = 0; num6 < 4; num6++)
			{
				pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
				pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
				pathElement = pathElement2;
			}
			pathElement2 = InstantiatePathElement(PathType.kPathStraightHoleVert);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement.IsTutorialEnd = true;
			break;
		}
		case 2:
		{
			Debug.Log("ADD TUTORIAL: Slides");
			pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
			pathElement2.AddObstacle(ObstacleType.kObstacleTreeSlide);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			for (int n = 0; n < 5; n++)
			{
				pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
				pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
				pathElement = pathElement2;
			}
			pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
			pathElement2.AddObstacle(ObstacleType.kObstacleTreeSlide);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			for (int num = 0; num < 5; num++)
			{
				pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
				pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
				pathElement = pathElement2;
			}
			pathElement2 = InstantiatePathElement(PathType.kPathStraightFlameTowerHighVert);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement.IsTutorialEnd = true;
			break;
		}
		case 3:
		{
			Debug.Log("ADD TUTORIAL: Tilts");
			sAllowCoins = true;
			sAllowCenterCoins = false;
			sAllowCenterCoins = false;
			sAllowSideCoins = true;
			sAllowJumpCoins = false;
			sAllowSlantCoins = false;
			int num2 = sMinSpacesBetweenCoinRuns;
			int num3 = sMaxCoinsPerRun;
			sMinSpacesBetweenCoinRuns = 2;
			sMaxCoinsPerRun = 3;
			for (int num4 = 0; num4 < 6; num4++)
			{
				pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
				pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
				pathElement2.UpdatePathDescription(pathElement);
				pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
				pathElement = pathElement2;
			}
			pathElement.CoinRunCount = 0;
			pathElement.SpacesSinceLastCoinRun = 4;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightNarrowLongLeftAVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightNarrowLongLeftVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightNarrowLongLeftVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightNarrowLongLeftBVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement.SpacesSinceLastCoinRun = 0;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement.CoinRunCount = 0;
			pathElement.SpacesSinceLastCoinRun = 4;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightNarrowLongRightAVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightNarrowLongRightVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightNarrowLongRightVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightNarrowLongRightBVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement.SpacesSinceLastCoinRun = 0;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement.CoinRunCount = 0;
			pathElement.SpacesSinceLastCoinRun = 4;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightNarrowLongLeftAVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightNarrowLongLeftVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightNarrowLongLeftBVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
			pathElement2.AddCoins(PathDirection.kPathDirectionNorth, pathElement);
			pathElement2.UpdatePathDescription(pathElement);
			pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
			pathElement = pathElement2;
			pathElement.IsTutorialEnd = true;
			sAllowCoins = false;
			sAllowCenterCoins = true;
			sAllowSideCoins = true;
			sAllowJumpCoins = true;
			sAllowSlantCoins = false;
			sMinSpacesBetweenCoinRuns = num2;
			sMaxCoinsPerRun = num3;
			break;
		}
		case 4:
		{
			Debug.Log("ADD TUTORIAL: Straight");
			for (int i = 0; i < 1; i++)
			{
				pathElement2 = InstantiatePathElement(PathType.kPathStraightVert);
				pathElement.SetPath(pathElement2, PathDirection.kPathDirectionNorth);
				pathElement = pathElement2;
			}
			pathElement.IsTutorialEnd = true;
			break;
		}
		}
	}

	private bool SetPath(PathElement path, PathDirection direction)
	{
		if (path != null && DoesPieceTypeFitOnSide(path.Type, direction, false))
		{
			Vector3 position = base.transform.position;
			switch (direction)
			{
			case PathDirection.kPathDirectionNorth:
				path.transform.position = new Vector3(position.x, position.y, position.z + 60f);
				PathNorth = path;
				PathNorth.PathSouth = this;
				break;
			case PathDirection.kPathDirectionEast:
				path.transform.position = new Vector3(position.x + 60f, position.y, position.z);
				PathEast = path;
				PathEast.PathWest = this;
				break;
			case PathDirection.kPathDirectionSouth:
				path.transform.position = new Vector3(position.x, position.y, position.z - 60f);
				PathSouth = path;
				PathSouth.PathNorth = this;
				break;
			case PathDirection.kPathDirectionWest:
				path.transform.position = new Vector3(position.x - 60f, position.y, position.z);
				PathWest = path;
				PathWest.PathEast = this;
				break;
			}
			path.UpdateCachedComponents();
			if (IsTurn() && path.HasObstacles)
			{
				path.IsTutorialObstacle = false;
			}
			return true;
		}
		return false;
	}

	public void AddRandomPath(int depth, PathDirection origin)
	{
		if (depth == 0)
		{
			return;
		}
		if (origin != PathDirection.kPathDirectionNorth && CanPathExtendOnSide(PathDirection.kPathDirectionNorth))
		{
			if (PathNorth != null)
			{
				PathNorth.AddRandomPath(depth - 1, PathDirection.kPathDirectionSouth);
			}
			else
			{
				PathElement randomPathElementForSide = GetRandomPathElementForSide(PathDirection.kPathDirectionNorth);
				if (randomPathElementForSide != null)
				{
					SetPath(randomPathElementForSide, PathDirection.kPathDirectionNorth);
					randomPathElementForSide.AddRandomPath(depth - 1, PathDirection.kPathDirectionSouth);
				}
			}
		}
		if (origin != PathDirection.kPathDirectionEast && CanPathExtendOnSide(PathDirection.kPathDirectionEast))
		{
			if (PathEast != null)
			{
				PathEast.AddRandomPath(depth - 1, PathDirection.kPathDirectionWest);
			}
			else
			{
				PathElement randomPathElementForSide2 = GetRandomPathElementForSide(PathDirection.kPathDirectionEast);
				if (randomPathElementForSide2 != null)
				{
					SetPath(randomPathElementForSide2, PathDirection.kPathDirectionEast);
					randomPathElementForSide2.AddRandomPath(depth - 1, PathDirection.kPathDirectionWest);
				}
			}
		}
		if (origin != PathDirection.kPathDirectionSouth && CanPathExtendOnSide(PathDirection.kPathDirectionSouth))
		{
			if (PathSouth != null)
			{
				PathSouth.AddRandomPath(depth - 1, PathDirection.kPathDirectionNorth);
			}
			else
			{
				PathElement randomPathElementForSide3 = GetRandomPathElementForSide(PathDirection.kPathDirectionSouth);
				if (randomPathElementForSide3 != null)
				{
					SetPath(randomPathElementForSide3, PathDirection.kPathDirectionSouth);
					randomPathElementForSide3.AddRandomPath(depth - 1, PathDirection.kPathDirectionNorth);
				}
			}
		}
		if (origin == PathDirection.kPathDirectionWest || !CanPathExtendOnSide(PathDirection.kPathDirectionWest))
		{
			return;
		}
		if (PathWest != null)
		{
			PathWest.AddRandomPath(depth - 1, PathDirection.kPathDirectionEast);
			return;
		}
		PathElement randomPathElementForSide4 = GetRandomPathElementForSide(PathDirection.kPathDirectionWest);
		if (randomPathElementForSide4 != null)
		{
			SetPath(randomPathElementForSide4, PathDirection.kPathDirectionWest);
			randomPathElementForSide4.AddRandomPath(depth - 1, PathDirection.kPathDirectionEast);
		}
	}

	public void UpdatePathDescription(PathElement parentPath)
	{
		if (parentPath == null)
		{
			return;
		}
		if (IsTurn())
		{
			SpacesSinceLastTurn = 0;
		}
		else
		{
			SpacesSinceLastTurn = parentPath.SpacesSinceLastTurn + 1;
		}
		if (HasObstacles)
		{
			SpacesSinceLastObstacle = 0;
			if (DoesSidePathHaveObstacles())
			{
				BackToBackObstacleCount = parentPath.BackToBackObstacleCount + 1;
			}
			if (Obstacles == ObstacleType.kObstacleTreeSlide || Obstacles == ObstacleType.kObstacleFlameTower)
			{
				SpacesSinceLastTurn = 0;
			}
		}
		else
		{
			SpacesSinceLastObstacle = parentPath.SpacesSinceLastObstacle + 1;
			BackToBackObstacleCount = 0;
		}
		if (HasCoins)
		{
			CoinRunCount = parentPath.CoinRunCount + 1;
			if (CoinRunCount >= sMaxCoinsPerRun)
			{
				SpacesSinceLastCoinRun = 0;
			}
			else
			{
				SpacesSinceLastCoinRun = parentPath.SpacesSinceLastCoinRun;
			}
		}
		else
		{
			SpacesSinceLastCoinRun = parentPath.SpacesSinceLastCoinRun + 1;
			CoinRunCount = 0;
		}
		if (HasBonusItems)
		{
			SpacesSinceLastBonusItem = 0;
			SpacesSinceLastTurn = 0;
			SpacesSinceLastObstacle = 0;
		}
		else if (SkippedBonusItems)
		{
			SpacesSinceLastBonusItem = 0;
		}
		else
		{
			SpacesSinceLastBonusItem = parentPath.SpacesSinceLastBonusItem + 1;
		}
		if (IsPathTypeStepDown(Type) || IsPathTypeStepDown(parentPath.Type))
		{
			SpacesSinceLastObstacle = 0;
			SpacesSinceLastTurn = 0;
		}
	}

	public void OnSpawned()
	{
		Type = PathType.kPathNone;
		Level = PathLevel.kPathLevelNone;
		Obstacles = ObstacleType.kObstacleNone;
		HasObstacles = (HasCoins = (HasBonusItems = (SkippedBonusItems = (IsTutorialObstacle = (IsTutorialEnd = (IsCoinRunInAir = false))))));
		SpacesSinceLastBonusItem = 0;
		SpacesSinceLastCoinRun = 0;
		SpacesSinceLastObstacle = 0;
		SpacesSinceLastTurn = 0;
		BackToBackObstacleCount = 0;
		CoinRunCount = 0;
		TileIndex = 0;
		LastRenderState = RenderState.NotSet;
		IsInView = false;
		IsTutorialEnd = false;
	}

	public void OnDespawned()
	{
		primaryRenderer = null;
		if ((bool)SafteyNet)
		{
			Object.Destroy(SafteyNet);
			SafteyNet = null;
		}
		RemoveAllSubObjects();
		PathNorth = (PathSouth = (PathEast = (PathWest = null)));
	}

	public void Prune(PathDirection origin)
	{
		if (origin != PathDirection.kPathDirectionNorth && PathNorth != null)
		{
			PathNorth.Prune(PathDirection.kPathDirectionSouth);
			PathNorth.DestroySelf();
			PathNorth = null;
		}
		if (origin != PathDirection.kPathDirectionEast && PathEast != null)
		{
			PathEast.Prune(PathDirection.kPathDirectionWest);
			PathEast.DestroySelf();
			PathEast = null;
		}
		if (origin != PathDirection.kPathDirectionSouth && PathSouth != null)
		{
			PathSouth.Prune(PathDirection.kPathDirectionNorth);
			PathSouth.DestroySelf();
			PathSouth = null;
		}
		if (origin != PathDirection.kPathDirectionWest && PathWest != null)
		{
			PathWest.Prune(PathDirection.kPathDirectionEast);
			PathWest.DestroySelf();
			PathWest = null;
		}
	}

	private void RemoveAllSubObjects()
	{
		SpawnPool spawnPool = PoolManager.Pools["BonusItems"];
		BonusItem[] componentsInChildren = base.transform.GetComponentsInChildren<BonusItem>();
		BonusItem[] array = componentsInChildren;
		foreach (BonusItem bonusItem in array)
		{
			spawnPool.Despawn(bonusItem.transform);
		}
		MyRenderers = null;
		MySafteyNetSprite = null;
		IsInView = false;
		UpdateCachedComponents();
	}

	public void UpdateCachedComponents()
	{
		MyRenderers = GetComponentsInChildren<Renderer>();
		Rectangle = new Rect(base.transform.position.x - 30f, base.transform.position.z - 30f, 60f, 60f);
	}

	public PathElement GetNearestPathElement(Vector2 point, PathDirection origin, bool isRootNode, int maxTurns = 3)
	{
		if (Rectangle.Contains(point))
		{
			return this;
		}
		int num = maxTurns;
		if (IsTurn() && --num <= 0)
		{
			return null;
		}
		if ((origin != PathDirection.kPathDirectionNorth || isRootNode) && PathNorth != null)
		{
			PathElement nearestPathElement = PathNorth.GetNearestPathElement(point, PathDirection.kPathDirectionSouth, false, num);
			if (nearestPathElement != null)
			{
				return nearestPathElement;
			}
		}
		if ((origin != PathDirection.kPathDirectionEast || isRootNode) && PathEast != null)
		{
			PathElement nearestPathElement2 = PathEast.GetNearestPathElement(point, PathDirection.kPathDirectionWest, false, num);
			if (nearestPathElement2 != null)
			{
				return nearestPathElement2;
			}
		}
		if ((origin != PathDirection.kPathDirectionSouth || isRootNode) && PathSouth != null)
		{
			PathElement nearestPathElement3 = PathSouth.GetNearestPathElement(point, PathDirection.kPathDirectionNorth, false, num);
			if (nearestPathElement3 != null)
			{
				return nearestPathElement3;
			}
		}
		if ((origin != PathDirection.kPathDirectionWest || isRootNode) && PathWest != null)
		{
			PathElement nearestPathElement4 = PathWest.GetNearestPathElement(point, PathDirection.kPathDirectionEast, false, num);
			if (nearestPathElement4 != null)
			{
				return nearestPathElement4;
			}
		}
		return null;
	}

	public PathElement GetPathElementIfPointOverTurn(Vector3 point, float radius, PathDirection origin, bool isRootNode, int bailOut = 3)
	{
		return GetPathElementIfPointOverTurn2D(new Vector2(point.x, point.z), radius, origin, isRootNode, bailOut);
	}

	public PathElement GetPathElementIfPointOverTurn2D(Vector2 point, float radius, PathDirection origin, bool isRootNode, int bailOut = 3)
	{
		if (IsTurn())
		{
			Vector2 b = new Vector2(base.transform.position.x, base.transform.position.z);
			float num = Vector2.Distance(point, b);
			if (num <= radius)
			{
				return this;
			}
		}
		bailOut--;
		if (bailOut <= 0)
		{
			return null;
		}
		if ((origin != PathDirection.kPathDirectionNorth || isRootNode) && PathNorth != null)
		{
			PathElement pathElementIfPointOverTurn2D = PathNorth.GetPathElementIfPointOverTurn2D(point, radius, PathDirection.kPathDirectionSouth, false, bailOut);
			if (pathElementIfPointOverTurn2D != null)
			{
				return pathElementIfPointOverTurn2D;
			}
		}
		if ((origin != PathDirection.kPathDirectionEast || isRootNode) && PathEast != null)
		{
			PathElement pathElementIfPointOverTurn2D2 = PathEast.GetPathElementIfPointOverTurn2D(point, radius, PathDirection.kPathDirectionWest, false, bailOut);
			if (pathElementIfPointOverTurn2D2 != null)
			{
				return pathElementIfPointOverTurn2D2;
			}
		}
		if ((origin != PathDirection.kPathDirectionSouth || isRootNode) && PathSouth != null)
		{
			PathElement pathElementIfPointOverTurn2D3 = PathSouth.GetPathElementIfPointOverTurn2D(point, radius, PathDirection.kPathDirectionNorth, false, bailOut);
			if (pathElementIfPointOverTurn2D3 != null)
			{
				return pathElementIfPointOverTurn2D3;
			}
		}
		if ((origin != PathDirection.kPathDirectionWest || isRootNode) && PathWest != null)
		{
			PathElement pathElementIfPointOverTurn2D4 = PathWest.GetPathElementIfPointOverTurn2D(point, radius, PathDirection.kPathDirectionEast, false, bailOut);
			if (pathElementIfPointOverTurn2D4 != null)
			{
				return pathElementIfPointOverTurn2D4;
			}
		}
		return null;
	}

	public bool IsPointOverTurnArea(Vector3 point, float radius)
	{
		return IsPointOverTurnArea(new Vector2(point.x, point.z), radius);
	}

	public bool IsPointOverTurnArea(Vector2 point, float radius)
	{
		if (!IsTurn())
		{
			return false;
		}
		TurnRect.Set(base.transform.position.x - radius, base.transform.position.z - radius, radius * 2f, radius * 2f);
		return TurnRect.Contains(point);
	}

	public PathElement GetPathElementIfPointOverTutorialObstacle(Vector3 point, float radius, PathDirection origin, bool isRootNode)
	{
		return GetPathElementIfPointOverTutorialObstacle(new Vector2(point.x, point.z), radius, origin, isRootNode);
	}

	public PathElement GetPathElementIfPointOverTutorialObstacle(Vector2 point, float radius, PathDirection origin, bool isRootNode)
	{
		if (HasObstacles || IsTurn())
		{
			TutorialRect.Set(base.transform.position.x - radius, base.transform.position.z - radius, radius * 2f, radius * 2f);
			if (TutorialRect.Contains(point))
			{
				return this;
			}
		}
		if ((origin != PathDirection.kPathDirectionNorth || isRootNode) && PathNorth != null)
		{
			PathElement pathElementIfPointOverTutorialObstacle = PathNorth.GetPathElementIfPointOverTutorialObstacle(point, radius, PathDirection.kPathDirectionSouth, false);
			if (pathElementIfPointOverTutorialObstacle != null)
			{
				return pathElementIfPointOverTutorialObstacle;
			}
		}
		if ((origin != PathDirection.kPathDirectionEast || isRootNode) && PathEast != null)
		{
			PathElement pathElementIfPointOverTutorialObstacle2 = PathEast.GetPathElementIfPointOverTutorialObstacle(point, radius, PathDirection.kPathDirectionWest, false);
			if (pathElementIfPointOverTutorialObstacle2 != null)
			{
				return pathElementIfPointOverTutorialObstacle2;
			}
		}
		if ((origin != PathDirection.kPathDirectionSouth || isRootNode) && PathSouth != null)
		{
			PathElement pathElementIfPointOverTutorialObstacle3 = PathSouth.GetPathElementIfPointOverTutorialObstacle(point, radius, PathDirection.kPathDirectionNorth, false);
			if (pathElementIfPointOverTutorialObstacle3 != null)
			{
				return pathElementIfPointOverTutorialObstacle3;
			}
		}
		if ((origin != PathDirection.kPathDirectionWest || isRootNode) && PathWest != null)
		{
			PathElement pathElementIfPointOverTutorialObstacle4 = PathWest.GetPathElementIfPointOverTutorialObstacle(point, radius, PathDirection.kPathDirectionEast, false);
			if (pathElementIfPointOverTutorialObstacle4 != null)
			{
				return pathElementIfPointOverTutorialObstacle4;
			}
		}
		return null;
	}

	private void SetRendering(bool enable)
	{
		if (!(primaryRenderer != null) || MyRenderers == null || MyRenderers.Length <= 0)
		{
			return;
		}
		IsInView = primaryRenderer.IsVisibleFrom(Camera.main);
		if (enable && !IsInView)
		{
			enable = false;
		}
		if ((enable && LastRenderState == RenderState.Enabled) || (!enable && LastRenderState == RenderState.Disabled))
		{
			return;
		}
		Renderer[] myRenderers = MyRenderers;
		foreach (Renderer renderer in myRenderers)
		{
			renderer.enabled = enable;
		}
		if (enable)
		{
			base.gameObject.SetActiveRecursively(true);
			BonusItem[] componentsInChildren = GetComponentsInChildren<BonusItem>();
			BonusItem[] array = componentsInChildren;
			foreach (BonusItem bonusItem in array)
			{
				bonusItem.enabled = true;
			}
		}
		LastRenderState = (enable ? RenderState.Enabled : RenderState.Disabled);
	}

	public void RecursiveVisibility(bool activeFlag, PathDirection origin, bool isRootNode, int turnCount = 0)
	{
		if (isRootNode)
		{
			activeFlag = true;
		}
		else if (activeFlag)
		{
			if (IsTurn())
			{
				turnCount++;
			}
			if (turnCount > 2)
			{
				activeFlag = false;
			}
		}
		SetRendering(activeFlag);
		if (activeFlag && MySafteyNetSprite != null)
		{
			if (sSafetyNetEnabled)
			{
				MySafteyNetSprite.enabled = true;
				MySafteyNetSprite.color = sSafteyNetColor;
			}
			else
			{
				MySafteyNetSprite.enabled = false;
			}
		}
		if ((origin != PathDirection.kPathDirectionNorth || isRootNode) && PathNorth != null)
		{
			PathNorth.RecursiveVisibility(activeFlag, PathDirection.kPathDirectionSouth, false, turnCount);
		}
		if ((origin != PathDirection.kPathDirectionEast || isRootNode) && PathEast != null)
		{
			PathEast.RecursiveVisibility(activeFlag, PathDirection.kPathDirectionWest, false, turnCount);
		}
		if ((origin != PathDirection.kPathDirectionSouth || isRootNode) && PathSouth != null)
		{
			PathSouth.RecursiveVisibility(activeFlag, PathDirection.kPathDirectionNorth, false, turnCount);
		}
		if ((origin != PathDirection.kPathDirectionWest || isRootNode) && PathWest != null)
		{
			PathWest.RecursiveVisibility(activeFlag, PathDirection.kPathDirectionEast, false, turnCount);
		}
	}

	public bool IsPointInsideAnyRegion2D(Vector3 point)
	{
		Region2D[] componentsInChildren = base.transform.GetComponentsInChildren<Region2D>();
		Region2D[] array = componentsInChildren;
		foreach (Region2D region2D in array)
		{
			if (region2D.IsPointInside(point))
			{
				return true;
			}
		}
		return false;
	}

	private bool SideHasCoins(PathDirection direciton)
	{
		PathElement path = GetPath(direciton);
		return path != null && path.HasCoins;
	}

	public bool DoesSidePathhaveCoins()
	{
		if (SideHasCoins(PathDirection.kPathDirectionNorth))
		{
			return true;
		}
		if (SideHasCoins(PathDirection.kPathDirectionEast))
		{
			return true;
		}
		if (SideHasCoins(PathDirection.kPathDirectionSouth))
		{
			return true;
		}
		if (SideHasCoins(PathDirection.kPathDirectionWest))
		{
			return true;
		}
		return false;
	}

	private static void ResetItem(BonusItem item1)
	{
		float angularVelocity = item1.AngularVelocity;
		item1.ResetSimulation();
		item1.AngularVelocity = angularVelocity;
		item1.enabled = false;
	}

	public void AddCoins(PathDirection direction, PathElement parentPiece)
	{
		if (parentPiece == null || HasCoins || !sAllowCoins || parentPiece.CoinRunCount >= sMaxCoinsPerRun || parentPiece.SpacesSinceLastCoinRun <= sMinSpacesBetweenCoinRuns || (HasObstacles && Obstacles != ObstacleType.kObstacleTreeSlide && Obstacles != ObstacleType.kObstacleNarrow && !sAllowJumpCoins) || Obstacles == ObstacleType.kObstacleLevelChange || Type.IsAnyOf(PathType.kPathTUp, PathType.kPathTDown, PathType.kPathTLeft, PathType.kPathTRight))
		{
			return;
		}
		BonusItem.BonusItemType type = BonusItem.BonusItemType.kBonusItemCoin;
		BonusItem.BonusItemType type2 = BonusItem.BonusItemType.kBonusItemCoin;
		BonusItem.BonusItemType type3 = BonusItem.BonusItemType.kBonusItemCoin;
		if (sAllowDoubleCoins && !sAllowTripleCoins)
		{
			type = BonusItem.BonusItemType.kBonusItemCoin;
			type2 = BonusItem.BonusItemType.kBonusItemCoinDouble;
			type3 = BonusItem.BonusItemType.kBonusItemCoin;
		}
		else if (sAllowDoubleCoins && sAllowTripleCoins)
		{
			type = BonusItem.BonusItemType.kBonusItemCoinDouble;
			type2 = BonusItem.BonusItemType.kBonusItemCoinTriple;
			type3 = BonusItem.BonusItemType.kBonusItemCoin;
		}
		else if (!sAllowDoubleCoins && sAllowTripleCoins)
		{
			type = BonusItem.BonusItemType.kBonusItemCoinTriple;
			type2 = BonusItem.BonusItemType.kBonusItemCoinTriple;
			type3 = BonusItem.BonusItemType.kBonusItemCoin;
		}
		SpawnPool spawnPool = PoolManager.Pools["BonusItems"];
		BonusItem bonusItem = BonusItem.Instantiate(type);
		bonusItem.transform.eulerAngles = new Vector3(0f, -90f, 0f);
		bonusItem.transform.parent = base.transform;
		BonusItem bonusItem2 = BonusItem.Instantiate(type);
		bonusItem2.transform.eulerAngles = new Vector3(0f, -54f, 0f);
		bonusItem2.transform.parent = base.transform;
		BonusItem bonusItem3 = BonusItem.Instantiate(type2);
		bonusItem3.transform.eulerAngles = new Vector3(0f, -18f, 0f);
		bonusItem3.transform.parent = base.transform;
		BonusItem bonusItem4 = BonusItem.Instantiate(type2);
		bonusItem4.transform.eulerAngles = new Vector3(0f, -18f, 0f);
		bonusItem4.transform.parent = base.transform;
		BonusItem bonusItem5 = BonusItem.Instantiate(type3);
		bonusItem5.transform.eulerAngles = new Vector3(0f, 54f, 0f);
		bonusItem5.transform.parent = base.transform;
		ResetItem(bonusItem);
		ResetItem(bonusItem2);
		ResetItem(bonusItem3);
		ResetItem(bonusItem4);
		ResetItem(bonusItem5);
		bonusItem.transform.localPosition = Vector3.zero;
		bonusItem2.transform.localPosition = Vector3.zero;
		bonusItem3.transform.localPosition = Vector3.zero;
		bonusItem4.transform.localPosition = Vector3.zero;
		bonusItem5.transform.localPosition = Vector3.zero;
		if (IsStraightVert())
		{
			bonusItem.transform.localPosition = new Vector3(0f, 3f, -25f);
			bonusItem2.transform.localPosition = new Vector3(0f, 3f, -12.5f);
			bonusItem3.transform.localPosition = new Vector3(0f, 3f, 0f);
			bonusItem4.transform.localPosition = new Vector3(0f, 3f, 12.5f);
			bonusItem5.transform.localPosition = new Vector3(0f, 3f, 25f);
		}
		else if (IsStraightHorz())
		{
			bonusItem.transform.localPosition = new Vector3(25f, 3f, 0f);
			bonusItem2.transform.localPosition = new Vector3(12.5f, 3f, 0f);
			bonusItem3.transform.localPosition = new Vector3(0f, 3f, 0f);
			bonusItem4.transform.localPosition = new Vector3(-12.5f, 3f, 0f);
			bonusItem5.transform.localPosition = new Vector3(-25f, 3f, 0f);
		}
		else if (Type == PathType.kPathTurnA)
		{
			bonusItem.transform.localPosition = new Vector3(0f, 3f, -25f);
			bonusItem2.transform.localPosition = new Vector3(0f, 3f, -12.5f);
			bonusItem3.transform.localPosition = new Vector3(0f, 3f, 0f);
			bonusItem4.transform.localPosition = new Vector3(-12.5f, 3f, 0f);
			bonusItem5.transform.localPosition = new Vector3(-25f, 3f, 0f);
		}
		else if (Type == PathType.kPathTurnB)
		{
			bonusItem.transform.localPosition = new Vector3(0f, 3f, -25f);
			bonusItem2.transform.localPosition = new Vector3(0f, 3f, -12.5f);
			bonusItem3.transform.localPosition = new Vector3(0f, 3f, 0f);
			bonusItem4.transform.localPosition = new Vector3(12.5f, 3f, 0f);
			bonusItem5.transform.localPosition = new Vector3(25f, 3f, 0f);
		}
		else if (Type == PathType.kPathTurnC)
		{
			bonusItem.transform.localPosition = new Vector3(0f, 3f, 25f);
			bonusItem2.transform.localPosition = new Vector3(0f, 3f, 12.5f);
			bonusItem3.transform.localPosition = new Vector3(0f, 3f, 0f);
			bonusItem4.transform.localPosition = new Vector3(-12.5f, 3f, 0f);
			bonusItem5.transform.localPosition = new Vector3(-25f, 3f, 0f);
		}
		else if (Type == PathType.kPathTurnD)
		{
			bonusItem.transform.localPosition = new Vector3(0f, 3f, 25f);
			bonusItem2.transform.localPosition = new Vector3(0f, 3f, 12.5f);
			bonusItem3.transform.localPosition = new Vector3(0f, 3f, 0f);
			bonusItem4.transform.localPosition = new Vector3(12.5f, 3f, 0f);
			bonusItem5.transform.localPosition = new Vector3(25f, 3f, 0f);
		}
		float localY = GetPathHeight() + 3f;
		float localY2 = GetPathHeight() + 3f;
		float localY3 = GetPathHeight() + 3f;
		float localY4 = GetPathHeight() + 3f;
		float localY5 = GetPathHeight() + 3f;
		if (HasObstacles && Obstacles != ObstacleType.kObstacleTreeSlide && Obstacles != ObstacleType.kObstacleNarrow && ((Obstacles == ObstacleType.kObstacleFlameTower && Random.Range(0, 2) == 0 && sAllowJumpCoins) || Obstacles != ObstacleType.kObstacleFlameTower))
		{
			localY = GetPathHeight() + 18f;
			localY2 = GetPathHeight() + 20f;
			localY3 = GetPathHeight() + 20f;
			localY4 = GetPathHeight() + 20f;
			localY5 = GetPathHeight() + 18f;
			IsCoinRunInAir = true;
		}
		bool flag = sAllowCenterCoins;
		bool flag2 = sAllowSideCoins;
		bool flag3 = sAllowSideCoins;
		bool flag4 = sAllowSlantCoins;
		bool flag5 = sAllowSlantCoins;
		bool flag6 = sAllowSlantCoins;
		bool flag7 = sAllowSlantCoins;
		if (Obstacles == ObstacleType.kObstacleNarrow)
		{
			if (Type == PathType.kPathStraightNarrowSmallHorz || Type == PathType.kPathStraightNarrowSmallVert)
			{
				flag2 = false;
				flag3 = false;
			}
			else if (Type == PathType.kPathStraightNarrowLongUpHorz || Type == PathType.kPathStraightNarrowLongUpAHorz || Type == PathType.kPathStraightNarrowLongUpBHorz)
			{
				switch (direction)
				{
				case PathDirection.kPathDirectionEast:
					flag3 = false;
					break;
				case PathDirection.kPathDirectionWest:
					flag2 = false;
					break;
				}
			}
			else if (Type == PathType.kPathStraightNarrowLongDownHorz || Type == PathType.kPathStraightNarrowLongDownAHorz || Type == PathType.kPathStraightNarrowLongDownBHorz)
			{
				switch (direction)
				{
				case PathDirection.kPathDirectionEast:
					flag2 = false;
					break;
				case PathDirection.kPathDirectionWest:
					flag3 = false;
					break;
				}
			}
			else if (Type == PathType.kPathStraightNarrowLongLeftVert || Type == PathType.kPathStraightNarrowLongLeftAVert || Type == PathType.kPathStraightNarrowLongLeftBVert)
			{
				switch (direction)
				{
				case PathDirection.kPathDirectionNorth:
					flag3 = false;
					break;
				case PathDirection.kPathDirectionSouth:
					flag2 = false;
					break;
				}
			}
			else if (Type == PathType.kPathStraightNarrowLongRightVert || Type == PathType.kPathStraightNarrowLongRightAVert || Type == PathType.kPathStraightNarrowLongRightBVert)
			{
				switch (direction)
				{
				case PathDirection.kPathDirectionNorth:
					flag2 = false;
					break;
				case PathDirection.kPathDirectionSouth:
					flag3 = false;
					break;
				}
			}
			flag = false;
			flag5 = false;
			flag4 = false;
			flag7 = false;
			flag6 = false;
		}
		if (parentPiece != null)
		{
			if (parentPiece.CoinLocationEnd == CoinRunLocation.kCoinRunLeft)
			{
				flag = false;
				flag3 = false;
				flag5 = false;
				flag4 = false;
				flag6 = false;
			}
			else if (parentPiece.CoinLocationEnd == CoinRunLocation.kCoinRunCenter)
			{
				flag2 = false;
				flag3 = false;
				flag5 = false;
				flag7 = false;
			}
			else if (parentPiece.CoinLocationEnd == CoinRunLocation.kCoinRunRight)
			{
				flag = false;
				flag2 = false;
				flag7 = false;
				flag4 = false;
				flag6 = false;
			}
			if (IsTurn())
			{
				if (parentPiece.CoinLocationEnd != CoinRunLocation.kCoinRunCenter)
				{
					spawnPool.Despawn(bonusItem.transform);
					spawnPool.Despawn(bonusItem2.transform);
					spawnPool.Despawn(bonusItem3.transform);
					spawnPool.Despawn(bonusItem4.transform);
					spawnPool.Despawn(bonusItem5.transform);
					return;
				}
				flag2 = false;
				flag3 = false;
				flag5 = false;
				flag4 = false;
				flag7 = false;
				flag6 = false;
			}
			if (parentPiece.IsCoinRunInAir && !IsCoinRunInAir)
			{
				localY = GetPathHeight() + 15f;
				localY2 = GetPathHeight() + 11f;
				localY3 = GetPathHeight() + 6f;
				localY4 = GetPathHeight() + 1f;
				localY5 = GetPathHeight();
			}
		}
		int num = 0;
		int[] array = new int[7];
		if (flag)
		{
			array[num] = 0;
			num++;
		}
		if (flag2)
		{
			array[num] = 1;
			num++;
		}
		if (flag3)
		{
			array[num] = 2;
			num++;
		}
		if (flag4)
		{
			array[num] = 3;
			num++;
		}
		if (flag5)
		{
			array[num] = 4;
			num++;
		}
		if (flag6)
		{
			array[num] = 5;
			num++;
		}
		if (flag7)
		{
			array[num] = 6;
			num++;
		}
		int num2 = 0;
		if (num > 0)
		{
			num2 = array[Random.Range(0, num)];
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			switch (num2)
			{
			case 0:
				CoinLocationStart = CoinRunLocation.kCoinRunCenter;
				CoinLocationEnd = CoinRunLocation.kCoinRunCenter;
				break;
			case 1:
				num3 = 7f;
				num4 = 7f;
				num5 = 7f;
				num6 = 7f;
				num7 = 7f;
				CoinLocationStart = CoinRunLocation.kCoinRunLeft;
				CoinLocationEnd = CoinRunLocation.kCoinRunLeft;
				break;
			case 2:
				num3 = -7f;
				num4 = -7f;
				num5 = -7f;
				num6 = -7f;
				num7 = -7f;
				CoinLocationStart = CoinRunLocation.kCoinRunRight;
				CoinLocationEnd = CoinRunLocation.kCoinRunRight;
				break;
			case 3:
				num3 = 0f;
				num4 = 1.4f;
				num5 = 2.8f;
				num6 = 4.2f;
				num7 = 6.6f;
				CoinLocationStart = CoinRunLocation.kCoinRunCenter;
				CoinLocationEnd = CoinRunLocation.kCoinRunLeft;
				break;
			case 4:
				num3 = -6.6f;
				num4 = -4.2f;
				num5 = -2.8f;
				num6 = -1.4f;
				num7 = 0f;
				CoinLocationStart = CoinRunLocation.kCoinRunRight;
				CoinLocationEnd = CoinRunLocation.kCoinRunCenter;
				break;
			case 5:
				num3 = 0f;
				num4 = -1.4f;
				num5 = -2.8f;
				num6 = -4.2f;
				num7 = -6.6f;
				CoinLocationStart = CoinRunLocation.kCoinRunCenter;
				CoinLocationEnd = CoinRunLocation.kCoinRunRight;
				break;
			case 6:
				num3 = 6.6f;
				num4 = 4.2f;
				num5 = 2.8f;
				num6 = 1.4f;
				num7 = 0f;
				CoinLocationStart = CoinRunLocation.kCoinRunLeft;
				CoinLocationEnd = CoinRunLocation.kCoinRunCenter;
				break;
			}
			if (Level == PathLevel.kPathLevelCliff)
			{
				num3 *= 0.8f;
				num4 *= 0.8f;
				num5 *= 0.8f;
				num6 *= 0.8f;
				num7 *= 0.8f;
			}
			if (IsStraightVert())
			{
				switch (direction)
				{
				case PathDirection.kPathDirectionNorth:
					bonusItem.SetLocalX(0f - num3);
					bonusItem2.SetLocalX(0f - num4);
					bonusItem3.SetLocalX(0f - num5);
					bonusItem4.SetLocalX(0f - num6);
					bonusItem5.SetLocalX(0f - num7);
					bonusItem.SetLocalY(localY);
					bonusItem2.SetLocalY(localY2);
					bonusItem3.SetLocalY(localY3);
					bonusItem4.SetLocalY(localY4);
					bonusItem5.SetLocalY(localY5);
					break;
				case PathDirection.kPathDirectionSouth:
					bonusItem.SetLocalX(num7);
					bonusItem2.SetLocalX(num6);
					bonusItem3.SetLocalX(num5);
					bonusItem4.SetLocalX(num4);
					bonusItem5.SetLocalX(num3);
					bonusItem.SetLocalY(localY5);
					bonusItem2.SetLocalY(localY4);
					bonusItem3.SetLocalY(localY3);
					bonusItem4.SetLocalY(localY2);
					bonusItem5.SetLocalY(localY);
					break;
				}
			}
			else if (IsStraightHorz())
			{
				switch (direction)
				{
				case PathDirection.kPathDirectionEast:
					bonusItem.SetLocalZ(num3);
					bonusItem2.SetLocalZ(num4);
					bonusItem3.SetLocalZ(num5);
					bonusItem4.SetLocalZ(num6);
					bonusItem5.SetLocalZ(num7);
					bonusItem.SetLocalY(localY5);
					bonusItem2.SetLocalY(localY4);
					bonusItem3.SetLocalY(localY3);
					bonusItem4.SetLocalY(localY2);
					bonusItem5.SetLocalY(localY);
					break;
				case PathDirection.kPathDirectionWest:
					bonusItem.SetLocalZ(0f - num7);
					bonusItem2.SetLocalZ(0f - num6);
					bonusItem3.SetLocalZ(0f - num5);
					bonusItem4.SetLocalZ(0f - num4);
					bonusItem5.SetLocalZ(0f - num3);
					bonusItem.SetLocalY(localY);
					bonusItem2.SetLocalY(localY2);
					bonusItem3.SetLocalY(localY3);
					bonusItem4.SetLocalY(localY4);
					bonusItem5.SetLocalY(localY5);
					break;
				}
			}
			else
			{
				bonusItem.SetLocalY(localY);
				bonusItem2.SetLocalY(localY2);
				bonusItem3.SetLocalY(localY3);
				bonusItem4.SetLocalY(localY4);
				bonusItem5.SetLocalY(localY5);
			}
			HasCoins = true;
		}
		else
		{
			spawnPool.Despawn(bonusItem.transform);
			spawnPool.Despawn(bonusItem2.transform);
			spawnPool.Despawn(bonusItem3.transform);
			spawnPool.Despawn(bonusItem4.transform);
			spawnPool.Despawn(bonusItem5.transform);
		}
	}

	public void AddBonusItems(PathDirection direction, PathElement parentPiece)
	{
		if (!(parentPiece == null) && sAllowBonusItems && parentPiece.SpacesSinceLastBonusItem > sMinSpacesBetweenBonusItems && parentPiece.Obstacles != ObstacleType.kObstacleTreeSlide && !HasCoins && !HasBonusItems && parentPiece.SpacesSinceLastTurn > 2 && !IsTurn() && !(parentPiece.GetPathHeight() < GetPathHeight()) && Obstacles != ObstacleType.kObstacleTreeSlide)
		{
			float num = GetPathHeight() + 20f;
			if (parentPiece.GetPathHeight() > GetPathHeight())
			{
				num = parentPiece.GetPathHeight() + 20f;
			}
			if (Obstacles == ObstacleType.kObstacleFlameTower)
			{
				num += 5f;
			}
			BonusItem.BonusItemType bonusItemType = BonusItem.BonusItemType.kBonusItemNone;
			int num2 = 0;
			BonusItem.BonusItemType[] array = new BonusItem.BonusItemType[6];
			if (sProbabilityCoinBonus > 0f)
			{
				array[num2] = BonusItem.BonusItemType.kBonusItemCoinBonus;
				num2++;
			}
			if (sProbabilityInvincibility > 0f)
			{
				array[num2] = BonusItem.BonusItemType.kBonusItemInvincibility;
				num2++;
			}
			if (sProbabilityVacuum > 0f)
			{
				array[num2] = BonusItem.BonusItemType.kBonusItemVacuum;
				num2++;
			}
			if (sProbabilityBoost > 0f)
			{
				array[num2] = BonusItem.BonusItemType.kBonusItemBoost;
				num2++;
			}
			if (num2 > 0 && Random.Range(0, 2) == 0)
			{
				bonusItemType = array[Random.Range(0, num2)];
			}
			if (bonusItemType != 0)
			{
				BonusItem bonusItem = BonusItem.Instantiate(bonusItemType);
				bonusItem.transform.parent = base.transform;
				bonusItem.transform.localPosition = new Vector3(0f, num, 0f);
				HasBonusItems = true;
			}
			else
			{
				Debug.Log("... but no bonus placed, skipped! (" + num2 + ")");
				SkippedBonusItems = true;
			}
		}
	}
}
