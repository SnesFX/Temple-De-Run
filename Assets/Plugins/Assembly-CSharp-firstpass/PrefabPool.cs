using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrefabPool
{
	public Transform prefab;

	internal GameObject prefabGO;

	public int preloadAmount = 1;

	public bool limitInstances;

	public int limitAmount = 100;

	public bool cullDespawned;

	public int cullAbove = 50;

	public int cullDelay = 60;

	public int cullMaxPerPass = 5;

	public bool _logMessages;

	private bool forceLoggingSilent;

	internal SpawnPool spawnPool;

	private bool cullingActive;

	internal List<Transform> spawned = new List<Transform>();

	internal List<Transform> despawned = new List<Transform>();

	private bool _preloaded;

	private bool logMessages
	{
		get
		{
			if (forceLoggingSilent)
			{
				return false;
			}
			if (spawnPool.logMessages)
			{
				return spawnPool.logMessages;
			}
			return _logMessages;
		}
	}

	internal int totalCount
	{
		get
		{
			int num = 0;
			num += spawned.Count;
			return num + despawned.Count;
		}
	}

	internal bool preloaded
	{
		get
		{
			return _preloaded;
		}
		private set
		{
			_preloaded = value;
		}
	}

	public PrefabPool(Transform prefab)
	{
		this.prefab = prefab;
		prefabGO = prefab.gameObject;
	}

	public PrefabPool()
	{
	}

	internal void inspectorInstanceConstructor()
	{
		prefabGO = prefab.gameObject;
		spawned = new List<Transform>();
		despawned = new List<Transform>();
	}

	public void SelfDestruct()
	{
		prefab = null;
		prefabGO = null;
		spawnPool = null;
		foreach (Transform item in despawned)
		{
			UnityEngine.Object.Destroy(item);
		}
		foreach (Transform item2 in spawned)
		{
			UnityEngine.Object.Destroy(item2);
		}
		spawned.Clear();
		despawned.Clear();
	}

	internal bool DespawnInstance(Transform xform)
	{
		if (logMessages)
		{
			Debug.Log(string.Format("SpawnPool {0} ({1}): Despawning '{2}'", spawnPool.poolName, prefab.name, xform.name));
		}
		spawned.Remove(xform);
		despawned.Add(xform);
		xform.gameObject.BroadcastMessage("OnDespawned", SendMessageOptions.DontRequireReceiver);
		xform.gameObject.SetActiveRecursively(false);
		if (!cullingActive && cullDespawned && totalCount > cullAbove)
		{
			cullingActive = true;
			spawnPool.StartCoroutine(CullDespawned());
		}
		return true;
	}

	internal IEnumerator CullDespawned()
	{
		if (logMessages)
		{
			Debug.Log(string.Format("SpawnPool {0} ({1}): CULLING TRIGGERED! Waiting {2}sec to begin checking for despawns...", spawnPool.poolName, prefab.name, cullDelay));
		}
		yield return new WaitForSeconds(cullDelay);
		while (totalCount > cullAbove)
		{
			for (int i = 0; i < cullMaxPerPass; i++)
			{
				if (totalCount <= cullAbove)
				{
					break;
				}
				if (despawned.Count > 0)
				{
					Transform inst = despawned[0];
					despawned.RemoveAt(0);
					UnityEngine.Object.Destroy(inst.gameObject);
					if (logMessages)
					{
						Debug.Log(string.Format("SpawnPool {0} ({1}): CULLING to {2} instances. Now at {3}.", spawnPool.poolName, prefab.name, cullAbove, totalCount));
					}
				}
				else if (logMessages)
				{
					Debug.Log(string.Format("SpawnPool {0} ({1}): CULLING waiting for despawn. Checking again in {2}sec", spawnPool.poolName, prefab.name, cullDelay));
					break;
				}
			}
			yield return new WaitForSeconds(cullDelay);
		}
		if (logMessages)
		{
			Debug.Log(string.Format("SpawnPool {0} ({1}): CULLING FINISHED! Stopping", spawnPool.poolName, prefab.name));
		}
		cullingActive = false;
		yield return null;
	}

	internal Transform SpawnInstance(Vector3 pos, Quaternion rot)
	{
		Transform transform;
		if (despawned.Count == 0)
		{
			transform = SpawnNew(pos, rot);
		}
		else
		{
			transform = despawned[0];
			despawned.RemoveAt(0);
			spawned.Add(transform);
			if (transform == null)
			{
				string message = "Make sure you didn't delete a despawned instance directly.";
				throw new MissingReferenceException(message);
			}
			if (logMessages)
			{
				Debug.Log(string.Format("SpawnPool {0} ({1}): respawning '{2}'.", spawnPool.poolName, prefab.name, transform.name));
			}
			transform.position = pos;
			transform.rotation = rot;
			transform.gameObject.SetActiveRecursively(true);
		}
		if (transform != null)
		{
			transform.gameObject.BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);
		}
		return transform;
	}

	internal Transform SpawnNew(Vector3 pos, Quaternion rot)
	{
		if (limitInstances && totalCount >= limitAmount)
		{
			if (logMessages)
			{
				Debug.Log(string.Format("SpawnPool {0} ({1}): LIMIT REACHED! Not creating new instances!", spawnPool.poolName, prefab.name));
			}
			return null;
		}
		if (pos == Vector3.zero)
		{
			pos = spawnPool.group.position;
		}
		if (rot == Quaternion.identity)
		{
			rot = spawnPool.group.rotation;
		}
		Transform transform = (Transform)UnityEngine.Object.Instantiate(prefab, pos, rot);
		nameInstance(transform);
		transform.parent = spawnPool.group;
		spawned.Add(transform);
		if (logMessages)
		{
			Debug.Log(string.Format("SpawnPool {0} ({1}): Spawned new instance '{2}'.", spawnPool.poolName, prefab.name, transform.name));
		}
		return transform;
	}

	internal void AddUnpooled(Transform inst, bool despawn)
	{
		nameInstance(inst);
		if (despawn)
		{
			inst.gameObject.SetActiveRecursively(false);
			despawned.Add(inst);
		}
		else
		{
			spawned.Add(inst);
		}
	}

	internal List<Transform> PreloadInstances()
	{
		List<Transform> list = new List<Transform>();
		if (preloaded)
		{
			Debug.Log(string.Format("SpawnPool {0} ({1}): Already preloaded! You cannot preload twice. If you are running this through code, make sure it isn't also defined in the Inspector.", spawnPool.poolName, prefab.name));
			return list;
		}
		if (prefab == null)
		{
			Debug.LogError(string.Format("SpawnPool {0} ({1}): Prefab cannot be null.", spawnPool.poolName, prefab.name));
			return list;
		}
		forceLoggingSilent = true;
		while (totalCount < preloadAmount)
		{
			Transform transform = SpawnNew(Vector3.zero, Quaternion.identity);
			if (transform == null)
			{
				Debug.LogError(string.Format("SpawnPool {0} ({1}): You turned ON 'Limit Instances' and entered a 'Limit Amount' greater than the 'Preload Amount'!", spawnPool.poolName, prefab.name));
				continue;
			}
			DespawnInstance(transform);
			list.Add(transform);
		}
		forceLoggingSilent = false;
		if (cullDespawned && totalCount > cullAbove)
		{
			Debug.LogWarning(string.Format("SpawnPool {0} ({1}): You turned ON Culling and entered a 'Cull Above' threshold greater than the 'Preload Amount'! This will cause the culling feature to trigger immediatly, which is wrong conceptually. Only use culling for extreme situations. See the docs.", spawnPool.poolName, prefab.name));
		}
		return list;
	}

	private void nameInstance(Transform instance)
	{
		instance.name += (totalCount + 1).ToString("#000");
	}
}
