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
        [SerializeField]
        private List<MenuButton> cancelButtons = null;

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
            int buttonsCount = levelSelectButtons.Count;
            if (dir == MoveDirection.Left)
			{
                buttonID = buttonID > 0 ? --buttonID : (buttonsCount - 1);
			} 
            else if (dir == MoveDirection.Right)
			{
                buttonID = buttonID < (buttonsCount - 1) ? ++buttonID : 0;
			}
            else if (dir == MoveDirection.Up)
			{
                buttonID -= 8;

                if (buttonID < 0)
				{
                    buttonID += layoutRows.Count * itemsPerRow;
                    if (buttonID >= buttonsCount)
                        buttonID -= 8;
				}
			}
            else if (dir == MoveDirection.Down)
			{
                buttonID += 8;

                if (buttonID >= buttonsCount)
				{
                    buttonID %= itemsPerRow;
				}
			}

            buttonID = + buttonID - buttonsCount * (buttonID / buttonsCount); // modulo
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

            GameObject rowGo = null;
            Transform row = null;
            // Populate first N-1 rows as they will probably be full
            for (int i = 0; i < rowCount - 1; ++i)
            {
                // Create a row
                rowGo = Instantiate(horizontalLayoutPrefab, verticalLayout);
                rowGo.name = "Row: " + (i + 1);
                row = rowGo.transform;

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
            rowGo = Instantiate(horizontalLayoutPrefab, verticalLayout);
            rowGo.name = "Row: " + rowCount;
            row = rowGo.transform;

            for (int i = 0; i < lastRowItemCount; ++i)
            {
                var item = Instantiate(levelItemPrefab, row).GetComponent<LevelSelectButton>();
                item.SetData(levelData.levels[levelDataIndex], levelDataIndex);
                levelSelectButtons.Add(item);
                ++levelDataIndex;
            }

            layoutRows.Add(row);


            // Populate empty space of row with empty items to constrict the forced expansion of items on it.
            for (int i = 0; i < lastRowEmptyItemsCount; ++i)
            {
                Instantiate(emptyItemPrefab, row);
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
            EventSystem.current.SetSelectedGameObject(levelSelectButtons[0].gameObject);
		}

        public void Close()
		{
            gameObject.SetActive(false);
		}
	}
}
