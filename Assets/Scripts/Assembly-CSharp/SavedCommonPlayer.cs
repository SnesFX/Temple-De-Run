using System;
using UnityEngine;

[Serializable]
public class SavedCommonPlayer : SavedMovingObject
{
	public PathElement.PathDirection Facing;

	public Vector3 PreviousLocation;

	public SavedCommonPlayer()
	{
	}

	public SavedCommonPlayer(CommonPlayer p)
		: base(p)
	{
		Facing = p.Facing;
		PreviousLocation = p.PreviousLocation;
	}

	public void Apply(CommonPlayer p)
	{
		Apply((MovingObject)p);
		p.Facing = Facing;
		p.PreviousLocation = PreviousLocation;
	}
}
