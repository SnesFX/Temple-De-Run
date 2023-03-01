using System.Collections.Generic;
using UnityEngine;

public class DynamicElement : MonoBehaviour
{
	public SpawnPool Pool;

	public Renderer primaryRenderer;

	private static Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

	public GameObject SetDataModelReference(string prefabName, string poolName = null)
	{
		ClearAllChildren();
		string text = string.Format("Prefabs/Temple/{0}_prefab", prefabName);
		GameObject gameObject = null;
		if (_prefabs.ContainsKey(text))
		{
			gameObject = _prefabs[text];
		}
		else
		{
			gameObject = (GameObject)Resources.Load(text, typeof(GameObject));
			if (gameObject == null)
			{
				Debug.LogError("Could not load prefab: " + prefabName, this);
				return null;
			}
			_prefabs.Add(text, gameObject);
		}
		GameObject gameObject2;
		if (poolName != null)
		{
			SpawnPool spawnPool = PoolManager.Pools[poolName];
			gameObject2 = spawnPool.Spawn(gameObject.transform).gameObject;
			DynamicElement dynamicElement = gameObject2.GetComponent<DynamicElement>();
			if (dynamicElement == null)
			{
				dynamicElement = gameObject2.AddComponent<DynamicElement>();
			}
			dynamicElement.Pool = spawnPool;
		}
		else
		{
			Pool = null;
			gameObject2 = (GameObject)Object.Instantiate(gameObject);
		}
		gameObject2.transform.parent = base.transform;
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
		primaryRenderer = base.transform.GetComponentInChildren<Renderer>();
		return gameObject2;
	}

	public void ClearAllChildren()
	{
		if (base.transform.childCount == 0)
		{
			return;
		}
		foreach (Transform item in base.transform)
		{
			DynamicElement component = item.GetComponent<DynamicElement>();
			if (component == null)
			{
				Object.Destroy(item.gameObject);
			}
			else
			{
				component.DestroySelf();
			}
		}
	}

	public void DestroySelf()
	{
		primaryRenderer = null;
		if (Pool == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		base.transform.parent = Pool.transform;
		Pool.Despawn(base.transform);
	}
}
