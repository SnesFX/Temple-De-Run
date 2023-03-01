using UnityEngine;

public class CommonPlayer : MovingObject
{
	public PathElement.PathDirection Facing;

	public Vector3 PreviousLocation;

	private void Start()
	{
		PreviousLocation = base.transform.position;
	}

	public override void Update()
	{
		Facing = TravelingDirection();
		PreviousLocation = base.transform.position;
		base.Update();
	}

	public PathElement.PathDirection TravelingDirection()
	{
		int num = (int)base.transform.localEulerAngles.y;
		PathElement.PathDirection pathDirection = PathElement.PathDirection.kPathDirectionNone;
		if (num < 5)
		{
			return PathElement.PathDirection.kPathDirectionNorth;
		}
		if (num < 95)
		{
			return PathElement.PathDirection.kPathDirectionEast;
		}
		if (num < 185)
		{
			return PathElement.PathDirection.kPathDirectionSouth;
		}
		return PathElement.PathDirection.kPathDirectionWest;
	}

	public void Teleport(Vector3 pos)
	{
		base.transform.position = pos;
		PreviousLocation = pos;
	}

	public virtual void Reset()
	{
	}
}
