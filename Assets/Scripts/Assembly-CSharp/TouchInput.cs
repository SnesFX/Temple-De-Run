using System;
using UnityEngine;

public class TouchInput : GameInput
{
	public UISlider SensitivitySlider;

	private float _Sensitivity = 0.5f;

	public static TouchInput Instance;

	private bool IsSwiping;

	private float LastSwipeTime;

	private Vector2 LastSwipePoint = Vector2.zero;

	private int tapCount;

	private float doubleTapTimeout;

	private float ignoreTapTimeout;

	public float AccelerometerUpdateInterval = 1f / 60f;

	public float LowPassKernelWidthInSeconds = 0.1f;

	private Vector3 LowPassValue = Vector3.zero;

	public float Sensitivity
	{
		get
		{
			return _Sensitivity;
		}
		set
		{
			_Sensitivity = value;
			PlayerPrefs.SetFloat("TR Sensitivity", value);
		}
	}

	private void Start()
	{
		Instance = this;
		base.enabled = true;
		GameController.GameInput = this;
	}

	private void Awake()
	{
		_Sensitivity = PlayerPrefs.GetFloat("TR Sensitivity", 0.5f);
		SensitivitySlider.rawValue = _Sensitivity;
	}

	public void OnSensitivitySlider(float v)
	{
		string functionName = SensitivitySlider.functionName;
		SensitivitySlider.functionName = string.Empty;
		Sensitivity = v;
		SensitivitySlider.functionName = functionName;
	}

	private void DoubleTap()
	{
		GameController.UseAngelWings();
	}

	public override void Update()
	{
		base.Update();
		if (doubleTapTimeout > 0f)
		{
			doubleTapTimeout -= Time.smoothDeltaTime;
		}
		if (doubleTapTimeout <= 0f)
		{
			tapCount = 0;
			doubleTapTimeout = 0f;
		}
		if (ignoreTapTimeout > 0f)
		{
			ignoreTapTimeout -= Time.smoothDeltaTime;
		}
	}

	public void TouchEnded(Vector2 touchPoint)
	{
	}

	public void TouchBegan(Vector2 touchPoint)
	{
		if (!base.enabled || GameController.IsPaused || GameController.IsIntroScene)
		{
			return;
		}
		IsSwiping = true;
		LastSwipeTime = Time.timeSinceLevelLoad;
		LastSwipePoint = touchPoint;
		if (ignoreTapTimeout <= 0f)
		{
			if (tapCount == 0)
			{
				doubleTapTimeout = 0.25f;
			}
			tapCount++;
			if (tapCount == 2)
			{
				DoubleTap();
				tapCount = 0;
			}
			ignoreTapTimeout = 0.1f;
		}
	}

	public void TouchMoved(Vector2 touchPoint)
	{
		float num = (1f - Sensitivity) * 1.75f + 0.25f;
		float num2 = 44f * num;
		float num3 = 88f * num;
		if (!base.enabled || GameController.IsGameOver || GameController.IsPaused || GameController.IsIntroScene || !IsSwiping)
		{
			return;
		}
		float num4 = Vector2.Distance(touchPoint, LastSwipePoint);
		if (num4 < num2)
		{
			return;
		}
		Vector2 vector = touchPoint - LastSwipePoint;
		vector.Normalize();
		float num5 = (Mathf.Atan2(1f, 0f) - Mathf.Atan2(vector.y, vector.x)) * (180f / (float)Math.PI);
		if (num5 < 0f)
		{
			num5 += 360f;
		}
		float num6 = GetAccelerometerForceX() * -2f;
		PathElement pathElementIfPointOverTurn = GameController.PathRoot.GetPathElementIfPointOverTurn(GamePlayer.transform.position, num3, GameController.PathRootOrigin, true);
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Break();
		}
		if (num5 > 315f + num6 || num5 <= 45f + num6)
		{
			ShouldJump = true;
			TimeSinceShouldJump = 0f;
			IsSwiping = false;
			LastSwipeTime = Time.timeSinceLevelLoad;
		}
		else if (num5 > 135f + num6 && num5 <= 225f + num6)
		{
			ShouldSlide = true;
			TimeSinceShouldSlide = 0f;
			IsSwiping = false;
			LastSwipeTime = Time.timeSinceLevelLoad;
		}
		else if (num5 > 45f + num6 && num5 <= 135f + num6)
		{
			if ((pathElementIfPointOverTurn != null && pathElementIfPointOverTurn != GameController.LastTurnPathPiece) || (pathElementIfPointOverTurn == null && num4 > num3))
			{
				if (pathElementIfPointOverTurn != null || (pathElementIfPointOverTurn == null && num4 > num3))
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
				IsSwiping = false;
				LastSwipeTime = Time.timeSinceLevelLoad;
			}
			else
			{
				Debug.Log("RIGHT: turn element was null");
			}
		}
		else
		{
			if (!(num5 > 225f + num6) || !(num5 <= 315f + num6))
			{
				return;
			}
			if ((pathElementIfPointOverTurn != null && pathElementIfPointOverTurn != GameController.LastTurnPathPiece) || (pathElementIfPointOverTurn == null && num4 > num3))
			{
				if (!(pathElementIfPointOverTurn != null) && (!(pathElementIfPointOverTurn == null) || !(num4 > num3)))
				{
					return;
				}
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
				IsSwiping = false;
				LastSwipeTime = Time.timeSinceLevelLoad;
			}
			else
			{
				Debug.Log("LEFT: turn element was null");
			}
		}
	}

	public override float GetAccelerometerForceX()
	{
		float t = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;
		LowPassValue = Vector3.Lerp(LowPassValue, Input.acceleration, t);
		return 0f - LowPassValue.y;
	}
}
