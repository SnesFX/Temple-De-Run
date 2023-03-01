using System.Collections.Generic;
using UnityEngine;

public class Region2D : MonoBehaviour
{
	public List<Vector2> Points = new List<Vector2>();

	public Transform TestPoint;

	public float Height;

	public Vector3 testProjectedPoint;

	public bool testIsIn;

	public void AddPoint(float x, float y)
	{
		AddPoint(new Vector2(x, y));
	}

	public void AddPoint(Vector2 p)
	{
		Points.Add(p);
	}

	public void Clear()
	{
		Points.Clear();
	}

	private void DrawLine(Vector2 lastPoint, Vector2 nextPoint)
	{
		Vector3 from = base.transform.TransformPoint(new Vector3(lastPoint.x, Height, lastPoint.y));
		Vector3 to = base.transform.TransformPoint(new Vector3(nextPoint.x, Height, nextPoint.y));
		Gizmos.DrawLine(from, to);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 0f, 1f, 0.5f);
		if (Points.Count > 1)
		{
			Vector2 lastPoint = Points[0];
			for (int i = 1; i < Points.Count; i++)
			{
				Vector2 vector = Points[i];
				DrawLine(lastPoint, vector);
				lastPoint = vector;
			}
			DrawLine(lastPoint, Points[0]);
		}
		if (TestPoint != null)
		{
			if (TestPoint != null)
			{
				testIsIn = IsPointInside(TestPoint.position);
			}
			Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
			Gizmos.DrawSphere(TestPoint.position, 1f);
			if (testIsIn)
			{
				Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
			}
			else
			{
				Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
			}
			Gizmos.DrawSphere(testProjectedPoint, 1f);
			Gizmos.color = new Color(0f, 0.25f, 1f, 0.5f);
			Gizmos.DrawLine(TestPoint.position, testProjectedPoint);
		}
	}

	public bool IsPointInside(Vector3 point)
	{
		return IsPointInside(new Vector2(point.x, point.z));
	}

	public bool IsPointInside(Vector2 worldPoint)
	{
		int num = Points.Count - 1;
		if (num < 2)
		{
			return false;
		}
		Vector3 position = base.transform.InverseTransformPoint(new Vector3(worldPoint.x, 0f, worldPoint.y));
		float x = position.x;
		float z = position.z;
		if (TestPoint != null)
		{
			testProjectedPoint = base.transform.TransformPoint(position);
			testProjectedPoint.y = base.transform.position.y;
		}
		bool flag = false;
		for (int i = 0; i < Points.Count; i++)
		{
			Vector2 vector = Points[i];
			Vector2 vector2 = Points[num];
			if (((vector.y < z && vector2.y >= z) || (vector2.y < z && vector.y >= z)) && (vector.x <= x || vector2.x < x) && vector.x + (z - vector.y) / (vector2.y - vector.y) * (vector2.x - vector.x) < x)
			{
				flag = !flag;
			}
			num = i;
		}
		return flag;
	}

	public bool DoesLineSegementIntersect(Vector2 p1, Vector2 p2)
	{
		Vector3 vector = base.transform.InverseTransformPoint(p1.x, 0f, p1.y);
		Vector3 vector2 = base.transform.InverseTransformPoint(p2.x, 0f, p2.y);
		Vector3 vector3 = new Vector2(vector.x, vector.z);
		Vector3 vector4 = new Vector2(vector2.x, vector2.z);
		Line line = new Line(vector3, vector4);
		Polygon polygon = new Polygon(Points.ToArray());
		Intersection intersection = Geometry.IntersectionOf(line, polygon);
		return intersection == Intersection.Containment || intersection == Intersection.Intersection;
	}
}
