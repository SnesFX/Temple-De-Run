using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Path-o-logical/PoolManager/SpawnPool")]
public sealed class SpawnPool : MonoBehaviour, IList<Transform>, ICollection<Transform>, IEnumerable<Transform>, IEnumerable
{
	public string poolName = string.Empty;

	public bool dontDestroyOnLoad;

	public bool logMessages;

	public List<PrefabPool> _perPrefabPoolOptions = new List<PrefabPool>();

	public Dictionary<object, bool> prefabsFoldOutStates = new Dictionary<object, bool>();

	[HideInInspector]
	public float maxParticleDespawnTime = 60f;

	public PrefabsDict prefabs = new PrefabsDict();

	public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();

	private List<PrefabPool> _prefabPools = new List<PrefabPool>();

	private List<Transform> _spawned = new List<Transform>();

	public Transform group { get; private set; }

	public Transform this[int index]
	{
		get
		{
			return _spawned[index];
		}
		set
		{
			throw new NotImplementedException("Read-only.");
		}
	}

	public int Count
	{
		get
		{
			return _spawned.Count;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		foreach (Transform item in _spawned)
		{
			yield return item;
		}
	}

	bool ICollection<Transform>.Remove(Transform item)
	{
		throw new NotImplementedException();
	}

	private void Awake()
	{
		if (dontDestroyOnLoad)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		group = base.transform;
		if (poolName == string.Empty)
		{
			poolName = group.name.Replace("Pool", string.Empty);
			poolName = poolName.Replace("(Clone)", string.Empty);
		}
		if (logMessages)
		{
			Debug.Log(string.Format("SpawnPool {0}: Initializing..", poolName));
		}
		foreach (PrefabPool perPrefabPoolOption in _perPrefabPoolOptions)
		{
			if (perPrefabPoolOption.prefab == null)
			{
				Debug.LogWarning(string.Format("Initialization Warning: Pool '{0}' contains a PrefabPool with no prefab reference. Skipping.", poolName));
				continue;
			}
			perPrefabPoolOption.inspectorInstanceConstructor();
			CreatePrefabPool(perPrefabPoolOption);
		}
		PoolManager.Pools.Add(this);
	}

	private void OnDestroy()
	{
		if (logMessages)
		{
			Debug.Log(string.Format("SpawnPool {0}: Destroying...", poolName));
		}
		PoolManager.Pools.Remove(this);
		StopAllCoroutines();
		_spawned.Clear();
		foreach (PrefabPool prefabPool in _prefabPools)
		{
			prefabPool.SelfDestruct();
		}
		_prefabPools.Clear();
		prefabs._Clear();
	}

	public List<Transform> CreatePrefabPool(PrefabPool prefabPool)
	{
		if (GetPrefab(prefabPool.prefab) == null || 1 == 0)
		{
			prefabPool.spawnPool = this;
			_prefabPools.Add(prefabPool);
			prefabs._Add(prefabPool.prefab.name, prefabPool.prefab);
		}
		List<Transform> list = new List<Transform>();
		if (!prefabPool.preloaded)
		{
			if (logMessages)
			{
				Debug.Log(string.Format("SpawnPool {0}: Preloading {1} {2}", poolName, prefabPool.preloadAmount, prefabPool.prefab.name));
			}
			list.AddRange(prefabPool.PreloadInstances());
		}
		return list;
	}

	public void Add(Transform instance, string prefabName, bool despawn, bool parent)
	{
		foreach (PrefabPool prefabPool in _prefabPools)
		{
			if (prefabPool.prefabGO == null)
			{
				Debug.LogError("Unexpected Error: PrefabPool.prefabGO is null");
				return;
			}
			if (prefabPool.prefabGO.name == prefabName)
			{
				prefabPool.AddUnpooled(instance, despawn);
				if (logMessages)
				{
					Debug.Log(string.Format("SpawnPool {0}: Adding previously unpooled instance {1}", poolName, instance.name));
				}
				if (parent)
				{
					instance.parent = group;
				}
				if (!despawn)
				{
					_spawned.Add(instance);
				}
				return;
			}
		}
		Debug.LogError(string.Format("SpawnPool {0}: PrefabPool {1} not found.", poolName, prefabName));
	}

	public void Add(Transform item)
	{
		string message = "Use SpawnPool.Spawn() to properly add items to the pool.";
		throw new NotImplementedException(message);
	}

	public void Remove(Transform item)
	{
		string message = "Use Despawn() to properly manage items that should remain in the pool but be deactivated.";
		throw new NotImplementedException(message);
	}

	public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot)
	{
		Transform transform;
		foreach (PrefabPool prefabPool2 in _prefabPools)
		{
			if (prefabPool2.prefabGO == prefab.gameObject)
			{
				transform = prefabPool2.SpawnInstance(pos, rot);
				if (transform == null)
				{
					return null;
				}
				if (transform.parent != group)
				{
					transform.parent = group;
				}
				_spawned.Add(transform);
				return transform;
			}
		}
		PrefabPool prefabPool = new PrefabPool(prefab);
		CreatePrefabPool(prefabPool);
		transform = prefabPool.SpawnInstance(pos, rot);
		transform.parent = group;
		_spawned.Add(transform);
		return transform;
	}

