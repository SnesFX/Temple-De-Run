using UnityEngine;

public class PCInput : GameInput
{
	public float SimulatedTiltAmount;

	public float SimulatedTiltRecovery = 1f;

	public float SimulatedTileSpeed = 1f;

	public bool JoystickMode = true;

	public int joystickPosition;

	public static PCInput Instance;

	public float LastSimulatedTiltAmount;

	public float Force;

	private void Start()
	{
		Instance = this;
		base.enabled = false;
		JoystickMode = false;
	}

	public void SensitivityChanged()
	{
	}

	public override void Update()
	{
		if (GameController.SharedInstance.IsIntroScene)
		{
			return;
		}
		base.Update();
		PathElement pathElementIfPointOverTurn = GameController.PathRoot.GetPathElementIfPointOverTurn(GamePlayer.transform.position, 50f, GameController.PathRootOrigin, true);
		if (Input.GetKeyDown(KeyCode.P))
		{
			PathElement.DumpStatics();
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				GamePlayer.StartBoost(2560f, false);
			}
			else
			{
				GamePlayer.StartBoost(256f, false);
			}
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			GamePlayer.StartVacuum(25f);
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				GamePlayer.StartAngelWings(5f);
			}
			else
			{
				GameController.SharedInstance.UseAngelWings();
			}
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Break();
		}
		if (Input.GetKeyDown(KeyCode.F1))
		{
			SavedGame.SaveGame();
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			SavedGame.LoadGame();
		}
		if (JoystickMode)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
			{
				ShouldJump = true;
				TimeSinceShouldJump = 0f;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
			{
				ShouldSlide = true;
				TimeSinceShouldSlide = 0f;
			}
			if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
			{
				if ((pathElementIfPointOverTurn != null && pathElementIfPointOverTurn != GameController.LastTurnPathPiece) || (pathElementIfPointOverTurn == null && joystickPosition == 1))
				{
					ShouldTurnRight = true;
					TimeSinceShouldTurn = 0f;
					if (pathElementIfPointOverTurn == null && !GamePlayer.IsJumping && !GamePlayer.IsSliding && !GamePlayer.HasBoost)
					{
						if (GamePlayer.GroundHeight <= -7.5f)
						{
							GamePlayer.Stumble(1f);
							GamePlayer.PlayerXOffset = -15f;
						}
						else if (GamePlayer.GroundHeight >= 7.5f)
						{
							GamePlayer.Stumble(1f);
							GamePlayer.PlayerXOffset = -15f;
						}
						else
						{
							GamePlayer.Stumble(0.25f);
							MainCamera.Shake(1f, 0.5f, 1f, 0.25f);
							GamePlayer.PlayerXOffset = -9.5f;
						}
					}
				}
				if (joystickPosition < 1)
				{
					joystickPosition++;
				}
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
			{
				if ((pathElementIfPointOverTurn != null && pathElementIfPointOverTurn != GameController.LastTurnPathPiece) || (pathElementIfPointOverTurn == null && joystickPosition == -1))
				{
					ShouldTurnLeft = true;
					TimeSinceShouldTurn = 0f;
					if (pathElementIfPointOverTurn == null && !GamePlayer.IsJumping && !GamePlayer.IsSliding && !GamePlayer.HasBoost)
					{
						if (GamePlayer.GroundHeight <= -7.5f)
						{
							GamePlayer.Stumble(1f);
							GamePlayer.PlayerXOffset = 15f;
						}
						else if (GamePlayer.GroundHeight >= 7.5f)
						{
							GamePlayer.Stumble(1f);
							GamePlayer.PlayerXOffset = 15f;
						}
						else
						{
							GamePlayer.Stumble(0.25f);
							MainCamera.Shake(1f, 0.5f, 1f, 0.25f);
							GamePlayer.PlayerXOffset = 9.5f;
						}
					}
				}
				if (joystickPosition > -1)
				{
					joystickPosition--;
				}
			}
			if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				joystickPosition = 0;
			}
			return;
		}
		SimulateTilt();
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
		{
			ShouldJump = true;
			TimeSinceShouldJump = 0f;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
		{
			ShouldSlide = true;
			TimeSinceShouldSlide = 0f;
		}
		float num = 51f;
		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
		{
			if (pathElementIfPointOverTurn != null || (pathElementIfPointOverTurn == null && num > 50f))
			{
				ShouldTurnRight = true;
				TimeSinceShouldTurn = 0f;
				if (pathElementIfPointOverTurn == null && !GamePlayer.IsJumping && !GamePlayer.IsSliding && !GamePlayer.HasBoost)
				{
					if ((double)GamePlayer.GroundHeight <= -7.5)
					{
						GamePlayer.Stumble(1f);
						GamePlayer.PlayerXOffset = -15f;
					}
					else
					{
						GamePlayer.Stumble(0.25f);
						MainCamera.Shake(1f, 0.5f, 0.25f, 0f);
						GamePlayer.PlayerXOffset = -9.5f;
					}
				}
			}
			else
			{
				Debug.Log("RIGHT: turn element was null");
			}
		}
		if (!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.A))
		{
			return;
		}
		if (pathElementIfPointOverTurn != null || (pathElementIfPointOverTurn == null && num > 50f))
		{
			ShouldTurnLeft = true;
			TimeSinceShouldTurn = 0f;
			if (pathElementIfPointOverTurn == null && !GamePlayer.IsJumping && !GamePlayer.IsSliding && !GamePlayer.HasBoost)
			{
				if ((double)GamePlayer.GroundHeight <= -7.5)
				{
					GamePlayer.Stumble(1f);
					GamePlayer.PlayerXOffset = 15f;
				}
				else
				{
					GamePlayer.Stumble(0.25f);
					MainCamera.Shake(1f, 0.5f, 0.25f, 0f);
					GamePlayer.PlayerXOffset = 9.5f;
				}
			}
		}
		else
		{
			Debug.Log("LEFT: turn element was null");
		}
	}

	private void SimulateTilt()
	{
		bool flag = true;
		if (Input.GetKey(KeyCode.Z))
		{
			flag = false;
			SimulatedTiltAmount -= SimulatedTileSpeed * Time.smoothDeltaTime;
			if (SimulatedTiltAmount < -1f)
			{
				SimulatedTiltAmount = -1f;
			}
		}
		if (Input.GetKey(KeyCode.X))
		{
			flag = false;
			SimulatedTiltAmount += SimulatedTileSpeed * Time.smoothDeltaTime;
			if (SimulatedTiltAmount > 1f)
			{
				SimulatedTiltAmount = 1f;
			}
		}
		if (!flag)
		{
			return;
		}
		if (SimulatedTiltAmount < 0f)
		{
			SimulatedTiltAmount += SimulatedTiltRecovery * Time.smoothDeltaTime;
			if (SimulatedTiltAmount > 0f)
			{
				SimulatedTiltAmount = 0f;
			}
		}
		if (SimulatedTiltAmount > 0f)
		{
			SimulatedTiltAmount -= SimulatedTiltRecovery * Time.smoothDeltaTime;
			if (SimulatedTiltAmount < 0f)
			{
				SimulatedTiltAmount = 0f;
			}
		}
	}

	public override float GetAccelerometerForceX()
	{
		return SimulatedTiltAmount;
	}
}
