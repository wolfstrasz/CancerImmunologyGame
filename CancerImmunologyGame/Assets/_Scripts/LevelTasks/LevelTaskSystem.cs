using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.UI;

using TMPro;
using UnityEngine.UI;

namespace ImmunotherapyGame.LevelTasks
{
    public class LevelTaskSystem : Singleton<LevelTaskSystem>
    {

        [Header ("Data")]
        [SerializeField] private List<LevelTaskType> allLevelTaskObjects = null;

        [Header("UI Linking")]
        [SerializeField] private GameObject view = null;
        [SerializeField] private GameObject levelTaskUILayout = null;
        [SerializeField] private GameObject levelTaskUIPrefab = null;
        [SerializeField] private RefreshLayouts layoutsRefresh = null;

        private Dictionary<LevelTaskType, List<LevelTask>> levelTaskTable = new Dictionary<LevelTaskType, List<LevelTask>>();
        private Dictionary<LevelTask, LevelTaskVisual> levelTaskVisualTable = new Dictionary<LevelTask, LevelTaskVisual>();
        [SerializeField] [ReadOnly] private List<LevelTask> allTasks = new List<LevelTask>();
        [SerializeField] [ReadOnly] private List<LevelTask> allActiveTasks = new List<LevelTask>();
        [SerializeField] [ReadOnly] private List<LevelTaskVisual> allTaskVisuals = new List<LevelTaskVisual>();

        [SerializeField] [ReadOnly] private TMP_Text currentLongestTaskText = null;
        [SerializeField] [ReadOnly] private int currentLongestLength = -1;

        public TMP_Text CurrentLongestTaskText => currentLongestTaskText;

        public void Initialise()
		{
            // Generate empty levelTaskTable
            levelTaskTable = new Dictionary<LevelTaskType, List<LevelTask>>(allLevelTaskObjects.Count);
            foreach (LevelTaskType ltObject in allLevelTaskObjects)
			{
                levelTaskTable.Add(ltObject, new List<LevelTask>());
			}

            view.SetActive(false);
		}

        public void Clear()
		{

            for (int i = 0; i < allTaskVisuals.Count; ++i)
			{
                Destroy(allTaskVisuals[i].gameObject);
			}

            foreach (var key in levelTaskTable.Keys)
			{
                levelTaskTable[key].Clear();
			}
            levelTaskVisualTable.Clear();

            allTasks.Clear();
            allActiveTasks.Clear();
            allTaskVisuals.Clear();

            currentLongestTaskText = null;
            currentLongestLength = -1;
            view.SetActive(false);


		}


        public void CreateTask(LevelTask levelTask)
		{
            view.SetActive(true);
            levelTask.Initialise();
            levelTaskTable[levelTask.levelTaskType].Add(levelTask);
            LevelTaskVisual newLevelTaskVisual = Instantiate(levelTaskUIPrefab, levelTaskUILayout.transform).GetComponent<LevelTaskVisual>();
            newLevelTaskVisual.SetInfo(levelTask);
            layoutsRefresh.shouldRefreshFromStart = true;

            levelTaskVisualTable.Add(levelTask, newLevelTaskVisual);

            allTasks.Add(levelTask);
            allActiveTasks.Add(levelTask);
            allTaskVisuals.Add(newLevelTaskVisual);

            // Update size
            if (levelTask.title.Length > currentLongestLength)
			{
                Debug.Log(this + ": new longer task msg -> RESIZING");
                currentLongestLength = levelTask.title.Length;
                currentLongestTaskText = newLevelTaskVisual.taskTitle;

                for (int i = 0; i < allTaskVisuals.Count; i++)
				{
                    allTaskVisuals[i].UpdateSize();
				}
                layoutsRefresh.shouldRefreshFromStart = true;


            }

        }


        internal void TaskObjectComplete(LevelTaskType ltObject)
		{
            Debug.Log("Task Object Complete: " + ltObject);
            if (!levelTaskTable.ContainsKey(ltObject))
            {
                Debug.LogWarning("Level Task Table does not contain key: " + ltObject);
                return;
            }

            List<LevelTask> tasks = levelTaskTable[ltObject];

            for (int i = 0; i < tasks.Count; ++i)
			{

                LevelTask task = tasks[i];
                Debug.Log("Should Update task: " + task.title + " " + !task.isComplete);

                if (!task.isComplete)
				{
                    Debug.Log("Updating task: " + task.title);
                    ++task.currentCount;
                    LevelTaskVisual ltVisual = levelTaskVisualTable[task];
                    ltVisual.UpdateInfo();

                    if (task.count == task.currentCount)
					{
                        task.Complete();
                        ltVisual.OnTaskComplete();
					}

				}
			}
		}

        internal void RemoveTaskFromList(LevelTask task, LevelTaskVisual taskVisual)
		{
            allActiveTasks.Remove(task);
            allTaskVisuals.Remove(taskVisual);

            // Update sizes 
            int maxLength = -1;
            for (int i = 0; i < allActiveTasks.Count; ++i)
			{
                if (allActiveTasks[i].title.Length > maxLength)
				{
                    maxLength = allActiveTasks[i].title.Length;
                    currentLongestLength = maxLength;
                    currentLongestTaskText = levelTaskVisualTable[allActiveTasks[i]].taskTitle;
				}
			}

            if (maxLength < 0)
			{
                currentLongestTaskText = null;
                currentLongestLength = -1;
			}

            for (int i = 0; i < allTaskVisuals.Count; i++)
            {
                allTaskVisuals[i].UpdateSize();
            }
            layoutsRefresh.shouldRefreshFromStart = true;

            // Update view
            if (allActiveTasks.Count <= 0)
            {
                view.SetActive(false);
            }
        }
    }
}
