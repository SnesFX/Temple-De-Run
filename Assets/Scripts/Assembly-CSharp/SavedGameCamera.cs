using System;
using UnityEngine;

[Serializable]
public class SavedGameCamera : SavedMovingObject
{
	public float GroundHeight;

	public float FollowHeight;

	public float FollowDistance;

	public float FocusHeight;

	public bool NeedsToDuckCamera;

	public Vector3 TargetCameraLocation;

	public SavedMovingObject CameraFocus;

	public SavedGameCamera()
	{
	}

	public SavedGameCamera(GameCamera c)
		: base(c)
	{
		GroundHeight = c.GroundHeight;
		FollowHeight = c.FollowHeight;
		FollowDistance = c.FollowDistance;
		FocusHeight = c.FocusHeight;
		NeedsToDuckCamera = c.NeedsToDuckCamera;
		TargetCameraLocation = c.TargetCameraLocation;
		CameraFocus = new SavedMovingObject(c.MainCameraFocusPoint);
	}

	public void Apply(GameCamera c)
	{
		Apply((MovingObject)c);
		c.GroundHeight = GroundHeight;
		c.FollowHeight = FollowHeight;
		c.FollowDistance = FollowDistance;
		c.FocusHeight = FocusHeight;
		c.NeedsToDuckCamera = NeedsToDuckCamera;
		c.TargetCameraLocation = TargetCameraLocation;
		CameraFocus.Apply(c.MainCameraFocusPoint);
	}
}
