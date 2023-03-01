using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite (Basic)")]
[ExecuteInEditMode]
public class UISprite : UIWidget
{
	[SerializeField]
	private UIAtlas mAtlas;

	[SerializeField]
	private string mSpriteName;

	protected UIAtlas.Sprite mSprite;

	protected Rect mOuter;

	protected Rect mOuterUV;

	private bool mSpriteSet;

	private string mLastName = string.Empty;

	public Rect outerUV
	{
		get
		{
			UpdateUVs(false);
			return mOuterUV;
		}
	}

	public UIAtlas atlas
	{
		get
		{
			return mAtlas;
		}
		set
		{
			if (mAtlas != value)
			{
				mAtlas = value;
				mSpriteSet = false;
				mSprite = null;
				material = ((!(mAtlas != null)) ? null : mAtlas.spriteMaterial);
				if (string.IsNullOrEmpty(mSpriteName) && mAtlas != null && mAtlas.spriteList.Count > 0)
				{
					sprite = mAtlas.spriteList[0];
					mSpriteName = mSprite.name;
				}
				if (!string.IsNullOrEmpty(mSpriteName))
				{
					string text = mSpriteName;
					mSpriteName = string.Empty;
					spriteName = text;
					mChanged = true;
					UpdateUVs(true);
				}
			}
		}
	}

	public string spriteName
	{
		get
		{
			return mSpriteName;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(mSpriteName))
				{
					mSpriteName = string.Empty;
					mSprite = null;
					mChanged = true;
				}
			}
			else if (mSpriteName != value)
			{
				mSpriteName = value;
				mSprite = null;
				mChanged = true;
				if (mSprite != null)
				{
					UpdateUVs(true);
				}
			}
		}
	}

	protected UIAtlas.Sprite sprite
	{
		get
		{
			if (!mSpriteSet)
			{
				mSprite = null;
			}
			if (mSprite == null && mAtlas != null)
			{
				if (!string.IsNullOrEmpty(mSpriteName))
				{
					sprite = mAtlas.GetSprite(mSpriteName);
				}
				if (mSprite == null && mAtlas.spriteList.Count > 0)
				{
					sprite = mAtlas.spriteList[0];
					mSpriteName = mSprite.name;
				}
				if (mSprite != null)
				{
					material = mAtlas.spriteMaterial;
				}
			}
			return mSprite;
		}
		set
		{
			mSprite = value;
			mSpriteSet = true;
			material = ((mSprite == null || !(mAtlas != null)) ? null : mAtlas.spriteMaterial);
		}
	}

	public override Vector2 pivotOffset
	{
		get
		{
			Vector2 zero = Vector2.zero;
			if (sprite != null)
			{
				Pivot pivot = base.pivot;
				switch (pivot)
				{
				case Pivot.Top:
				case Pivot.Center:
				case Pivot.Bottom:
					zero.x = (-1f - mSprite.paddingRight + mSprite.paddingLeft) * 0.5f;
					break;
				case Pivot.TopRight:
				case Pivot.Right:
				case Pivot.BottomRight:
					zero.x = -1f - mSprite.paddingRight;
					break;
				default:
					zero.x = mSprite.paddingLeft;
					break;
				}
				switch (pivot)
				{
				case Pivot.Left:
				case Pivot.Center:
				case Pivot.Right:
					zero.y = (1f + mSprite.paddingBottom - mSprite.paddingTop) * 0.5f;
					break;
				case Pivot.BottomLeft:
				case Pivot.Bottom:
				case Pivot.BottomRight:
					zero.y = 1f + mSprite.paddingBottom;
					break;
				default:
					zero.y = 0f - mSprite.paddingTop;
					break;
				}
			}
			return zero;
		}
	}

	public override Material material
	{
		get
		{
			Material material = base.material;
			if (material == null)
			{
				material = ((!(mAtlas != null)) ? null : mAtlas.spriteMaterial);
				mSprite = null;
				this.material = material;
				if (material != null)
				{
					UpdateUVs(true);
				}
			}
			return material;
		}
	}

	public virtual void UpdateUVs(bool force)
	{
		if (sprite == null || (!force && !(mOuter != mSprite.outer)))
		{
			return;
		}
		Texture texture = base.mainTexture;
		if (texture != null)
		{
			mOuter = mSprite.outer;
			mOuterUV = mOuter;
			if (mAtlas.coordinates == UIAtlas.Coordinates.Pixels)
			{
				mOuterUV = NGUIMath.ConvertToTexCoords(mOuterUV, texture.width, texture.height);
			}
			mChanged = true;
		}
	}

	public override void MakePixelPerfect()
	{
		Texture texture = base.mainTexture;
		if (texture != null)
		{
			Rect rect = NGUIMath.ConvertToPixels(outerUV, texture.width, texture.height, true);
			Vector3 localScale = base.cachedTransform.localScale;
			float pixelSize = atlas.pixelSize;
			localScale.x = rect.width * pixelSize;
			localScale.y = rect.height * pixelSize;
			localScale.z = 1f;
			base.cachedTransform.localScale = localScale;
		}
		base.MakePixelPerfect();
	}

	protected override void OnStart()
	{
		if (mAtlas != null)
		{
			UpdateUVs(true);
		}
	}

	public override bool OnUpdate()
	{
		if (mLastName != mSpriteName)
		{
			mSprite = null;
			mChanged = true;
			mLastName = mSpriteName;
			UpdateUVs(false);
			return true;
		}
		UpdateUVs(false);
		return false;
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
	{
		Vector2 item = new Vector2(mOuterUV.xMin, mOuterUV.yMin);
		Vector2 item2 = new Vector2(mOuterUV.xMax, mOuterUV.yMax);
		verts.Add(new Vector3(1f, 0f, 0f));
		verts.Add(new Vector3(1f, -1f, 0f));
		verts.Add(new Vector3(0f, -1f, 0f));
		verts.Add(new Vector3(0f, 0f, 0f));
		uvs.Add(item2);
		uvs.Add(new Vector2(item2.x, item.y));
		uvs.Add(item);
		uvs.Add(new Vector2(item.x, item2.y));
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
	}
}
