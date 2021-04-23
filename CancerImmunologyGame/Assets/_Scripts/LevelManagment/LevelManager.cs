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
        private LevelDataList levelList = null;

        public List<LevelData> Levels => levelList.levels;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				SaveLevelData();
			}

			if (Input.GetKeyDown(KeyCode.L))
			{
				LoadLevelData();
				LevelManagerUI.Instance.Initialise();
			}

			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				OnLevelComplete(3);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				OnLevelComplete(2);
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
			LevelManagerUI.Instance.Initialise();
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

					if (i < Levels.Count -1)
					{
						Levels[i + 1].isLocked = false;
						LevelManagerUI.Instance.UpdateLevelItem(i + 1);
					}
				}
			}
		}

        public void OnLevelRestart()
		{

		}

        public void OnLevelStart()
		{

		}

        private void SaveLevelData()
		{
			// If success make current SO have that data
			SaveManager.Instance.SaveData<SerializableLevelDataList>(new SerializableLevelDataList(Levels));
		}

        private void LoadLevelData()
		{
			// Try to load levels
			SerializableLevelDataList loadList = SaveManager.Instance.LoadData<SerializableLevelDataList>();
			if (loadList != null)
			{
				Debug.Log("Success in loading");
				for (int i = 0; i < loadList.levels.Count; ++i)
				{
					Debug.Log(loadList.levels[i]);
				}
				
				for (int i = 0; i < loadList.levels.Count; ++i)
				{
					levelList.levels[i].isCompleted = loadList.levels[i].isCompleted;
					levelList.levels[i].isLocked = loadList.levels[i].isLocked;
					levelList.levels[i].sceneIndex = loadList.levels[i].sceneIndex;
					levelList.levels[i].levelIndex = loadList.levels[i].levelIndex;
				}

				int loadedLevelCount = loadList.levels.Count;

				if (loadedLevelCount < levelList.levels.Count)
				{
					if (loadList.levels[loadedLevelCount - 1].isCompleted)
					{
						levelList.levels[loadedLevelCount].isLocked = false;
					}
					SaveLevelData();
				}
			}
			else
			{
				SaveLevelData();
			}

		}

		public void ResetLevelData()
		{

		}

	}
}
