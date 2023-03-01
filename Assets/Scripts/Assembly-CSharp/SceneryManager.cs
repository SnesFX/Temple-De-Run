using UnityEngine;

public class SceneryManager : MonoBehaviour
{
	public enum SceneryType
	{
		kSceneryTypeUnknown = 0,
		kSceneryTypeTree = 1,
		kSceneryTypeRock = 2,
		kSceneryTypeCliff = 3,
		kSceneryTypeGroundPatch = 4,
		kSceneryTypeTempleHead = 5
	}

	public const int kSceneryGridCountX = 19;

	public const int kSceneryGridCountY = 19;

	public DynamicElement DynamicElementPrefab;

	public int LastGridX;

	public int LastGridY;

	public float ObjectDensity;

	public SceneryElement[] SceneryElements;

	public static SceneryManager Instance;

	private int gridX;

	private int gridY;

	private int minGridX;

	private int minGridY;

	private int maxGridX;

	private int maxGridY;

	private bool[,] GridMask = new bool[19, 19];

	private void Start()
	{
		Instance = this;
		LastGridX = -99999;
		LastGridY = -99999;
		ObjectDensity = 0.25f;
		SceneryElements = new SceneryElement[361];
		for (int i = 0; i < 361; i++)
		{
			SceneryElement sceneryElement = null;
			int num = Random.Range(0, 21);
			if (num <= 10)
			{
				string[] array = new string[4] { "Trees/treeAlone", "Trees/treeAlone2", "Trees/treeAlone3", "Trees/treeAlone4" };
				sceneryElement = SceneryElement.Instantiate(array[Random.Range(0, 4)]);
				sceneryElement.Type = SceneryType.kSceneryTypeTree;
			}
			else if (num <= 14)
			{
				string[] array2 = new string[3] { "Trees/rock", "Trees/rock2", "Trees/rock3" };
				sceneryElement = SceneryElement.Instantiate(array2[Random.Range(0, 3)]);
				sceneryElement.Type = SceneryType.kSceneryTypeRock;
			}
			else if (num <= 16)
			{
				string[] array3 = new string[3] { "Trees/cliff", "Trees/cliff2", "Trees/cliff3" };
				sceneryElement = SceneryElement.Instantiate(array3[Random.Range(0, 3)]);
				sceneryElement.Type = SceneryType.kSceneryTypeCliff;
			}
			else if (num <= 18)
			{
				sceneryElement = SceneryElement.Instantiate("Trees/overgrowthTrees");
				sceneryElement.Type = SceneryType.kSceneryTypeTree;
			}
			else
			{
				switch (num)
				{
				case 19:
					sceneryElement = SceneryElement.Instantiate("Trees/templeHead");
					sceneryElement.Type = SceneryType.kSceneryTypeTempleHead;
					break;
				case 20:
					sceneryElement = SceneryElement.Instantiate("Trees/groundPatchTemple");
					sceneryElement.Type = SceneryType.kSceneryTypeGroundPatch;
					break;
				}
			}
			if (sceneryElement == null)
			{
				Debug.LogError("Could not create scenery element: " + i, this);
			}
			sceneryElement.name = "Scenery Object " + i;
			SceneryElements[i] = sceneryElement;
		}
	}

	private void ClearMask()
	{
		for (int i = 0; i < 19; i++)
		{
			for (int j = 0; j < 19; j++)
			{
				GridMask[i, j] = false;
			}
		}
	}

	private void SetMaskForPoint(Vector3 point)
	{
		int num = (int)(point.x / 60f);
		int num2 = (int)(point.z / 60f);
		num -= minGridX / 60;
		num2 -= minGridY / 60;
		if (num >= 0 && num2 >= 0 && num < 19 && num2 < 19)
		{
			GridMask[num, num2] = true;
		}
	}

	private bool MaskOccupied(Vector2 gridPoint)
	{
		int num = (int)(gridPoint.x / 60f);
		int num2 = (int)(gridPoint.y / 60f);
		num -= minGridX / 60;
		num2 -= minGridY / 60;
		if (num >= 0 && num2 >= 0 && num < 19 && num2 < 19)
		{
			return GridMask[num, num2];
		}
		return false;
	}

	private bool MaskOccupiedTest(Vector2 gridPoint, GameObject go)
	{
		int num = (int)(gridPoint.x / 60f);
		int num2 = (int)(gridPoint.y / 60f);
		Debug.Log(string.Concat("gridPoint: ", gridPoint, "  gX: ", num, "  gY: ", num2), go);
		num -= minGridX / 60;
		num2 -= minGridY / 60;
		Debug.Log("Offset:  gX: " + num + "  gY: " + num2, go);
		if (num >= 0 && num2 >= 0 && num < 19 && num2 < 19)
		{
			return GridMask[num, num2];
		}
		return false;
	}

