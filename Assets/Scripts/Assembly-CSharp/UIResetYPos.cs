using UnityEngine;

public class UIResetYPos : MonoBehaviour
{
	public float yPos;

	private void Awake()
	{
		yPos = base.transform.localPosition.y;
	}

	private void OnEnable()
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y = yPos;
		base.transform.localPosition = localPosition;
	}
}
