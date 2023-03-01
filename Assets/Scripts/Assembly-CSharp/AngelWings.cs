using UnityEngine;

public class AngelWings : MonoBehaviour
{
	public CharacterPlayer GamePlayer;

	private UISprite Sprite;

	private float LastHeight = -1f;

	private void Start()
	{
		Sprite = GetComponentInChildren<UISprite>();
	}

	private void Update()
	{
		if (!GamePlayer.IsDead && GamePlayer.HasAngelWings && !GamePlayer.IsSliding)
		{
			Sprite.enabled = true;
			float num = 10f;
			if (GamePlayer.ActiveCharacterId == 1 || GamePlayer.ActiveCharacterId == 2 || GamePlayer.ActiveCharacterId == 3 || GamePlayer.ActiveCharacterId == 4)
			{
				num = ((!GamePlayer.IsJumping) ? 9.5f : 8.5f);
			}
			else if (GamePlayer.ActiveCharacterId == 5 || GamePlayer.ActiveCharacterId == 6)
			{
				num = ((!GamePlayer.IsJumping) ? 10f : 9f);
			}
			if (num != LastHeight)
			{
				LastHeight = num;
				base.transform.localPosition = new Vector3(0f, num + 2f, 0f);
			}
			float num2 = 0.8f;
			if (GamePlayer.AngelWingsTimeLeft < 2.5f)
			{
				num2 = Sprite.color.a;
				num2 -= Time.smoothDeltaTime * 2f;
				if (num2 <= 0f)
				{
					num2 = 0.8f;
				}
			}
			if (Sprite.color.a != num2)
			{
				Sprite.color = new Color(num2, num2, num2, num2);
			}
		}
		else
		{
			Sprite.enabled = false;
		}
	}
}
