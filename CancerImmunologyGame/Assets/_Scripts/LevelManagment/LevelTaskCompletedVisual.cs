using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using ImmunotherapyGame.LevelTasks;

namespace ImmunotherapyGame.LevelManagement
{
    public class LevelTaskCompletedVisual : MonoBehaviour
    {
        [SerializeField] private TMP_Text taskTitle = null;
        [SerializeField] private TMP_Text taskCompletion = null;
        [SerializeField] private TMP_Text taskAward = null;

        internal int SetInfoAndRetrievePoints(LevelTaskCompletion levelTaskCompletion)
		{
            LevelTask task = levelTaskCompletion.levelTask;
            taskTitle.text = task.title;
            
            if (levelTaskCompletion.isCompleted)
			{
                taskCompletion.text = "Already Completed";
                taskAward.text = "+0";
                return 0;
			}

            if (task.isComplete)
			{
                levelTaskCompletion.isCompleted = true;
                taskCompletion.text = "Completed";
                taskAward.text = "+" + task.awardPoints.ToString();
                return task.awardPoints;
			}

             // not already completed and not completed
            taskCompletion.text = "Not Completed";
            taskAward.text = "+0";
            return 0;
		}
    }
}
