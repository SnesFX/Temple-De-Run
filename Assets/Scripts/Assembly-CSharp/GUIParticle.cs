using UnityEngine;

public class GUIParticle : MonoBehaviour
{
	public enum FadeType
	{
		None = 0,
		FadeIn = 1,
		FadeOut = 2
	}

	public UISprite Sprite;

	public Vector3 Motion;

	public Vector3 FinalScale;

	public bool Gravity;

	public float LifeSpan;

	public float RotationRate;

	public FadeType Fade;

	public float Delay;

	public float GravityValue;

	private float Lived;

	private Vector3 StartScale;

	private float StartAlpha;

	public void Setup()
	{
		Sprite = GetComponentInChildren<UISprite>();
		Die();
	}

	private void Update()
	{
		if (Delay > 0f)
		{
			Delay -= Time.smoothDeltaTime;
			if (Delay <= 0f)
			{
				Sprite.enabled = true;
			}
			return;
		}
		Lived += Time.smoothDeltaTime;
		if (Lived > LifeSpan)
		{
			Die();
			return;
		}
		float num = Lived / LifeSpan;
		if (Fade != 0)
		{
			float num2 = ((Fade != FadeType.FadeOut) ? 1f : 0f);
			float num3 = num2 - StartAlpha;
			float num4 = StartAlpha + num3 * num;
			Sprite.color = new Color(num4, num4, num4, num4);
		}
		Vector3 vector = Motion * Time.smoothDeltaTime;
		if (Gravity)
		{
			Motion.y -= GravityValue * Time.smoothDeltaTime;
		}
		base.transform.localPosition = base.transform.localPosition + vector;
		float z = base.transform.localEulerAngles.z;
		z += RotationRate * Time.smoothDeltaTime;
		base.transform.localEulerAngles = new Vector3(0f, 0f, z);
		Vector3 vector2 = FinalScale - StartScale;
		base.transform.localScale = StartScale + vector2 * num;
	}

	public void Reset()
	{
		Sprite.enabled = true;
		base.enabled = true;
		Sprite.color = Color.white;
		base.transform.localEulerAngles = Vector3.zero;
		base.transform.localScale = Vector3.one;
		Lived = 0f;
		Delay = 0f;
		Gravity = false;
		RotationRate = 0f;
		FinalScale = Vector3.one;
		GravityValue = 1500f;
	}

	public void Die()
	{
		base.enabled = false;
		Sprite.enabled = false;
	}

	public void BeginAnimation()
	{
		StartScale = base.transform.localScale;
		StartAlpha = Sprite.color.a;
		Sprite.enabled = Delay <= 0f;
	}
}
