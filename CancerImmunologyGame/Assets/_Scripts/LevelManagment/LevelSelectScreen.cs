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

        [Header ("Links")]
        [SerializeField] private InterfaceControlPanel panel = null;

        [Header ("Data")]
        [SerializeField] private LevelData levelData = null;
        [SerializeField] private GameObject horizontalLayoutPrefab = null;
        [SerializeField] private GameObject levelItemPrefab = null;
        [SerializeField] private GameObject emptyItemPrefab = null;

        [Header("Layout")]
        [SerializeField] private Transform verticalLayout = null;
        [SerializeField] internal int buttonsPerRow = 8;

        [Header("Debug")]
        [SerializeField] [ReadOnly] internal List<LevelSelectButton> levelSelectButtons = null;
        [SerializeField] [ReadOnly] private bool initialised = false;
        [SerializeField] [ReadOnly] List<Transform> layoutRows = null;



		public void Open()
		{
            
            if (!initialised)
            {
                AddOptions();
            }

            // Refresh the button visuals
            foreach (var btn in levelSelectButtons)
            {
                btn.RefreshView();
            }

            panel.Open();
		}

        public void Close()
		{
            panel.Close();
        }

        private void AddOptions()
        {
            int itemCount = levelData.levels.Count;
            int rowCount = itemCount / buttonsPerRow + 1;
            int itemSpaceCount = rowCount * buttonsPerRow;
            int lastRowEmptyItemsCount = itemSpaceCount - itemCount;
            int lastRowItemCount = buttonsPerRow - lastRowEmptyItemsCount;
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
                for (int j = 0; j < buttonsPerRow; ++j)
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


            // Add initial button 
            foreach (var btn in levelSelectButtons)
            {
                panel.nodesToListen.Add(btn);
            }

            panel.initialControlNode = levelSelectButtons[0];
            initialised = true;
        }


        /// <summary>
        /// Selects a new button dependent on the current button's ID and the controller direction command
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="buttonID"></param>
        internal void SelectObjectOnButtonMove(MoveDirection dir, int buttonID)
        {
            int pressedButtonID = buttonID;
            int maxButtonIndex = levelSelectButtons.Count - 1;

            if (dir == MoveDirection.Left)
            {
                if ( buttonID > 0)
				{
                    --buttonID;
				}
            }
            else if (dir == MoveDirection.Right)
            {
                if (buttonID < maxButtonIndex)
				{
                    ++buttonID;
				}
            }
            else if (dir == MoveDirection.Up)
            {
                if (buttonID >= buttonsPerRow)
                {
                    buttonID -= buttonsPerRow;
                }
            }
            else if (dir == MoveDirection.Down)
            {
                if (buttonID <= maxButtonIndex - buttonsPerRow)
				{
                    buttonID += buttonsPerRow;
                }
            }

            if (buttonID != pressedButtonID)
			{
                EventSystem.current.SetSelectedGameObject(levelSelectButtons[buttonID].gameObject);
			}
        }


    }
}
