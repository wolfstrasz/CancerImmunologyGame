using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.SaveSystem;

namespace ImmunotherapyGame.LevelManagement
{

    [CreateAssetMenu(menuName = "LevelDataList")]
    [System.Serializable]
    public class LevelDataList : ScriptableObject
    {
        public List<LevelData> levels = null;
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
        public List<LevelData> levels = new List<LevelData>();

        public SerializableLevelDataList(List<LevelData> levels)
		{
            this.levels = levels;
		}

        public SerializableLevelDataList()
		{
            levels = new List<LevelData>();
		}
	}
}
