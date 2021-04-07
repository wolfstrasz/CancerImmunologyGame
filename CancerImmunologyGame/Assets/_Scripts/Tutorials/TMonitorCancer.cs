using System.Collections.Generic;
using UnityEngine;
using Cancers;

namespace ImmunotherapyGame.Tutorials
{
	public class TMonitorCancer : TutorialEvent , ICancerFullObserver
	{
		[SerializeField]
		private CancerMonitorValue monitorValue = CancerMonitorValue.DIVISION_START;
		[SerializeField]
		private int detectionTreshold = 0;

		[Header("Debug (ReadOnly)")]
		[SerializeField]
		private int detectionValue = 0;
		[SerializeField]
		private List<Cancer> cancers = null;
		[SerializeField]
		private int previousFrameCancerCount = 0;


		protected override void OnEndEvent()
		{
			Unsubscribe();
		}

		protected override void OnStartEvent()
		{
			cancers = GlobalGameData.Cancers;

			Subscribe();
			detectionValue = 0;
			// Update detection countdown to be equal to alive cancers
			if (monitorValue == CancerMonitorValue.ALIVE_COUNT)
			{
				UpdateAliveDetectionCountdown();
			}
		}

		protected override bool OnUpdateEvent()
		{
			// Check first if cancer count has been changed
			int currentFrameCancerCount = cancers.Count;

			if (currentFrameCancerCount != previousFrameCancerCount)
			{
				Subscribe();

				// If we are looking for 
				if (monitorValue == CancerMonitorValue.ALIVE_COUNT)
				{
					UpdateAliveDetectionCountdown();
				}
			}
			return detectionValue == detectionTreshold;
		}

		private void UpdateAliveDetectionCountdown()
		{
			int value = 0;

			for (int i = 0; i < cancers.Count; ++i)
			{
				if (cancers[i].IsAlive)
					value++;
			}
			detectionValue = value;
		}


		private void Subscribe()
		{
			foreach (Cancer cancer in cancers)
			{
				if (monitorValue == CancerMonitorValue.DIVISION_END || monitorValue == CancerMonitorValue.DIVISION_START)
				{
					cancer.SubscribeDivisionObserver(this);
				}
				if (monitorValue == CancerMonitorValue.DEATH_COUNT || monitorValue == CancerMonitorValue.ALIVE_COUNT)
				{
					cancer.SubscribeDeathObserver(this);
				}
			}
		}

		private void Unsubscribe()
		{
			foreach (Cancer cancer in cancers)
			{
				if (monitorValue == CancerMonitorValue.DIVISION_END || monitorValue == CancerMonitorValue.DIVISION_START)
				{
					cancer.UnsubscribeDivisionObserver(this);
				}
				if (monitorValue == CancerMonitorValue.DEATH_COUNT || monitorValue == CancerMonitorValue.ALIVE_COUNT)
				{
					cancer.UnsubscribeDeathObserver(this);
				}
			}
		}


		public void OnDivisionStart(Cancer dividingCancer)
		{
			if (monitorValue == CancerMonitorValue.DIVISION_START)
			{
				if (GameCamera2D.Instance.IsInCameraViewBounds(dividingCancer.CellToDivide.transform.position))
				{
					detectionValue++;
				}
			}
		}

		public void OnDivisionEnd(Cancer dividingCancer)
		{
			if (monitorValue == CancerMonitorValue.DIVISION_END)
			{
				if (GameCamera2D.Instance.IsInCameraViewBounds(dividingCancer.CellToDivide.transform.position))
				{
					detectionValue++;
				}
			}
		}

		public void OnCancerDeath()
		{
			if (monitorValue == CancerMonitorValue.DEATH_COUNT)
			{
				detectionValue++;
			}
		}

		private enum CancerMonitorValue { DIVISION_START, DIVISION_END, DEATH_COUNT, ALIVE_COUNT }
	}
}