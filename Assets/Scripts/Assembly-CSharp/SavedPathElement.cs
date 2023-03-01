using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedPathElement
{
	public string Name;

	public Vector3 Position;

	public PathElement.PathType Type;

	public PathElement.PathLevel Level;

	public PathElement.ObstacleType Obstacles;

	public bool HasCoins;

	public bool HasBonusItems;

	public bool SkippedBonusItems;

	public bool IsTutorialObstacle;

	public bool IsTutorialEnd;

	public PathElement.CoinRunLocation CoinLocationStart;

	public PathElement.CoinRunLocation CoinLocationEnd;

	public bool IsCoinRunInAir;

	public SavedPathElement PathNorth;

	public SavedPathElement PathEast;

	public SavedPathElement PathWest;

	public SavedPathElement PathSouth;

	public int TileIndex;

	public int SpacesSinceLastBonusItem;

	public int SpacesSinceLastTurn;

	public int SpacesSinceLastObstacle;

	public int BackToBackObstacleCount;

	public int SpacesSinceLastCoinRun;

	public int CoinRunCount;

	public List<SavedBonusItem> BonusItems;

	public bool HasSafteyNet;

	public Vector3 SafteyNetPosition;

	public Quaternion SafteynetRotation;

	public SavedPathElement()
	{
	}

	public SavedPathElement(PathElement e, PathElement.PathDirection origin = PathElement.PathDirection.kPathDirectionNone)
	{
		Name = e.name;
		Position = e.transform.position;
		Type = e.Type;
		Level = e.Level;
		Obstacles = e.Obstacles;
		HasCoins = e.HasCoins;
		HasBonusItems = e.HasBonusItems;
		SkippedBonusItems = e.SkippedBonusItems;
		IsTutorialObstacle = e.IsTutorialObstacle;
		IsTutorialEnd = e.IsTutorialEnd;
		CoinLocationStart = e.CoinLocationStart;
		CoinLocationEnd = e.CoinLocationEnd;
		IsCoinRunInAir = e.IsCoinRunInAir;
		SpacesSinceLastBonusItem = e.SpacesSinceLastBonusItem;
		SpacesSinceLastTurn = e.SpacesSinceLastTurn;
		SpacesSinceLastObstacle = e.SpacesSinceLastObstacle;
		BackToBackObstacleCount = e.BackToBackObstacleCount;
		SpacesSinceLastCoinRun = e.SpacesSinceLastCoinRun;
		CoinRunCount = e.CoinRunCount;
		if ((bool)e.PathNorth && origin != PathElement.PathDirection.kPathDirectionNorth)
		{
			PathNorth = new SavedPathElement(e.PathNorth, PathElement.PathDirection.kPathDirectionSouth);
		}
		if ((bool)e.PathEast && origin != PathElement.PathDirection.kPathDirectionEast)
		{
			PathEast = new SavedPathElement(e.PathEast, PathElement.PathDirection.kPathDirectionWest);
		}
		if ((bool)e.PathSouth && origin != PathElement.PathDirection.kPathDirectionSouth)
		{
			PathSouth = new SavedPathElement(e.PathSouth, PathElement.PathDirection.kPathDirectionNorth);
		}
		if ((bool)e.PathWest && origin != PathElement.PathDirection.kPathDirectionWest)
		{
			PathWest = new SavedPathElement(e.PathWest, PathElement.PathDirection.kPathDirectionEast);
		}
		BonusItem[] componentsInChildren = e.GetComponentsInChildren<BonusItem>();
		if (componentsInChildren.Length > 0)
		{
			BonusItems = new List<SavedBonusItem>(componentsInChildren.Length);
			BonusItem[] array = componentsInChildren;
			foreach (BonusItem bi in array)
			{
				BonusItems.Add(new SavedBonusItem(bi));
			}
		}
		if (e.SafteyNet != null)
		{
			HasSafteyNet = true;
			SafteyNetPosition = e.SafteyNet.transform.position;
			SafteynetRotation = e.SafteyNet.transform.rotation;
		}
	}

	public PathElement Apply(SavedGameController savedController, PathElement.PathDirection origin = PathElement.PathDirection.kPathDirectionNone, PathElement parent = null)
	{
		GameObject gameObject = PathElement.Instantiate(Type, Level);
		gameObject.transform.position = Position;
		PathElement component = gameObject.GetComponent<PathElement>();
		component.AddObstacle(Obstacles);
		gameObject.name = Name;
		component.HasCoins = HasCoins;
		component.HasBonusItems = HasBonusItems;
		component.SkippedBonusItems = SkippedBonusItems;
		component.IsTutorialObstacle = IsTutorialObstacle;
		component.IsTutorialEnd = IsTutorialEnd;
		component.CoinLocationStart = CoinLocationStart;
		component.CoinLocationEnd = CoinLocationEnd;
		component.IsCoinRunInAir = IsCoinRunInAir;
		component.SpacesSinceLastBonusItem = SpacesSinceLastBonusItem;
		component.SpacesSinceLastTurn = SpacesSinceLastTurn;
		component.SpacesSinceLastObstacle = SpacesSinceLastObstacle;
		component.BackToBackObstacleCount = BackToBackObstacleCount;
		component.SpacesSinceLastCoinRun = SpacesSinceLastCoinRun;
		component.CoinRunCount = CoinRunCount;
		if (PathNorth != null && origin != PathElement.PathDirection.kPathDirectionNorth)
		{
			component.PathNorth = PathNorth.Apply(savedController, PathElement.PathDirection.kPathDirectionSouth, component);
			component.PathNorth.PathSouth = component;
		}
		if (PathEast != null && origin != PathElement.PathDirection.kPathDirectionEast)
		{
			component.PathEast = PathEast.Apply(savedController, PathElement.PathDirection.kPathDirectionWest, component);
			component.PathEast.PathWest = component;
		}
		if (PathSouth != null && origin != PathElement.PathDirection.kPathDirectionSouth)
		{
			component.PathSouth = PathSouth.Apply(savedController, PathElement.PathDirection.kPathDirectionNorth, component);
			component.PathSouth.PathNorth = component;
		}
		if (PathWest != null && origin != PathElement.PathDirection.kPathDirectionWest)
		{
			component.PathWest = PathWest.Apply(savedController, PathElement.PathDirection.kPathDirectionEast, component);
			component.PathWest.PathEast = component;
		}
		if (BonusItems != null)
		{
			foreach (SavedBonusItem bonusItem in BonusItems)
			{
				bonusItem.Apply(component);
			}
		}
		if (HasSafteyNet)
		{
			GameObject gameObject2 = component.InstantiateSafteynet();
			gameObject2.transform.position = SafteyNetPosition;
			gameObject2.transform.rotation = SafteynetRotation;
			component.SafteyNet = gameObject2;
			component.MySafteyNetSprite = gameObject2.GetComponentInChildren<UISprite>();
		}
		component.UpdateCachedComponents();
		savedController.ProcessPathElement(component);
		return component;
	}
}