	private void FillMask(PathElement element, PathElement.PathDirection origin, bool isRootNode)
	{
		SetMaskForPoint(element.transform.position);
		if ((origin != PathElement.PathDirection.kPathDirectionNorth || isRootNode) && element.PathNorth != null)
		{
			FillMask(element.PathNorth, PathElement.PathDirection.kPathDirectionSouth, false);
		}
		if ((origin != PathElement.PathDirection.kPathDirectionEast || isRootNode) && element.PathEast != null)
		{
			FillMask(element.PathEast, PathElement.PathDirection.kPathDirectionWest, false);
		}
		if ((origin != PathElement.PathDirection.kPathDirectionSouth || isRootNode) && element.PathSouth != null)
		{
			FillMask(element.PathSouth, PathElement.PathDirection.kPathDirectionNorth, false);
		}
		if ((origin != PathElement.PathDirection.kPathDirectionWest || isRootNode) && element.PathWest != null)
		{
			FillMask(element.PathWest, PathElement.PathDirection.kPathDirectionEast, false);
		}
	}

	private void DumpMask()
	{
		string text = string.Empty;
		for (int i = 0; i < 19; i++)
		{
			for (int j = 0; j < 19; j++)
			{
				text += ((!GridMask[i, j]) ? ".  " : "# ");
			}
			text += "\n";
		}
		Debug.Log("SCENERY MASK:\n" + text);
	}

	public void Reset()
	{
		Debug.Log("Scenery Reset: " + GameController.SharedInstance.PathRoot);
		Simulate(GameController.SharedInstance.PathRoot, true);
	}

	public void Simulate(PathElement pathRoot, bool checkPathOverlap)
	{
		gridX = (int)(Camera.main.transform.position.x / 60f) * 60;
		gridY = (int)(Camera.main.transform.position.z / 60f) * 60;
		if (gridX == LastGridX && gridY == LastGridY && !checkPathOverlap)
		{
			return;
		}
		LastGridX = gridX;
		LastGridY = gridY;
		minGridX = gridX - 540;
		minGridY = gridY - 540;
		maxGridX = gridX + 540;
		maxGridY = gridY + 540;
		ClearMask();
		FillMask(pathRoot, PathElement.PathDirection.kPathDirectionNone, true);
		for (int i = 0; i < 361; i++)
		{
			SceneryElement sceneryElement = SceneryElements[i];
			if (sceneryElement == null)
			{
				Debug.LogError("Null scenery element: " + i, this);
			}
			if (sceneryElement.Active)
			{
				if (sceneryElement.Quadrant.x < (float)minGridX || sceneryElement.Quadrant.x > (float)maxGridX || sceneryElement.Quadrant.y < (float)minGridY || sceneryElement.Quadrant.y > (float)maxGridY)
				{
					sceneryElement.Active = false;
					sceneryElement.Visible = false;
				}
				else if (MaskOccupied(sceneryElement.Quadrant))
				{
					sceneryElement.Active = false;
					sceneryElement.Visible = false;
				}
			}
		}
		for (int j = 0; j < 19; j++)
		{
			for (int k = 0; k < 19; k++)
			{
				float num = minGridX + j * 60;
				float num2 = minGridY + k * 60;
				bool flag = false;
				for (int l = 0; l < 361; l++)
				{
					SceneryElement sceneryElement2 = SceneryElements[l];
					if (sceneryElement2.Active && sceneryElement2.Quadrant.x == num && sceneryElement2.Quadrant.y == num2)
					{
						flag = true;
						break;
					}
				}
				if (flag || MaskOccupied(new Vector2(num, num2)) || (!(num < -60f) && !(num > 60f) && !(num2 < 0f) && !(num2 > 240f)))
				{
					continue;
				}
				for (int m = 0; m < 361; m++)
				{
					SceneryElement sceneryElement3 = SceneryElements[m];
					if (!sceneryElement3.Active)
					{
						sceneryElement3.Active = true;
						sceneryElement3.Quadrant = new Vector2(num, num2);
						sceneryElement3.transform.position = new Vector3(num + (float)(Random.Range(0, 21) - 10), 0f, num2 + (float)(Random.Range(0, 21) - 10));
						sceneryElement3.Visible = (float)Random.Range(0, 1000) / 1000f < ObjectDensity;
						break;
					}
				}
			}
		}
	}

	public SceneryElement GetSceneryElementIntersectingSphere(Vector3 point, float radius)
	{
		radius *= radius;
		for (int i = 0; i < 361; i++)
		{
			SceneryElement sceneryElement = SceneryElements[i];
			if (sceneryElement.DebugMaker)
			{
				Debug.Log("se: " + sceneryElement.name + "  active: " + sceneryElement.Active + "  Visible: " + sceneryElement.Visible);
			}
			if (sceneryElement.Active && sceneryElement.Visible)
			{
				float num = sceneryElement.transform.GetComponentInChildren<MeshRenderer>().bounds.SqrDistance(point);
				if (sceneryElement.DebugMaker)
				{
					Debug.Log("dist^2 = " + num);
				}
				if (num <= radius)
				{
					Debug.Log("Hit: " + sceneryElement.name, sceneryElement);
					return sceneryElement;
				}
			}
		}
		return null;
	}
}
