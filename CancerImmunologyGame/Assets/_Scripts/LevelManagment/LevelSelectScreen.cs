using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.LevelManagement;
namespace ImmunotherapyGame.UI
{
    public class LevelSelectScreen : Singleton<LevelSelectScreen>
    {
        [SerializeField]
        private LevelData levelData = null;
        [SerializeField]
        private GameObject horizontalLayoutPrefab = null;
        [SerializeField]
        private GameObject levelItemPrefab = null;
        [SerializeField]
        private GameObject emptyItemPrefab = null;


        [Header("Layout")]
        [SerializeField]
        private Transform verticalLayout = null;
        [SerializeField]
        internal int itemsPerRow = 8;

        [Header("Debug")]
        [ReadOnly]
        internal List<LevelSelectButton> levelSelectButtons = null;
        [ReadOnly]
        private bool initialised = false;
        [ReadOnly]
        List<Transform> layoutRows = null;

        
        internal void SelectObjectOnButtonMove(MoveDirection dir, int buttonID)
        {
            int buttonCount = levelSelectButtons.Count;
            switch (dir)
            {
                case MoveDirection.Up: buttonID -= itemsPerRow; break;
                case MoveDirection.Down: buttonID += itemsPerRow; break;
                case MoveDirection.Left: --buttonID; break;
                case MoveDirection.Right: ++buttonID; break;
                default: break;
            }
            buttonID += buttonCount;
            buttonID = + buttonID - buttonCount * (buttonID / buttonCount); // modulo
            EventSystem.current.SetSelectedGameObject(levelSelectButtons[buttonID].gameObject);
        }
    

        private void RefreshOptions()
		{
            foreach (var btn in levelSelectButtons)
            {
                btn.RefreshView();
            }
        }

        private void AddOptions()
		{
            int itemCount = levelData.levels.Count;
            int rowCount = itemCount / itemsPerRow + 1;
            int itemSpaceCount = rowCount * itemsPerRow;
            int lastRowEmptyItemsCount = itemSpaceCount - itemCount;
            int lastRowItemCount = itemsPerRow - lastRowEmptyItemsCount;
            int levelDataIndex = 0;

            levelSelectButtons = new List<LevelSelectButton>(itemCount);
            layoutRows = new List<Transform>();

            // Populate first N-1 rows as they will probably be full
            for (int i = 0; i < rowCount - 1; ++i)
            {
                // Create a row
                var row = Instantiate(horizontalLayoutPrefab, verticalLayout).transform;

                // Add items on row
                for (int j = 0; j < itemsPerRow; ++j)
                {
                    var item = Instantiate(levelItemPrefab, row).GetComponent<LevelSelectButton>();
                    item.SetData(levelData.levels[levelDataIndex], levelDataIndex);
                    levelSelectButtons.Add(item);
                    ++levelDataIndex;
                }

                layoutRows.Add(row);
            }

            // Populate last row
            var lastRow = Instantiate(horizontalLayoutPrefab, verticalLayout).transform;

            for (int i = 0; i < lastRowItemCount; ++i)
            {
                var item = Instantiate(levelItemPrefab, lastRow).GetComponent<LevelSelectButton>();
                item.SetData(levelData.levels[levelDataIndex], levelDataIndex);
                levelSelectButtons.Add(item);
                ++levelDataIndex;
            }

            layoutRows.Add(lastRow);


            // Populate empty space of row with empty items to constrict the forced expansion of items on it.
            for (int i = 0; i < lastRowEmptyItemsCount; ++i)
            {
                Instantiate(emptyItemPrefab, lastRow);
            }

            initialised = true;
        }

        public void Open()
		{
            if (!initialised)
            {
                AddOptions();
            }

            RefreshOptions();

            gameObject.SetActive(true);
		}

        public void Close()
		{
            gameObject.SetActive(false);
		}
    }
}
