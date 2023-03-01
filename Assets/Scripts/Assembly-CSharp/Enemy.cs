using UnityEngine;

public class Enemy : MovingObject
{
	public bool IsFalling;

	public bool IsJumping;

	public bool JumpAfterDelay;

	public float JumpDelay;

	public float JumpVelocity;

	public bool IsOverGround;

	public float GroundHeight;

	public bool _bIsSelected = true;

	private void Start()
	{
		Reset();
	}

	public void Run()
	{
	}

	public void Jump(float delay = 0f)
	{
		if (!IsJumping && !IsFalling)
		{
			if (delay > 0f)
			{
				JumpAfterDelay = true;
				JumpDelay = delay;
			}
			else
			{
				JumpAfterDelay = false;
				IsJumping = true;
				Velocity.y = JumpVelocity;
			}
		}
	}

	public override void Update()
	{
		if (GameController.SharedInstance.GamePlayer.Hold || base.transform.position.y < -40f)
		{
			return;
		}
		if (JumpAfterDelay)
		{
			JumpDelay -= Time.smoothDeltaTime;
			if (JumpDelay <= 0f)
			{
				Jump(0f);
			}
		}
		ApplyForce(new Vector3(0f, -250f, 0f));
		base.Update();
		if (IsOverGround && base.transform.position.y < GroundHeight && !IsFalling)
		{
			SetY(GroundHeight);
			Velocity.y = 0f;
			IsJumping = false;
		}
		if (base.transform.position.y < GroundHeight - 2f)
		{
			IsFalling = true;
		}
	}

	public void Reset()
	{
		SetY(0f);
		IsFalling = false;
		IsJumping = false;
		JumpAfterDelay = false;
		JumpVelocity = 80f;
		IsOverGround = false;
		GroundHeight = 0f;
	}

	private void OnDrawGizmos()
	{
		if (_bIsSelected)
		{
			OnDrawGizmosSelected();
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(base.transform.position, 0.1f);
		if (base.transform.GetComponent<Renderer>() != null)
		{
			Gizmos.DrawWireCube(base.transform.position, base.transform.GetComponent<Renderer>().bounds.size);
		}
	}
}
