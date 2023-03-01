using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite (Filled)")]
[ExecuteInEditMode]
public class UIFilledSprite : UISprite
{
	public enum FillDirection
	{
		TowardRight = 0,
		TowardTop = 1,
		TowardLeft = 2,
		TowardBottom = 3
	}

	[SerializeField]
	private FillDirection mFillDirection;

	[SerializeField]
	private float mFillAmount = 1f;

	public FillDirection fillDirection
	{
		get
		{
			return mFillDirection;
		}
		set
		{
			if (mFillDirection != value)
			{
				mFillDirection = value;
				mChanged = true;
			}
		}
	}

	public float fillAmount
	{
		get
		{
			return mFillAmount;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (mFillAmount != num)
			{
				mFillAmount = num;
				mChanged = true;
			}
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
	{
		Vector2 vector = new Vector2(mOuterUV.xMin, mOuterUV.yMin);
		Vector2 vector2 = new Vector2(mOuterUV.xMax, mOuterUV.yMax);
		Vector2 vector3 = vector2 - vector;
		vector3 *= mFillAmount;
		float x = 0f;
		float y = 0f;
		float num = 1f;
		float num2 = -1f;
		switch (fillDirection)
		{
		case FillDirection.TowardBottom:
			num2 *= mFillAmount;
			vector.y = vector2.y - vector3.y;
			break;
		case FillDirection.TowardTop:
			y = 0f - (1f - mFillAmount);
			vector2.y = vector.y + vector3.y;
			break;
		case FillDirection.TowardRight:
			num *= mFillAmount;
			vector2.x = vector.x + vector3.x;
			break;
		case FillDirection.TowardLeft:
			x = 1f - mFillAmount;
			vector.x = vector2.x - vector3.x;
			break;
		}
		verts.Add(new Vector3(num, y, 0f));
		verts.Add(new Vector3(num, num2, 0f));
		verts.Add(new Vector3(x, num2, 0f));
		verts.Add(new Vector3(x, y, 0f));
		uvs.Add(vector2);
		uvs.Add(new Vector2(vector2.x, vector.y));
		uvs.Add(vector);
		uvs.Add(new Vector2(vector.x, vector2.y));
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
	}
}
