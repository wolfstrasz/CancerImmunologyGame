using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace ImmunotherapyGame.Tutorials
{
    public class TutorialLogsDisplay : MonoBehaviour
    {
		[SerializeField] private TutorialLogsData data;

		[Header("UI Links")]
		[SerializeField] private Image logImage;
		[SerializeField] private TMP_Text logTitle;
		[SerializeField] private TMP_Text pageText;
		[SerializeField] private GameObject leftBtn;
		[SerializeField] private GameObject rightBtn;
		[SerializeField] private GameObject quitBtn;

 		[Header("Debug")]
		[SerializeField] [ReadOnly] private int activePagesNumber;
		[SerializeField] [ReadOnly] private int currentPageNumber;
		[SerializeField] [ReadOnly] private int logIndex;

		private void OnEnable()
		{
			FindActivePages();
			if (activePagesNumber > 0)
			{
				SetPage(data.allLogs[logIndex]);
			}
		
		}

		private void OnDisable()
		{
			rightBtn.SetActive(false);
			leftBtn.SetActive(false);
			logImage.gameObject.SetActive(false);
			pageText.gameObject.SetActive(false);
			logTitle.gameObject.SetActive(false);

		}


		private void FindActivePages()
		{
			currentPageNumber = 1;
			activePagesNumber = 0;
			logIndex = -1;
			for (int i = data.allLogs.Count - 1; i >= 0; --i)
			{
				if (data.allLogs[i].isUnlocked)
				{
					++activePagesNumber;
					logIndex = i;
				}
			}
		}

		public void NextPage()
		{
			for (int i = logIndex + 1; i < data.allLogs.Count; ++i)
			{
				if (data.allLogs[i].isUnlocked)
				{
					logIndex = i;
					++currentPageNumber;
					SetPage(data.allLogs[i]);


					if (!rightBtn.gameObject.activeInHierarchy)
					{
						if (leftBtn.gameObject.activeInHierarchy)
						{
							EventSystem.current.SetSelectedGameObject(leftBtn);
						}
						else
						{
							EventSystem.current.SetSelectedGameObject(quitBtn);
						}
					}

					break;

				}
			}
		}

		public void PreviousPage()
		{
			for (int i = logIndex - 1; i >= 0; --i)
			{
				if (data.allLogs[i].isUnlocked)
				{
					logIndex = i;
					--currentPageNumber;
					SetPage(data.allLogs[i]);

					if (!leftBtn.gameObject.activeInHierarchy)
					{
						if (rightBtn.gameObject.activeInHierarchy)
						{
							EventSystem.current.SetSelectedGameObject(rightBtn);
						}
						else
						{
							EventSystem.current.SetSelectedGameObject(quitBtn);
						}
					}

					break;
				}
			}
		}


		private void SetPage(TutorialLog log)
		{
			logImage.sprite = log.imageSprite;
			logTitle.text = log.title;
			pageText.text = "Pages: " + currentPageNumber + " of " + activePagesNumber;

			rightBtn.SetActive(currentPageNumber < activePagesNumber);
			leftBtn.SetActive(currentPageNumber > 1);

			logTitle.gameObject.SetActive(true);
			logImage.gameObject.SetActive(true);
			pageText.gameObject.SetActive(true);
		}
	}
}
