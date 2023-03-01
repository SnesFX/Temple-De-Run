using UnityEngine;

public class AngelWingIndicator : MonoBehaviour
{
	public GameController Controller;

	public CharacterPlayer GamePlayer;

	private void Start()
	{
	}

	private void Update()
	{
		float num = 0f;
		if (!Controller.IsTutorialMode && GamePlayer.AngelWingsCount > 0 && GamePlayer.AngelWingsRechargeTimeLeft == 0f)
		{
			num = -44f;
		}
		if (base.transform.localPosition.x != num)
		{
			base.transform.localPosition = new Vector3(num, 0f, 0f);
		}
	}
}
