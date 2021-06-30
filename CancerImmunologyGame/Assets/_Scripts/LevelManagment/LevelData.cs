using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.SaveSystem;

namespace ImmunotherapyGame.LevelManagement
{

    [CreateAssetMenu(menuName = "LevelData")]
    public class LevelData : ScriptableObject
    {
        public int currentLevel = 0;
        public List<LevelDataObject> levels = null;

        internal void ResetData()
		{
			currentLevel = 0;
            if (levels.Count <= 0)
			{
                Debug.LogError("Level list is empty! Not possible to reset it!");
                return;
			}

            levels[0].isLocked = false;
            levels[0].isCompleted = false;

            for (int i = 1; i < levels.Count; ++i)
			{
                levels[i].isLocked = true;
                levels[i].isCompleted = false;
			}
		}
    }

    [System.Serializable]
    public class LevelDataObject
	{
        public int sceneIndex;
        public int levelIndex;
        public bool isCompleted;
        public bool isLocked;
	}

    [System.Serializable]
    public class SerializableLevelData : SaveableObject
	{
        public int currentLevel = 0;
        public List<LevelDataObject> levels = new List<LevelDataObject>();

        public SerializableLevelData(LevelData data)
		{
            this.currentLevel = data.currentLevel;
            this.levels = data.levels;
		}

        public SerializableLevelData()
		{
            currentLevel = 0;
            levels = new List<LevelDataObject>();
		}

        public void CopyTo(LevelData data)
		{
			data.currentLevel = currentLevel;

			int levelsCount = levels.Count;
			for (int i = 0; i < levelsCount; ++i)
			{
				data.levels[i].isCompleted = levels[i].isCompleted;
				data.levels[i].isLocked = levels[i].isLocked;
				data.levels[i].sceneIndex = levels[i].sceneIndex;
				data.levels[i].levelIndex = levels[i].levelIndex;
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
