using System.Collections;
using UnityEngine;

public class MessageBoard : MonoBehaviour
{
	public UILabel Label;

	public int MessageBoardLastDistance;

	private Vector3 UpPosition = new Vector3(0f, 270f, 0f);

	private Vector3 DownPosition = new Vector3(0f, 80f, 0f);

	public static MessageBoard Instance;

	private void Start()
	{
		Instance = this;
		base.transform.localPosition = UpPosition;
	}

	private void Update()
	{
		int num = (int)GameController.SharedInstance.DistanceTraveled;
		if (num < 1249 && num >= MessageBoardLastDistance + 250)
		{
			ShowMessageboard(string.Format("{0}m", MessageBoardLastDistance + 250));
			MessageBoardLastDistance += 250;
		}
		else if (num >= MessageBoardLastDistance + 500)
		{
			ShowMessageboard(string.Format("{0}m", MessageBoardLastDistance + 500));
			MessageBoardLastDistance += 500;
		}
	}

	public void ShowMessageboard(string message)
	{
		Label.text = message;
		StartCoroutine(AnimateMessageBoard());
	}

	public IEnumerator AnimateMessageBoard()
	{
		AudioManager.Instance.PlayFX(AudioManager.Effects.swish);
		base.transform.localPosition = UpPosition;
		TweenPosition tp2 = TweenPosition.Begin(base.gameObject, 0.25f, DownPosition);
		yield return new WaitForSeconds(2f);
		tp2 = TweenPosition.Begin(base.gameObject, 0.25f, UpPosition);
	}

	public void OnEnable()
	{
		MessageBoardLastDistance = 0;
		base.transform.localPosition = UpPosition;
		TweenPosition component = GetComponent<TweenPosition>();
		if ((bool)component)
		{
			Object.Destroy(component);
		}
	}
}
