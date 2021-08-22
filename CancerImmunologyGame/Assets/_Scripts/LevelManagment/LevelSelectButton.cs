
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using TMPro;

using ImmunotherapyGame.UI;


namespace ImmunotherapyGame.LevelManagement
{
    [RequireComponent(typeof(Selectable))]
    public class LevelSelectButton : UIMenuNode, ISubmitHandler, IMoveHandler, ICancelHandler
    {        
        [Header("Linking")]
        [SerializeField] TMP_Text levelText = null;
        [SerializeField] private List<GameObject> levelLockedView = null;
        [SerializeField] private List<GameObject> levelCompletedView = null;
        [SerializeField] private TMP_Text pointsCollectedText = null;

        [Header("Debug")]
        [SerializeField] [ReadOnly] private LevelDataObject data = null;
        [SerializeField] [ReadOnly] private int buttonID = 0;

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
            levelText.text = data.levelID.ToString();
            pointsCollectedText.gameObject.SetActive(data.isCompleted);
            pointsCollectedText.text = data.GetCollectedPoints().ToString() + " l " + data.MaxPoints.ToString();
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
				SceneManager.LoadScene(data.sceneName);
			}
			else
			{
                Debug.Log("Level Locked");
			}
        }

        public void OnSubmit (BaseEventData eventData)
		{
            LoadLevel();
		}


		public void OnMove(AxisEventData eventData)
		{
            LevelSelectScreen.Instance.SelectObjectOnButtonMove(eventData.moveDir, buttonID);
		}

		protected override void OnPointerLeftClick(PointerEventData eventData)
		{
            LoadLevel();
        }

	}
}
