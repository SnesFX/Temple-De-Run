using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsDict : IDictionary<string, Transform>, ICollection<KeyValuePair<string, Transform>>, IEnumerable<KeyValuePair<string, Transform>>, IEnumerable
{
	private Dictionary<string, Transform> _prefabs = new Dictionary<string, Transform>();

	bool ICollection<KeyValuePair<string, Transform>>.IsReadOnly
	{
		get
		{
			return true;
		}
	}

	public int Count
	{
		get
		{
			return _prefabs.Count;
		}
	}

	public Transform this[string key]
	{
		get
		{
			//Discarded unreachable code: IL_002c
			try
			{
				return _prefabs[key];
			}
			catch (KeyNotFoundException)
			{
				string message = string.Format("A Prefab with the name '{0}' not found. \nPrefabs={1}", key, ToString());
				throw new KeyNotFoundException(message);
			}
		}
		set
		{
			throw new NotImplementedException("Read-only.");
		}
	}

	public ICollection<string> Keys
	{
		get
		{
			string message = "If you need this, please request it.";
			throw new NotImplementedException(message);
		}
	}

	public ICollection<Transform> Values
	{
		get
		{
			string message = "If you need this, please request it.";
			throw new NotImplementedException(message);
		}
	}

	private bool IsReadOnly
	{
		get
		{
			return true;
		}
	}

	void ICollection<KeyValuePair<string, Transform>>.CopyTo(KeyValuePair<string, Transform>[] array, int arrayIndex)
	{
		string message = "Cannot be copied";
		throw new NotImplementedException(message);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _prefabs.GetEnumerator();
	}

	public override string ToString()
	{
		string[] array = new string[_prefabs.Count];
		_prefabs.Keys.CopyTo(array, 0);
		return string.Format("[{0}]", string.Join(", ", array));
	}

	internal void _Add(string prefabName, Transform prefab)
	{
		_prefabs.Add(prefabName, prefab);
	}

	internal bool _Remove(string prefabName)
	{
		return _prefabs.Remove(prefabName);
	}

	internal void _Clear()
	{
		_prefabs.Clear();
	}

	public bool ContainsKey(string prefabName)
	{
		return _prefabs.ContainsKey(prefabName);
	}

	public bool TryGetValue(string prefabName, out Transform prefab)
	{
		return _prefabs.TryGetValue(prefabName, out prefab);
	}

	public void Add(string key, Transform value)
	{
		throw new NotImplementedException("Read-Only");
	}

	public bool Remove(string prefabName)
	{
		throw new NotImplementedException("Read-Only");
	}

	public bool Contains(KeyValuePair<string, Transform> item)
	{
		string message = "Use Contains(string prefabName) instead.";
		throw new NotImplementedException(message);
	}

	public void Add(KeyValuePair<string, Transform> item)
	{
		throw new NotImplementedException("Read-only");
	}

	public void Clear()
	{
		throw new NotImplementedException();
	}

	private void CopyTo(KeyValuePair<string, Transform>[] array, int arrayIndex)
	{
		string message = "Cannot be copied";
		throw new NotImplementedException(message);
	}

	public bool Remove(KeyValuePair<string, Transform> item)
	{
		throw new NotImplementedException("Read-only");
	}

	public IEnumerator<KeyValuePair<string, Transform>> GetEnumerator()
	{
		return _prefabs.GetEnumerator();
	}
}
