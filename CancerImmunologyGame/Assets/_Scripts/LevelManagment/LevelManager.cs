using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.SaveSystem;

namespace ImmunotherapyGame.LevelManagement
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField]
        private LevelDataList levelData = null;
		private SerializableLevelDataList savedLevelData = null;

        private List<LevelData> Levels => levelData.levels;
		private List<LevelData> SavedLevels => savedLevelData.Levels;

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
				SaveManager.Instance.ClearSaveData<SerializableLevelDataList>();
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

		private void Start()
		{
			Initialise();
		}

		public void Initialise()
		{
			LoadLevelData();

			// Initilise the Level Manager UI
		}

        public void OnLevelComplete(int sceneIndex)
		{
			for (int i = 0; i < Levels.Count; i++)
			{
				if (Levels[i].sceneIndex == sceneIndex)
				{
					Debug.Log("Levels [" + i + "] has scene index = " + sceneIndex);
					Levels[i].isCompleted = true;
					LevelManagerUI.Instance.UpdateLevelItem(i);

					// If next level unlock it
					if (i < Levels.Count -1)
					{
						Levels[i + 1].isLocked = false;
						LevelManagerUI.Instance.UpdateLevelItem(i + 1);
					}
				}
			}
			SaveLevelData();
		}

        private void SaveLevelData()
		{
			// If success make current SO have that data
			savedLevelData = new SerializableLevelDataList(Levels);
			SaveManager.Instance.SaveData<SerializableLevelDataList>(savedLevelData);
		}

        private void LoadLevelData()
		{
			// Try to load levels
			savedLevelData = SaveManager.Instance.LoadData<SerializableLevelDataList>();
			Debug.Log("HELLO");

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
				int savedLevelsCount = SavedLevels.Count;
				for (int i = 0; i < savedLevelsCount; ++i)
				{
					Levels[i].isCompleted = SavedLevels[i].isCompleted;
					Levels[i].isLocked = SavedLevels[i].isLocked;
					Levels[i].sceneIndex = SavedLevels[i].sceneIndex;
					Levels[i].levelIndex = SavedLevels[i].levelIndex;
				}

				if (savedLevelsCount < Levels.Count)
				{
					Debug.Log("New levels uploaded. Checking to unlock first new level.");
					if (SavedLevels[savedLevelsCount - 1].isCompleted)
					{
						Debug.Log("Unlocking first new level.");
						Levels[savedLevelsCount].isLocked = false;
					}
				}
			}
			SaveLevelData();
			LevelManagerUI.Instance.Initialise(levelData);
		}

		public void ResetLevelData()
		{
			levelData.ResetLevelData();
			SaveLevelData();
		}

	}
}
