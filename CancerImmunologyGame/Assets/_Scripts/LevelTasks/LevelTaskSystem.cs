using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.UI;

using TMPro;


namespace ImmunotherapyGame.LevelTasks
{
    public class LevelTaskSystem : Singleton<LevelTaskSystem>
    {

        [Header ("Data")]
        [SerializeField] private List<LevelTaskType> allLevelTaskTypes = null;

        [Header("UI Linking")]
        [SerializeField] private GameObject view = null;
        [SerializeField] private GameObject levelTaskUILayout = null;
        [SerializeField] private GameObject levelTaskUIPrefab = null;
        [SerializeField] private RefreshLayouts layoutsRefresh = null;
        [SerializeField] private AudioSource source = null;

        // Data management containers
        private Dictionary<LevelTaskType, List<LevelTask>> levelTaskTable = new Dictionary<LevelTaskType, List<LevelTask>>();
        private Dictionary<LevelTask, LevelTaskVisual> levelTaskVisualsTable = new Dictionary<LevelTask, LevelTaskVisual>();
        [SerializeField] [ReadOnly] private List<LevelTask> allTasks = new List<LevelTask>();
        [SerializeField] [ReadOnly] private List<LevelTask> allActiveTasks = new List<LevelTask>();
        [SerializeField] [ReadOnly] private List<LevelTaskVisual> allTaskVisuals = new List<LevelTaskVisual>();

        [Header("Debug")]
        [SerializeField] [ReadOnly] private TMP_Text currentLongestTaskTMPText = null;
        [SerializeField] [ReadOnly] private int currentLongestTaskTextLength = -1;

        public TMP_Text CurrentLongestTaskText => currentLongestTaskTMPText;

        public void Initialise()
		{
            // Generate empty levelTaskTable
            levelTaskTable = new Dictionary<LevelTaskType, List<LevelTask>>(allLevelTaskTypes.Count);
            foreach (LevelTaskType levelTaskType in allLevelTaskTypes)
			{
                levelTaskTable.Add(levelTaskType, new List<LevelTask>());
			}

            view.SetActive(false);
		}

		public void Clear()
		{
            // Clear visuals of tasks
            for (int i = 0; i < allTaskVisuals.Count; ++i)
			{
                Destroy(allTaskVisuals[i].gameObject);
			}
            levelTaskVisualsTable.Clear();

            // Clear all tasks of each task type
            foreach (var key in levelTaskTable.Keys)
			{
                levelTaskTable[key].Clear();
			}

            allTasks.Clear();
            allActiveTasks.Clear();
            allTaskVisuals.Clear();

            currentLongestTaskTMPText = null;
            currentLongestTaskTextLength = -1;
            view.SetActive(false);
		}

        public void CreateTask(LevelTask levelTask)
		{
            view.SetActive(true);

            // Initialise and create a visual
            levelTask.Initialise();
            LevelTaskVisual newLevelTaskVisual = Instantiate(levelTaskUIPrefab, levelTaskUILayout.transform).GetComponent<LevelTaskVisual>();
            newLevelTaskVisual.SetInfo(levelTask);

            // Add to tables
            levelTaskTable[levelTask.levelTaskType].Add(levelTask);
            levelTaskVisualsTable.Add(levelTask, newLevelTaskVisual);

            allTasks.Add(levelTask);
            allActiveTasks.Add(levelTask);
            allTaskVisuals.Add(newLevelTaskVisual);

            // Update size and 
            if (levelTask.title.Length > currentLongestTaskTextLength)
			{
                currentLongestTaskTextLength = levelTask.title.Length;
                // Update value for resizers to use
                currentLongestTaskTMPText = newLevelTaskVisual.taskTitleText;

                for (int i = 0; i < allTaskVisuals.Count; i++)
				{
                    allTaskVisuals[i].UpdateResizerSize();
				}
            }

            // Refresh visual layouts
            layoutsRefresh.ForceRefresh();
        }


        internal void TaskObjectComplete(LevelTaskType ltObject)
		{
            if (!levelTaskTable.ContainsKey(ltObject))
            {
                Debug.LogWarning("Level Task Table does not contain key: " + ltObject);
                return;
            }

            // Cache tasks of given task type
            List<LevelTask> tasks = levelTaskTable[ltObject];

            for (int i = 0; i < tasks.Count; ++i)
			{
                LevelTask task = tasks[i];

                if (task == null)
				{
                    continue;
				}

                if (!task.isComplete)
				{
                    ++task.currentCount;
                    LevelTaskVisual levelTaskVisual = levelTaskVisualsTable[task];
                    levelTaskVisual.UpdateCompletionTextInfo();

                    if (task.count == task.currentCount)
					{
                        task.isComplete = true;
                        levelTaskVisual.OnTaskComplete();
                        source.Play();
					}

				}
			}
		}

        internal void RemoveTaskFromList(LevelTask task, LevelTaskVisual taskVisual)
		{
            allActiveTasks.Remove(task);
            allTaskVisuals.Remove(taskVisual);

            // Update layout max size 
            int maxLength = -1;
            for (int i = 0; i < allActiveTasks.Count; ++i)
			{
                if (allActiveTasks[i].title.Length > maxLength)
				{
                    maxLength = allActiveTasks[i].title.Length;
                    currentLongestTaskTextLength = maxLength;
                    currentLongestTaskTMPText = levelTaskVisualsTable[allActiveTasks[i]].taskTitleText;
				}
			}

            if (maxLength < 0)
			{
                currentLongestTaskTMPText = null;
                currentLongestTaskTextLength = -1;
			}

            // Resize items
            for (int i = 0; i < allTaskVisuals.Count; i++)
            {
                allTaskVisuals[i].UpdateResizerSize();
            }
            layoutsRefresh.ForceRefresh();

            // Update view
            if (allActiveTasks.Count <= 0)
            {
                view.SetActive(false);
            }
        }
    }
}
