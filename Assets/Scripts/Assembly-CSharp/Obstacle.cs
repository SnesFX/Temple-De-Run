using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public bool MustJumpOver;

	public bool MustSlideUnder;

	public bool DoesKill;

	public bool DoesLineSegementIntersect(Vector2 p1, Vector2 p2)
	{
		Region2D[] components = base.transform.GetComponents<Region2D>();
		Region2D[] array = components;
		foreach (Region2D region2D in array)
		{
			if (region2D.DoesLineSegementIntersect(p1, p2))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsPointInRegion(Vector2 p)
	{
		Region2D[] components = base.transform.GetComponents<Region2D>();
		Region2D[] array = components;
		foreach (Region2D region2D in array)
		{
			if (region2D.IsPointInside(p))
			{
				return true;
			}
		}
		return false;
	}
}
