using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Strings : MonoBehaviour
{
	public TextAsset StringsFile;

	public Dictionary<string, string> Store;

	public static Strings sharedInstance;

	private void Awake()
	{
		sharedInstance = this;
		Parse();
	}

	public static string Txt(string key)
	{
		if (sharedInstance == null)
		{
			return "STRINGS NOT INITILIZED";
		}
		if (!sharedInstance.Store.ContainsKey(key))
		{
			Debug.LogError("Could not find string for key: [" + key + "]");
		}
		return sharedInstance.Store[key];
	}

	private void Parse()
	{
		if (StringsFile == null)
		{
			Debug.LogError("Missing strings file");
			return;
		}
		Store = new Dictionary<string, string>();
		string[] array = StringsFile.text.Split('\n');
		string[] array2 = array;
		foreach (string text in array2)
		{
			string[] array3 = text.Split('=');
			if (array3.Length == 2)
			{
				string text2 = array3[0].Trim();
				string text3 = array3[1].Trim();
				text2 = text2.Replace("\"", string.Empty);
				text3 = text3.Replace("\"", string.Empty);
				text3 = text3.Replace(";", string.Empty);
				Store.Add(text2, text3);
			}
		}
	}
}
