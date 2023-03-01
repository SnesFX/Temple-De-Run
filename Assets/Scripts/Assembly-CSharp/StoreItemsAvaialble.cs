using UnityEngine;

public class StoreItemsAvaialble : MonoBehaviour
{
	public UILabel Text;

	public UISprite Plate;

	private void OnEnable()
	{
		if (!(RecordManager.Instance == null))
		{
			int affordableUpgradeTypeCount = RecordManager.Instance.GetAffordableUpgradeTypeCount(PlayerManager.Instance.GetActivePlayer());
			Debug.Log("avail: " + affordableUpgradeTypeCount);
			if (affordableUpgradeTypeCount == 0)
			{
				Text.enabled = false;
				Plate.enabled = false;
			}
			else
			{
				Text.enabled = true;
				Text.text = affordableUpgradeTypeCount.ToString();
				Plate.enabled = true;
			}
		}
	}
}
