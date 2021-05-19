
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using TMPro;

using ImmunotherapyGame.UI;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame.LevelManagement
{
    [RequireComponent(typeof(Selectable))]
    public class LevelSelectButton : UIMenuNode, IPointerClickHandler, ISubmitHandler, IMoveHandler, ICancelHandler
    {
        [ReadOnly]
        private LevelDataObject data = null;
        [ReadOnly]
        private int buttonID = 0;
        

        [Header("Attributes")]
        [SerializeField]
        TMP_Text levelText = null;

        [Header("Views")]
        [SerializeField]
        private List<GameObject> levelCompletedView = null;
        private bool LevelCompletedView
		{
            set
			{
                foreach (GameObject obj in levelCompletedView)
				{
                    obj.SetActive(value);
				}
			}
		}

        [SerializeField]
        private List<GameObject> levelLockedView = null;
        private bool LevelLockedView
        {
            set
            {
                foreach (GameObject obj in levelLockedView)
                {
                    obj.SetActive(value);
                }
            }
        }


        internal void SetData(LevelDataObject data, int id)
        {
            this.data = data;
            buttonID = id;
            RefreshView();
        }

        internal void RefreshView()
        {
            LevelCompletedView = data.isCompleted;
            LevelLockedView = data.isLocked;
            levelText.text = data.levelIndex.ToString();
        }

        private void LoadLevel()
		{
            if (!data.isLocked)
            {
                Debug.Log("Level Loading");
                if (data.isCompleted)
				{
                    Debug.Log("Completed level loading");
				}
                LevelSelectScreen.Instance.Close();
				SceneManager.LoadScene(data.sceneIndex);
			}
			else
			{
                Debug.Log("Level Locked");
			}
        }

        public void OnPointerClick(PointerEventData eventData)
		{
            LoadLevel();
        }

        public void OnSubmit (BaseEventData eventData)
		{
            LoadLevel();
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
		}
		public void OnMove(AxisEventData eventData)
		{
            LevelSelectScreen.Instance.SelectObjectOnButtonMove(eventData.moveDir, buttonID);
		}

		public override void OnCancel(BaseEventData eventData)
		{
            LevelSelectScreen.Instance.Close();
            OnDeselect(eventData);
            base.OnCancel(eventData);
		}
	}
}
