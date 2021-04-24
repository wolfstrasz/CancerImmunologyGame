using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.LevelManagement
{
    public class LevelManagerUI : Singleton<LevelManagerUI>
    {
        [SerializeField]
        private LevelDataList levelData;
        [SerializeField]
        private GameObject levelItemPrefab;
        [SerializeField]
        private GameObject levelItemLayout;
        [SerializeField]
        private List<LevelSelectButton> levelItemButtons;


        internal void Initialise(LevelDataList levelData)
		{
            this.levelData = levelData;
            var previousButtons = levelItemLayout.GetComponentsInChildren<LevelSelectButton>();
            for (int i = 0; i < previousButtons.Length; ++i)
			{
                Destroy(previousButtons[i].gameObject);
        
			}
            levelItemButtons.Clear();

            for (int i = 0; i < this.levelData.levels.Count; ++i)
			{
				LevelSelectButton newButton = Instantiate(levelItemPrefab, levelItemLayout.transform).GetComponent<LevelSelectButton>();
                newButton.UpdateData(this.levelData.levels[i]);
				levelItemButtons.Add(newButton);
			}
		}

        internal void UpdateLevelItem(int index = 0)
		{
            levelItemButtons[index].UpdateData(levelData.levels[index]);

		}


        public void Open()
		{
            gameObject.SetActive(true);
		}

        public void Close()
		{
            gameObject.SetActive(false);
		}
    }
}
