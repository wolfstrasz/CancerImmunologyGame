using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.LevelTasks
{
    [CreateAssetMenu(menuName = "MyAssets/Level Task")]
    public class LevelTask : ScriptableObject, ISerializationCallbackReceiver
    {
        public LevelTaskType levelTaskType;
        public string title;
        public int count;
        public int awardPoints;
        public int currentCount = 0;
        public bool isComplete = false;

        public void Initialise()
		{
            currentCount = 0;
            isComplete = false;
		}

        public void Complete()
		{
            isComplete = true;
		}

		public void Reset()
		{
			currentCount = 0;
			isComplete = false;
		}

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
			if (levelTaskType == null)
			{
				Debug.LogWarning("Task has no task type!");
			}
		}
	}
}
