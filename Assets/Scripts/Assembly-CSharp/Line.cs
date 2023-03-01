using UnityEngine;

public struct Line
{
	public static Line Empty;

	private Vector2 p1;

	private Vector2 p2;

	public Vector2 P1
	{
		get
		{
			return p1;
		}
		set
		{
			p1 = value;
		}
	}

	public Vector2 P2
	{
		get
		{
			return p2;
		}
		set
		{
			p2 = value;
		}
	}

	public float X1
	{
		get
		{
			return p1.x;
		}
		set
		{
			p1.x = value;
		}
	}

	public float X2
	{
		get
		{
			return p2.x;
		}
		set
		{
			p2.x = value;
		}
	}

	public float Y1
	{
		get
		{
			return p1.y;
		}
		set
		{
			p1.y = value;
		}
	}

	public float Y2
	{
		get
		{
			return p2.y;
		}
		set
		{
			p2.y = value;
		}
	}

	public Line(Vector2 p1, Vector2 p2)
	{
		this.p1 = p1;
		this.p2 = p2;
	}
}
