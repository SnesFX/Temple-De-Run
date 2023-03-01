using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
	[Serializable]
	public class MovingObjectPathNode
	{
		public float TimeStamp;

		public Vector3 Location;

		public bool Ease;

		public override string ToString()
		{
			return string.Concat("PN(", TimeStamp, " ", Location, " ", Ease, ")");
		}
	}

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

	public List<MovingObjectPathNode> FollowingPathNodes;

	private void Start()
	{
	}

	public virtual void Update()
	{
		Simulate();
	}

	public bool IsMoving()
	{
		if (IsFollowingPath && FollowingPathNodes != null && FollowingPathNodes.Count > 1)
		{
			return true;
		}
		if (Velocity.sqrMagnitude == 0f && AngularVelocity == 0f)
		{
			return false;
		}
		return true;
	}

	public void AddFollowingPathNode(MovingObjectPathNode node)
	{
		if (FollowingPathNodes == null)
		{
			FollowingPathNodes = new List<MovingObjectPathNode>();
		}
		FollowingPathNodes.Add(node);
	}

	public void AddFollowingPathNode(float timeStamp, Vector3 location, bool ease)
	{
		MovingObjectPathNode movingObjectPathNode = new MovingObjectPathNode();
		movingObjectPathNode.TimeStamp = timeStamp;
		movingObjectPathNode.Location = location;
		movingObjectPathNode.Ease = ease;
		MovingObjectPathNode node = movingObjectPathNode;
		AddFollowingPathNode(node);
	}

	public void StartFollowingPath()
	{
		TimeSinceFollowingPathStart = 0f;
		IsFollowingPath = true;
	}

	public void StopFollowingPath()
	{
		IsFollowingPath = false;
	}

	public void ClearFollowingPath()
	{
		IsFollowingPath = false;
		if (FollowingPathNodes != null)
		{
			FollowingPathNodes.Clear();
		}
	}

	public void ApplyForce(Vector3 force)
	{
		Force += force;
		HasForce = true;
	}

	public void ResetForce()
	{
		Force = new Vector3(0f, 0f, 0f);
		HasForce = false;
	}

	public void ResetSimulation()
	{
		TimeSinceFollowingPathStart = 0f;
		IsFollowingPath = false;
		HasForce = false;
		Force = new Vector3(0f, 0f, 0f);
		Velocity = new Vector3(0f, 0f, 0f);
		AngularVelocity = 0f;
	}

	public void Simulate()
	{
		if (HasForce)
		{
			Velocity += Force * Time.smoothDeltaTime;
			Force = new Vector3(0f, 0f, 0f);
			HasForce = false;
		}
		if (!IsMoving())
		{
			return;
		}
		if (IsFollowingPath && FollowingPathNodes != null && FollowingPathNodes.Count > 1)
		{
			TimeSinceFollowingPathStart += Time.smoothDeltaTime;
			bool flag = false;
			MovingObjectPathNode movingObjectPathNode = null;
			MovingObjectPathNode movingObjectPathNode2 = null;
			for (int i = 0; i < FollowingPathNodes.Count - 1; i++)
			{
				MovingObjectPathNode movingObjectPathNode3 = FollowingPathNodes[i];
				MovingObjectPathNode movingObjectPathNode4 = FollowingPathNodes[i + 1];
				if (i == 0)
				{
					movingObjectPathNode = movingObjectPathNode3;
				}
				movingObjectPathNode2 = movingObjectPathNode4;
				if (TimeSinceFollowingPathStart >= movingObjectPathNode3.TimeStamp && TimeSinceFollowingPathStart <= movingObjectPathNode4.TimeStamp)
				{
					movingObjectPathNode = movingObjectPathNode3;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				Vector3 vector = movingObjectPathNode2.Location - movingObjectPathNode.Location;
				float num = 1f;
				if (movingObjectPathNode.TimeStamp != movingObjectPathNode2.TimeStamp)
				{
					num = (TimeSinceFollowingPathStart - movingObjectPathNode.TimeStamp) / (movingObjectPathNode2.TimeStamp - movingObjectPathNode.TimeStamp);
				}
				if (movingObjectPathNode.Ease && !movingObjectPathNode2.Ease)
				{
					num = num * num * num;
				}
				else if (!movingObjectPathNode.Ease && movingObjectPathNode2.Ease)
				{
					float num2 = num - 1f;
					num = num2 * num2 * num2 + 1f;
				}
				else if (movingObjectPathNode.Ease && movingObjectPathNode2.Ease)
				{
					if (num < 0.5f)
					{
						float num3 = num * 2f;
						num = 0.5f * (num3 * num3 * num3);
					}
					else
					{
						float num4 = num * 2f - 2f;
						num = 0.5f * (num4 * num4 * num4 + 2f);
					}
				}
				base.transform.position = movingObjectPathNode.Location + vector * num;
			}
			else if (movingObjectPathNode != null && (TimeSinceFollowingPathStart < movingObjectPathNode.TimeStamp || movingObjectPathNode2 == null))
			{
				base.transform.position = movingObjectPathNode.Location;
				IsFollowingPath = false;
			}
			else if (movingObjectPathNode2 != null)
			{
				base.transform.position = movingObjectPathNode2.Location;
				IsFollowingPath = false;
			}
			else
			{
				IsFollowingPath = false;
			}
			return;
		}
		if (LimitVelocity)
		{
			if (MaxVelocityMagnitudeTotal >= 0f && Velocity.magnitude > MaxVelocityMagnitudeTotal)
			{
				Velocity.Normalize();
				Velocity *= MaxVelocityMagnitudeTotal;
			}
			if (MaxVelocityMagnitudeX >= 0f && Mathf.Abs(Velocity.x) > MaxVelocityMagnitudeX)
			{
				if (Velocity.x < 0f)
				{
					Velocity.x = 0f - MaxVelocityMagnitudeX;
				}
				if (MaxVelocityMagnitudeX > 0f)
				{
					Velocity.x = MaxVelocityMagnitudeX;
				}
			}
			if (MaxVelocityMagnitudeY >= 0f && Mathf.Abs(Velocity.y) > MaxVelocityMagnitudeY)
			{
				if (Velocity.y < 0f)
				{
					Velocity.y = 0f - MaxVelocityMagnitudeY;
				}
				if (MaxVelocityMagnitudeY > 0f)
				{
					Velocity.y = MaxVelocityMagnitudeY;
				}
			}
			if (MaxVelocityMagnitudeZ >= 0f && Mathf.Abs(Velocity.z) > MaxVelocityMagnitudeZ)
			{
				if (Velocity.z < 0f)
				{
					Velocity.z = 0f - MaxVelocityMagnitudeZ;
				}
				if (MaxVelocityMagnitudeZ > 0f)
				{
					Velocity.z = MaxVelocityMagnitudeZ;
				}
			}
		}
		if (Velocity != Vector3.zero)
		{
			base.transform.Translate(Velocity * Time.smoothDeltaTime, Space.World);
		}
		if (AngularVelocity != 0f)
		{
			base.transform.RotateAround(RotationAxis, AngularVelocity * Time.smoothDeltaTime);
		}
	}

	public virtual void SetVisibility(bool visible)
	{
		Renderer[] componentsInChildren = base.transform.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			renderer.enabled = visible;
		}
		UISprite[] componentsInChildren2 = GetComponentsInChildren<UISprite>();
		UISprite[] array2 = componentsInChildren2;
		foreach (UISprite uISprite in array2)
		{
			uISprite.enabled = visible;
		}
	}

	public void SetX(float x)
	{
		base.transform.position = new Vector3(x, base.transform.position.y, base.transform.position.z);
	}

	public void SetY(float y)
	{
		base.transform.position = new Vector3(base.transform.position.x, y, base.transform.position.z);
	}

	public void SetZ(float z)
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, z);
	}

	public void SetLocalX(float x)
	{
		base.transform.localPosition = new Vector3(x, base.transform.localPosition.y, base.transform.localPosition.z);
	}

	public void SetLocalY(float y)
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, y, base.transform.localPosition.z);
	}

	public void SetLocalZ(float z)
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, z);
	}

	public bool DoesSphereIntersectBoundingBox(Vector3 point, float radius)
	{
		float num = GetWorldSpaceBounds().SqrDistance(point);
		return num <= radius * radius;
	}

	public virtual Bounds GetWorldSpaceBounds()
	{
		return base.transform.GetComponentInChildren<Renderer>().bounds;
	}

	public Vector2 GetPosition2D()
	{
		return new Vector2(base.transform.position.x, base.transform.position.z);
	}
}
