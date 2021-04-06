using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace ImmunotherapyGame.Tutorials
{
	public class TMonitorPlayerDeath : TutorialEvent, IPlayerObserver
	{
		private bool playerDied = false;

		public void OnPlayerDeath()
		{
			playerDied = true;
		}

		protected override void OnEndEvent()
		{
			PlayerController.Instance.UnsubscribeObserver(this);
		}

		protected override void OnStartEvent()
		{
			PlayerController.Instance.SubscribeObserver(this);
		}

		protected override bool OnUpdateEvent()
		{
			return playerDied;
		}


	}
}