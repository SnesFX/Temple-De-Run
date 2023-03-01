using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedMovingObject
{
	public Vector3 Position;

	public Quaternion Rotation;

	public Vector3 Scale;

	public float Mass = 1f;

	public bool HasForce;

	public Vector3 Force;

	public Vector3 Velocity;

	public Vector3 RotationAxis = new Vector3(0f, 1f, 0f);

	public float AngularVelocity;

	public bool LimitVelocity;

	public float MaxVelocityMagnitudeTotal = -1f;

	public float MaxVelocityMagnitudeX = -1f;

	public float MaxVelocityMagnitudeY = -1f;

	public float MaxVelocityMagnitudeZ = -1f;

	public bool IsFollowingPath;

	public float TimeSinceFollowingPathStart;

	public List<MovingObject.MovingObjectPathNode> FollowingPathNodes;

	public SavedMovingObject()
	{
	}

	public SavedMovingObject(MovingObject o)
	{
		Position = o.transform.position;
		Rotation = o.transform.rotation;
		Mass = o.Mass;
		HasForce = o.HasForce;
		Force = o.Force;
		Velocity = o.Velocity;
		RotationAxis = o.RotationAxis;
		AngularVelocity = o.AngularVelocity;
		LimitVelocity = o.LimitVelocity;
		MaxVelocityMagnitudeTotal = o.MaxVelocityMagnitudeTotal;
		MaxVelocityMagnitudeX = o.MaxVelocityMagnitudeX;
		MaxVelocityMagnitudeY = o.MaxVelocityMagnitudeY;
		MaxVelocityMagnitudeZ = o.MaxVelocityMagnitudeZ;
		IsFollowingPath = o.IsFollowingPath;
		TimeSinceFollowingPathStart = o.TimeSinceFollowingPathStart;
		if (o.FollowingPathNodes != null)
		{
			FollowingPathNodes = new List<MovingObject.MovingObjectPathNode>(o.FollowingPathNodes);
		}
	}

	public void Apply(MovingObject o)
	{
		o.transform.position = Position;
		o.transform.rotation = Rotation;
		o.Mass = Mass;
		o.HasForce = HasForce;
		o.Force = Force;
		o.Velocity = Velocity;
		o.RotationAxis = RotationAxis;
		o.AngularVelocity = AngularVelocity;
		o.LimitVelocity = LimitVelocity;
		o.MaxVelocityMagnitudeTotal = MaxVelocityMagnitudeTotal;
		o.MaxVelocityMagnitudeX = MaxVelocityMagnitudeX;
		o.MaxVelocityMagnitudeY = MaxVelocityMagnitudeY;
		o.MaxVelocityMagnitudeZ = MaxVelocityMagnitudeZ;
		o.IsFollowingPath = IsFollowingPath;
		o.TimeSinceFollowingPathStart = TimeSinceFollowingPathStart;
		if (FollowingPathNodes != null)
		{
			o.FollowingPathNodes = new List<MovingObject.MovingObjectPathNode>(FollowingPathNodes);
		}
	}
}
