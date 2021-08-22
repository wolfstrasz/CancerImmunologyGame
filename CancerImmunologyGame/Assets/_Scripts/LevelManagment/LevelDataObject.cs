using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.LevelTasks;

namespace ImmunotherapyGame.LevelManagement
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "MyAssets/Level Data Object")]
    public class LevelDataObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public string sceneName;
        public int levelID;
        public bool isCompleted;
        public bool isLocked;

        public List<LevelTaskCompletion> LevelTaskCompletions = new List<LevelTaskCompletion>();

        [SerializeField] [ReadOnly] private int maxPoints;

#if DEBUG
        public void OnAfterDeserialize()
        {
            int points = 0;
            for (int i = 0; i < LevelTaskCompletions.Count; ++i)
            {
                points += LevelTaskCompletions[i].levelTask.awardPoints;
            }
            maxPoints = points;
        }
#else
        public void OnAfterDeserialize()
        {
           
        }
#endif


        public int MaxPoints => maxPoints;

        public int GetCollectedPoints()
		{
            int points = 0;
            for (int i = 0; i < LevelTaskCompletions.Count; ++i)
            {
                LevelTaskCompletion task = LevelTaskCompletions[i];
                if (task.isCompleted)
				{
                    points += task.levelTask.awardPoints;
				}
            }
            return points;
        }

		public void OnBeforeSerialize() {
            int points = 0;
            for (int i = 0; i < LevelTaskCompletions.Count; ++i)
            {
                points += LevelTaskCompletions[i].levelTask.awardPoints;
            }
            maxPoints = points;
        }
	}

    [System.Serializable]
    public class LevelTaskCompletion
    {
        public LevelTask levelTask;
        public bool isCompleted;
    }
}
