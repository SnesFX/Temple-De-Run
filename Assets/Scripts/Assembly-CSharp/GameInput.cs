using UnityEngine;

public class GameInput : MonoBehaviour
{
	public bool JustTurned;

	public bool ShouldTurnLeft;

	public bool ShouldTurnRight;

	public float TimeSinceShouldTurn;

	public float TimeSinceLastTurn;

	public bool ShouldJump;

	public float TimeSinceShouldJump;

	public bool ShouldSlide;

	public float TimeSinceShouldSlide;

	public GameController GameController;

	public CharacterPlayer GamePlayer;

	public GameCamera MainCamera;

	private void Start()
	{
	}

	public virtual void Update()
	{
		HandleAccelerometerForce();
		if (ShouldTurnRight || ShouldTurnLeft)
		{
			TimeSinceShouldTurn += Time.smoothDeltaTime;
			if (TimeSinceShouldTurn > 1f)
			{
				ShouldTurnLeft = (ShouldTurnRight = false);
				TimeSinceShouldTurn = 0f;
			}
		}
		if (ShouldJump)
		{
			TimeSinceShouldJump += Time.smoothDeltaTime;
			if (TimeSinceShouldJump > 0.5f)
			{
				ShouldJump = false;
				TimeSinceShouldJump = 0f;
			}
		}
		if (ShouldSlide)
		{
			TimeSinceShouldSlide += Time.smoothDeltaTime;
			if (TimeSinceShouldSlide > 0.5f)
			{
				ShouldSlide = false;
				TimeSinceShouldSlide = 0f;
			}
		}
	}

	private void HandleAccelerometerForce()
	{
		float accelerometerForceX = GetAccelerometerForceX();
		if (!GameController.IsIntroScene)
		{
			float num = 0.5f;
			float num2 = 0f;
			if (accelerometerForceX < 0f)
			{
				num2 = -1f;
			}
			else if (accelerometerForceX > 0f)
			{
				num2 = 1f;
			}
			float num3 = Mathf.Abs(accelerometerForceX);
			num3 *= 0.25f + 19.75f * num;
			if (num3 > 1f)
			{
				num3 = 1f;
			}
			float num4 = num3 * 7f;
			Vector3 vector = new Vector3(1f, 0f, 0f) * num4 * num2;
			if (!GamePlayer.IsStumbling && !GamePlayer.StumbleAfterDelay)
			{
				GamePlayer.PlayerXOffset = 0f - vector.x;
			}
		}
	}

	public virtual float GetAccelerometerForceX()
	{
		return 0f;
	}

	public void Reset()
	{
		JustTurned = false;
		ShouldTurnLeft = false;
		ShouldTurnRight = false;
		TimeSinceShouldTurn = 0f;
		TimeSinceLastTurn = 0f;
		ShouldJump = false;
		TimeSinceShouldJump = 0f;
		ShouldSlide = false;
		TimeSinceShouldSlide = 0f;
	}
}
