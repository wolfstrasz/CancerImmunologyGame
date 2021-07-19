using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ImmunotherapyGame.LevelTasks
{
    public class LevelTaskActivator : MonoBehaviour
    {
        [SerializeField] private LevelTaskType taskObject = null;
        [SerializeField] private bool isApplicationQuitting = false;
        protected void OnDisable()
		{
            if (!isApplicationQuitting)
            {
                LevelTaskSystem.Instance.TaskObjectComplete(taskObject);
            }
		}

		private void OnApplicationQuit()
		{
            isApplicationQuitting = true;
		}
	}
}
