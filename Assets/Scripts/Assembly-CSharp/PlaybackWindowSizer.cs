using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlaybackWindowSizer : MonoBehaviour
{
	private static KeyValuePair<int, int>[] resolutions = new KeyValuePair<int, int>[12]
	{
		new KeyValuePair<int, int>(320, 240),
		new KeyValuePair<int, int>(400, 240),
		new KeyValuePair<int, int>(800, 480),
		new KeyValuePair<int, int>(854, 480),
		new KeyValuePair<int, int>(960, 540),
		new KeyValuePair<int, int>(1024, 600),
		new KeyValuePair<int, int>(800, 600),
		new KeyValuePair<int, int>(1200, 800),
		new KeyValuePair<int, int>(1200, 800),
		new KeyValuePair<int, int>(1024, 768),
		new KeyValuePair<int, int>(960, 640),
		new KeyValuePair<int, int>(640, 960)
	};

	private void OnGUI()
	{
		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
		{
			GUIStyle gUIStyle = new GUIStyle();
			gUIStyle.alignment = TextAnchor.LowerRight;
			KeyValuePair<int, int>[] array = resolutions;
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<int, int> keyValuePair = array[i];
				GUILayout.BeginArea(new Rect(keyValuePair.Key - 55, keyValuePair.Value - 15, 50f, 15f));
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label(keyValuePair.Key + "x" + keyValuePair.Value, gUIStyle, GUILayout.ExpandHeight(true));
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
				GUI.Box(new Rect(keyValuePair.Key - 7, keyValuePair.Value - 7, 5f, 5f), string.Empty);
			}
			GUI.Label(new Rect(0f, 0f, 50f, 20f), Screen.width + "x" + Screen.height);
		}
	}
}
