using UnityEngine;

public class ESCAsClick : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SendMessage("OnClick");
		}
	}
}
