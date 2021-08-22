using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.LevelTasks
{
    [CreateAssetMenu(menuName = "MyAssets/Level Task")]
    public class LevelTask : ScriptableObject
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
    }
}
