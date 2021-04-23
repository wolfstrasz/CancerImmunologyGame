using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.LevelManagement
{
    public class LevelManagerUI : Singleton<LevelManagerUI>
    {
        [SerializeField]
        private LevelDataList levelList;
        [SerializeField]
        private GameObject levelItemPrefab;
        [SerializeField]
        private GameObject levelItemLayout;
        [SerializeField]
        private List<LevelSelectItem> levelItemButtons;


        internal void Initialise()
		{
            var previousButtons = levelItemLayout.GetComponentsInChildren<LevelSelectItem>();
            for (int i = 0; i < previousButtons.Length; ++i)
			{
                Destroy(previousButtons[i].gameObject);
        
			}
            levelItemButtons.Clear();

            for (int i = 0; i < levelList.levels.Count; ++i)
			{
                LevelSelectItem newButton = Instantiate(levelItemPrefab, levelItemLayout.transform).GetComponent<LevelSelectItem>();
                newButton.UpdateData(levelList.levels[i]);
                levelItemButtons.Add(newButton);
			}
		}

        internal void UpdateLevelItem(int index = 0)
		{
            levelItemButtons[index].UpdateData(levelList.levels[index]);
		}
    }
}
