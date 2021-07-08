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
        [SerializeField] private List<LevelTaskObject> allLevelTaskObjects = null;

        [Header("UI Linking")]
        [SerializeField] private GameObject view = null;
        [SerializeField] private GameObject levelTaskUILayout = null;
        [SerializeField] private GameObject levelTaskUIPrefab = null;
        [SerializeField] private RefreshLayouts layoutsRefresh = null;

        private Dictionary<LevelTaskObject, List<LevelTask>> levelTaskTable = new Dictionary<LevelTaskObject, List<LevelTask>>();
        private Dictionary<LevelTask, LevelTaskVisual> levelTaskVisualTable = new Dictionary<LevelTask, LevelTaskVisual>();
        [SerializeField] [ReadOnly] private List<LevelTask> allTasks = new List<LevelTask>();
        [SerializeField] [ReadOnly] private List<LevelTask> allCompletedTasks = new List<LevelTask>();
        [SerializeField] [ReadOnly] private List<LevelTask> allActiveTasks = new List<LevelTask>();
        [SerializeField] [ReadOnly] private List<LevelTaskVisual> allTaskVisuals = new List<LevelTaskVisual>();
        public List<LevelTask> AllCompletedTasks => allCompletedTasks;

        [SerializeField] [ReadOnly] private TMP_Text currentLongestTaskText = null;
        [SerializeField] [ReadOnly] private int currentLongestLength = -1;

        public TMP_Text CurrentLongestTaskText => currentLongestTaskText;

        public void Initialise()
		{
            // Generate empty levelTaskTable
            levelTaskTable = new Dictionary<LevelTaskObject, List<LevelTask>>(allLevelTaskObjects.Count);
            foreach (LevelTaskObject ltObject in allLevelTaskObjects)
			{
                levelTaskTable.Add(ltObject, new List<LevelTask>());
			}

            view.SetActive(false);
		}

        public void Clear()
		{
            var allVisualGameobjects = levelTaskUILayout.GetComponentsInChildren<GameObject>();

            for (int i = 0; i < allVisualGameobjects.Length; ++i)
			{
                Destroy(allVisualGameobjects[i]);
			}

            levelTaskTable.Clear();
            levelTaskVisualTable.Clear();

            allTasks.Clear();
            allActiveTasks.Clear();
            allCompletedTasks.Clear();
            allTaskVisuals.Clear();

            currentLongestTaskText = null;
            currentLongestLength = -1;
            view.SetActive(false);


		}


        public void CreateTask(LevelTaskObject ltObject, string title, int count, int awardPoints)
		{
            view.SetActive(true);
            LevelTask newLevelTask = new LevelTask(title, count, awardPoints);
            levelTaskTable[ltObject].Add(newLevelTask);
            LevelTaskVisual newLevelTaskVisual = Instantiate(levelTaskUIPrefab, levelTaskUILayout.transform).GetComponent<LevelTaskVisual>();
            newLevelTaskVisual.SetInfo(newLevelTask);
            layoutsRefresh.shouldRefreshFromStart = true;

            levelTaskVisualTable.Add(newLevelTask, newLevelTaskVisual);

            allTasks.Add(newLevelTask);
            allActiveTasks.Add(newLevelTask);
            allTaskVisuals.Add(newLevelTaskVisual);

            // Update size
            if (title.Length > currentLongestLength)
			{
                Debug.Log(this + ": new longer task msg -> RESIZING");
                currentLongestLength = title.Length;
                currentLongestTaskText = newLevelTaskVisual.taskTitle;

                for (int i = 0; i < allTaskVisuals.Count; i++)
				{
                    allTaskVisuals[i].UpdateSize();
				}
                layoutsRefresh.shouldRefreshFromStart = true;


            }

        }


        internal void TaskObjectComplete(LevelTaskObject ltObject)
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
                        task.isComplete = true;
                        ltVisual.OnTaskComplete();
					}

				}
			}
		}

        internal void RemoveTaskFromList(LevelTask task, LevelTaskVisual taskVisual)
		{
            allCompletedTasks.Add(task);
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
            if (allCompletedTasks.Count == allTasks.Count)
            {
                view.SetActive(false);
            }
        }
    }

    public class LevelTask
    {
        public string title;

        public int count;
        public int awardPoints;
        public int currentCount = 0;

        public bool isComplete = false;

        public LevelTask(string title,int count, int awardPoints)
		{
            this.count = count;
            this.awardPoints = awardPoints;
            this.title = title;
		}
    }
}
