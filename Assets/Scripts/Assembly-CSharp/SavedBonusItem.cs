using System;

[Serializable]
public class SavedBonusItem : SavedMovingObject
{
	public BonusItem.BonusItemType BonusType;

	public int Value;

	public bool isMagnetized;

	public SavedBonusItem()
	{
	}

	public SavedBonusItem(BonusItem bi)
		: base(bi)
	{
		BonusType = bi.BonusType;
		Value = bi.Value;
		isMagnetized = bi.isMagnetized;
	}

	public BonusItem Apply(PathElement pe)
	{
		BonusItem bonusItem = BonusItem.Instantiate(BonusType);
		Apply(bonusItem);
		bonusItem.transform.parent = pe.transform;
		bonusItem.Value = Value;
		bonusItem.isMagnetized = isMagnetized;
		return bonusItem;
	}
}
