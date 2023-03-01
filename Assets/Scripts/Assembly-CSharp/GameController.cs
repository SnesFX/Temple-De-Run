using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public PathElement PathRoot;

	public PathElement.PathDirection PathRootOrigin;

	public PathElement LastTurnPathPiece;

	public Region2D SafetyNetRegion;

	public int TutorialID;

	public GameCamera MainCamera;

	public MovingObject MainCameraFocusPoint;

	public SceneryManager Scenery;

	public CommonPlayer Player;

	public CharacterPlayer GamePlayer;

	public GameInput GameInput;

	public UISprite Countdown;

	public PathElement NearestElement;

	public PathElement LastNearestElement;

	public PathElement.PathLevel LastLevel;

	public GameGUI GameInterface;

	public AwardedObjectives GameOverInterface;

	public RecordManager RecordManager;

	public PlayerManager PlayerManager;

	public UISprite TutorialSprite;

	public UISprite TutorialTiltSprite;

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

	public float HeadStartStartDistance = 25f;

	public float HeadStartEndDistance = 100f;

	public float HeadStartBoostDistance = 950f;

	public float HeadStartMegaBoostDistance = 2350f;

	public bool HandelingEndGame;

	public bool IsInCountdown;

	public float TimeSinceCountdownStarted;

	public bool IsStartOfCountDown;

	public bool Has3SoundPlayed;

	public bool Has2SoundPlayed;

	public bool Has1SoundPlayed;

	public int EnemyCount = 8;

	private List<Enemy> Enemies = new List<Enemy>();

	public float EnemySpreadLeftRight = 12f;

	public float EnemySpreadFrontBack = 6f;

	public float EnemyFollowDistance = 7.5f;

	public GameObject EnemyPrefab;

	public Vector3 AccForceVector;

	public bool IsSwiping;

	public Vector2 LastSwipePoint;

	public float LastSwipeTime;

	public int RunCycleLength;

	public float TimeSinceLastCoin;

	public GameObject SafteyNetPrefab;

	public AudioManager Audio;

	public static GameController SharedInstance;

	public PauseMenu PauseGameGUI;

	public List<string> AwardedAchievements = new List<string>();

	public bool SkipFrame;

	public bool DebugHyperCoins;

	private void Start()
	{
		Debug.Log("GAME CONTROLLER: START");
		SharedInstance = this;
		LoadLevelInformation();
		Countdown.enabled = false;
		TutorialSprite.enabled = false;
		TutorialTiltSprite.enabled = false;
		StartCoroutine(WaitAndWarmResources());
		GameInterface.HideAll();
	}

	private void OnApplicationPause(bool paused)
	{
		Debug.Log("APP PAUSE: " + paused);
		if (paused && IsGameStarted && !IsGameOver)
		{
			SavedGame.SaveGame();
			PauseGameGUI.Show();
		}
		if (!paused && IsGameStarted && !IsGameOver)
		{
			SavedGame.DeleteSavedGame();
		}
	}

	private IEnumerator WaitAndWarmResources()
	{
		yield return new WaitForEndOfFrame();
		FlurryAndroid.onStartSession("EMEGPG783IXHUNKUC3D4");
		PathElement.WarmResources();
		if (!SavedGame.LoadGame())
		{
			SetMainMenuAnimation();
		}
	}

	private void OnApplicationQuit()
	{
		FlurryAndroid.onEndSession();
		SavedGame.SaveGame();
	}

	public void GameStart()
	{
		if (!IsGameStarted)
		{
			LoadLevelInformation();
			HandelingEndGame = false;
			GameInterface.ShowAll();
			IsGameStarted = true;
			TimeSinceGameStart = 0f;
			if (IsIntroScene)
			{
				MainCamera.SnapStartGame();
				MovingObject.MovingObjectPathNode movingObjectPathNode = new MovingObject.MovingObjectPathNode();
				movingObjectPathNode.TimeStamp = 0f;
				movingObjectPathNode.Location = MainCamera.transform.position;
				movingObjectPathNode.Ease = false;
				MainCamera.AddFollowingPathNode(movingObjectPathNode);
				movingObjectPathNode = new MovingObject.MovingObjectPathNode();
				movingObjectPathNode.TimeStamp = 0.25f;
				movingObjectPathNode.Location = MainCamera.EndLocation;
				movingObjectPathNode.Ease = true;
				MainCamera.AddFollowingPathNode(movingObjectPathNode);
				movingObjectPathNode = new MovingObject.MovingObjectPathNode();
				movingObjectPathNode.TimeStamp = 2.75f;
				movingObjectPathNode.Location = new Vector3(-20f, 45f, 210f);
				movingObjectPathNode.Ease = true;
				MainCamera.AddFollowingPathNode(movingObjectPathNode);
				MainCamera.StartFollowingPath();
				Audio.StartGameMusic();
				Audio.PlayFX(AudioManager.Effects.monkeys, 0.2f);
			}
		}
	}

	public void SnapMainMenu()
	{
		Debug.Log("GAME CONTROLLER: SNAP MAIN MENU");
		RestartGame();
		GameInterface.HideAll();
		IsIntroScene = true;
		MainCamera.SetMainMenuAniatmion(true);
		Audio.StartMainMenuMusic();
	}

	public void ResetTutorial()
	{
		TutorialID = 0;
		TutorialSprite.enabled = false;
		TutorialTiltSprite.enabled = false;
		IsTutorialMode = false;
		TutorialSprite.color = new Color(0f, 0f, 0f, 0f);
		TutorialTiltSprite.color = TutorialSprite.color;
	}

	public void RestartGame()
	{
		NearestElement = null;
		LastNearestElement = null;
		LastLevel = PathElement.PathLevel.kPathLevelTemple;
		TimeSinceGameStart = 0f;
		GameInterface.Reset();
		LoadLevelInformation();
		MainCamera.Reset();
		MainCameraFocusPoint.transform.position = Vector3.zero;
		if (IsIntroScene)
		{
			MainCamera.ClearFollowingPath();
			MainCamera.MainCameraFocusPoint.transform.position = new Vector3(0f, 10f, 150f);
		}
		Scenery.Reset();
		GameInterface.ShowAll();
		IsGameStarted = false;
		IsGameOver = false;
		IsGameOverFinished = false;
		AutoRestart = false;
		IsInCountdown = false;
		IsPaused = false;
	}

	public void SetMainMenuAnimation()
	{
		GameInterface.HideAll();
		MainCamera.SetMainMenuAniatmion();
		Audio.StartMainMenuMusic();
	}

	private void SetPathElementVisibility()
	{
		PathRoot.RecursiveVisibility(true, PathRootOrigin, true);
	}

	private void Update()
	{
		if (SkipFrame)
		{
			SkipFrame = false;
			SetPathElementVisibility();
			return;
		}
		if (IsResurrecting)
		{
			HandleResurrecting();
			SetPathElementVisibility();
		}
		else if (IsInCountdown)
		{
			HandleCountDown();
			SetPathElementVisibility();
		}
		else if (!IsPaused)
		{
			if (IsGameStarted)
			{
				TimeSinceGameStart += Time.smoothDeltaTime;
				TimeSinceLastPause += Time.smoothDeltaTime;
				TimeSinceLastCoin += Time.smoothDeltaTime;
			}
			if (IsIntroScene && TimeSinceGameStart > 3f)
			{
				DisableIntroScene();
			}
			if (GamePlayer == null)
			{
				ComputeDistanceTraveled();
				HandlePacing();
				Vector2 point = new Vector2(Player.transform.position.x, Player.transform.position.z);
				NearestElement = PathRoot.GetNearestPathElement(point, PathRootOrigin, true);
				Prune();
			}
			else
			{
				if (IsGameStarted && (!IsIntroScene || TimeSinceGameStart > 1f))
				{
					GamePlayer.Hold = false;
					if (GameInput.JustTurned)
					{
						GameInput.TimeSinceLastTurn += Time.smoothDeltaTime;
						if (GameInput.TimeSinceLastTurn > 0.25f)
						{
							GameInput.JustTurned = false;
						}
					}
					HandleControls();
					HandleDelayedJumping();
					HandleDelayedSliding();
					HandleDelayedTurning();
					if (!GamePlayer.IsSliding)
					{
						MainCamera.NeedsToDuckCamera = false;
					}
					Vector3 previousLocation = GamePlayer.PreviousLocation;
					Vector3 position = GamePlayer.transform.position;
					MakeSureDidntRunPastTurn(previousLocation, position);
					HandleInvincibility();
					if (!GamePlayer.IsDead)
					{
						if (!GamePlayer.IsOnGround || GamePlayer.IsSliding || !GamePlayer.HasBoost)
						{
						}
						ComputeDistanceTraveled();
						HandlePacing();
						HandleTutorialMode();
						bool flag = false;
						bool flag2 = false;
						bool isOverGround = GamePlayer.IsOverGround;
						Vector2 point2 = new Vector2(Player.transform.position.x, Player.transform.position.z);
						NearestElement = PathRoot.GetNearestPathElement(point2, PathRootOrigin, true);
						if (NearestElement != null)
						{
							LastNearestElement = NearestElement;
						}
						if (NearestElement != null)
						{
							float groundHeight = GamePlayer.GroundHeight;
							PathElement pathForward = NearestElement.GetPathForward(PathRootOrigin);
							if (NearestElement.Obstacles.IsAnyOf(PathElement.ObstacleType.kObstacleHole, PathElement.ObstacleType.kObstacleNarrow, PathElement.ObstacleType.kObstacleLevelChange) || (pathForward != null && pathForward.Obstacles == PathElement.ObstacleType.kObstacleLevelChange && NearestElement.Level != PathElement.PathLevel.kPathLevelCliff))
							{
								SafetyNetRegion.transform.position = new Vector3(NearestElement.transform.position.x, 0f, NearestElement.transform.position.z);
								SafetyNetRegion.transform.rotation = NearestElement.transform.rotation;
								if (NearestElement.IsStraightHorz())
								{
									SafetyNetRegion.transform.eulerAngles = new Vector3(0f, 90f, 0f);
								}
								else
								{
									SafetyNetRegion.transform.eulerAngles = new Vector3(0f, 0f, 0f);
								}
								if (SafetyNetRegion.IsPointInside(GamePlayer.transform.position))
								{
									flag2 = true;
									if (GamePlayer.HasInvincibility)
									{
										flag = true;
									}
								}
							}
							if (!flag && NearestElement.IsPointInsideAnyRegion2D(GamePlayer.transform.position))
							{
								flag = true;
							}
							if (flag)
							{
								if (GamePlayer.HasInvincibility && flag2)
								{
									float num = 0f;
									float pathHeight = NearestElement.GetPathHeight();
									if (pathForward != null)
									{
										pathHeight = pathForward.GetPathHeight();
										num = ((!NearestElement.IsStraightHorz()) ? ((GamePlayer.transform.position.z - NearestElement.transform.position.z) / (pathForward.transform.position.z - NearestElement.transform.position.z)) : ((GamePlayer.transform.position.x - NearestElement.transform.position.x) / (pathForward.transform.position.x - NearestElement.transform.position.x)));
										num += 0.5f;
									}
									GamePlayer.SetGroundHeight(NearestElement.GetPathHeight() + num * (pathHeight - NearestElement.GetPathHeight()));
								}
								else
								{
									GamePlayer.SetGroundHeight(NearestElement.GetPathHeight());
								}
							}
							if ((groundHeight < GamePlayer.GroundHeight || (!isOverGround && flag)) && GamePlayer.transform.position.y < GamePlayer.GroundHeight - 6f && !GamePlayer.IsDead && !GamePlayer.HasInvincibility)
							{
								GamePlayer.Kill(CharacterPlayer.DeathTypes.kDeathTypeLedge);
								Debug.Log("Killed wall");
								GamePlayer.SetVisibility(false);
								MainCamera.Shake(1f, 1f, 1f, 0f);
							}
							Prune();
						}
						if (!flag && !flag2 && !GamePlayer.IsDead)
						{
							CheckHittingScenery();
						}
						HandleBonusItemPickup(PathRoot, PathRootOrigin, true);
						HandleCollisionWithObstacles();
						GamePlayer.IsOverGround = flag;
						SimulateEnemies();
					}
				}
				if (GamePlayer.IsDead && !IsGameOver)
				{
					if (IsTutorialMode)
					{
						EndGame(2f, true);
					}
					else if (GamePlayer.HasAngelWings)
					{
						bool flag3 = ResurrectPlayer();
						UsedPowers = true;
						if (!flag3)
						{
							EndGame(0.5f, false);
						}
					}
					else
					{
						EndGame(0.5f, false);
					}
				}
				if (IsGameOver && !HandelingEndGame)
				{
					HandleEndGame();
				}
			}
			SimulateWorldElements();
			SetPathElementVisibility();
		}
		if (!IsGameOver)
		{
			return;
		}
		TimeSinceGameEnd += Time.smoothDeltaTime;
		if (TimeSinceGameEnd > EndGameDelayTime)
		{
			if (AutoRestart)
			{
				Debug.Log(">>>>>>>> AutoRestart");
				RestartGame();
				GameStart();
			}
			else
			{
				IsGameOverFinished = true;
			}
		}
	}

	private void SimulateWorldElements()
	{
		if (!IsIntroScene && Scenery != null)
		{
			Scenery.ObjectDensity = ((!(Time.smoothDeltaTime > 0.02f) || IsIntroScene) ? 0.25f : 0.1f);
			Scenery.Simulate(PathRoot, IsIntroScene);
		}
	}

	private void EndGame(float delayTime, bool autoRestart)
	{
		Debug.Log("END GAME");
		if (!IsGameOver)
		{
			IsGameOver = true;
			TimeSinceGameEnd = 0f;
			EndGameDelayTime = delayTime;
			AutoRestart = autoRestart;
			if (GamePlayer.CoinCountTotal == 0)
			{
				MaxDistanceWithoutCoins = DistanceTraveled;
			}
			UpdateAndSaveRecords();
			if (autoRestart)
			{
			}
		}
	}

	private bool ResurrectPlayer()
	{
		Debug.Log(string.Concat("RESSURECT PLAYER\nRoot: ", PathRoot, "\nLastNearestElement: ", LastNearestElement));
		Audio.StopFX();
		if (PathRoot != null)
		{
			int num = 0;
			bool flag = true;
			PathElement pathElement = null;
			PathElement pathElement2 = PathRoot;
			PathElement.PathDirection pathDirection = PathRootOrigin;
			while (pathElement == null && num < 10)
			{
				Debug.Log(string.Concat("Try: ", num, "\ncurrentNode: ", pathElement2, "\ncurrentOrigin: ", pathDirection));
				if (!pathElement2.HasObstacles && !pathElement2.IsTurn())
				{
					pathElement = pathElement2;
					Debug.Log(" SafeSpot", pathElement);
					continue;
				}
				if (pathElement2.IsTurn())
				{
					flag = false;
				}
				Debug.Log(" isStraightShot: " + flag, pathElement2);
				PathElement pathElement3 = null;
				if (pathDirection != 0)
				{
					pathElement3 = pathElement2.GetPathForward(pathDirection);
					Debug.Log(" Forward: " + pathElement3, pathElement3);
					if (pathElement3 == null)
					{
						pathElement3 = pathElement2.GetPathLeft(pathDirection);
						if (pathElement3 != null)
						{
							switch (pathDirection)
							{
							case PathElement.PathDirection.kPathDirectionNorth:
								pathDirection = PathElement.PathDirection.kPathDirectionWest;
								break;
							case PathElement.PathDirection.kPathDirectionSouth:
								pathDirection = PathElement.PathDirection.kPathDirectionEast;
								break;
							case PathElement.PathDirection.kPathDirectionEast:
								pathDirection = PathElement.PathDirection.kPathDirectionNorth;
								break;
							case PathElement.PathDirection.kPathDirectionWest:
								pathDirection = PathElement.PathDirection.kPathDirectionSouth;
								break;
							}
						}
					}
					if (pathElement3 == null)
					{
						pathElement3 = pathElement2.GetPathRight(pathDirection);
						Debug.Log(" Right: " + pathElement3, pathElement3);
						if (pathElement3 != null)
						{
							switch (pathDirection)
							{
							case PathElement.PathDirection.kPathDirectionNorth:
								pathDirection = PathElement.PathDirection.kPathDirectionEast;
								break;
							case PathElement.PathDirection.kPathDirectionSouth:
								pathDirection = PathElement.PathDirection.kPathDirectionWest;
								break;
							case PathElement.PathDirection.kPathDirectionEast:
								pathDirection = PathElement.PathDirection.kPathDirectionSouth;
								break;
							case PathElement.PathDirection.kPathDirectionWest:
								pathDirection = PathElement.PathDirection.kPathDirectionNorth;
								break;
							}
						}
					}
					if (pathElement3 == null)
					{
						pathElement3 = pathElement2.GetPathBackward(pathDirection);
						Debug.Log(" Left: " + pathElement3, pathElement3);
						if (pathElement3 != null)
						{
							switch (pathDirection)
							{
							case PathElement.PathDirection.kPathDirectionNorth:
								pathDirection = PathElement.PathDirection.kPathDirectionSouth;
								break;
							case PathElement.PathDirection.kPathDirectionSouth:
								pathDirection = PathElement.PathDirection.kPathDirectionNorth;
								break;
							case PathElement.PathDirection.kPathDirectionEast:
								pathDirection = PathElement.PathDirection.kPathDirectionWest;
								break;
							case PathElement.PathDirection.kPathDirectionWest:
								pathDirection = PathElement.PathDirection.kPathDirectionEast;
								break;
							}
						}
					}
				}
				else
				{
					pathElement3 = pathElement2.GetPath(PathElement.PathDirection.kPathDirectionNorth);
					if (pathElement3 != null)
					{
						pathDirection = PathElement.PathDirection.kPathDirectionSouth;
					}
					else
					{
						pathElement3 = pathElement2.GetPath(PathElement.PathDirection.kPathDirectionSouth);
						if (pathElement3 != null)
						{
							pathDirection = PathElement.PathDirection.kPathDirectionNorth;
						}
						else
						{
							pathElement3 = pathElement2.GetPath(PathElement.PathDirection.kPathDirectionEast);
							if (pathElement3 != null)
							{
								pathDirection = PathElement.PathDirection.kPathDirectionWest;
							}
							else
							{
								pathElement3 = pathElement2.GetPath(PathElement.PathDirection.kPathDirectionWest);
								if (pathElement3 != null)
								{
									pathDirection = PathElement.PathDirection.kPathDirectionEast;
								}
								else
								{
									Debug.LogError("NO PLACE TO RESSURECT PLAYER! (1)");
								}
							}
						}
					}
					Debug.Log(" Random: " + pathElement3, pathElement3);
				}
				pathElement2 = pathElement3;
				num++;
			}
			if (!(pathElement == null))
			{
				Debug.Log("Found A Safe Spot: " + pathElement.name, pathElement);
				PathRoot = pathElement;
				PathRootOrigin = pathDirection;
				IsResurrecting = true;
				TimeSinceResurrectStart = 0f;
				ResurrectionCount++;
				GameInput.Reset();
				GamePlayer.transform.position = pathElement.transform.position;
				GamePlayer.SetGroundHeight(pathElement.GetPathHeight());
				GamePlayer.Resurrect();
				GamePlayer.transform.eulerAngles = new Vector3(0f, 0f, 0f);
				if (PathRootOrigin == PathElement.PathDirection.kPathDirectionNorth)
				{
					GamePlayer.TurnLeft();
					GamePlayer.TurnLeft();
				}
				else if (PathRootOrigin == PathElement.PathDirection.kPathDirectionEast)
				{
					GamePlayer.TurnLeft();
				}
				else if (PathRootOrigin == PathElement.PathDirection.kPathDirectionWest)
				{
					GamePlayer.TurnRight();
				}
				GamePlayer.Velocity = GamePlayer.transform.forward * GamePlayer.StartRunVelocity;
				GamePlayer.SetVisibility(true);
				GamePlayer.StartInvcibility(6f);
				GamePlayer.SetAlpha(0f);
				PathElement.sSafteyNetColor = Color.white;
				PathElement.sSafetyNetEnabled = true;
				MainCamera.GroundHeight = GamePlayer.GroundHeight;
				MainCameraFocusPoint.ClearFollowingPath();
				Vector3 position = GamePlayer.transform.position;
				position.y += MainCamera.FocusHeightRun;
				Vector3 eulerAngles = GamePlayer.transform.eulerAngles;
				eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y + 180f, eulerAngles.z);
				MovingObject.MovingObjectPathNode movingObjectPathNode;
				if (flag)
				{
					MainCameraFocusPoint.transform.position = position;
					MainCameraFocusPoint.transform.eulerAngles = eulerAngles;
				}
				else
				{
					movingObjectPathNode = new MovingObject.MovingObjectPathNode();
					movingObjectPathNode.TimeStamp = 0f;
					movingObjectPathNode.Location = MainCameraFocusPoint.transform.position;
					movingObjectPathNode.Ease = true;
					MainCameraFocusPoint.AddFollowingPathNode(movingObjectPathNode);
					Vector3 position2 = GamePlayer.transform.position;
					position2.y = MainCamera.GroundHeight + MainCamera.FocusHeightRun;
					MainCameraFocusPoint.AddFollowingPathNode(movingObjectPathNode);
					MainCameraFocusPoint.transform.eulerAngles = eulerAngles;
					movingObjectPathNode = new MovingObject.MovingObjectPathNode();
					movingObjectPathNode.TimeStamp = 2f;
					movingObjectPathNode.Location = position2;
					movingObjectPathNode.Ease = true;
					MainCameraFocusPoint.AddFollowingPathNode(movingObjectPathNode);
					MainCameraFocusPoint.StartFollowingPath();
				}
				MainCamera.ClearFollowingPath();
				MainCamera.TargetCameraLocation = GamePlayer.transform.position + MainCameraFocusPoint.transform.forward * MainCamera.FollowDistanceRun + MainCameraFocusPoint.transform.up * MainCamera.FollowHeightRun;
				movingObjectPathNode = new MovingObject.MovingObjectPathNode();
				movingObjectPathNode.TimeStamp = 0f;
				movingObjectPathNode.Location = MainCamera.transform.position;
				movingObjectPathNode.Ease = true;
				MainCamera.AddFollowingPathNode(movingObjectPathNode);
				movingObjectPathNode = new MovingObject.MovingObjectPathNode();
				movingObjectPathNode.TimeStamp = 1f;
				movingObjectPathNode.Location = MainCamera.transform.position;
				movingObjectPathNode.Ease = true;
				MainCamera.AddFollowingPathNode(movingObjectPathNode);
				movingObjectPathNode = new MovingObject.MovingObjectPathNode();
				movingObjectPathNode.TimeStamp = ((!flag) ? 3f : 2f);
				movingObjectPathNode.Location = MainCamera.TargetCameraLocation;
				movingObjectPathNode.Ease = true;
				MainCamera.AddFollowingPathNode(movingObjectPathNode);
				MainCamera.StartFollowingPath();
				return true;
			}
			Debug.LogError("NO PLACE TO RESSURECT PLAYER! (2)");
		}
		else
		{
			Debug.LogError("Can't find a place to resurrect root path is null");
		}
		return false;
	}

	private void Prune()
	{
		if (!(NearestElement != PathRoot))
		{
			return;
		}
		LastTurnPathPiece = null;
		PathElement.PathDirection pathDirection = Player.TravelingDirection();
		PathElement.PathDirection opposite = PathElement.GetOpposite(pathDirection);
		if (pathDirection != 0)
		{
			if (!IsIntroScene)
			{
				PathRoot.Prune(pathDirection);
			}
			PathRootOrigin = opposite;
			PathRoot = NearestElement;
			if (PathRoot != null)
			{
				PathRoot.AddRandomPath(10, PathRootOrigin);
			}
		}
	}

	private void HandleResurrecting()
	{
		TimeSinceResurrectStart += Time.smoothDeltaTime;
		if (TimeSinceResurrectStart >= 1f && !MainCamera.IsFollowingPath && !MainCameraFocusPoint.IsFollowingPath)
		{
			IsResurrecting = false;
			Unpause();
		}
	}

	private void HandleCountDown()
	{
		if (!IsStartOfCountDown)
		{
			TimeSinceCountdownStarted += Time.smoothDeltaTime;
		}
		else
		{
			Audio.StopFX();
			Audio.PlayCoin();
			Has1SoundPlayed = false;
			Has2SoundPlayed = false;
			Has3SoundPlayed = true;
			IsStartOfCountDown = false;
			Countdown.enabled = true;
			GamePlayer.StopAllAnimation();
		}
		int num = Mathf.FloorToInt(4f - TimeSinceCountdownStarted);
		Countdown.spriteName = num.ToString();
		if (TimeSinceCountdownStarted > 1f && !Has2SoundPlayed)
		{
			Audio.PlayCoin();
			Has2SoundPlayed = true;
		}
		else if (TimeSinceCountdownStarted > 2f && !Has1SoundPlayed)
		{
			Audio.PlayCoin();
			Has1SoundPlayed = true;
		}
		if (TimeSinceCountdownStarted > 3f)
		{
			GamePlayer.AnimateObject.Play();
			Countdown.enabled = false;
			IsInCountdown = false;
			if (GamePlayer.IsSliding)
			{
				Audio.PlayFX(AudioManager.Effects.slide);
			}
			if (GamePlayer.HasVacuum)
			{
				Audio.PlayFX(AudioManager.Effects.magnet);
			}
			if (GamePlayer.HasInvincibility && !GamePlayer.HasBoost)
			{
				Audio.PlayFX(AudioManager.Effects.shimmer);
			}
			if (GamePlayer.HasBoost)
			{
				Audio.PlayFX(AudioManager.Effects.boostLoop);
			}
		}
		if (GamePlayer.GetAlpha() <= 0.5f)
		{
			float alpha = GamePlayer.GetAlpha();
			alpha += Time.smoothDeltaTime * 0.5f;
			if (alpha > 0.5f)
			{
				alpha = 0.5f;
			}
			GamePlayer.SetAlpha(alpha);
		}
	}

	private void DisableIntroScene()
	{
		IsIntroScene = false;
	}

	private void HandleDelayedJumping()
	{
		if (!GameInput.ShouldJump || GamePlayer.IsDead || !GamePlayer.Jump(0f))
		{
			return;
		}
		foreach (Enemy enemy in Enemies)
		{
			float num = Vector2.Distance(GamePlayer.GetPosition2D(), enemy.GetPosition2D());
			float delay = num / GamePlayer.GetRunVelocity();
			enemy.Jump(delay);
		}
		GameInput.ShouldJump = false;
		GameInput.TimeSinceShouldJump = 0f;
	}

	private void HandleDelayedSliding()
	{
		if (GameInput.ShouldSlide && !GamePlayer.IsDead && GamePlayer.Slide())
		{
			GameInput.ShouldSlide = false;
			GameInput.TimeSinceShouldSlide = 0f;
		}
	}

	private void HandleDelayedTurning()
	{
		if (!((PathRoot != null) & !GamePlayer.IsDead))
		{
			return;
		}
		float num = 10f;
		if (GamePlayer.GroundHeight > 7.5f)
		{
			num = 6.5f;
		}
		PathElement pathElementIfPointOverTurn = PathRoot.GetPathElementIfPointOverTurn(GamePlayer.transform.position, num, PathRootOrigin, true);
		if (!(pathElementIfPointOverTurn != null))
		{
			return;
		}
		if (pathElementIfPointOverTurn != PathRoot)
		{
			Vector2 vector = new Vector2(GamePlayer.transform.position.x, GamePlayer.transform.position.z);
			Vector2 vector2 = new Vector2(pathElementIfPointOverTurn.transform.position.x, pathElementIfPointOverTurn.transform.position.z);
			float num2 = Vector2.Distance(vector, vector2);
			Debug.Log(string.Concat("point: ", vector, "  tile: ", vector2, "  d: ", num2, "  radius: ", num));
			Debug.LogError("turnElement != PathRoot", pathElementIfPointOverTurn);
		}
		if (PathRootOrigin == PathElement.PathDirection.kPathDirectionNone)
		{
			Debug.LogError("PathRootOrigin == kPathDirectionNone", this);
		}
		bool flag = false;
		if (GamePlayer.HasBoost && pathElementIfPointOverTurn != LastTurnPathPiece)
		{
			flag = true;
			if (pathElementIfPointOverTurn.CanTurnLeft(PathRootOrigin) && pathElementIfPointOverTurn.CanTurnRight(PathRootOrigin))
			{
				if (!GameInput.ShouldTurnLeft && !GameInput.ShouldTurnRight)
				{
					if (Random.Range(0, 2) == 0)
					{
						GameInput.ShouldTurnLeft = true;
					}
					else
					{
						GameInput.ShouldTurnRight = true;
					}
				}
			}
			else if (pathElementIfPointOverTurn.CanTurnLeft(PathRootOrigin))
			{
				GameInput.ShouldTurnLeft = true;
				GameInput.ShouldTurnRight = false;
			}
			else if (pathElementIfPointOverTurn.CanTurnRight(PathRootOrigin))
			{
				GameInput.ShouldTurnRight = true;
				GameInput.ShouldTurnLeft = false;
			}
		}
		else if (PCInput.Instance != null && PCInput.Instance.joystickPosition != 0 && pathElementIfPointOverTurn != LastTurnPathPiece)
		{
			if (PCInput.Instance.joystickPosition == -1 && pathElementIfPointOverTurn.CanTurnLeft(PathRootOrigin))
			{
				GameInput.ShouldTurnLeft = true;
				GameInput.ShouldTurnRight = false;
			}
			else if (PCInput.Instance.joystickPosition == 1 && pathElementIfPointOverTurn.CanTurnRight(PathRootOrigin))
			{
				GameInput.ShouldTurnRight = true;
				GameInput.ShouldTurnLeft = false;
			}
		}
		if (GameInput.ShouldTurnLeft && pathElementIfPointOverTurn.CanTurnLeft(PathRootOrigin))
		{
			GamePlayer.TurnLeft();
			GameInput.JustTurned = true;
			GameInput.TimeSinceLastTurn = 0f;
			GameInput.ShouldTurnLeft = false;
			GameInput.ShouldTurnRight = false;
			LastTurnPathPiece = pathElementIfPointOverTurn;
		}
		else if (GameInput.ShouldTurnRight && pathElementIfPointOverTurn.CanTurnRight(PathRootOrigin))
		{
			GamePlayer.TurnRight();
			GameInput.JustTurned = true;
			GameInput.TimeSinceLastTurn = 0f;
			GameInput.ShouldTurnLeft = false;
			GameInput.ShouldTurnRight = false;
			LastTurnPathPiece = pathElementIfPointOverTurn;
		}
		else if (flag)
		{
			Debug.LogError(string.Concat("Boostbit: ", flag, "  turnElement: ", pathElementIfPointOverTurn, "  STL: ", GameInput.ShouldTurnLeft, "  STR: ", GameInput.ShouldTurnRight), pathElementIfPointOverTurn);
		}
	}

	private void HandleInvincibility()
	{
		if (GamePlayer.HasInvincibility)
		{
			PathElement.sSafetyNetEnabled = true;
			if (GamePlayer.InvincibilityTimeLeft <= 2.5f)
			{
				float a = PathElement.sSafteyNetColor.a;
				a -= Time.smoothDeltaTime * 2f;
				if (a <= 0f)
				{
					a = 1f;
				}
				PathElement.sSafteyNetColor = new Color(a, a, a, a);
			}
			else
			{
				PathElement.sSafteyNetColor = new Color(1f, 1f, 1f, 1f);
			}
		}
		else
		{
			PathElement.sSafetyNetEnabled = false;
			PathElement.sSafteyNetColor = Color.white;
		}
	}

	private void HandleTutorialMode()
	{
	}

	private void MakeSureDidntRunPastTurn(Vector3 oldPlayerPosition, Vector3 newPlayerPosition)
	{
		if (PathRoot == null)
		{
			return;
		}
		float radius = ((!(GamePlayer.GroundHeight > 7.5f)) ? 10f : 6.5f);
		Vector2 vector = new Vector2(oldPlayerPosition.x, oldPlayerPosition.z);
		if (PathRoot.IsPointOverTurnArea(vector, radius))
		{
			return;
		}
		PathElement pathElementIfPointOverTurn = PathRoot.GetPathElementIfPointOverTurn(vector, 50f, PathRootOrigin, true);
		if (!(pathElementIfPointOverTurn != null))
		{
			return;
		}
		Vector2 point = new Vector2(newPlayerPosition.x, newPlayerPosition.z);
		if (!PathRoot.IsPointOverTurnArea(point, radius))
		{
			return;
		}
		int num = 5;
		Vector3 vector2 = (newPlayerPosition - oldPlayerPosition) * (1f / (float)num);
		if (!(vector2.magnitude > 0f))
		{
			return;
		}
		for (int i = 1; i < num - 1; i++)
		{
			Vector3 position = oldPlayerPosition + vector2 * i;
			Vector2 point2 = new Vector2(position.x, position.z);
			if (pathElementIfPointOverTurn.IsPointOverTurnArea(point2, radius))
			{
				GamePlayer.transform.position = position;
			}
		}
	}

	private void CheckHittingScenery()
	{
		Vector3 position = GamePlayer.transform.position;
		position.y += 3f;
		SceneryElement sceneryElementIntersectingSphere = Scenery.GetSceneryElementIntersectingSphere(position, 1f);
		if (sceneryElementIntersectingSphere != null)
		{
			switch (sceneryElementIntersectingSphere.Type)
			{
			case SceneryManager.SceneryType.kSceneryTypeTree:
				GamePlayer.Kill(CharacterPlayer.DeathTypes.kDeathTypeTree);
				break;
			case SceneryManager.SceneryType.kSceneryTypeRock:
				GamePlayer.Kill(CharacterPlayer.DeathTypes.kDeathTypeSceneryRock);
				break;
			case SceneryManager.SceneryType.kSceneryTypeCliff:
				GamePlayer.Kill(CharacterPlayer.DeathTypes.kDeathTypeSceneryRock);
				break;
			default:
				GamePlayer.Kill(CharacterPlayer.DeathTypes.kDeathTypeUnknown);
				break;
			}
			Debug.Log("Killed Scenery: " + sceneryElementIntersectingSphere.name, sceneryElementIntersectingSphere);
			GamePlayer.SetVisibility(false);
			MainCamera.Shake(1f, 1f, 1f, 0f);
		}
	}

	public void HandleBonusItemPickup(PathElement pathElement, PathElement.PathDirection origin, bool isRootNode, int bailOut = 3)
	{
		if (pathElement == null)
		{
			return;
		}
		if (pathElement.HasBonusItems || pathElement.HasCoins)
		{
			BonusItem[] componentsInChildren = pathElement.GetComponentsInChildren<BonusItem>();
			BonusItem[] array = componentsInChildren;
			foreach (BonusItem bonusItem in array)
			{
				if (!bonusItem.enabled)
				{
					continue;
				}
				float radius = 2.25f;
				if (bonusItem.BonusType == BonusItem.BonusItemType.kBonusItemCoin || bonusItem.BonusType == BonusItem.BonusItemType.kBonusItemInvincibility || bonusItem.BonusType == BonusItem.BonusItemType.kBonusItemVacuum)
				{
					radius = 3f;
				}
				if (!GamePlayer.IsDead && bonusItem.IsACoin() && (GamePlayer.HasVacuum || bonusItem.isMagnetized))
				{
					float num = Vector3.SqrMagnitude(bonusItem.transform.position - GamePlayer.transform.position);
					if (num > 0f && num <= 8100f)
					{
						Vector3 vector = GamePlayer.transform.position - bonusItem.transform.position;
						vector.Normalize();
						bonusItem.ApplyForce(vector * (1f / num) * 10000000f);
						bonusItem.isMagnetized = true;
						radius = 12f;
					}
				}
				if (!GamePlayer.DoesSphereIntersectBoundingBox(bonusItem.transform.position, radius))
				{
					continue;
				}
				bonusItem.DestroySelf();
				GamePlayer.AddScore(bonusItem.Value);
				if (bonusItem.IsACoin())
				{
					int num2 = 1;
					if (GamePlayer.HasVacuum)
					{
						num2 = CoinMagnetMultiplier;
					}
					int num3 = 1;
					if (bonusItem.BonusType == BonusItem.BonusItemType.kBonusItemCoinDouble)
					{
						num3 = 2;
					}
					if (bonusItem.BonusType == BonusItem.BonusItemType.kBonusItemCoinTriple)
					{
						num3 = 3;
					}
					GamePlayer.AddCoins(num3);
					GameInterface.SpawnSpriteParticleForCoin(num3);
					Audio.PlayCoin();
				}
				else if (bonusItem.BonusType == BonusItem.BonusItemType.kBonusItemCoinBonus)
				{
					Audio.PlayFX(AudioManager.Effects.cymbalCrash);
					GamePlayer.PickupEffect.enabled = true;
					GamePlayer.StartCoinBonus(CoinBonusValue);
					GameInterface.SpawnMegaCoinParticle(25);
				}
				else if (bonusItem.BonusType == BonusItem.BonusItemType.kBonusItemInvincibility)
				{
					UsedPowers = true;
					Audio.PlayFX(AudioManager.Effects.bonusPickup);
					GamePlayer.PickupEffect.enabled = true;
					GamePlayer.StartInvcibility(InvincibilityDuration);
				}
				else if (bonusItem.BonusType == BonusItem.BonusItemType.kBonusItemVacuum)
				{
					UsedPowers = true;
					Audio.PlayFX(AudioManager.Effects.bonusPickup);
					GamePlayer.PickupEffect.enabled = true;
					GamePlayer.StartVacuum(VacuumDuration);
				}
				else if (bonusItem.BonusType == BonusItem.BonusItemType.kBonusItemBoost)
				{
					Audio.PlayFX(AudioManager.Effects.bonusPickup);
					BoostPlayer(BoostDistance);
				}
			}
		}
		bailOut--;
		if (bailOut > 0)
		{
			if (origin != PathElement.PathDirection.kPathDirectionNorth || isRootNode)
			{
				HandleBonusItemPickup(pathElement.PathNorth, PathElement.PathDirection.kPathDirectionSouth, false, bailOut);
			}
			if (origin != PathElement.PathDirection.kPathDirectionEast || isRootNode)
			{
				HandleBonusItemPickup(pathElement.PathEast, PathElement.PathDirection.kPathDirectionWest, false, bailOut);
			}
			if (origin != PathElement.PathDirection.kPathDirectionSouth || isRootNode)
			{
				HandleBonusItemPickup(pathElement.PathSouth, PathElement.PathDirection.kPathDirectionNorth, false, bailOut);
			}
			if (origin != PathElement.PathDirection.kPathDirectionWest || isRootNode)
			{
				HandleBonusItemPickup(pathElement.PathWest, PathElement.PathDirection.kPathDirectionEast, false, bailOut);
			}
		}
	}

	public void BoostPlayer(float distance, bool isMega = false)
	{
		GamePlayer.StartBoost(distance, isMega);
		if (isMega)
		{
			MainCamera.Shake(0.75f, distance / 175f, 2f, 0f);
		}
		else
		{
			MainCamera.Shake(0.75f, distance / 100f, 2f, 0f);
		}
		Audio.PlayFX(AudioManager.Effects.boostLoop);
	}

	private void HandleCollisionWithObstacles()
	{
		if (NearestElement == null)
		{
			return;
		}
		Obstacle[] componentsInChildren = NearestElement.GetComponentsInChildren<Obstacle>();
		Obstacle[] array = componentsInChildren;
		foreach (Obstacle obstacle in array)
		{
			if (!obstacle.gameObject.active)
			{
				continue;
			}
			Region2D component = obstacle.transform.GetComponent<Region2D>();
			if (!(component != null))
			{
				continue;
			}
			if (obstacle.MustSlideUnder && GamePlayer.IsSliding)
			{
				MainCamera.NeedsToDuckCamera = true;
			}
			if (obstacle.MustSlideUnder && !obstacle.MustJumpOver && GamePlayer.HasInvincibility)
			{
				MainCamera.NeedsToDuckCamera = true;
			}
			if (component.IsPointInside(GamePlayer.GetPreviousPosition2D()))
			{
				continue;
			}
			bool flag = true;
			if (obstacle.MustJumpOver && GamePlayer.transform.position.y >= component.Height)
			{
				flag = false;
			}
			if (obstacle.MustSlideUnder && GamePlayer.IsSliding)
			{
				flag = false;
			}
			if (!flag || !obstacle.DoesLineSegementIntersect(GamePlayer.GetPreviousPosition2D(), GamePlayer.GetPosition2D()))
			{
				continue;
			}
			if (GamePlayer.HasInvincibility)
			{
				if (NearestElement.Obstacles == PathElement.ObstacleType.kObstacleFlameTower)
				{
					obstacle.gameObject.SetActiveRecursively(false);
				}
			}
			else if (obstacle.DoesKill)
			{
				if (obstacle.MustJumpOver && obstacle.MustSlideUnder)
				{
					if (NearestElement.Level == PathElement.PathLevel.kPathLevelPlank)
					{
						GamePlayer.Kill(CharacterPlayer.DeathTypes.kDeathTypeTangle);
					}
					else
					{
						GamePlayer.Kill(CharacterPlayer.DeathTypes.kDeathTypeBurnt);
					}
				}
				else if (obstacle.MustJumpOver)
				{
					GamePlayer.Kill(CharacterPlayer.DeathTypes.kDeathTypeTree);
				}
				else if (obstacle.MustSlideUnder)
				{
					GamePlayer.Kill(CharacterPlayer.DeathTypes.kDeathTypeSlide);
				}
				Debug.Log("Killed Obstacle");
				GamePlayer.SetVisibility(false);
				MainCamera.Shake(1f, 1f, 1f, 0f);
			}
			else
			{
				GamePlayer.Stumble(0f);
				MainCamera.Shake(1f, 1.5f, 1f, 0f);
				DidStumble = true;
			}
		}
	}

	private void ComputeDistanceTraveled()
	{
		if (!(GamePlayer == null) && GamePlayer.IsFalling)
		{
			return;
		}
		float num = Vector2.Distance(new Vector2(Player.transform.position.x, Player.transform.position.z), new Vector2(Player.PreviousLocation.x, Player.PreviousLocation.z));
		float num2 = num / Time.smoothDeltaTime * (1f / 60f);
		float num3 = num2 * num2 * (Time.smoothDeltaTime / (1f / 60f)) / 20f;
		DistanceTraveled += num3;
		DistanceTraveledSinceLastLevelChange += num3;
		DistanceTraveledSinceLastTurnSection += num3;
		DistanceRemainder += num3;
		int num4 = (int)DistanceRemainder;
		DistanceRemainder -= num4;
		GamePlayer.AddScore(num4);
		if (GamePlayer.HasBoost)
		{
			GamePlayer.BoostDistanceLeft -= num3;
			if (MaxDistanceWithoutCoins == 0f && GamePlayer.CoinCountTotal > 0)
			{
				MaxDistanceWithoutCoins = num3;
			}
		}
		if (ShowTutorialGuides || IsTutorialMode)
		{
			HandleTutorialHints();
		}
		if (IsIntroScene || !IsTutorialMode || !TutorialSprite.enabled)
		{
			return;
		}
		TimeSinceTutorialSectionEnded += Time.smoothDeltaTime;
		if (TimeSinceTutorialSectionEnded > 1f && TimeSinceTutorialSectionEnded < 3f)
		{
			float a = TutorialSprite.color.a;
			a += Time.smoothDeltaTime * 4f;
			if (a > 1f)
			{
				a = 1f;
			}
			TutorialSprite.color = new Color(a, a, a, a);
			TutorialTiltSprite.color = TutorialSprite.color;
		}
		else
		{
			if (!(TimeSinceTutorialSectionEnded >= 3f + TutorialTextExtraDuration))
			{
				return;
			}
			float a2 = TutorialSprite.color.a;
			a2 -= Time.smoothDeltaTime * 2f;
			if (a2 < 0f)
			{
				a2 = 0f;
				if (TutorialID > 4)
				{
					TutorialSprite.enabled = false;
					TutorialTiltSprite.enabled = false;
					Debug.Log("Exiting tutorial mode");
					TutorialID = 0;
					IsTutorialMode = false;
					PlayerPrefs.SetString("TR Tutorial", "Off");
					DistanceTraveled = 0f;
					DistanceTraveledSinceLastLevelChange = 0f;
					DistanceTraveledSinceLastTurnSection = 0f;
					MaxDistanceWithoutCoins = 0f;
				}
			}
			TutorialSprite.color = new Color(a2, a2, a2, a2);
			TutorialTiltSprite.color = TutorialSprite.color;
		}
	}

	private GUIParticle HintSprite(string name, Vector3 start, Vector3 motion, bool startAnimation = true)
	{
		GUIParticle gUIParticle = GameInterface.NextFreeParticle(name);
		gUIParticle.transform.parent = GameInterface.transform;
		gUIParticle.transform.localPosition = start;
		gUIParticle.Motion = motion;
		gUIParticle.Fade = GUIParticle.FadeType.FadeOut;
		gUIParticle.LifeSpan = 0.75f;
		gUIParticle.RotationRate = 0f;
		if (startAnimation)
		{
			gUIParticle.BeginAnimation();
		}
		return gUIParticle;
	}

	private void HandleTutorialHints()
	{
		if (!(PathRoot != null))
		{
			return;
		}
		PathElement pathElementIfPointOverTutorialObstacle = PathRoot.GetPathElementIfPointOverTutorialObstacle(GamePlayer.GetPosition2D(), 120f, PathRootOrigin, true);
		if (!(pathElementIfPointOverTutorialObstacle != null) || !pathElementIfPointOverTutorialObstacle.IsTutorialObstacle)
		{
			return;
		}
		pathElementIfPointOverTutorialObstacle.IsTutorialObstacle = false;
		PathElement pathLeft = pathElementIfPointOverTutorialObstacle.GetPathLeft(PathRootOrigin);
		if (pathLeft != null && !pathLeft.HasObstacles)
		{
			HintSprite("arrowLeft.png", new Vector3(20f, 0f, 0f), new Vector3(-300f, 0f, 0f));
			AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
		}
		PathElement pathRight = pathElementIfPointOverTutorialObstacle.GetPathRight(PathRootOrigin);
		if (pathRight != null && !pathRight.HasObstacles)
		{
			HintSprite("arrowRight.png", new Vector3(-20f, 0f, 0f), new Vector3(300f, 0f, 0f));
			AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
		}
		if (pathElementIfPointOverTutorialObstacle.HasObstacles && pathElementIfPointOverTutorialObstacle.Obstacles != PathElement.ObstacleType.kObstacleNarrow)
		{
			if (pathElementIfPointOverTutorialObstacle.Obstacles == PathElement.ObstacleType.kObstacleTreeSlide || pathElementIfPointOverTutorialObstacle.Obstacles == PathElement.ObstacleType.kObstacleFlameTower)
			{
				HintSprite("arrowDown.png", new Vector3(0f, 40f, 0f), new Vector3(0f, -300f, 0f));
				AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
			}
			else
			{
				HintSprite("arrowUp.png", new Vector3(0f, -40f, 0f), new Vector3(0f, 300f, 0f));
				AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
			}
		}
	}

	private void ChangeLevel()
	{
		switch (PathElement.sPathLevel)
		{
		case PathElement.PathLevel.kPathLevelPlank:
			PathElement.sPathLevel = PathElement.PathLevel.kPathLevelTemple;
			break;
		case PathElement.PathLevel.kPathLevelTemple:
			PathElement.sPathLevel = ((Random.Range(0, 2) == 0) ? PathElement.PathLevel.kPathLevelPlank : PathElement.PathLevel.kPathLevelCliff);
			break;
		case PathElement.PathLevel.kPathLevelCliff:
			PathElement.sPathLevel = ((Random.Range(0, 2) != 0) ? PathElement.PathLevel.kPathLevelPlank : PathElement.PathLevel.kPathLevelTemple);
			break;
		}
	}

	private void HandlePacing()
	{
		if (!IsTutorialMode)
		{
			TutorialSprite.enabled = false;
			PathElement.sAllowBonusItems = GamePlayer == null || GamePlayer.TimeSinceLastPowerup >= 15f;
			float num = ((PathElement.sPathLevel != PathElement.PathLevel.kPathLevelTemple) ? DistanceToChangeAtOther : DistanceToChangeAtTemple);
			if (DistanceTraveledSinceLastLevelChange > num)
			{
				DistanceTraveledSinceLastLevelChange = 0f;
				if (Random.Range(0, 4) <= 2)
				{
					ChangeLevel();
				}
			}
			PathElement.sAllowDoubleCoins = DistanceTraveled >= DistanceToChangeDoubleCoins;
			PathElement.sAllowTripleCoins = DistanceTraveled >= DistanceToChangeTripleCoins;
			if (DistanceTraveled < 250f)
			{
				PathElement.sAllowCoins = true;
				PathElement.sMinSpacesBetweenCoinRuns = 12;
				PathElement.sMaxCoinsPerRun = 5;
				PathElement.sAllowTurns = true;
				PathElement.sMinSpaceBetweenTurns = 12;
				PathElement.sAllowPathHoles = true;
				PathElement.sAllowTreeStumble = true;
				PathElement.sAllowTreeJump = true;
				PathElement.sMinSpaceBetweenObstacles = 6;
			}
			else if (DistanceTraveled < 500f)
			{
				PathElement.sMinSpacesBetweenCoinRuns = 10;
				PathElement.sMinSpaceBetweenTurns = 10;
				PathElement.sMinSpaceBetweenObstacles = 5;
			}
			else if (DistanceTraveled < 1000f)
			{
				PathElement.sMinSpacesBetweenCoinRuns = 9;
				PathElement.sMinSpaceBetweenTurns = 8;
				PathElement.sAllowTreeSlide = true;
				PathElement.sAllowFlameTowers = true;
				PathElement.sMinSpaceBetweenObstacles = 4;
				PathElement.sDoubleObstaclePercent = 0.05f;
			}
			else if (DistanceTraveled < 1500f)
			{
				PathElement.sMinSpacesBetweenCoinRuns = 8;
				PathElement.sMinSpaceBetweenTurns = 6;
				DistanceToChangeAtTemple = 450f;
				DistanceToChangeAtOther = 250f;
				DistanceToTurnSection = 850f;
			}
			else if (DistanceTraveled < 2000f)
			{
				PathElement.sMinSpacesBetweenCoinRuns = 7;
				PathElement.sAllowPathNarrows = true;
				PathElement.sDoubleObstaclePercent = 0.1f;
				PathElement.sMinSpaceBetweenTurns = 5;
				PathElement.sMinSpaceBetweenObstacles = 3;
			}
			else if (DistanceTraveled < 3250f)
			{
				PathElement.sMinSpacesBetweenCoinRuns = 6;
				PathElement.sDoubleObstaclePercent = 0.15f;
				PathElement.sMinSpaceBetweenTurns = 4;
			}
			else if (DistanceTraveled < 4500f)
			{
				PathElement.sMinSpacesBetweenCoinRuns = 6;
				PathElement.sDoubleObstaclePercent = 0.175f;
				PathElement.sMinSpaceBetweenTurns = 3;
			}
			else if (DistanceTraveled >= 4500f)
			{
				DistanceToChangeAtTemple = 375f;
				DistanceToChangeAtOther = 300f;
				DistanceToTurnSection = 750f;
				PathElement.sMinSpacesBetweenCoinRuns = 6;
				PathElement.sDoubleObstaclePercent = 0.2f;
				PathElement.sMinSpaceBetweenTurns = 2;
				PathElement.sMinSpaceBetweenObstacles = 2;
			}
			if ((!PathElement.sIsFastTurnSection && DistanceTraveledSinceLastTurnSection > DistanceToTurnSection) || (PathElement.sIsFastTurnSection && DistanceTraveledSinceLastTurnSection > DistanceToTurnSectionEnd))
			{
				DistanceTraveledSinceLastTurnSection = 0f;
				if (Random.Range(0, 4) <= 2)
				{
					PathElement.sIsFastTurnSection = !PathElement.sIsFastTurnSection;
				}
			}
			if (PathElement.sIsFastTurnSection)
			{
				PathElement.sMinSpaceBetweenTurns = (int)((float)PathElement.sMinSpaceBetweenTurns * 0.65f);
				if (PathElement.sMinSpaceBetweenTurns < 2)
				{
					PathElement.sMinSpaceBetweenTurns = 2;
				}
				PathElement.sMinSpacesBetweenCoinRuns = 0;
			}
			if (!IsNewHighScore && GamePlayer.Score > HighScore)
			{
				GUIParticle gUIParticle = HintSprite("messageHighScore.png", new Vector3(0f, 400f, 0f), Vector3.zero, false);
				gUIParticle.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
				gUIParticle.FinalScale = new Vector3(3f, 3f, 1f);
				gUIParticle.LifeSpan = 2f;
				gUIParticle.Gravity = true;
				gUIParticle.Fade = GUIParticle.FadeType.FadeOut;
				gUIParticle.BeginAnimation();
				AudioManager.Instance.PlayFX(AudioManager.Effects.woohoo);
				IsNewHighScore = true;
			}
			return;
		}
		if (PathRoot.IsTutorialEnd)
		{
			PathRoot.IsTutorialEnd = false;
			if (TutorialID < 4)
			{
				GUIParticle gUIParticle2 = HintSprite("messageNiceJob.png", new Vector3(0f, 400f, 0f), Vector3.zero, false);
				gUIParticle2.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
				gUIParticle2.FinalScale = new Vector3(3f, 3f, 1f);
				gUIParticle2.LifeSpan = 2f;
				gUIParticle2.Gravity = true;
				gUIParticle2.Fade = GUIParticle.FadeType.FadeOut;
				gUIParticle2.BeginAnimation();
				Audio.PlayFX(AudioManager.Effects.scoreBlast, 0.3f);
			}
			TutorialID++;
			if (TutorialID > 5)
			{
				TutorialID = 5;
			}
			TutorialSprite.color = new Color(0f, 0f, 0f, 0f);
			TutorialTextExtraDuration = 0f;
			switch (TutorialID)
			{
			case 0:
				TutorialSprite.spriteName = "instructionsTurn";
				TutorialTiltSprite.enabled = false;
				break;
			case 1:
				TutorialSprite.spriteName = "instructionsJump";
				TutorialTiltSprite.enabled = false;
				break;
			case 2:
				TutorialSprite.spriteName = "instructionsSlide";
				TutorialTiltSprite.enabled = false;
				break;
			case 3:
				TutorialSprite.spriteName = "instructionsTilt";
				TutorialTiltSprite.enabled = true;
				break;
			case 4:
				TutorialSprite.spriteName = "instructionsMeter";
				TutorialTextExtraDuration = 1f;
				TutorialTiltSprite.enabled = false;
				break;
			case 5:
				TutorialSprite.spriteName = "instructionsFinished";
				TutorialTiltSprite.enabled = false;
				break;
			}
			TutorialSprite.MakePixelPerfect();
			if (TutorialID < 5)
			{
				PathRoot.AddTutorialPath(TutorialID);
			}
			TimeSinceTutorialSectionEnded = 0f;
		}
		if (!TutorialSprite.enabled)
		{
			TutorialSprite.spriteName = "instructionsTurn";
			TutorialSprite.MakePixelPerfect();
			TutorialSprite.color = new Color(0f, 0f, 0f, 0f);
			TutorialSprite.enabled = true;
			TutorialTiltSprite.color = new Color(0f, 0f, 0f, 0f);
		}
	}

	private void HandleControls()
	{
		if (GamePlayer.IsDead || NearestElement == null)
		{
			return;
		}
		if (PCInput.Instance != null && PCInput.Instance.JoystickMode)
		{
			if (PCInput.Instance.joystickPosition == 1)
			{
				GamePlayer.PlayerXOffset = -6f;
			}
			else if (PCInput.Instance.joystickPosition == -1)
			{
				GamePlayer.PlayerXOffset = 6f;
			}
			else
			{
				GamePlayer.PlayerXOffset = 0f;
			}
		}
		Vector2 rhs = new Vector2(GamePlayer.Velocity.x, GamePlayer.Velocity.z);
		Vector2 lhs = new Vector2(GamePlayer.transform.right.x, GamePlayer.transform.right.z);
		lhs.Normalize();
		float num = Vector2.Dot(lhs, rhs);
		Vector3 force = GamePlayer.transform.right * (0f - num) * 10f;
		GamePlayer.ApplyForce(force);
		if (GamePlayer.HasBoost && GamePlayer.GroundHeight > 7.5f)
		{
			if (GamePlayer.PlayerXOffset >= 6f)
			{
				GamePlayer.PlayerXOffset = 6f;
			}
			else if (GamePlayer.PlayerXOffset <= -6f)
			{
				GamePlayer.PlayerXOffset = -6f;
			}
		}
		float num2 = Time.smoothDeltaTime;
		if (num2 > 1f / 15f)
		{
			num2 = 1f / 15f;
		}
		num2 *= 50f;
		float num3 = TouchInput.Instance.Sensitivity * 1.75f + 0.25f;
		num2 *= num3;
		if (!GamePlayer.IsFalling)
		{
			if (Mathf.Abs(GamePlayer.transform.right.x) > Mathf.Abs(GamePlayer.transform.right.z))
			{
				float x = NearestElement.transform.position.x;
				float num4 = x + (0f - GamePlayer.PlayerXOffset) * GamePlayer.transform.forward.z;
				float num5 = num4 - GamePlayer.transform.position.x;
				float num6 = num5 * 0.1f * num2;
				if (GamePlayer.HasBoost && GamePlayer.GroundHeight > 7.5f && Mathf.Abs(x - GamePlayer.transform.position.x) > 6f)
				{
					GamePlayer.SetX(GamePlayer.transform.position.x + num5);
				}
				else
				{
					GamePlayer.SetX(GamePlayer.transform.position.x + num6);
				}
			}
			else
			{
				float z = NearestElement.transform.position.z;
				float num7 = z + GamePlayer.PlayerXOffset * GamePlayer.transform.forward.x;
				float num8 = num7 - GamePlayer.transform.position.z;
				float num9 = num8 * 0.1f * num2;
				if (GamePlayer.HasBoost && GamePlayer.GroundHeight > 7.5f && Mathf.Abs(z - GamePlayer.transform.position.z) > 6f)
				{
					GamePlayer.SetZ(GamePlayer.transform.position.z + num8);
				}
				else
				{
					GamePlayer.SetZ(GamePlayer.transform.position.z + num9);
				}
			}
		}
		float runVelocity = GamePlayer.GetRunVelocity();
		float num10 = runVelocity / GamePlayer.GetMaxRunVelocity();
		if (!(runVelocity < GamePlayer.GetMaxRunVelocity()) && (!GamePlayer.HasBoost || !(GamePlayer.BoostDistanceLeft <= GamePlayer.BoostSlowdownDistance) || GamePlayer.IsJumping || GamePlayer.IsSliding))
		{
			return;
		}
		LastLevel = NearestElement.Level;
		Vector3 normalized = GamePlayer.transform.forward.normalized;
		float num11 = 45f;
		if (GamePlayer.IsDead || !(GamePlayer.DeathRunVelocity > 0f))
		{
			num11 = ((num10 < 0.5f) ? 30f : ((num10 >= 0.5f && num10 <= 0.65f) ? 10f : ((!(num10 >= 0.65f) || !(num10 <= 0.85f)) ? 0.5f : 1.5f)));
		}
		else
		{
			num11 = ((num10 < 0.5f) ? 30f : ((num10 >= 0.5f && num10 < 0.65f) ? 10f : ((!(num10 >= 0.65f) || !(num10 < 0.85f)) ? 2.5f : 7.5f)));
			if (GamePlayer.GetRunVelocity() >= GamePlayer.DeathRunVelocity * 0.85f)
			{
				GamePlayer.DeathRunVelocity = 0f;
			}
		}
		if (GamePlayer.HasBoost)
		{
			if (GamePlayer.BoostDistanceLeft > GamePlayer.BoostSlowdownDistance)
			{
				num11 = 150f;
			}
			else if (GamePlayer.GetRunVelocity() > GamePlayer.VelocityBeforeBoost)
			{
				num11 = -150f;
			}
		}
		if (!IsTutorialMode || (IsTutorialMode && num10 < 0.65f))
		{
			GamePlayer.ApplyForce(normalized * num11);
		}
	}

	public void SimulateEnemies()
	{
		if (Enemies.Count == 0)
		{
			return;
		}
		if (!IsIntroScene && Enemies.Count > 3)
		{
			for (int i = 3; i < Enemies.Count; i++)
			{
				Object.Destroy(Enemies[i].gameObject);
			}
			Enemies.RemoveRange(3, Enemies.Count - 3);
		}
		float num = GamePlayer.StumbleKillTimer / 10f;
		float num2 = 46f;
		float num3 = 23f - num2;
		float num4 = num2 * num + num3;
		if (num4 < 11.5f)
		{
			num4 = 11.5f;
		}
		else if (num4 > 25f)
		{
			num4 = 75f;
		}
		float num5 = 100f;
		if (GamePlayer.IsSliding && MainCamera.NeedsToDuckCamera)
		{
			num4 += 75f;
			num5 = 1000f;
		}
		else if (GameInput.JustTurned)
		{
			num4 += 10f;
		}
		if (GamePlayer.IsDead && GamePlayer.DeathType == CharacterPlayer.DeathTypes.kDeathTypeEaten)
		{
			num4 = 0f;
		}
		float num6 = num4 - EnemyFollowDistance;
		float num7 = 1f;
		if (num6 < 0f)
		{
			num7 = -1f;
			num5 = 100f;
		}
		float num8 = num7 * Time.smoothDeltaTime * num5;
		if (Mathf.Abs(num8) > Mathf.Abs(num6))
		{
			num8 = num6;
		}
		EnemyFollowDistance += num8;
		int num9 = -1;
		foreach (Enemy enemy in Enemies)
		{
			num9++;
			enemy.GroundHeight = GamePlayer.GroundHeight;
			if (1 == 0 || GamePlayer.IsFalling)
			{
				continue;
			}
			enemy.transform.eulerAngles = GamePlayer.transform.eulerAngles;
			float num10 = 1f * GamePlayer.GetRunVelocity();
			if (num10 < 80f)
			{
				num10 = 80f;
			}
			else if (num10 > 100f)
			{
				num10 = 100f;
			}
			enemy.JumpVelocity = num10;
			float num11 = 0f;
			float num12 = 0f;
			switch (num9)
			{
			case 0:
				num11 = 0f;
				num12 = 0f;
				break;
			case 1:
				num11 = EnemySpreadFrontBack * 0.45454547f;
				num12 = EnemySpreadLeftRight * -0.28888887f;
				break;
			case 2:
				num11 = EnemySpreadFrontBack * 0.6363636f;
				num12 = EnemySpreadLeftRight * (19f / 45f);
				break;
			case 3:
				num11 = EnemySpreadFrontBack * 4.5454545f;
				num12 = EnemySpreadLeftRight * (-1f / 6f);
				break;
			case 4:
				num11 = EnemySpreadFrontBack * 5.818182f;
				num12 = EnemySpreadLeftRight * (16f / 45f);
				break;
			case 5:
				num11 = EnemySpreadFrontBack * 7.309091f;
				num12 = EnemySpreadLeftRight * -0.46666664f;
				break;
			case 6:
				num11 = EnemySpreadFrontBack * 9.599999f;
				num12 = EnemySpreadLeftRight * (1f / 45f);
				break;
			case 7:
				num11 = EnemySpreadFrontBack * 12.818182f;
				num12 = EnemySpreadLeftRight * (8f / 15f);
				break;
			}
			if (GamePlayer.IsDead && GamePlayer.DeathType == CharacterPlayer.DeathTypes.kDeathTypeEaten)
			{
				num12 *= 0.25f;
			}
			float y = enemy.transform.position.y;
			Vector3 position = GamePlayer.transform.position;
			if (!GamePlayer.IsFalling && (!GamePlayer.IsDead || GamePlayer.DeathType != CharacterPlayer.DeathTypes.kDeathTypeEaten))
			{
				Vector2 vector = new Vector2(GamePlayer.transform.right.x, GamePlayer.transform.right.z);
				vector.Normalize();
				if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
				{
					float num13 = 1f;
					num13 = ((!(GamePlayer.transform.position.x > 0f)) ? (-1f) : 1f);
					float num14 = (float)((int)(GamePlayer.transform.position.x + num13 * 30f) / 60) * 60f;
					float num15 = num14 - GamePlayer.transform.position.x;
					position.x = GamePlayer.transform.position.x + num15 * 0.5f;
				}
				else
				{
					float num16 = 1f;
					num16 = ((!(GamePlayer.transform.position.z > 0f)) ? (-1f) : 1f);
					float num17 = (float)((int)(GamePlayer.transform.position.z + num16 * 30f) / 60) * 60f;
					float num18 = num17 - GamePlayer.transform.position.z;
					position.z = GamePlayer.transform.position.z + num18 * 0.5f;
				}
			}
			enemy.transform.position = position + GamePlayer.transform.forward * (0f - (EnemyFollowDistance + num11)) + GamePlayer.transform.right * num12;
			enemy.SetY(y);
			enemy.IsOverGround = true;
		}
	}

	public void DeleteAnyExistingPath()
	{
		if ((bool)PathRoot)
		{
			Debug.Log("PRUNING EXISTING PATH!  Starting At:" + PathRoot.name, PathRoot);
			PathRoot.Prune(PathElement.PathDirection.kPathDirectionNone);
			PathRoot.DestroySelf();
			PathRoot = null;
		}
	}

	public void LoadLevelInformation()
	{
		Debug.Log("LOAD LEVEL INFORMATION");
		DistanceRemainder = 0f;
		DistanceTraveled = 0f;
		DistanceToChangeAtTemple = 500f;
		DistanceToChangeAtOther = 200f;
		DistanceTraveledSinceLastLevelChange = 0f;
		DistanceToTurnSection = 1000f;
		DistanceToTurnSectionEnd = 100f;
		DistanceTraveledSinceLastTurnSection = 0f;
		DistanceToChangeDoubleCoins = 1E+09f;
		DistanceToChangeTripleCoins = 1E+09f;
		AverageScorePerBlock = 100f;
		Player.Reset();
		GamePlayer = Player as CharacterPlayer;
		LastBlockScore = 0;
		EnemyFollowDistance = 7.5f;
		IsSwiping = false;
		LastSwipeTime = 0f;
		MainCamera.NeedsToDuckCamera = false;
		TimeSinceTutorialSectionEnded = 0f;
		TutorialTextExtraDuration = 0f;
		IsResurrecting = false;
		TimeSinceResurrectStart = 0f;
		ResurrectionCount = 0;
		UsedHeadStart = false;
		MaxDistanceWithoutCoins = 0f;
		LastTurnPathPiece = null;
		DidStumble = false;
		UsedPowers = false;
		string @string = PlayerPrefs.GetString("TR Tutorial", "On");
		IsTutorialMode = @string == "On";
		Debug.Log("IsTutorialMode: " + IsTutorialMode);
		SetupEnemy();
		VacuumDuration = 15f;
		BoostDistance = 250f;
		InvincibilityDuration = 15f;
		CoinBonusValue = 50;
		PathElement.sProbabilityCoinBonus = 0f;
		int playerLevelForUpgradeType = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), RecordManager.StoreItemType.kStoreItemCoinBonus);
		if (playerLevelForUpgradeType == 1)
		{
			CoinBonusValue = 50;
			PathElement.sProbabilityCoinBonus = 0.2f;
		}
		else if (playerLevelForUpgradeType == 2)
		{
			CoinBonusValue = 75;
			PathElement.sProbabilityCoinBonus = 0.2f;
		}
		else if (playerLevelForUpgradeType == 3)
		{
			CoinBonusValue = 100;
			PathElement.sProbabilityCoinBonus = 0.2f;
		}
		else if (playerLevelForUpgradeType == 4)
		{
			CoinBonusValue = 125;
			PathElement.sProbabilityCoinBonus = 0.2f;
		}
		else if (playerLevelForUpgradeType >= 5)
		{
			CoinBonusValue = 150;
			PathElement.sProbabilityCoinBonus = 0.2f;
		}
		PathElement.sProbabilityInvincibility = 0f;
		playerLevelForUpgradeType = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), RecordManager.StoreItemType.kStoreItemInvincibility);
		if (playerLevelForUpgradeType == 1)
		{
			InvincibilityDuration = 15f;
			PathElement.sProbabilityInvincibility = 0.2f;
		}
		else if (playerLevelForUpgradeType == 2)
		{
			InvincibilityDuration = 17.5f;
			PathElement.sProbabilityInvincibility = 0.2f;
		}
		else if (playerLevelForUpgradeType == 3)
		{
			InvincibilityDuration = 20f;
			PathElement.sProbabilityInvincibility = 0.2f;
		}
		else if (playerLevelForUpgradeType == 4)
		{
			InvincibilityDuration = 22.5f;
			PathElement.sProbabilityInvincibility = 0.2f;
		}
		else if (playerLevelForUpgradeType >= 5)
		{
			InvincibilityDuration = 25f;
			PathElement.sProbabilityInvincibility = 0.2f;
		}
		PathElement.sProbabilityVacuum = 0f;
		playerLevelForUpgradeType = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), RecordManager.StoreItemType.kStoreItemVacuum);
		if (playerLevelForUpgradeType == 1)
		{
			VacuumDuration = 15f;
			CoinMagnetMultiplier = 1;
			PathElement.sProbabilityVacuum = 0.2f;
		}
		else if (playerLevelForUpgradeType == 2)
		{
			VacuumDuration = 20f;
			CoinMagnetMultiplier = 1;
			PathElement.sProbabilityVacuum = 0.2f;
		}
		else if (playerLevelForUpgradeType == 3)
		{
			VacuumDuration = 20f;
			CoinMagnetMultiplier = 2;
			PathElement.sProbabilityVacuum = 0.2f;
		}
		else if (playerLevelForUpgradeType == 4)
		{
			VacuumDuration = 25f;
			CoinMagnetMultiplier = 2;
			PathElement.sProbabilityVacuum = 0.2f;
		}
		else if (playerLevelForUpgradeType >= 5)
		{
			VacuumDuration = 25f;
			CoinMagnetMultiplier = 3;
			PathElement.sProbabilityVacuum = 0.2f;
		}
		PathElement.sProbabilityBoost = 0f;
		playerLevelForUpgradeType = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), RecordManager.StoreItemType.kStoreItemBoost);
		if (playerLevelForUpgradeType == 1)
		{
			BoostDistance = 250f;
			PathElement.sProbabilityBoost = 0.2f;
		}
		else if (playerLevelForUpgradeType == 2)
		{
			BoostDistance = 375f;
			PathElement.sProbabilityBoost = 0.2f;
		}
		else if (playerLevelForUpgradeType == 3)
		{
			BoostDistance = 500f;
			PathElement.sProbabilityBoost = 0.2f;
		}
		else if (playerLevelForUpgradeType == 4)
		{
			BoostDistance = 625f;
			PathElement.sProbabilityBoost = 0.2f;
		}
		else if (playerLevelForUpgradeType >= 5)
		{
			BoostDistance = 750f;
			PathElement.sProbabilityBoost = 0.2f;
		}
		playerLevelForUpgradeType = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), RecordManager.StoreItemType.kStoreItemCoinValue);
		if (playerLevelForUpgradeType == 1)
		{
			DistanceToChangeDoubleCoins = 1500f;
		}
		else if (playerLevelForUpgradeType == 2)
		{
			DistanceToChangeDoubleCoins = 1000f;
		}
		else if (playerLevelForUpgradeType == 3)
		{
			DistanceToChangeDoubleCoins = 1000f;
			DistanceToChangeTripleCoins = 3000f;
		}
		else if (playerLevelForUpgradeType == 4)
		{
			DistanceToChangeDoubleCoins = 1000f;
			DistanceToChangeTripleCoins = 2500f;
		}
		else if (playerLevelForUpgradeType >= 5)
		{
			DistanceToChangeDoubleCoins = 1000f;
			DistanceToChangeTripleCoins = 2000f;
		}
		IsNewHighScore = false;
		HighScore = RecordManager.GetBestScore(PlayerManager.GetActivePlayer());
		if (RecordManager.GetLifetimePlays(PlayerManager.GetActivePlayer()) == 0)
		{
			IsNewHighScore = true;
		}
		if (!IsTutorialMode)
		{
			PathElement.sAllowTurns = false;
			PathElement.sMinSpaceBetweenTurns = 6;
			PathElement.sMinSpaceBetweenObstacles = 6;
			PathElement.sDoubleObstaclePercent = 0f;
			PathElement.sMaxBackToBackObstacles = 2;
			PathElement.sAllowPathHoles = false;
			PathElement.sAllowPathNarrows = false;
			PathElement.sAllowFlameTowers = false;
			PathElement.sAllowTreeStumble = false;
			PathElement.sAllowTreeJump = false;
			PathElement.sAllowTreeSlide = false;
			PathElement.sAllowTurnAfterObstacles = false;
			PathElement.sAllowSidePaths = false;
			PathElement.sAllowCoins = false;
			PathElement.sAllowDoubleCoins = false;
			PathElement.sAllowTripleCoins = false;
			PathElement.sAllowCenterCoins = true;
			PathElement.sAllowSideCoins = true;
			PathElement.sAllowJumpCoins = true;
			PathElement.sAllowSlantCoins = false;
			PathElement.sMinSpacesBetweenCoinRuns = 15;
			PathElement.sMaxCoinsPerRun = 5;
			PathElement.sPathLevel = PathElement.PathLevel.kPathLevelTemple;
		}
		else
		{
			PathElement.sAllowTurns = false;
			PathElement.sMinSpaceBetweenTurns = 6;
			PathElement.sMinSpaceBetweenObstacles = 6;
			PathElement.sDoubleObstaclePercent = 0f;
			PathElement.sMaxBackToBackObstacles = 2;
			PathElement.sAllowPathHoles = false;
			PathElement.sAllowPathNarrows = false;
			PathElement.sAllowFlameTowers = false;
			PathElement.sAllowTreeStumble = false;
			PathElement.sAllowTreeJump = false;
			PathElement.sAllowTreeSlide = false;
			PathElement.sAllowTurnAfterObstacles = false;
			PathElement.sAllowSidePaths = false;
			PathElement.sAllowCoins = false;
			PathElement.sAllowDoubleCoins = false;
			PathElement.sAllowTripleCoins = false;
			PathElement.sAllowCenterCoins = true;
			PathElement.sAllowSideCoins = true;
			PathElement.sAllowJumpCoins = true;
			PathElement.sAllowSlantCoins = false;
			PathElement.sMinSpacesBetweenCoinRuns = 6;
			PathElement.sMaxCoinsPerRun = 5;
			PathElement.sPathLevel = PathElement.PathLevel.kPathLevelTemple;
		}
		GameInterface.Reset();
		DeleteAnyExistingPath();
		Player.transform.position = new Vector3(0f, 0f, 0f);
		Player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		Player.Reset();
		Player.SetVisibility(true);
		int playerLevelForUpgradeType2 = RecordManager.GetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), RecordManager.StoreItemType.kStoreItemAngelWings);
		GamePlayer.AngelWingsCount = playerLevelForUpgradeType2;
		PathElement.PathType type = PathElement.PathType.kPathStraightVert;
		int depth = 8;
		IsIntroScene = false;
		if ((IsTutorialMode && TutorialID == 0) || !IsTutorialMode)
		{
			IsIntroScene = true;
			type = PathElement.PathType.kPathTemple;
			depth = 8;
			Player.transform.position = new Vector3(0f, 0f, -10f);
		}
		Debug.Log("Intro: " + IsIntroScene);
		GameObject gameObject = PathElement.Instantiate(type);
		gameObject.transform.position = new Vector3(0f, 0f, 0f);
		(PathRoot = gameObject.GetComponent<PathElement>()).UpdateCachedComponents();
		PathRootOrigin = PathElement.PathDirection.kPathDirectionSouth;
		PathElement.sAllowVariations = false;
		PathRoot.AddRandomPath(6, PathRootOrigin);
		PathElement.sAllowVariations = true;
		PathRoot.AddRandomPath(depth, PathRootOrigin);
		if (IsTutorialMode && TutorialID < 5)
		{
			Debug.Log("Settuping tutorial ID: " + TutorialID);
			PathRoot.AddTutorialPath(TutorialID);
		}
		if (GamePlayer != null)
		{
			GamePlayer.Velocity = GamePlayer.transform.forward * GamePlayer.StartRunVelocity;
		}
		foreach (Enemy enemy in Enemies)
		{
			enemy.transform.eulerAngles = Vector3.zero;
			enemy.transform.position = Player.transform.position + Player.transform.forward * (0f - EnemyFollowDistance);
		}
		Scenery.Reset();
	}

	public void SetupEnemy()
	{
		for (int i = 0; i < Enemies.Count; i++)
		{
			Object.Destroy(Enemies[i].gameObject);
		}
		Enemies.Clear();
		for (int j = 0; j < EnemyCount; j++)
		{
			GameObject gameObject = Object.Instantiate(EnemyPrefab) as GameObject;
			Enemy component = gameObject.GetComponent<Enemy>();
			Enemies.Add(component);
		}
	}

	private void UpdateAndSaveRecords()
	{
		Debug.Log("UPDATE AND SAVE RECORDS");
		if (DebugHyperCoins)
		{
			GamePlayer.CoinCountTotal *= 1000;
		}
		RecordManager.UpdateRecordsForSession(PlayerManager.GetActivePlayer(), GamePlayer.Score, GamePlayer.CoinCountTotal, (int)DistanceTraveled);
		UpdateAchievementRecords();
		RecordManager.SaveRecords();
	}

	private void HandleEndGame()
	{
		HandelingEndGame = true;
		if (!IsTutorialMode)
		{
			GameInterface.HideAll();
			GameOverInterface.Show();
		}
	}

	public void UseAngelWings()
	{
		if (!IsPaused && !IsInCountdown && !IsGameOver && !IsTutorialMode && !IsIntroScene && !GamePlayer.IsDead && !GamePlayer.IsFalling && GamePlayer.AngelWingsRechargeTimeLeft == 0f && GamePlayer.AngelWingsCount > 0)
		{
			GamePlayer.StartAngelWings(30f);
			Audio.PlayFX(AudioManager.Effects.bonusPickup);
			Audio.PlayFX(AudioManager.Effects.angelWings, 0.75f);
			RecordManager.SetPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), RecordManager.StoreItemType.kStoreItemAngelWings, GamePlayer.AngelWingsCount);
			RecordManager.SaveRecords();
			UsedPowers = true;
		}
	}

	public void UseHeadStart()
	{
		if (!IsPaused && !IsInCountdown && !IsGameOver && !IsTutorialMode && !IsIntroScene && !GamePlayer.IsDead && !GamePlayer.IsFalling && DistanceTraveled > HeadStartStartDistance && DistanceTraveled < HeadStartEndDistance)
		{
			Audio.PlayFX(AudioManager.Effects.bonusPickup);
			RecordManager.AdjustPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), RecordManager.StoreItemType.kStoreItemHeadStart, -1);
			RecordManager.SaveRecords();
			float distance = HeadStartBoostDistance - DistanceTraveled;
			GamePlayer.StartBoost(distance, false);
			UsedHeadStart = true;
			UsedPowers = true;
		}
	}

	public void UseHeadStartMega()
	{
		if (!IsPaused && !IsInCountdown && !IsGameOver && !IsTutorialMode && !IsIntroScene && !GamePlayer.IsDead && !GamePlayer.IsFalling && DistanceTraveled > HeadStartStartDistance && DistanceTraveled < HeadStartEndDistance)
		{
			Audio.PlayFX(AudioManager.Effects.bonusPickup);
			RecordManager.AdjustPlayerLevelForUpgradeType(PlayerManager.GetActivePlayer(), RecordManager.StoreItemType.kStoreItemHeadStartMega, -1);
			RecordManager.SaveRecords();
			float distance = HeadStartMegaBoostDistance - DistanceTraveled;
			GamePlayer.StartBoost(distance, false);
			UsedHeadStart = true;
			UsedPowers = true;
		}
	}

	public void Pause()
	{
		IsPaused = true;
		CharacterPlayer.Instance.PauseAnimation();
	}

	public void Unpause()
	{
		IsPaused = false;
		IsStartOfCountDown = true;
		IsInCountdown = true;
		TimeSinceCountdownStarted = 0.1f;
		Has3SoundPlayed = false;
		Has2SoundPlayed = false;
		Has1SoundPlayed = false;
		CharacterPlayer.Instance.UnpauseAnimation();
	}

	private void UpdateAchievementRecords()
	{
		AwardedAchievements.Clear();
		int activePlayer = PlayerManager.GetActivePlayer();
		if (RecordManager.GetProgressForAchievement(activePlayer, "distance500") < 100f)
		{
			float num = DistanceTraveled / 500f * 100f;
			if (num >= 100f)
			{
				AwardedAchievements.Add("distance500");
				num = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "distance500", num);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "distance500NoCoins") < 100f)
		{
			float num2 = MaxDistanceWithoutCoins / 500f * 100f;
			if (num2 >= 100f)
			{
				AwardedAchievements.Add("distance500NoCoins");
				num2 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "distance500NoCoins", num2);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "distance1000") < 100f)
		{
			float num3 = DistanceTraveled / 1000f * 100f;
			if (num3 >= 100f)
			{
				AwardedAchievements.Add("distance1000");
				num3 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "distance1000", num3);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "distance1000NoCoins") < 100f)
		{
			float num4 = MaxDistanceWithoutCoins / 1000f * 100f;
			if (num4 >= 100f)
			{
				AwardedAchievements.Add("distance1000NoCoins");
				num4 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "distance1000NoCoins", num4);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "distance2500") < 100f)
		{
			float num5 = DistanceTraveled / 2500f * 100f;
			if (num5 >= 100f)
			{
				AwardedAchievements.Add("distance2500");
				num5 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "distance2500", num5);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "distance5000") < 100f)
		{
			float num6 = DistanceTraveled / 5000f * 100f;
			if (num6 >= 100f)
			{
				AwardedAchievements.Add("distance5000");
				num6 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "distance5000", num6);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "distance10000") < 100f)
		{
			float num7 = DistanceTraveled / 10000f * 100f;
			if (num7 >= 100f)
			{
				AwardedAchievements.Add("distance10000");
				num7 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "distance10000", num7);
		}
		if (!DidStumble && RecordManager.GetProgressForAchievement(activePlayer, "notrip5000") < 100f)
		{
			float num8 = DistanceTraveled / 5000f * 100f;
			if (num8 >= 100f)
			{
				AwardedAchievements.Add("notrip5000");
				num8 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "notrip5000", num8);
		}
		if (!DidStumble && RecordManager.GetProgressForAchievement(activePlayer, "notrip2500") < 100f)
		{
			float num9 = DistanceTraveled / 2500f * 100f;
			if (num9 >= 100f)
			{
				AwardedAchievements.Add("notrip2500");
				num9 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "notrip2500", num9);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "score25000") < 100f)
		{
			float num10 = (float)GamePlayer.Score / 25000f * 100f;
			if (num10 >= 100f)
			{
				AwardedAchievements.Add("score25000");
				num10 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "score25000", num10);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "score50000") < 100f)
		{
			float num11 = (float)GamePlayer.Score / 50000f * 100f;
			if (num11 >= 100f)
			{
				AwardedAchievements.Add("score50000");
				num11 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "score50000", num11);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "score100000") < 100f)
		{
			float num12 = (float)GamePlayer.Score / 100000f * 100f;
			if (num12 >= 100f)
			{
				AwardedAchievements.Add("score100000");
				num12 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "score100000", num12);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "score250000") < 100f)
		{
			float num13 = (float)GamePlayer.Score / 250000f * 100f;
			if (num13 >= 100f)
			{
				AwardedAchievements.Add("score250000");
				num13 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "score250000", num13);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "score500000") < 100f)
		{
			float num14 = (float)GamePlayer.Score / 500000f * 100f;
			if (num14 >= 100f)
			{
				AwardedAchievements.Add("score500000");
				num14 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "score500000", num14);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "score1000000") < 100f)
		{
			float num15 = (float)GamePlayer.Score / 1000000f * 100f;
			if (num15 >= 100f)
			{
				AwardedAchievements.Add("score1000000");
				num15 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "score1000000", num15);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "score2500000") < 100f)
		{
			float num16 = (float)GamePlayer.Score / 2500000f * 100f;
			if (num16 >= 100f)
			{
				AwardedAchievements.Add("score2500000");
				num16 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "score2500000", num16);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "score5000000") < 100f)
		{
			float num17 = (float)GamePlayer.Score / 5000000f * 100f;
			if (num17 >= 100f)
			{
				AwardedAchievements.Add("score5000000");
				num17 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "score5000000", num17);
		}
		if (!UsedPowers && RecordManager.GetProgressForAchievement(activePlayer, "score1000000nopowers") < 100f)
		{
			float num18 = (float)GamePlayer.Score / 1000000f * 100f;
			if (num18 >= 100f)
			{
				AwardedAchievements.Add("score1000000nopowers");
				num18 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "score1000000nopowers", num18);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "score10000000") < 100f)
		{
			float num19 = (float)GamePlayer.Score / 10000000f * 100f;
			if (num19 >= 100f)
			{
				AwardedAchievements.Add("score10000000");
				num19 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "score10000000", num19);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "coins100") < 100f)
		{
			float num20 = (float)GamePlayer.CoinCountTotal / 100f * 100f;
			if (num20 >= 100f)
			{
				AwardedAchievements.Add("coins100");
				num20 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "coins100", num20);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "coins250") < 100f)
		{
			float num21 = (float)GamePlayer.CoinCountTotal / 250f * 100f;
			if (num21 >= 100f)
			{
				AwardedAchievements.Add("coins250");
				num21 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "coins250", num21);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "coins500") < 100f)
		{
			float num22 = (float)GamePlayer.CoinCountTotal / 500f * 100f;
			if (num22 >= 100f)
			{
				AwardedAchievements.Add("coins500");
				num22 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "coins500", num22);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "coins750") < 100f)
		{
			float num23 = (float)GamePlayer.CoinCountTotal / 750f * 100f;
			if (num23 >= 100f)
			{
				AwardedAchievements.Add("coins750");
				num23 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "coins750", num23);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "coins1000") < 100f)
		{
			float num24 = (float)GamePlayer.CoinCountTotal / 1000f * 100f;
			if (num24 >= 100f)
			{
				AwardedAchievements.Add("coins1000");
				num24 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "coins1000", num24);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "coins2500") < 100f)
		{
			float num25 = (float)GamePlayer.CoinCountTotal / 2500f * 100f;
			if (num25 >= 100f)
			{
				AwardedAchievements.Add("coins2500");
				num25 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "coins2500", num25);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "coins5000") < 100f)
		{
			float num26 = (float)GamePlayer.CoinCountTotal / 5000f * 100f;
			if (num26 >= 100f)
			{
				AwardedAchievements.Add("coins5000");
				num26 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "coins5000", num26);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "totemFull") < 100f)
		{
			float num27 = (float)GamePlayer.BonusLevelMax / 5f * 100f;
			if (num27 >= 100f)
			{
				AwardedAchievements.Add("totemFull");
				num27 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "totemFull", num27);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "resurrection") < 100f)
		{
			float num28 = (float)ResurrectionCount / 1f * 100f;
			if (num28 >= 100f)
			{
				AwardedAchievements.Add("resurrection");
				num28 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "resurrection", num28);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "resurrection2") < 100f)
		{
			float num29 = (float)ResurrectionCount / 2f * 100f;
			if (num29 >= 100f)
			{
				AwardedAchievements.Add("resurrection2");
				num29 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "resurrection2", num29);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "headstart") < 100f)
		{
			float num30 = 0f;
			if (UsedHeadStart)
			{
				num30 = 100f;
			}
			if (num30 >= 100f)
			{
				AwardedAchievements.Add("headstart");
				num30 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "headstart", num30);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "upgrade1") < 100f)
		{
			int num31 = 0;
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemInvincibility) >= 1)
			{
				num31++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemCoinBonus) >= 1)
			{
				num31++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemCoinValue) >= 1)
			{
				num31++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemVacuum) >= 1)
			{
				num31++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemBoost) >= 1)
			{
				num31++;
			}
			float num32 = (float)num31 / 5f * 100f;
			if (num32 >= 100f)
			{
				AwardedAchievements.Add("upgrade1");
				num32 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "upgrade1", num32);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "upgrade5") < 100f)
		{
			int num33 = 0;
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemInvincibility) >= 5)
			{
				num33++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemCoinBonus) >= 5)
			{
				num33++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemCoinValue) >= 5)
			{
				num33++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemVacuum) >= 5)
			{
				num33++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemBoost) >= 5)
			{
				num33++;
			}
			float num34 = (float)num33 / 5f * 100f;
			if (num34 >= 100f)
			{
				AwardedAchievements.Add("upgrade5");
				num34 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "upgrade5", num34);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "character2") < 100f)
		{
			int num35 = 0;
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerGirl) >= 1)
			{
				num35++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerBigB) >= 1)
			{
				num35++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerChina) >= 1)
			{
				num35++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerIndi) >= 1)
			{
				num35++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerConquistador) >= 1)
			{
				num35++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerFootball) >= 1)
			{
				num35++;
			}
			float num36 = (float)num35 / 2f * 100f;
			if (num36 >= 100f)
			{
				AwardedAchievements.Add("character2");
				num36 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "character2", num36);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "character4") < 100f)
		{
			int num37 = 0;
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerGirl) >= 1)
			{
				num37++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerBigB) >= 1)
			{
				num37++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerChina) >= 1)
			{
				num37++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerIndi) >= 1)
			{
				num37++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerConquistador) >= 1)
			{
				num37++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerFootball) >= 1)
			{
				num37++;
			}
			float num38 = (float)num37 / 4f * 100f;
			if (num38 >= 100f)
			{
				AwardedAchievements.Add("character4");
				num38 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "character4", num38);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "character6") < 100f)
		{
			int num39 = 0;
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerGirl) >= 1)
			{
				num39++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerBigB) >= 1)
			{
				num39++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerChina) >= 1)
			{
				num39++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerIndi) >= 1)
			{
				num39++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerConquistador) >= 1)
			{
				num39++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemPlayerFootball) >= 1)
			{
				num39++;
			}
			float num40 = (float)num39 / 6f * 100f;
			if (num40 >= 100f)
			{
				AwardedAchievements.Add("character6");
				num40 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "character6", num40);
		}
		if (RecordManager.GetProgressForAchievement(activePlayer, "wallpaper3") < 100f)
		{
			int num41 = 0;
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemWallpaperA) >= 1)
			{
				num41++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemWallpaperB) >= 1)
			{
				num41++;
			}
			if (RecordManager.GetPlayerLevelForUpgradeType(activePlayer, RecordManager.StoreItemType.kStoreItemWallpaperC) >= 1)
			{
				num41++;
			}
			float num42 = (float)num41 / 3f * 100f;
			if (num42 >= 100f)
			{
				AwardedAchievements.Add("wallpaper3");
				num42 = 100f;
			}
			RecordManager.UpdateAchievementRecord(activePlayer, "wallpaper3", num42);
		}
		RecordManager.SaveRecords();
	}
}
