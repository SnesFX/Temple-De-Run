using UnityEngine;

public class EtceteraAndroidEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		EtceteraAndroidManager.alertButtonClickedEvent += alertButtonClickedEvent;
		EtceteraAndroidManager.alertCancelledEvent += alertCancelledEvent;
		EtceteraAndroidManager.promptFinishedWithTextEvent += promptFinishedWithTextEvent;
		EtceteraAndroidManager.promptCancelledEvent += promptCancelledEvent;
		EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent += twoFieldPromptFinishedWithTextEvent;
		EtceteraAndroidManager.twoFieldPromptCancelledEvent += twoFieldPromptCancelledEvent;
		EtceteraAndroidManager.webViewCancelledEvent += webViewCancelledEvent;
		EtceteraAndroidManager.albumChooserCancelledEvent += albumChooserCancelledEvent;
		EtceteraAndroidManager.albumChooserSucceededEvent += albumChooserSucceededEvent;
		EtceteraAndroidManager.photoChooserCancelledEvent += photoChooserCancelledEvent;
		EtceteraAndroidManager.photoChooserSucceededEvent += photoChooserSucceededEvent;
		EtceteraAndroidManager.ttsInitializedEvent += ttsInitializedEvent;
		EtceteraAndroidManager.ttsFailedToInitializeEvent += ttsFailedToInitializeEvent;
		EtceteraAndroidManager.askForReviewDontAskAgainEvent += askForReviewDontAskAgainEvent;
		EtceteraAndroidManager.askForReviewRemindMeLaterEvent += askForReviewRemindMeLaterEvent;
		EtceteraAndroidManager.askForReviewWillOpenMarketEvent += askForReviewWillOpenMarketEvent;
	}

	private void OnDisable()
	{
		EtceteraAndroidManager.alertButtonClickedEvent -= alertButtonClickedEvent;
		EtceteraAndroidManager.alertCancelledEvent -= alertCancelledEvent;
		EtceteraAndroidManager.promptFinishedWithTextEvent -= promptFinishedWithTextEvent;
		EtceteraAndroidManager.promptCancelledEvent -= promptCancelledEvent;
		EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent -= twoFieldPromptFinishedWithTextEvent;
		EtceteraAndroidManager.twoFieldPromptCancelledEvent -= twoFieldPromptCancelledEvent;
		EtceteraAndroidManager.webViewCancelledEvent -= webViewCancelledEvent;
		EtceteraAndroidManager.albumChooserCancelledEvent -= albumChooserCancelledEvent;
		EtceteraAndroidManager.albumChooserSucceededEvent -= albumChooserSucceededEvent;
		EtceteraAndroidManager.photoChooserCancelledEvent -= photoChooserCancelledEvent;
		EtceteraAndroidManager.photoChooserSucceededEvent -= photoChooserSucceededEvent;
		EtceteraAndroidManager.ttsInitializedEvent -= ttsInitializedEvent;
		EtceteraAndroidManager.ttsFailedToInitializeEvent -= ttsFailedToInitializeEvent;
		EtceteraAndroidManager.askForReviewDontAskAgainEvent -= askForReviewDontAskAgainEvent;
		EtceteraAndroidManager.askForReviewRemindMeLaterEvent -= askForReviewRemindMeLaterEvent;
		EtceteraAndroidManager.askForReviewWillOpenMarketEvent -= askForReviewWillOpenMarketEvent;
	}

	private void alertButtonClickedEvent(string positiveButton)
	{
		Debug.Log("alertButtonClickedEvent: " + positiveButton);
	}

	private void alertCancelledEvent()
	{
		Debug.Log("alertCancelledEvent");
	}

	private void promptFinishedWithTextEvent(string param)
	{
		Debug.Log("promptFinishedWithTextEvent: " + param);
	}

	private void promptCancelledEvent()
	{
		Debug.Log("promptCancelledEvent");
	}

	private void twoFieldPromptFinishedWithTextEvent(string text1, string text2)
	{
		Debug.Log("twoFieldPromptFinishedWithTextEvent: " + text1 + ", " + text2);
	}

	private void twoFieldPromptCancelledEvent()
	{
		Debug.Log("twoFieldPromptCancelledEvent");
	}

	private void webViewCancelledEvent()
	{
		Debug.Log("webViewCancelledEvent");
	}

	private void albumChooserCancelledEvent()
	{
		Debug.Log("albumChooserCancelledEvent");
	}

	private void albumChooserSucceededEvent(Texture2D tex)
	{
		Debug.Log("albumChooserSucceededEvent: " + tex);
	}

	private void photoChooserCancelledEvent()
	{
		Debug.Log("photoChooserCancelledEvent");
	}

	private void photoChooserSucceededEvent(Texture2D tex)
	{
		Debug.Log("photoChooserSucceededEvent: " + tex);
	}

	private void ttsInitializedEvent()
	{
		Debug.Log("ttsInitializedEvent");
	}

	private void ttsFailedToInitializeEvent()
	{
		Debug.Log("ttsFailedToInitializeEvent");
	}

	private void askForReviewDontAskAgainEvent()
	{
		Debug.Log("askForReviewDontAskAgainEvent");
	}

	private void askForReviewRemindMeLaterEvent()
	{
		Debug.Log("askForReviewRemindMeLaterEvent");
	}

	private void askForReviewWillOpenMarketEvent()
	{
		Debug.Log("askForReviewWillOpenMarketEvent");
	}
}
