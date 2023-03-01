using UnityEngine;

public class FaceCamera : MonoBehaviour
{
	private void Update()
	{
		base.transform.LookAt(Camera.main.transform.position, Camera.main.transform.up);
	}
}
