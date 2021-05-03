using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;
using ImmunotherapyGame.SaveSystem;

namespace ImmunotherapyGame.LevelManagement
{
    public class LevelManager : Singleton<LevelManager>, IDataManager
    {
        [SerializeField]
        private LevelData levelData = null;
		private SerializableLevelData savedLevelData = null;

        private List<LevelDataObject> Levels => levelData.levels;
		private List<LevelDataObject> SavedLevels => savedLevelData.levels;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				SaveLevelData();
			}

			if (Input.GetKeyDown(KeyCode.L))
			{
				LoadLevelData();
			}

			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				SaveManager.Instance.ClearSaveData<SerializableLevelData>();
			}

			// TODO: remove from run
			for (int i = 1; i < 10; ++i)
			{
				if (Input.GetKeyDown((KeyCode)(48 + i)))
				{
					OnLevelComplete(i + 1); // Hardcoded cuz level = scenebuildIndex + 1 (code is active only atm)
				}
			}
		}

        public void OnLevelComplete(int sceneIndex)
		{
			for (int i = 0; i < Levels.Count; i++)
			{
				if (Levels[i].sceneIndex == sceneIndex)
				{
					Levels[i].isCompleted = true;

					// If next level unlock it
					if (i < Levels.Count - 1)
					{
						Levels[i + 1].isLocked = false;
					}
				}
			}
			SaveLevelData();
		}

        private void SaveLevelData()
		{
			// If success make current SO have that data
			savedLevelData = new SerializableLevelData(levelData);
			SaveManager.Instance.SaveData<SerializableLevelData>(savedLevelData);
		}

        private void LoadLevelData()
		{
			// Try to load levels
			savedLevelData = SaveManager.Instance.LoadData<SerializableLevelData>();

			if (savedLevelData == null)
			{
				Debug.Log("No previous saved data found. Creating new level data save.");
			}
			else if (SavedLevels.Count > levelData.levels.Count)
			{
				Debug.LogWarning("Saved level data is larger than current level data! Creating new level data save.");
			}
			else
			{
				savedLevelData.CopyTo(levelData);
			}
			SaveLevelData();
		}

		public void ResetLevelData()
		{
			levelData.ResetLevelData();
			PlayerPrefs.DeleteKey("GameInProgress");
		}

		public void LoadData()
		{
			LoadLevelData();
		}

		public void SaveData()
		{
			SaveLevelData();
		}

		public void ResetData()
		{
			ResetLevelData();
		}
	}
}
