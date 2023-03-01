using UnityEngine;

public class BoostEffect : MonoBehaviour
{
	public CharacterPlayer GamePlayer;

	public Camera GameCamera;

	private Color defaultColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);

	public UISprite Sprite;

	public static BoostEffect Instance;

	private void Start()
	{
		Instance = this;
		Sprite.color = defaultColor;
	}

	public float GetAlpha()
	{
		return Sprite.color.a;
	}

	private void Update()
	{
		if (GamePlayer.IsDead || !GamePlayer.HasBoost)
		{
			Sprite.enabled = false;
			return;
		}
		Sprite.enabled = true;
		Vector3 vector = GameCamera.transform.position - GamePlayer.transform.position;
		vector.y = 0f;
		vector.Normalize();
		float num = 12f;
		base.transform.position = GamePlayer.transform.position + vector * num;
		base.transform.localScale = GamePlayer.transform.localScale * 0.2f;
		float y = GamePlayer.transform.position.y + ((!GamePlayer.IsSliding) ? 5f : 0f);
		base.transform.position = new Vector3(base.transform.position.x, y, base.transform.position.z);
		base.transform.LookAt(GameCamera.transform.position, Vector3.up);
		if (GamePlayer.BoostDistanceLeft < 50f)
		{
			float a = Sprite.color.a;
			a -= Time.smoothDeltaTime * 2f;
			if (a <= 0f)
			{
				a = 0.8f;
			}
			Sprite.color = new Color(a, a, a, a);
		}
		else
		{
			Sprite.color = defaultColor;
		}
		if (GamePlayer.BoostDistanceLeft <= 0f && Sprite.color.a < 0.2f)
		{
			Sprite.enabled = false;
			Sprite.color = defaultColor;
		}
	}
}
