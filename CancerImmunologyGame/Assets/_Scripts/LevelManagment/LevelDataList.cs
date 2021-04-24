using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.SaveSystem;

namespace ImmunotherapyGame.LevelManagement
{

    [CreateAssetMenu(menuName = "LevelDataList")]
    public class LevelDataList : ScriptableObject
    {
        public List<LevelData> levels = null;
        internal void ResetLevelData()
		{
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
    public class LevelData
	{
        public int sceneIndex;
        public int levelIndex;
        public bool isCompleted;
        public bool isLocked;
	}

    [System.Serializable]
    public class SerializableLevelDataList : SavableObject
	{
        public List<LevelData> Levels = new List<LevelData>();

        public SerializableLevelDataList(List<LevelData> levels)
		{
            this.Levels = levels;
		}

        public SerializableLevelDataList()
		{
            Levels = new List<LevelData>();
		}
	}
}
