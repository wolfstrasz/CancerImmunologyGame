using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ImmunotherapyGame.LevelTasks
{
    public class LevelTaskVisual : MonoBehaviour
    {
        [SerializeField][ReadOnly] private LevelTask task = null;
        [SerializeField] internal TMP_Text taskTitleText = null;
        [SerializeField] private TMP_Text taskCompletionText = null;
        [SerializeField] private GameObject taskAwardPointsVisual = null;
        [SerializeField] private TMP_Text taskAwardPointsText = null;
        [SerializeField] private LevelTaskTextResizer textResizer = null;
        [SerializeField] private Animator anim = null;

        internal void SetInfo(LevelTask task)
        {
            this.task = task;
            taskTitleText.text = task.title;
            taskCompletionText.text = "0/" + task.count.ToString();
            taskAwardPointsText.text = task.awardPoints.ToString();
            taskAwardPointsVisual.SetActive(task.awardPoints > 0);
            UpdateResizerSize();
        }

        internal void UpdateCompletionTextInfo()
		{
            taskCompletionText.text = task.currentCount.ToString() + "/" + task.count.ToString();
		}

        internal void UpdateResizerSize()
		{
            // Point the personal resiser to look at the current longest TMP_text in the system
            textResizer.Text = LevelTaskSystem.Instance.CurrentLongestTaskText;
            textResizer.Refresh();
        }

        internal void OnTaskComplete()
		{
            anim.SetTrigger("TaskComplete");
		}

        public void OnAnimationComplete()
		{
            LevelTaskSystem.Instance.RemoveTaskFromList(task, this);
            Destroy(gameObject);
        }
    }
}
