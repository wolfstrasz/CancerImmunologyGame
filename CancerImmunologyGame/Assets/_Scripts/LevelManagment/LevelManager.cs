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
        [SerializeField] private LevelData data = null;
		private SerializableLevelData savedData = null;

        public void OnLevelComplete(int sceneIndex)
		{
			for (int i = 0; i < data.levels.Count; i++)
			{
				if (data.levels[i].sceneIndex == sceneIndex)
				{
					data.levels[i].isCompleted = true;

					// If next level unlock it
					if (i < data.levels.Count - 1)
					{
						data.levels[i + 1].isLocked = false;
					}
				}
			}
			SaveData();
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
	}
}
