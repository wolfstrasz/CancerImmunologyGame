using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ImmunotherapyGame.LevelTasks
{
    public class LevelTaskActivator : MonoBehaviour
    {
        [SerializeField] private LevelTaskType taskObject = null;
        protected void OnDisable()
		{
            LevelTaskSystem.Instance.TaskObjectComplete(taskObject);
		}
    }
}
