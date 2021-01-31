using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorials
{
	public abstract class TutorialEvent : MonoBehaviour
	{
		[SerializeField]
		private bool skip = false;

		[SerializeField]
		internal TutorialStage owner = null;

		[SerializeField]
		protected bool removeControl;
		[SerializeField]
		protected bool pauseGameplay;

		private bool prevGameplayValue = false;
		private bool prevControlValue = true;
		private bool finished = true;

		public void StartEvent()
		{
			if (skip)
			{
				finished = true;
				owner.OnEventFinished();
			}

			if (removeControl)
			{
				prevControlValue = GlobalGameData.areControlsEnabled;
				Debug.Log("On start control: " + prevControlValue);

				GlobalGameData.areControlsEnabled = false;
			}
			if (pauseGameplay)
			{

				prevGameplayValue = GlobalGameData.isGameplayPaused;
				Debug.Log("On start gameplay: " + prevGameplayValue);
				GlobalGameData.isGameplayPaused = true;
			}

			OnStartEvent();
			finished = false;
		}

		public void EndEvent()
		{
			if (removeControl)
			{
				GlobalGameData.areControlsEnabled = prevControlValue;
				Debug.Log("On end controls: " + GlobalGameData.areControlsEnabled);
			}

			if (pauseGameplay)
			{
				GlobalGameData.isGameplayPaused = prevGameplayValue;
				Debug.Log("On end pasued: " + GlobalGameData.isGameplayPaused);
			}

			OnEndEvent();
		}

		protected abstract void OnStartEvent();

		protected abstract bool OnUpdate();

		protected abstract void OnEndEvent();

		void Update()
		{
			if (finished) return;
			if (OnUpdate())
			{
				EndEvent();
				finished = true;
				owner.OnEventFinished();
			}
		}
	}
}
