using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager Instance;

	private void Start()
	{
		Instance = this;
	}

	public int GetActivePlayer()
	{
		return 1;
	}
}
