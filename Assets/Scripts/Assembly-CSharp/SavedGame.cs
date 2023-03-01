using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class SavedGame
{
	public int Version;

	public SavedGameController Controller;

	public SavedCharacterPlayer Player;

	public SavedGameCamera Camera;

	public SavedPathElementStatics PEStatics;

	public SavedGame()
	{
	}

	public SavedGame(int version)
	{
		Version = version;
		PEStatics = new SavedPathElementStatics(Version);
		Controller = new SavedGameController(GameController.SharedInstance);
		Player = new SavedCharacterPlayer(CharacterPlayer.Instance);
		Camera = new SavedGameCamera(GameCamera.SharedInstance);
	}

	public void Apply()
	{
		Debug.Log("Applying Saved Game, Version: " + Version);
		PEStatics.Apply();
		int sCount = PathElement.sCount;
		Controller.Apply(GameController.SharedInstance);
		PathElement.sCount = sCount;
		Player.Apply(CharacterPlayer.Instance);
		Camera.Apply(GameCamera.SharedInstance);
	}

	private static string SavedFilePath()
	{
		return Path.Combine(Application.persistentDataPath, "SavedGame.dat");
	}

	public static void SaveGame()
	{
		if (!GameController.SharedInstance.IsGameStarted || GameController.SharedInstance.IsGameOver)
		{
			return;
		}
		using (MemoryStream memoryStream = new MemoryStream())
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(SavedGame));
			xmlSerializer.Serialize(memoryStream, new SavedGame(1));
			Warble warble = new Warble(22343);
			byte[] data = memoryStream.GetBuffer();
			byte[] array = warble.BlabidyJibber(ref data);
			using (FileStream fileStream = File.Create(SavedFilePath()))
			{
				fileStream.Write(array, 0, array.Length);
				fileStream.Close();
			}
		}
	}

	public static void DeleteSavedGame()
	{
		File.Delete(SavedFilePath());
	}

	private static bool DeseralizeRecord(byte[] originalData)
	{
		//Discarded unreachable code: IL_0048
		using (MemoryStream stream = new MemoryStream(originalData))
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(SavedGame));
			try
			{
				SavedGame savedGame = (SavedGame)xmlSerializer.Deserialize(stream);
				savedGame.Apply();
			}
			catch (Exception ex)
			{
				Debug.Log("Decode Exception: " + ex);
				return false;
			}
		}
		return true;
	}

	public bool SaveExists()
	{
		return File.Exists(SavedFilePath());
	}

	public static bool LoadGame()
	{
		if (!File.Exists(SavedFilePath()))
		{
			Debug.Log("No saved game found!");
			return false;
		}
		using (FileStream fileStream = new FileStream(SavedFilePath(), FileMode.Open, FileAccess.Read))
		{
			byte[] data = new byte[fileStream.Length];
			int num = (int)fileStream.Length;
			int num2 = 0;
			while (num > 0)
			{
				int num3 = fileStream.Read(data, num2, num);
				if (num3 == 0)
				{
					break;
				}
				num2 += num3;
				num -= num3;
			}
			fileStream.Close();
			DeleteSavedGame();
			Warble warble = new Warble(22343);
			byte[] array = warble.BlopJabber(ref data);
			if (array == null)
			{
				Debug.Log("DECRYPT FAIL");
				return false;
			}
			if (!DeseralizeRecord(array))
			{
				return false;
			}
		}
		SceneryManager.Instance.Simulate(GameController.SharedInstance.PathRoot, true);
		GameController.SharedInstance.SetupEnemy();
		GameController.SharedInstance.SimulateEnemies();
		MainMenu.Instance.HideAll();
		GameGUI.Instance.ShowAll();
		PauseMenu.Instance.Show();
		return true;
	}
}
