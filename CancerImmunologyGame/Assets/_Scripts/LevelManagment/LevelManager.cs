using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;
using ImmunotherapyGame.SaveSystem;
using ImmunotherapyGame.ImmunotherapyResearchSystem;
using ImmunotherapyGame.LevelTasks;
using ImmunotherapyGame.GameManagement;

namespace ImmunotherapyGame.LevelManagement
{
    public class LevelManager : Singleton<LevelManager>, IDataManager
    {
        [SerializeField] private LevelData data = null;
		private SerializableLevelData savedData = null;

		[SerializeField] [ReadOnly] private int currentCompletedLevelIndex = -1;
		[SerializeField] [ReadOnly] private string currentCompletedLevelName = "";

        public void OnLevelComplete()
		{
			FindCompletedLevelIndex();

			if (currentCompletedLevelIndex == -1)
			{
				Debug.LogError("Missing level in level data with name: " + currentCompletedLevelName);
				return;
			}

			data.levels[currentCompletedLevelIndex].isCompleted = true;

			// Interact with level complete screen to assign points
			int pointsEarned = LevelCompleteScreen.Instance.PopulateAndGetEarnedPoints(data.levels[currentCompletedLevelIndex]);
			LevelTaskSystem.Instance.Clear();
			ImmunotherapyResearch.Instance.AddPoints(pointsEarned);

			// Unlock next level
			bool HasNextLevel = currentCompletedLevelIndex < (data.levels.Count - 1);
			if (HasNextLevel)
			{
				data.levels[currentCompletedLevelIndex + 1].isLocked = false;
			}
				
			// Save Data
			GameManager.Instance.SaveData();

			LevelCompleteScreen.Instance.Open(HasNextLevel);
		}

		private void FindCompletedLevelIndex()
		{
			currentCompletedLevelIndex = -1;
			currentCompletedLevelName = SceneManager.GetActiveScene().name;

			// Unlock next level;
			for (int levelIndex = 0; levelIndex < data.levels.Count; levelIndex++)
			{
				if (data.levels[levelIndex].sceneName == currentCompletedLevelName)
				{
					currentCompletedLevelIndex = levelIndex;
					break;
				}
			}
		}

		public void LoadData()
		{
			// Try to load levels
			savedData = SaveManager.Instance.LoadData<SerializableLevelData>();

			if (savedData == null)
			{
				Debug.Log("No previous saved data found. Creating new level data save.");
				ResetData();
				SaveData();
			}
			else
			{
				savedData.CopyTo(data);
			}
		}

		public void SaveData()
		{
			// If success make current SO have that data
			savedData = new SerializableLevelData(data);
			SaveManager.Instance.SaveData<SerializableLevelData>(savedData);
		}

		public void ResetData()
		{
			data.ResetData();
		}

		public void RestartLevel()
		{
			OnLevelExit();
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		internal void LoadMainMenu()
		{
			OnLevelExit();
			SceneManager.LoadScene("MainMenu");
		}

		internal void LoadNextLevel()
		{
			OnLevelExit();
			SceneManager.LoadScene(data.levels[currentCompletedLevelIndex + 1].sceneName);
		}

		internal void OnLevelExit()
		{
			GameManager.Instance.ReloadData();
			LevelTaskSystem.Instance.Clear();
		}
	}
}
