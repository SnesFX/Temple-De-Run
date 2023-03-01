using System.Collections.Generic;
using UnityEngine;

public class BonusItem : MovingObject
{
	public enum BonusItemType
	{
		kBonusItemNone = 0,
		kBonusItemCoin = 1,
		kBonusItemCoinDouble = 2,
		kBonusItemCoinTriple = 3,
		kBonusItemCoinBonus = 4,
		kBonusItemInvincibility = 5,
		kBonusItemVacuum = 6,
		kBonusItemBoost = 7
	}

	public BonusItemType BonusType;

	public int Value;

	public bool isMagnetized;

	private static int sCount;

	private static Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

	private void Start()
	{
	}

	public override void Update()
	{
		base.Update();
	}

	public bool IsACoin()
	{
		return BonusType > BonusItemType.kBonusItemNone && BonusType <= BonusItemType.kBonusItemCoinTriple;
	}

	public static BonusItem Instantiate(BonusItemType type)
	{
		string text = "Prefabs/Temple/Pickups/";
		switch (type)
		{
		case BonusItemType.kBonusItemCoin:
			text += "coinYellow";
			break;
		case BonusItemType.kBonusItemCoinDouble:
			text += "coinRed";
			break;
		case BonusItemType.kBonusItemCoinTriple:
			text += "coinBlue";
			break;
		case BonusItemType.kBonusItemBoost:
			text += "pickupBoost";
			break;
		case BonusItemType.kBonusItemCoinBonus:
			text += "pickupCoinBonus";
			break;
		case BonusItemType.kBonusItemInvincibility:
			text += "pickupInvincibility";
			break;
		case BonusItemType.kBonusItemVacuum:
			text += "pickupVacuum";
			break;
		default:
			Debug.LogError("Not setup for this type of bonus item yet: " + type);
			break;
		}
		text += "_prefab";
		GameObject gameObject;
		if (_prefabs.ContainsKey(text))
		{
			gameObject = _prefabs[text];
		}
		else
		{
			gameObject = (GameObject)Resources.Load(text, typeof(GameObject));
			_prefabs.Add(text, gameObject);
		}
		SpawnPool spawnPool = PoolManager.Pools["BonusItems"];
		GameObject gameObject2 = spawnPool.Spawn(gameObject.transform).gameObject;
		BonusItem component = gameObject2.GetComponent<BonusItem>();
		if (component.IsACoin())
		{
			gameObject2.name = "Coin " + sCount++;
		}
		else
		{
			gameObject2.name = "Bonus " + sCount++;
		}
		switch (type)
		{
		case BonusItemType.kBonusItemCoin:
			component.Value = 5;
			component.AngularVelocity = -4.5f;
			break;
		case BonusItemType.kBonusItemCoinDouble:
			component.Value = 10;
			component.AngularVelocity = -4.5f;
			break;
		case BonusItemType.kBonusItemCoinTriple:
			component.Value = 15;
			component.AngularVelocity = -4.5f;
			break;
		}
		component.enabled = true;
		component.SetVisibility(true);
		return component;
	}

	public void OnDespawned()
	{
		isMagnetized = false;
		base.transform.parent = PoolManager.Pools["BonusItems"].transform;
	}

	public void DestroySelf()
	{
		SpawnPool spawnPool = PoolManager.Pools["BonusItems"];
		base.transform.parent = spawnPool.transform;
		spawnPool.Despawn(base.transform);
	}
}
