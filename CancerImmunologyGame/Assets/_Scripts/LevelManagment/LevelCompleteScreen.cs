using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.UI;

namespace ImmunotherapyGame.LevelManagement
{
    public class LevelCompleteScreen : Singleton<LevelCompleteScreen>
    {
        [SerializeField] private InterfaceControlPanel controlPanel = null;
        [SerializeField] private Transform levelTasksComepletedLayout = null;
        [SerializeField] private GameObject levelTaskCompletedPrefab = null;
        [SerializeField] private GameObject nextLevelButton = null;

        [SerializeField] [ReadOnly] private List<GameObject> levelTaskObjects = new List<GameObject>();
        [SerializeField] [ReadOnly] LevelDataObject currentLevelData = null;

        public void Open(bool nextLevelAvailable = false)
        {
            nextLevelButton.SetActive(nextLevelAvailable);
            controlPanel.Open();
        }

        public int PopulateAndGetEarnedPoints(LevelDataObject levelDataObject)
		{
            Clear();
            currentLevelData = levelDataObject;

            var levelTaskCompletions = currentLevelData.LevelTaskCompletions;
            int earnedPoints = 0;
            for (int i = 0; i < levelTaskCompletions.Count; ++i)
            {

                if (levelTaskCompletions[i].levelTask.awardPoints > 0)
                {
                    GameObject levelTaskObject = Instantiate(levelTaskCompletedPrefab, levelTasksComepletedLayout);

                    LevelTaskCompletedVisual completedLevelTask = levelTaskObject.GetComponent<LevelTaskCompletedVisual>();
                    earnedPoints += completedLevelTask.SetInfoAndRetrievePoints(levelTaskCompletions[i]);
                }
            }

            return earnedPoints;
        }

        private void Clear()
		{
            for (int i = 0; i < levelTaskObjects.Count; ++i)
			{
                Destroy(levelTaskObjects[i]);
			}
            levelTaskObjects.Clear();
		}

        public void OpenMainMenu()
		{
            Debug.Log("OPEN MAIN MENU CALLED");
            LevelManager.Instance.LoadMainMenu();
		}

        public void NextLevel()
		{
            Debug.Log("NEXT LEVEL CALLED BY BUTTON");
            LevelManager.Instance.LoadNextLevel();
		}
        
    }
}
