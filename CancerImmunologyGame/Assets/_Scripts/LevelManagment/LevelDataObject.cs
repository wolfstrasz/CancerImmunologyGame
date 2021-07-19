using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.LevelTasks;

namespace ImmunotherapyGame.LevelManagement
{
    [System.Serializable]
    [CreateAssetMenu (menuName = "MyAssets/Level Data Object")]
    public class LevelDataObject : ScriptableObject
    {
        public string sceneName;
        public int levelID;
        public bool isCompleted;
        public bool isLocked;

        public List<LevelTaskCompletion> LevelTaskCompletions = new List<LevelTaskCompletion>();
	}

    [System.Serializable]
    public class LevelTaskCompletion
    {
        public LevelTask levelTask;
        public bool isCompleted;
    }
}
