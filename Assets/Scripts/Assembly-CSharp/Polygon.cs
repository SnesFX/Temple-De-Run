using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Polygon : IEnumerable<Vector2>, IEnumerable
{
	private Vector2[] points;

	public Vector2[] Points
	{
		get
		{
			return points;
		}
		set
		{
			points = value;
		}
	}

	public int Length
	{
		get
		{
			return points.Length;
		}
	}

	public Vector2 this[int index]
	{
		get
		{
			return points[index];
		}
		set
		{
			points[index] = value;
		}
	}

	public Polygon(Vector2[] points)
	{
		this.points = points;
	}

	IEnumerator<Vector2> IEnumerable<Vector2>.GetEnumerator()
	{
		return (IEnumerator<Vector2>)points.GetEnumerator();
	}

	public IEnumerator GetEnumerator()
	{
		return points.GetEnumerator();
	}

	public static implicit operator Vector2[](Polygon polygon)
	{
		return polygon.points;
	}

	public static implicit operator Polygon(Vector2[] points)
	{
		return new Polygon(points);
	}
}
