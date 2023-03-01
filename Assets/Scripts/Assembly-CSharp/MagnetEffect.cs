using UnityEngine;

public class MagnetEffect : MonoBehaviour
{
	private Color defaultColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);

	public CharacterPlayer GamePlayer;

	public UISprite Sprite;

	public GameCamera GameCamera;

	private void Start()
	{
	}

	private void Update()
	{
		if (GamePlayer.IsDead || !GamePlayer.HasVacuum)
		{
			Sprite.enabled = false;
			return;
		}
		Vector3 position = GamePlayer.transform.position;
		position.y += ((!GamePlayer.IsSliding) ? 6f : 2f);
		Vector3 vector = GameCamera.transform.position - position;
		vector.Normalize();
		float num = 12f;
		base.transform.position = position + vector * num;
		base.transform.localScale = GamePlayer.transform.localScale * 0.1f;
		base.transform.LookAt(GameCamera.transform.position, GameCamera.transform.up);
		if (GamePlayer.VacuumTimeLeft < 2.5f)
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
		Sprite.enabled = true;
		if (GamePlayer.VacuumTimeLeft <= 0f)
		{
			Sprite.enabled = false;
			Sprite.color = defaultColor;
		}
	}
}
