using UnityEngine;

public class SceneryElement : MonoBehaviour
{
	public Vector2 Quadrant = new Vector2(-9999f, -9999f);

	public SceneryManager.SceneryType Type;

	public bool Active;

	public bool DebugMaker;

	public bool Visible
	{
		get
		{
			return base.gameObject.active;
		}
		set
		{
			if (value)
			{
				Activate();
			}
			else
			{
				Deactivate();
			}
		}
	}

	private void Awake()
	{
		Visible = false;
	}

	private void Activate()
	{
		base.gameObject.SetActiveRecursively(true);
	}

	private void Deactivate()
	{
		base.gameObject.SetActiveRecursively(false);
	}

	public static SceneryElement Instantiate(string prefabName)
	{
		GameObject gameObject = (GameObject)Resources.Load("Prefabs/Temple/" + prefabName + "_prefab", typeof(GameObject));
		if (gameObject == null)
		{
			Debug.LogError("Could not load prefab: " + prefabName);
			return null;
		}
		GameObject gameObject2 = (GameObject)Object.Instantiate(gameObject);
		gameObject2.transform.position = new Vector3(0f, 0f, 0f);
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
		return gameObject2.GetComponent<SceneryElement>();
	}
}
