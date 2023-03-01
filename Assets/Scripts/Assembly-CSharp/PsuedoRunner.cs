using UnityEngine;

public class PsuedoRunner : CommonPlayer
{
	public PathElement OnPath;

	public PathElement.PathDirection Direction = PathElement.PathDirection.kPathDirectionSouth;

	public float NodeToNodeTime = 0.5f;

	private float TimeSinceLastNode;

	private float DesiredCameraY;

	private PathElement NextPath;

	public float CalculatedVelocity;

	public Vector3 CalculatedVelocityVector;

	public MovingObject CameraFocus;

	private void Start()
	{
	}

	public override void Update()
	{
		Vector3 position = base.transform.position;
		if (OnPath == null)
		{
			SnapToFirstPathElemeent();
		}
		if (OnPath != null && NextPath != null)
		{
			TimeSinceLastNode += Time.smoothDeltaTime;
			float num = TimeSinceLastNode / NodeToNodeTime;
			if (num >= 1f)
			{
				base.transform.position = NextPath.transform.position;
				OnPath = NextPath;
				PicknNextPathElement();
				TurnCameraToFaceDirection(Direction);
				base.transform.localEulerAngles = new Vector3(0f, DesiredCameraY, 0f);
				TimeSinceLastNode -= NodeToNodeTime;
			}
			else
			{
				base.transform.position = Vector3.Lerp(OnPath.transform.position, NextPath.transform.position, num);
			}
		}
		Vector3 vector = base.transform.position - position;
		CalculatedVelocityVector = vector / Time.smoothDeltaTime;
		CalculatedVelocity = vector.magnitude / Time.smoothDeltaTime;
	}

	private void SnapToFirstPathElemeent()
	{
		PathElement pathRoot = GameController.SharedInstance.PathRoot;
		OnPath = pathRoot;
		Direction = GameController.SharedInstance.PathRootOrigin;
		PicknNextPathElement();
		TimeSinceLastNode = 0f;
		TurnCameraToFaceDirection(Direction);
		DesiredCameraY = 0f;
	}

	private PathElement PicknNextPathElement()
	{
		if (OnPath.PathNorth == null && OnPath.PathEast == null && OnPath.PathSouth == null && OnPath.PathWest == null)
		{
			return null;
		}
		PathElement.PathDirection pathDirection = TravelingDirection();
		int num = 0;
		while (++num <= 200)
		{
			switch (Random.Range(0, 4))
			{
			case 0:
				if (OnPath.PathNorth != null && pathDirection != PathElement.PathDirection.kPathDirectionSouth)
				{
					NextPath = OnPath.PathNorth;
					Direction = PathElement.PathDirection.kPathDirectionNorth;
					return NextPath;
				}
				break;
			case 1:
				if (OnPath.PathEast != null && pathDirection != PathElement.PathDirection.kPathDirectionWest)
				{
					NextPath = OnPath.PathEast;
					Direction = PathElement.PathDirection.kPathDirectionEast;
					return NextPath;
				}
				break;
			case 2:
				if (OnPath.PathSouth != null && pathDirection != PathElement.PathDirection.kPathDirectionNorth)
				{
					NextPath = OnPath.PathSouth;
					Direction = PathElement.PathDirection.kPathDirectionSouth;
					return NextPath;
				}
				break;
			case 3:
				if (OnPath.PathWest != null && pathDirection != PathElement.PathDirection.kPathDirectionEast)
				{
					NextPath = OnPath.PathWest;
					Direction = PathElement.PathDirection.kPathDirectionWest;
					return NextPath;
				}
				break;
			}
		}
		Debug.Log(string.Concat(num, " tries: ", pathDirection, "  OnPath: ", OnPath), OnPath);
		return null;
	}

	private void TurnCameraToFaceDirection(PathElement.PathDirection direction)
	{
		switch (direction)
		{
		case PathElement.PathDirection.kPathDirectionNorth:
			DesiredCameraY = 0f;
			break;
		case PathElement.PathDirection.kPathDirectionEast:
			DesiredCameraY = 90f;
			break;
		case PathElement.PathDirection.kPathDirectionSouth:
			DesiredCameraY = 180f;
			break;
		case PathElement.PathDirection.kPathDirectionWest:
			DesiredCameraY = 270f;
			break;
		}
	}
}
