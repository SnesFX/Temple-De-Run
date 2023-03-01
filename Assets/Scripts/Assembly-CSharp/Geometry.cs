using System;
using UnityEngine;

public static class Geometry
{
	public static Intersection IntersectionOf(Line line, Polygon polygon)
	{
		if (polygon.Length == 0)
		{
			return Intersection.None;
		}
		if (polygon.Length == 1)
		{
			return IntersectionOf(polygon[0], line);
		}
		bool flag = false;
		for (int i = 0; i < polygon.Length; i++)
		{
			int index = (i + 1) % polygon.Length;
			Intersection intersection = IntersectionOf(line, new Line(polygon[i], polygon[index]));
			switch (intersection)
			{
			case Intersection.Intersection:
				return intersection;
			case Intersection.Tangent:
				flag = true;
				break;
			}
		}
		return flag ? Intersection.Tangent : IntersectionOf(line.P1, polygon);
	}

	public static Intersection IntersectionOf(Vector2 point, Polygon polygon)
	{
		switch (polygon.Length)
		{
		case 0:
			return Intersection.None;
		case 1:
			if (polygon[0].x == point.x && polygon[0].y == point.y)
			{
				return Intersection.Tangent;
			}
			return Intersection.None;
		case 2:
			return IntersectionOf(point, new Line(polygon[0], polygon[1]));
		default:
		{
			int num = 0;
			int length = polygon.Length;
			Vector2 vector = polygon[0];
			if (point == vector)
			{
				return Intersection.Tangent;
			}
			for (int i = 1; i <= length; i++)
			{
				Vector2 vector2 = polygon[i % length];
				if (point == vector2)
				{
					return Intersection.Tangent;
				}
				if (point.y > Math.Min(vector.y, vector2.y) && point.y <= Math.Max(vector.y, vector2.y) && point.x <= Math.Max(vector.x, vector2.x) && vector.y != vector2.y)
				{
					double num2 = (point.y - vector.y) * (vector2.x - vector.x) / (vector2.y - vector.y) + vector.x;
					if (vector.x == vector2.x || (double)point.x <= num2)
					{
						num++;
					}
				}
				vector = vector2;
			}
			return (num % 2 == 1) ? Intersection.Containment : Intersection.None;
		}
		}
	}

	public static Intersection IntersectionOf(Vector2 point, Line line)
	{
		float num = Math.Min(line.Y1, line.Y2);
		float num2 = Math.Max(line.Y1, line.Y2);
		bool flag = point.y >= num && point.y <= num2;
		if (line.X1 == line.X2)
		{
			if (point.x == line.X1 && flag)
			{
				return Intersection.Tangent;
			}
			return Intersection.None;
		}
		float num3 = (line.X2 - line.X1) / (line.Y2 - line.Y1);
		if (line.Y1 - point.y == num3 * (line.X1 - point.x) && flag)
		{
			return Intersection.Tangent;
		}
		return Intersection.None;
	}

	public static Intersection IntersectionOf(Line line1, Line line2)
	{
		if ((line1.X1 == line1.X2 && line1.Y1 == line1.Y2) || (line2.X1 == line2.X2 && line2.Y1 == line2.Y2))
		{
			return Intersection.None;
		}
		if ((line1.X1 == line2.X1 && line1.Y1 == line2.Y1) || (line1.X2 == line2.X1 && line1.Y2 == line2.Y1))
		{
			return Intersection.Intersection;
		}
		if ((line1.X1 == line2.X2 && line1.Y1 == line2.Y2) || (line1.X2 == line2.X2 && line1.Y2 == line2.Y2))
		{
			return Intersection.Intersection;
		}
		line1.X2 -= line1.X1;
		line1.Y2 -= line1.Y1;
		line2.X1 -= line1.X1;
		line2.Y1 -= line1.Y1;
		line2.X2 -= line1.X1;
		line2.Y2 -= line1.Y1;
		float num = (float)Math.Sqrt(line1.X2 * line1.X2 + line1.Y2 * line1.Y2);
		float num2 = line1.X2 / num;
		float num3 = line1.Y2 / num;
		float x = line2.X1 * num2 + line2.Y1 * num3;
		line2.Y1 = line2.Y1 * num2 - line2.X1 * num3;
		line2.X1 = x;
		x = line2.X2 * num2 + line2.Y2 * num3;
		line2.Y2 = line2.Y2 * num2 - line2.X2 * num3;
		line2.X2 = x;
		if ((line2.Y1 < 0f && line2.Y2 < 0f) || (line2.Y1 >= 0f && line2.Y2 >= 0f))
		{
			return Intersection.None;
		}
		double num4 = line2.X2 + (line2.X1 - line2.X2) * line2.Y2 / (line2.Y2 - line2.Y1);
		if (num4 < 0.0 || num4 > (double)num)
		{
			return Intersection.None;
		}
		return Intersection.Intersection;
	}
}
