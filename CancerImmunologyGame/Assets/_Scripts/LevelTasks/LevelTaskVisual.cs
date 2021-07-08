using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ImmunotherapyGame.LevelTasks
{
    public class LevelTaskVisual : MonoBehaviour
    {
        [SerializeField] internal TMP_Text taskTitle = null;
        [SerializeField] private TMP_Text taskCounting = null;
        [SerializeField] private TMP_Text taskAwardPoints = null;
        [SerializeField] private LevelTaskTextResizer resizer = null;
        [SerializeField] private GameObject pointsVisual = null;
        [SerializeField] private Animator anim = null;
        [SerializeField] private new AudioSource audio = null;

        private LevelTask task = null;

        internal void SetInfo(LevelTask task)
        {
            this.task = task;
            taskTitle.text = task.title;
            taskCounting.text = "0/" + task.count.ToString();
            taskAwardPoints.text = task.awardPoints.ToString();
            pointsVisual.SetActive(task.awardPoints > 0);
            UpdateSize();
        }

        internal void UpdateInfo()
		{
            taskCounting.text = task.currentCount.ToString() + "/" + task.count.ToString();
		}

        internal void UpdateSize()
		{
            resizer.Text = LevelTaskSystem.Instance.CurrentLongestTaskText;
            resizer.Refresh();
        }

        internal void OnTaskComplete()
		{
            audio.Play();
            anim.SetTrigger("TaskComplete");
		}

        public void OnAnimationComplete()
		{
            LevelTaskSystem.Instance.RemoveTaskFromList(task, this);
            Destroy(gameObject);
        }
    }
}
