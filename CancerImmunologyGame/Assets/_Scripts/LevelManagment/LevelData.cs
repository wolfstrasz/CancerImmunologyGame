using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.SaveSystem;

namespace ImmunotherapyGame.LevelManagement
{

    [CreateAssetMenu(menuName = "LevelData")]
    public class LevelData : ScriptableObject
    {
        public int currentReachedLevel = 0;
        public List<LevelDataObject> levels = null;

        internal void ResetData()
		{
			currentReachedLevel = 0;
            if (levels.Count <= 0)
			{
                Debug.LogError("Level list is empty! Not possible to reset it!");
                return;
			}

            for (int levelIndex = 0; levelIndex < levels.Count; ++levelIndex)
			{
				Debug.Log("Level Data: Resetting level: " + levels[levelIndex].sceneName);
				var level = levels[levelIndex];
				level.isLocked = levelIndex != 0;
				level.isCompleted = false;

				for (int taskID = 0; taskID < level.LevelTaskCompletions.Count; ++taskID)
				{
					Debug.Log("Level Data (" + level.sceneName + ") resetting tasks: " + level.LevelTaskCompletions[taskID].levelTask.title);
					level.LevelTaskCompletions[taskID].isCompleted = false;
				}
			}
		}
    }

	[System.Serializable]
	public class LevelDataSerializableObject
	{
		public string sceneName;
		public bool isCompleted;
		public bool isLocked;
		public List<bool> taskCompletions;

		public LevelDataSerializableObject(LevelDataObject data)
		{
			sceneName = data.sceneName;
			isCompleted = data.isCompleted;
			isLocked = data.isLocked;
			taskCompletions = new List<bool>();
			for (int i = 0; i < data.LevelTaskCompletions.Count; ++i)
			{
				taskCompletions.Add(data.LevelTaskCompletions[i].isCompleted);
			}
		}
	}


	[System.Serializable]
    public class SerializableLevelData : SaveableObject
	{
        public int currentReachedLevel = 0;
        public List<LevelDataSerializableObject> levels = new List<LevelDataSerializableObject>();

        public SerializableLevelData(LevelData data)
		{
            this.currentReachedLevel = data.currentReachedLevel;
			this.levels = new List<LevelDataSerializableObject>();

			for (int i = 0; i < data.levels.Count; ++i)
			{
				LevelDataSerializableObject ldso = new LevelDataSerializableObject(data.levels[i]);
				levels.Add(ldso);
			}
		}

        public SerializableLevelData()
		{
            currentReachedLevel = 0;
            levels = new List<LevelDataSerializableObject>();
		}

        public void CopyTo(LevelData data)
		{
			data.currentReachedLevel = currentReachedLevel;

			int levelsCount = levels.Count;
			for (int i = 0; i < levelsCount; ++i)
			{
				LevelDataObject dataLevelObject = data.levels[i];
				LevelDataSerializableObject loadedLevelObject = levels[i];

				if (dataLevelObject.sceneName != loadedLevelObject.sceneName)
				{
					Debug.LogWarning("Scene name mismatch when loading levels. -> " + dataLevelObject.sceneName + " tries to load " + loadedLevelObject.sceneName + ". Delete save");
				}

				dataLevelObject.isCompleted = loadedLevelObject.isCompleted;
				dataLevelObject.isLocked = loadedLevelObject.isLocked;

				for (int j = 0; j < loadedLevelObject.taskCompletions.Count; ++j)
				{
					dataLevelObject.LevelTaskCompletions[j].isCompleted = loadedLevelObject.taskCompletions[j];
				}
			}

			if (levelsCount < levels.Count)
			{
				Debug.Log("New levels uploaded. Checking to unlock first new level.");
				if (levels[levelsCount - 1].isCompleted)
				{
					Debug.Log("Unlocking first new level.");
					data.levels[levelsCount].isLocked = false;
				}
			}
		}
	}
}
