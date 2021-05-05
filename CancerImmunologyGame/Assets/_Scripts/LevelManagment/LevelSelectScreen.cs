using System.Collections.Generic;

using UnityEngine;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.LevelManagement;

namespace ImmunotherapyGame.UI
{
    public class LevelSelectScreen : Singleton<LevelSelectScreen>
    {
        [SerializeField]
        private LevelData levelData = null;
        [SerializeField]
        private GameObject levelItemPrefab = null;
        [SerializeField]
        private GameObject levelItemLayout = null;
        [SerializeField]
        private List<LevelSelectButton> levelSelectButtons = null;
        [ReadOnly]
        private bool initialised = false;

        public void Initialise()
		{
            var previousButtons = levelItemLayout.GetComponentsInChildren<LevelSelectButton>();
            for (int i = 0; i < previousButtons.Length; ++i)
			{
                Destroy(previousButtons[i].gameObject);
			}

            levelSelectButtons.Clear();

            for (int i = 0; i < levelData.levels.Count; ++i)
			{
				LevelSelectButton newButton = Instantiate(levelItemPrefab, levelItemLayout.transform).GetComponent<LevelSelectButton>();
                newButton.UpdateData(levelData.levels[i]);
				levelSelectButtons.Add(newButton);
			}
            initialised = true;
		}

        internal void UpdateLevelItem(int index = 0)
		{
            levelSelectButtons[index].UpdateData(levelData.levels[index]);

		}


        public void Open()
		{
            if (!initialised)
			{
                Initialise();
			}
			{
                for (int i = 0; i < levelSelectButtons.Count; ++i)
                {
                    levelSelectButtons[i].UpdateData(levelData.levels[i]);
                }
            }

            gameObject.SetActive(true);
		}

        public void Close()
		{
            gameObject.SetActive(false);
		}
    }
}
