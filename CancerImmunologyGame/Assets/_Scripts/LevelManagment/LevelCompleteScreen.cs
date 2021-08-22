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
        [SerializeField] private InterfaceControlPanel endSceenPanel = null;
        [SerializeField] private Transform levelTasksComepletedLayout = null;
        [SerializeField] private GameObject levelTaskCompletedPrefab = null;
        [SerializeField] private GameObject nextLevelButton = null;
        [SerializeField] private GameObject levelTaskDescription = null;
        [SerializeField] [ReadOnly] private List<GameObject> levelTaskObjects = new List<GameObject>();
        [SerializeField] [ReadOnly] LevelDataObject currentLevelData = null;

        public void Open(bool nextLevelAvailable = false)
        {
            nextLevelButton.SetActive(nextLevelAvailable);
            controlPanel.Open();

            if (!nextLevelAvailable)
			{
                controlPanel.onOpenInterface += OpenEndScreen;
            }

        }

        private void OpenEndScreen()
		{
            endSceenPanel.Open();
            controlPanel.onOpenInterface -= OpenEndScreen;
        }

        public int PopulateAndGetEarnedPoints(LevelDataObject levelDataObject)
		{
            Clear();
            currentLevelData = levelDataObject;

            var levelTaskCompletions = currentLevelData.LevelTaskCompletions;

            levelTaskDescription.SetActive(levelTaskCompletions.Count > 0);

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
            LevelManager.Instance.LoadMainMenu();
		}

        public void NextLevel()
		{
            LevelManager.Instance.LoadNextLevel();
		}

        public void OpenWebpage()
		{
            Application.OpenURL("https://www.cancerimmunology.co.uk/");
		}
        
    }
}
