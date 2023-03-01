using UnityEngine;

public static class GuiHelper
{
	private static Texture2D _coloredLineTexture;

	private static Color _coloredLineColor;

	public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Color color)
	{
		DrawLine(lineStart, lineEnd, color, 1);
	}

	public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Color color, int thickness)
	{
		if (_coloredLineTexture == null || _coloredLineColor != color)
		{
			_coloredLineColor = color;
			_coloredLineTexture = new Texture2D(1, 1);
			_coloredLineTexture.SetPixel(0, 0, _coloredLineColor);
			_coloredLineTexture.wrapMode = TextureWrapMode.Repeat;
			_coloredLineTexture.Apply();
		}
		DrawLineStretched(lineStart, lineEnd, _coloredLineTexture, thickness);
	}

	public static void DrawLineStretched(Vector2 lineStart, Vector2 lineEnd, Texture2D texture, int thickness)
	{
		Vector2 vector = lineEnd - lineStart;
		float num = 57.29578f * Mathf.Atan(vector.y / vector.x);
		if (vector.x < 0f)
		{
			num += 180f;
		}
		if (thickness < 1)
		{
			thickness = 1;
		}
		int num2 = (int)Mathf.Ceil(thickness / 2);
		GUIUtility.RotateAroundPivot(num, lineStart);
		GUI.DrawTexture(new Rect(lineStart.x, lineStart.y - (float)num2, vector.magnitude, thickness), texture);
		GUIUtility.RotateAroundPivot(0f - num, lineStart);
	}

	public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Texture2D texture)
	{
		DrawLine(lineStart, lineEnd, texture, 1);
	}

	public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Texture2D texture, int thickness)
	{
		Vector2 vector = lineEnd - lineStart;
		float num = 57.29578f * Mathf.Atan(vector.y / vector.x);
		if (vector.x < 0f)
		{
			num += 180f;
		}
		if (thickness < 1)
		{
			thickness = 1;
		}
		int num2 = (int)Mathf.Ceil(thickness / 2);
		Rect position = new Rect(lineStart.x, lineStart.y - (float)num2, Vector2.Distance(lineStart, lineEnd), thickness);
		GUIUtility.RotateAroundPivot(num, lineStart);
		GUI.BeginGroup(position);
		int num3 = Mathf.RoundToInt(position.width);
		int num4 = Mathf.RoundToInt(position.height);
		for (int i = 0; i < num4; i += texture.height)
		{
			for (int j = 0; j < num3; j += texture.width)
			{
				GUI.DrawTexture(new Rect(j, i, texture.width, texture.height), texture);
			}
		}
		GUI.EndGroup();
		GUIUtility.RotateAroundPivot(0f - num, lineStart);
	}
}
