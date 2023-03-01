using System.Collections.Generic;
using UnityEngine;

public class FTPSGraph : MonoBehaviour
{
	public List<float> Points = new List<float>(1000);

	private float width;

	private float height;

	public static FTPSGraph Instance;

	public int TargetFrameRate = 60;

	public float MaxDeltaTime = 0.1f;

	private Material lineMaterial;

	private void Start()
	{
		Instance = this;
		CreateLineMaterial();
		width = Screen.width;
		height = (float)Screen.height * 0.1f;
		Debug.Log("Before FRT: " + Application.targetFrameRate + " MDT: " + Time.maximumDeltaTime);
		Application.targetFrameRate = TargetFrameRate;
		Time.maximumDeltaTime = MaxDeltaTime;
	}

	public void AddPoint(float p)
	{
		Points.Add(p);
	}

	private void CreateLineMaterial()
	{
		if (!lineMaterial)
		{
			lineMaterial = new Material("Shader \"Lines/Colored Blended\" {SubShader {  Tags { \"Queue\" = \"Transparent+1000\" }  Pass {     Blend SrcAlpha OneMinusSrcAlpha     ZWrite Off Cull Off Fog { Mode Off }     BindChannels {      Bind \"vertex\", vertex Bind \"color\", color }} } }");
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	private void Vert(float x, float y)
	{
		GL.Vertex3(x / (float)Screen.width, y / (float)Screen.height, 0f);
	}

	private void OnPostRender()
	{
		float num = 0f;
		foreach (float point in Points)
		{
			float num2 = point;
			if (num2 > num)
			{
				num = num2;
			}
		}
		lineMaterial.SetPass(0);
		GL.PushMatrix();
		CreateLineMaterial();
		lineMaterial.SetPass(0);
		GL.LoadOrtho();
		GL.Color(new Color(0f, 0f, 0f, 0.5f));
		GL.Begin(4);
		Vert(0f, 0f);
		Vert(0f, height);
		Vert(width, 0f);
		Vert(0f, height);
		Vert(width, height);
		Vert(width, 0f);
		GL.End();
		GL.Begin(1);
		GL.Color(new Color(1f, 0f, 1f, 0.5f));
		Vert(0f, height + 1f);
		Vert(width, height + 1f);
		GL.End();
		GL.Begin(1);
		GL.Color(Color.white);
		float num3 = width;
		for (int num4 = Points.Count - 1; num4 >= 0; num4--)
		{
			float num5 = Points[num4];
			float y = num5 / num * height;
			Vert(num3, y);
			num3 -= 1f;
			if (num3 < 0f)
			{
				Points.RemoveAt(0);
				break;
			}
		}
		GL.End();
		GL.PopMatrix();
	}
}
