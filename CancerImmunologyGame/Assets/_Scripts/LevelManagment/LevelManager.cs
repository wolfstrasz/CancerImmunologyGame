using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;
using ImmunotherapyGame.SaveSystem;
using ImmunotherapyGame.ImmunotherapyResearchSystem;
using ImmunotherapyGame.LevelTasks;

namespace ImmunotherapyGame.LevelManagement
{
    public class LevelManager : Singleton<LevelManager>, IDataManager
    {
        [SerializeField] private LevelData data = null;
		private SerializableLevelData savedData = null;

		private int CurrentCompletedLevelID = -1;

        public void OnLevelComplete()
		{
			UpdateCompletedLevelID();
			Debug.Log("Completed Level ID: " + CurrentCompletedLevelID);
			data.levels[CurrentCompletedLevelID].isCompleted = true;

			if (CurrentCompletedLevelID >= data.levels.Count)
			{
				Debug.LogError("Completed Level name does not match any Level ID in Level Data");
				return;
			}

			bool HasNextLevel = CurrentCompletedLevelID < (data.levels.Count - 1);
			Debug.Log("Has next level: (should unlock)" + HasNextLevel);

			// Unlock levels
			if (HasNextLevel)
			{
				data.levels[CurrentCompletedLevelID + 1].isLocked = false;
			}

			// Interact with level complete screen to assign points to the RAS system
			int pointsEarned = LevelCompleteScreen.Instance.PopulateAndGetEarnedPoints(data.levels[CurrentCompletedLevelID]);
			Debug.Log("Points earned: " + pointsEarned);

			ImmunotherapyResearch.Instance.AddPoints(pointsEarned);
			// Save all data from level
			for (int i = 0; i < GlobalGameData.dataManagers.Count; ++i)
			{
				GlobalGameData.dataManagers[i].SaveData();
			}

			// Clear tasks
			LevelTaskSystem.Instance.Clear();

			LevelCompleteScreen.Instance.Open(HasNextLevel);
		}

		private void UpdateCompletedLevelID()
		{
			string completedSceneName = SceneManager.GetActiveScene().name;
			Debug.Log("Completed Scene Name: " + completedSceneName);
			// Unlock next level;
			int levelIndex;
			for (levelIndex = 0; levelIndex < data.levels.Count; levelIndex++)
			{
				Debug.Log("level index: " + levelIndex);
				if (data.levels[levelIndex].sceneName == completedSceneName)
				{
					Debug.Log("Found match for index: " + levelIndex);
					CurrentCompletedLevelID = levelIndex;
					return;
				}
			}

			CurrentCompletedLevelID = levelIndex + 1;
		}

		public void LoadData()
		{
			// Try to load levels
			savedData = SaveManager.Instance.LoadData<SerializableLevelData>();

			if (savedData == null)
			{
				Debug.Log("No previous saved data found. Creating new level data save.");
			}
			else
			{
				savedData.CopyTo(data);
			}
			SaveData();
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
			SceneManager.LoadScene(1);
		}

		internal void LoadNextLevel()
		{
			OnLevelExit();
			Debug.Log("Load Next Level: " + data.levels[CurrentCompletedLevelID + 1].sceneName);
			SceneManager.LoadScene(data.levels[CurrentCompletedLevelID + 1].sceneName);
		}

		internal void OnLevelExit()
		{
			Immunotherapy.Instance.RemoveImmunotherapyEffects();
		}
	}
}
