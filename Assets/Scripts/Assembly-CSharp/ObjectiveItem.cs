using UnityEngine;

public class ObjectiveItem : MonoBehaviour
{
	public UISprite AwardedSprite;

	public UISprite NotAwardedSprite;

	public UILabel NameLabel;

	public UILabel DescriptionLabel;

	public UIDragPanelContents DragPanel;

	public void Setup(RecordManager.cAchievement achievement, bool awarded, bool inList = true)
	{
		AwardedSprite.enabled = awarded;
		NotAwardedSprite.enabled = !awarded;
		NameLabel.text = achievement.name;
		DescriptionLabel.text = ((!awarded) ? achievement.preDescription : achievement.postDescription);
		GetComponent<UIStackSpacing>().enabled = inList;
		GetComponent<UIDragPanelContents>().enabled = inList;
	}
}