	public Transform Spawn(Transform prefab)
	{
		return Spawn(prefab, Vector3.zero, Quaternion.identity);
	}

	public ParticleEmitter Spawn(ParticleEmitter prefab, Vector3 pos, Quaternion quat)
	{
		Transform transform = Spawn(prefab.transform, pos, quat);
		if (transform == null)
		{
			return null;
		}
		ParticleAnimator component = transform.GetComponent<ParticleAnimator>();
		if (component != null)
		{
			component.autodestruct = false;
		}
		ParticleEmitter component2 = transform.GetComponent<ParticleEmitter>();
		component2.emit = true;
		StartCoroutine(ListenForEmitDespawn(component2));
		return component2;
	}

	public ParticleEmitter Spawn(ParticleEmitter prefab, Vector3 pos, Quaternion quat, string colorPropertyName, Color color)
	{
		Transform transform = Spawn(prefab.transform, pos, quat);
		if (transform == null)
		{
			return null;
		}
		ParticleAnimator component = transform.GetComponent<ParticleAnimator>();
		if (component != null)
		{
			component.autodestruct = false;
		}
		ParticleEmitter component2 = transform.GetComponent<ParticleEmitter>();
		component2.GetComponent<Renderer>().material.SetColor(colorPropertyName, color);
		component2.emit = true;
		StartCoroutine(ListenForEmitDespawn(component2));
		return component2;
	}

	public void Despawn(Transform xform)
	{
		bool flag = false;
		foreach (PrefabPool prefabPool in _prefabPools)
		{
			if (prefabPool.spawned.Contains(xform))
			{
				flag = prefabPool.DespawnInstance(xform);
				break;
			}
			if (prefabPool.despawned.Contains(xform))
			{
				Debug.LogError(string.Format("SpawnPool {0}: {1} has already been despawned. You cannot despawn something more than once!", poolName, xform.name));
				return;
			}
		}
		if (!flag)
		{
			Debug.LogError(string.Format("SpawnPool {0}: {1} not found in SpawnPool", poolName, xform.name));
		}
		else
		{
			_spawned.Remove(xform);
		}
	}

	public void Despawn(Transform instance, float seconds)
	{
		StartCoroutine(DoDespawnAfterSeconds(instance, seconds));
	}

	private IEnumerator DoDespawnAfterSeconds(Transform instance, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Despawn(instance);
	}

	public void DespawnAll()
	{
		List<Transform> list = new List<Transform>(_spawned);
		foreach (Transform item in list)
		{
			Despawn(item);
		}
	}

	public bool IsSpawned(Transform instance)
	{
		return _spawned.Contains(instance);
	}

	public Transform GetPrefab(Transform prefab)
	{
		foreach (PrefabPool prefabPool in _prefabPools)
		{
			if (prefabPool.prefabGO == null)
			{
				Debug.LogError(string.Format("SpawnPool {0}: PrefabPool.prefabGO is null", poolName));
			}
			if (prefabPool.prefabGO == prefab.gameObject)
			{
				return prefabPool.prefab;
			}
		}
		return null;
	}

	public GameObject GetPrefab(GameObject prefab)
	{
		foreach (PrefabPool prefabPool in _prefabPools)
		{
			if (prefabPool.prefabGO == null)
			{
				Debug.LogError(string.Format("SpawnPool {0}: PrefabPool.prefabGO is null", poolName));
			}
			if (prefabPool.prefabGO == prefab)
			{
				return prefabPool.prefabGO;
			}
		}
		return null;
	}

	private IEnumerator ListenForEmitDespawn(ParticleEmitter emitter)
	{
		yield return null;
		yield return new WaitForEndOfFrame();
		float safetimer = 0f;
		while (emitter.particleCount > 0)
		{
			safetimer += Time.smoothDeltaTime;
			if (safetimer > maxParticleDespawnTime)
			{
				Debug.LogWarning(string.Format("SpawnPool {0}: Timed out while listening for all particles to die. Waited for {1}sec.", poolName, maxParticleDespawnTime));
			}
			yield return null;
		}
		emitter.emit = false;
		Despawn(emitter.transform);
		yield return null;
	}

	public bool Contains(Transform item)
	{
		string message = "Use IsSpawned(Transform instance) instead.";
		throw new NotImplementedException(message);
	}

	public void CopyTo(Transform[] array, int arrayIndex)
	{
		_spawned.CopyTo(array, arrayIndex);
	}

	public IEnumerator<Transform> GetEnumerator()
	{
		foreach (Transform item in _spawned)
		{
			yield return item;
		}
	}

	public int IndexOf(Transform item)
	{
		throw new NotImplementedException();
	}

	public void Insert(int index, Transform item)
	{
		throw new NotImplementedException();
	}

	public void RemoveAt(int index)
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		throw new NotImplementedException();
	}
}
