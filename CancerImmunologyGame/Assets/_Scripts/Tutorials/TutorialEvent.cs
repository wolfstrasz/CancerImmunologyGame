using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.GameManagement;

namespace Tutorials
{
	public abstract class TutorialEvent : MonoBehaviour
	{
		[SerializeField]
		private bool skip = false;

		[SerializeField]
		internal TutorialStage owner = null;

		[SerializeField]
		protected bool pauseGameplay;
		private bool prevGameplayValue = false;

		private bool finished = true;

		public void StartEvent()
		{
			if (skip)
			{
				finished = true;
				owner.OnEventFinished();
				return;
			}

			if (pauseGameplay)
			{
				GameManager.Instance.RequestGameplayPause();
				//prevGameplayValue = GlobalGameData.isGameplayPaused;
				//Debug.Log("On start gameplay: " + prevGameplayValue);
				//GlobalGameData.isGameplayPaused = true;
			}

			OnStartEvent();
			finished = false;
		}

		public void EndEvent()
		{
			if (pauseGameplay)
			{
				//GlobalGameData.isGameplayPaused = prevGameplayValue;
				//Debug.Log("On end pasued: " + GlobalGameData.isGameplayPaused);
				GameManager.Instance.RequestGameplayUnpause();
			}

			OnEndEvent();
		}

		protected abstract void OnStartEvent();

		protected abstract bool OnUpdateEvent();

		protected abstract void OnEndEvent();

		internal void OnUpdate()
		{
			if (finished) return;
			if (OnUpdateEvent())
			{
				EndEvent();
				finished = true;
				owner.OnEventFinished();
			}
		}
	}
}
